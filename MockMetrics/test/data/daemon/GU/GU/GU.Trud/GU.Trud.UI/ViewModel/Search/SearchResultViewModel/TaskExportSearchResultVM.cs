using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.SearchViewModel;
using GU.BL;
using GU.DataModel;
using GU.Trud.BL;
using GU.Trud.DataModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace GU.Trud.UI.ViewModel.Search.SearchResultViewModel
{
    public class TaskExportSearchResultVM : AbstractSearchResultVM<TaskExport>
    {
        public TaskExportSearchResultVM(TaskExport entity)
            : base(entity)
        {
            SaveExportFileCommand = new DelegateCommand(SaveExportFile);
        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                Filename = Result.Filename;
                AgencyName =
                    GuFacade.GetDictionaryManager().GetDictionary<Agency>().Single(t => t.Id == Result.AgencyId).Name;
                ExportStamp = Result.Stamp.HasValue ? 
                                string.Format("{0} {1}", Result.Stamp.Value.ToLongDateString(), Result.Stamp.Value.ToLongTimeString()) 
                                : 
                                string.Empty;
                ExportType =
                    TrudFacade.GetDictionaryManager().GetDictionary<ExportType>().Single(
                        t => t.Id == Result.ExportTypeId).Name;
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        #region Binding Properties

        public string Filename { get; private set; }

        public string AgencyName { get; private set; }

        public string ExportStamp { get; private set; }

        public string ExportType { get; private set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда сохранения выгрузки на диск
        /// </summary>
        public DelegateCommand SaveExportFileCommand { get; private set; }

        /// <summary>
        /// Сохраняет выгрузку на диск
        /// </summary>
        private void SaveExportFile()
        {
            try
            {
                var sfd = new SaveFileDialog();
                sfd.DefaultExt = ".xwp";
                sfd.Filter = "XWP files (.xwp)|*.xwp";
                sfd.FileName = Result.Filename;
                if (sfd.ShowDialog() == true)
                {
                    TrudFacade.GetSaveExportService().SaveExport(Result, sfd.FileName);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion

    }
}
