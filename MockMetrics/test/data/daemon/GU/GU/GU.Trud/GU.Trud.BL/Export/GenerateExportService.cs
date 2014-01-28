using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BLToolkit.Data.Linq;
using BLToolkit.EditableObjects;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.Trud.BL.Export.DataModel;
using GU.Trud.BL.Export.Event;
using GU.Trud.BL.Export.Interface;
using GU.Trud.DataModel;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Служба осуществления экспорта данных для интеграции с Катарсис.
    /// </summary>
    /// <remarks>
    /// Служба осуществляет экспорт данных в виде архивов из трех файлов:
    /// _baseExportFile - табличный файл базовой выгрузки - Заявки на поиск работы,
    /// _addinExportFile - дополнительный табличный файл фиксированного содержания, в него ничего не кладётся, он просто добавляется в архив,
    /// _xchExportFile - .xch файл, который тоже просто копируется.
    /// </remarks>
    internal class GenerateExportService : IGenerateExportService
    {
        private readonly IToFileDumper _toFileDumper;
        private readonly ICatalogArchivator _catalogArchivator;
        private readonly TaskScheduler _searchShceduler;
        private readonly CancellationTokenSource _cancelSource = new CancellationTokenSource();
        private readonly string _baseExportFile = ConfigurationManager.AppSettings["base_export_file"];
        private readonly string _addinExportFile = ConfigurationManager.AppSettings["addin_export_file"];
        private readonly string _xchExportFile = ConfigurationManager.AppSettings["xch_export_file"];
        private readonly string _baseExportTemplate = ConfigurationManager.AppSettings["base_export_template"];
        private readonly string _addinExportTemplate = ConfigurationManager.AppSettings["addin_axport_template"];
        private readonly string _xchExportTemplate = ConfigurationManager.AppSettings["xch_export_template"];
        private readonly string _archiveNameTemplate = ConfigurationManager.AppSettings["archive_name"];
        private readonly string _operatingCatalog;
        private const string _templatesCatalog = @"Export/Templates";

        /// <summary>
        /// Служба осуществления экспорта данных для интеграции с Катарсис.
        /// </summary>
        /// <param name="toFileDumper">Объект осуществляющий выгрузки</param>
        /// <param name="catalogArchivator">Объект архивирующий выгрузки</param>
        /// <param name="operatingCatalog">Каталог для работы с файлами выгрузки</param>
        public GenerateExportService(IToFileDumper toFileDumper, 
                                     ICatalogArchivator catalogArchivator,
                                     string operatingCatalog)
        {
            _searchShceduler = TaskScheduler.FromCurrentSynchronizationContext();
            _toFileDumper = toFileDumper;
            _catalogArchivator = catalogArchivator;
            _operatingCatalog = operatingCatalog;
        }

        #region IExportService

        #region Events

        /// <summary>
        /// Событие оповещающее о прогрессе процесса экспорта.
        /// </summary>
        public event PercentageProgressDelegate ExportProgressed;

        /// <summary>
        /// Вызывает событие ExportProgressed
        /// </summary>
        /// <param name="percentage">Процентов прогресса</param>
        /// <param name="message">Сообщение</param>
        private void OnExportProgressed(int percentage, string message)
        {
            PercentageProgressDelegate handler = ExportProgressed;
            if (handler != null)
            {
                handler(this, new PercentageProgressEventArgs(percentage, message));
            }
        }

        /// <summary>
        /// Вызывает событие, оповещающее о завершении экспорта.
        /// </summary>
        /// <param name="result"> </param>
        private void OnExportCompleted(IExportResult result)
        {
            PercentageProgressDelegate handler = ExportProgressed;
            string message = result.IsFailed ? "Экспорт завершен с ошибкой" : "Экспорт успешно завершен";
            if (handler != null)
            {
                handler(this, new PercentageProgressEventArgs(100, message, true));
            }
        }

        #endregion

        /// <summary>
        /// Выполняет экспорт данных.
        /// </summary>
        /// <param name="dbManager">Менеджер данных</param>
        public void ExportData(IDomainDbManager dbManager)
        {
           Task<IExportResult>.Factory.StartNew(() =>
                {
                    try
                    {
                        Thread.Sleep(1000);
                        DoExport(dbManager);
                        return new ExportResult();
                    }
                    catch (GUException ex)
                    {
                        return new ExportResult(ex);
                    }
                    catch (Exception ex)
                    {
                        return new ExportResult(new BLLException("Ошибка при проведении выгрузки", ex));
                    }
                }).ContinueWith(task =>
                {
                    ExportResult = task.Result;
                    OnExportCompleted(task.Result);
                }, _searchShceduler);
        }

        /// <summary>
        /// Инициирует отмену экспорта данных.
        /// </summary>
        public void Cancel()
        {
            try
            {
                _toFileDumper.CancelDump();
                _cancelSource.Cancel();
                DeleteOperatingCatalog();
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка отмены процедуры выгрузки.", ex);
            }
        }

        /// <summary>
        /// Возвращает результаты экспорта.
        /// </summary>
        public IExportResult ExportResult { get; private set; }

        #endregion

        #region Prepare Data

        /// <summary>
        /// Выполняет экспорт данных.
        /// </summary>
        /// <param name="dbManager">Менеджер данных</param>
        private void DoExport(IDomainDbManager dbManager)
        {
            OnExportProgressed(10, "Получение данных из БД");

            var exportView = dbManager.GetDomainTable<TaskJobSearch>().ToList();

            OnExportProgressed(30, "Подготовка данных к экспорту");

            List<IGrouping<int, TaskJobSearch>> exportGroups = exportView.GroupBy(t => t.AgencyId).Select(t => t).ToList();

            for (int i = 0; i < exportGroups.Count(); i++)
            {
                int percentage = Convert.ToInt32(30 + Math.Round((70 / (decimal)exportGroups.Count() * i)));
                var archiveName = string.Format(_archiveNameTemplate, (i + 1).ToString("00000"));

                OnExportProgressed(percentage, string.Format("Формирование выгрузки {0}", archiveName));

                var exportArcive = DoSingleExport(exportGroups[i].ToList(), archiveName);
                var taskExport = CreateTaskExport(exportArcive, exportGroups[i].ToList(), archiveName, exportGroups[i].Key);
                taskExport = TrudFacade.GetDataMapper<TaskExport>().Save(taskExport, dbManager);

                dbManager.GetDomainTable<TaskExport>()
                  .Where(e => e.Id == taskExport.Id)
                  .Set(e => e.Filename, string.Format(_archiveNameTemplate, (taskExport.Id).ToString("00000")))
                  .Update();
            }
        }

        /// <summary>
        /// Формирует объект-выгрузку.
        /// </summary>
        /// <param name="exportData">Массив байт файла выгрузки</param>
        /// <param name="taskJobSearchList">Список объектов с данными экспорта в Катарсис</param>
        /// <param name="filename">Имя файла выгрузки</param>
        /// <param name="agencyId">Id ведомста</param>
        /// <returns>Объект выгрузки</returns>
        private TaskExport CreateTaskExport(byte[] exportData, List<TaskJobSearch> taskJobSearchList, string filename, int agencyId)
        {
            var taskExport = TaskExport.CreateInstance();
            taskExport.Data = exportData;
            taskExport.Filename = filename;
            taskExport.ExportTypeId = TrudFacade.GetDictionaryManager().GetDictionary<ExportType>().First().Id;
            taskExport.AgencyId = agencyId;
            return AddTaskExportDet(taskExport, taskJobSearchList);
        }

        /// <summary>
        /// Добавляет детализацию к объекту выгрузки. Возвращает выгрузку.
        /// </summary>
        /// <param name="taskExport">Объект выгрузки</param>
        /// <param name="taskJobSearchList">Список объектов с данными экспорта в Катарсис</param>
        /// <returns>Детализировання выгрузка</returns>
        private TaskExport AddTaskExportDet(TaskExport taskExport, List<TaskJobSearch> taskJobSearchList)
        {
            taskExport.TaskExportDets = new EditableList<TaskExportDet>();
            foreach (var taskId in taskJobSearchList.Select(t => t.TaskId).Distinct())
            {
                var taskExportDet = TaskExportDet.CreateInstance();
                taskExportDet.TaskId = Convert.ToInt32(taskId);
                taskExport.TaskExportDets.Add(taskExportDet);
            }
            return taskExport;
        }

        #endregion

        #region Prepare Files

        /// <summary>
        /// Выполняет экспорт данных по одному ведомству. Возвращает массив байт файла выгрузки.
        /// </summary>
        /// <param name="data">Список объектов с данными экспорта в Катарсис</param>
        /// <param name="archiveName">Имя архива выгрузки</param>
        /// <returns>Массив байт файла выгрузки</returns>
        private byte[] DoSingleExport(List<TaskJobSearch> data, string archiveName)
        {
            var exportFile = DoBaseExport(data);
            CopyAddinExportFiles(Path.GetDirectoryName(exportFile));
            var exportArchiveFile = ArchiveFile(Path.GetDirectoryName(exportFile), archiveName);
            var exportArchive = File.ReadAllBytes(exportArchiveFile);
            Directory.Delete(Path.GetDirectoryName(exportFile), true);
            return exportArchive;
        }

        /// <summary>
        /// Выполняет базовую выгрузку в Катарсис в файл. Вовзращает имя файла с базовой выгрузкой (Заявки на поиск работы).
        /// </summary>
        /// <param name="data">Список объектов с данными экспорта в Катарсис</param>
        /// <returns>Имя файла с базовой выгрузкой</returns>
        private string DoBaseExport(List<TaskJobSearch> data)
        {
            var templateFile = Path.Combine(_templatesCatalog, _baseExportTemplate);
            CopyBaseExportTemplate(_operatingCatalog, _baseExportFile, templateFile);
            var exportFile = Path.Combine(_operatingCatalog, _baseExportFile);
            _toFileDumper.Dump(data.Cast<IToItemArray>().ToList(), exportFile);
            return exportFile;
        }

        /// <summary>
        /// Копирует шаблонный файл базовой выгрузки в указанный каталог.
        /// </summary>
        /// <param name="path">Каталог в который копирует шаблонный файл</param>
        /// <param name="filename">Имя файла выгрузки</param>
        /// <param name="templateFile">Полное имя шаблонного файла</param>
        /// <remarks>
        /// Шаблонный файл хранит структуру таблицы выгрузки. Наименее трудоёмкий вариант при работе с dbf-выгрузками. 
        /// </remarks>
        private void CopyBaseExportTemplate(string path, string filename, string templateFile)
        {
            var directoryInfo = Directory.CreateDirectory(path);
            directoryInfo.Attributes = FileAttributes.Hidden;
            File.Copy(templateFile, Path.Combine(path, filename));
        }

        /// <summary>
        /// Копирует дополнительные экпортные файлы, в которые имеют фиксированное содержание.
        /// </summary>
        /// <param name="archiveCatalog">Каталог архива выгрузки</param>
        private void CopyAddinExportFiles(string archiveCatalog)
        {
            File.Copy(Path.Combine(_templatesCatalog, _addinExportTemplate), Path.Combine(archiveCatalog, _addinExportFile));
            File.Copy(Path.Combine(_templatesCatalog, _xchExportTemplate), Path.Combine(archiveCatalog, _xchExportFile));
        }

        /// <summary>
        /// Архивирует каталог с экпортными файлами. Возвращает полный имя получившегося архива.
        /// </summary>
        /// <param name="exportFilesPath">Каталгог с экпортными файлами</param>
        /// <param name="archiveName">Имя архива</param>
        /// <returns>Полное имя архива</returns>
        private string ArchiveFile(string exportFilesPath, string archiveName)
        {
            var archiveFile = Path.Combine(exportFilesPath, archiveName);
            _catalogArchivator.Archive(exportFilesPath, archiveFile);
            return archiveFile;
        }

        #endregion

        #region RollbackExport

        /// <summary>
        /// Удаляет операционный каталог вместе со сем содержимым.
        /// </summary>
        private void DeleteOperatingCatalog()
        {
            if(Directory.Exists(_operatingCatalog))
            {
                Directory.Delete(_operatingCatalog, true);
            }
        }

        #endregion
    }
}
