/*
 *  Класс бессовестно сперт отсюда: http://contest2005.gotdotnet.ru/Request/Tools/UtilitiesLib/209099.aspx
 *
 *  UPD: ...за исключением импорта
 */

using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Threading;
using System.Data.Common;
using System.Collections.Generic;

namespace GU.Archive.Migration
{
    /// <summary>
    /// Класс - мини-обертка над Екселем
    /// </summary>
    public class ExcelHelper : IDisposable
    {

        #region Поля и свойства

        #region Ссылка на объект Excel
        /// <summary>
        /// Признак того, что экземпляр класса связан с
        /// запущенной копией Екселя
        /// </summary>
        private bool _isStartExcel = false;
        /// <summary>
        /// Ссылка на объект Excel
        /// </summary>
        private object _oExcel = null;
        /// <summary>
        /// Для запоминания текущей культуры.
        /// Так будем лечить баг: http://support.microsoft.com/kb/320369
        /// </summary>
        private CultureInfo _oldCI;
        /// <summary>
        /// Ссылка на объект Excel
        /// </summary>
        protected object oExcel
        {
            get
            {
                if (_isStartExcel && !isConnectExcel)
                {
                    //Если экземпляр уже создал Ексель а ссылка на него мертвая или
                    //в нем отсутствует невидимая книга-призрак, т.е. пользователь закрыл Ексель,
                    //а процесс еще полуживой (присутствует в списке процессов, что характерно для версий ниже 2003),
                    //тогда подчищаем все.
                    ClearComReferens();
                }
                if (_oExcel == null)
                {
                    string sAppProgID = "Excel.Application";
                    // Получаем ссылку на интерфейс IDispatch
                    Type tExcelObj = Type.GetTypeFromProgID(sAppProgID);
                    // Запускаем Excel
                    _oExcel = Activator.CreateInstance(tExcelObj);
                    //Запоминает значение текущей культуры потока
                    _oldCI = Thread.CurrentThread.CurrentCulture;
                    //Выставляем в американщину
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    //Загружаем в Ексель рабочую книгу и делаем ее невидимой.
                    //По ее наличию будем судить выгрузил ли кто-нибудь Ексель.
                    //Так будем бороться с известной проблемой с версиями Екселя ниже 2003.
                    //С одной стороны не совсем красиво, но лучше способа я не нашел.
                    emptyBook();
                    //Устанавливаем индикатор запуска Екселя
                    _isStartExcel = true;
                }
                return _oExcel;
            }
        }
        #endregion
        #region	Ссылка на объект Application
        /// <summary>
        /// Ссылка на объект Application
        /// </summary>
        private object _oApplication;
        /// <summary>
        /// Ссылка на объект Application
        /// </summary>
        protected object oApplication
        {
            get
            {
                if (_oApplication == null)
                {
                    //Получаем ссылку на объект Application
                    _oApplication = oExcel.GetType().InvokeMember("Application",
                        BindingFlags.GetProperty, null, oExcel, null);
                }
                return _oApplication;
            }
        }
        #endregion
        #region Ссылка на объект Workbooks
        /// <summary>
        /// Ссылка на объект Workbooks
        /// </summary>
        private object _oWorkbooks;
        /// <summary>
        /// Ссылка на объект Workbooks
        /// </summary>
        protected object oWorkbooks
        {
            get
            {
                if (_oWorkbooks == null)
                {
                    //Получаем ссылку на коллекцию рабочих книг -
                    //объект Workbooks
                    _oWorkbooks = oExcel.GetType().InvokeMember("Workbooks",
                        BindingFlags.GetProperty, null, oExcel, null);
                }
                return _oWorkbooks;
            }
        }
        #endregion

        #region Видимость Excel
        /// <summary> 
        /// Видимость Excel
        /// </summary>
        public bool Visible
        {
            get
            {
                if (!_isStartExcel)
                    return false;
                try
                {
                    //Получаем значение свойства Visible объкта Application
                    return (bool)oExcel.GetType().InvokeMember("Visible",
                        BindingFlags.GetProperty, null, oApplication, null);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    //Если Ексель не отвечает,
                    //почистим ссылку на него, понятно, что в этом случае он невидимый
                    ClearComReferens();
                }
                return false;
            }
            set
            {
                //Устанавливаем значение свойства Visible объкта Application
                object[] args1 = new object[1];
                args1[0] = value;
                oExcel.GetType().InvokeMember("Visible",
                    BindingFlags.SetProperty, null, oApplication, args1);
            }
        }
        #endregion

        #region Наличие корректной ссылки
        /// <summary>
        /// Наличие корректной ссылки на Ексель
        /// </summary>
        public bool isConnectExcel
        {
            get
            {
                if (!_isStartExcel)
                    return false;
                try
                {
                    if (_emptyBook != null)
                    {
                        //Если ссылка на невидимую книгу есть, проверим ее наличие.
                        //Попробуем получить свойство Name, если оно есть то все нормально.
                        _emptyBook.GetType().InvokeMember("Name",
                            BindingFlags.GetProperty, null, _emptyBook, null);
                        return true;
                    }
                    else
                        return false;
                }
                catch
                {
                    //Если произошла ошибка, значить чистим ссылку.
                    ClearComReferens();
                    return false;
                }
            }
        }
        #endregion

        #endregion

        #region Методы

        #region Создание и открытие рабочих книг
        /// <summary>
        /// Создание рабочей книги
        /// </summary>
        /// <returns>Возвращает имя созданной рабочей книги</returns>
        public string WorkbookAdd()
        {
            return WorkbookAdd(null);
        }

        /// <summary>
        /// Создание рабочей книги
        /// </summary>
        /// <param name="parTemplate">Путь к шаблону</param>
        /// <returns>Возвращает имя созданной рабочей книги</returns>
        public string WorkbookAdd(string parTemplate)
        {
            string getWorkbookName = null;
            if (isConnectExcel)
            {
                //Формируем параметры вызова для метода
                //Workbooks.Add
                object[] args1 = null;
                if (parTemplate != null)
                {
                    args1 = new object[1];
                    args1[0] = parTemplate;
                }
                //Создаем новую рабочую книгу
                object addWorkbook = oWorkbooks.GetType().InvokeMember("Add",
                    BindingFlags.InvokeMethod, null, oWorkbooks, args1);
                //Получаем ее имя
                getWorkbookName = (string)addWorkbook.GetType().InvokeMember("Name",
                    BindingFlags.GetProperty, null, addWorkbook, null);
            }
            return getWorkbookName;
        }
        /// <summary>
        /// Открытие рабочей книги
        /// </summary>
        /// <param name="parFullPath">Путь к файлу рабочей книги</param>
        /// <returns>Возвращает имя рабочей книги</returns>
        public string WorkbookOpen(string parFullPath)
        {
            string getWorkbookName = null;
            if (isConnectExcel)
            {
                //Формируем параметры вызова для метода
                //Workbooks.Open
                object[] args1 = new object[1];
                args1[0] = parFullPath;
                //Открываем рабочую книгу
                object openWorkbook = oWorkbooks.GetType().InvokeMember("Open",
                    BindingFlags.InvokeMethod, null, oWorkbooks, args1);
                //Получаем ее имя
                getWorkbookName = (string)openWorkbook.GetType().InvokeMember("Name",
                    BindingFlags.GetProperty, null, openWorkbook, null);
            }
            return getWorkbookName;
        }
        #endregion

        #region Запуск VBA процедур или функций, вызов членов объектной модели Екселя
        /// <summary>
        /// Запуск VBA процедуры или функции
        /// </summary>
        /// <param name="parNameProcVba">Имя VBA процедуры или функции</param>
        /// <returns>Возращаемое значение (если оно конечно есть)</returns>
        public object RunVbaProc(string parNameProcVba)
        {
            object returnVal = null;
            returnVal = RunVbaProc(parNameProcVba, null, null);
            return returnVal;
        }
        /// <summary>
        /// Запуск VBA процедуры или функции
        /// </summary>
        /// <param name="parNameProcVba">Имя VBA процедуры или функции</param>
        /// <param name="parArgsProcVba">Аргументы для VBA процедуры или функции</param>
        /// <returns>Возращаемое значение (если оно конечно есть)</returns>
        public object RunVbaProc(string parNameProcVba, object[] parArgsProcVba)
        {
            object returnVal = null;
            returnVal = RunVbaProc(parNameProcVba, null, parArgsProcVba);
            return returnVal;
        }
        /// <summary>
        /// Запуск VBA процедуры или функции
        /// </summary>
        /// <param name="parNameProcVba">Имя VBA процедуры или функции</param>
        /// <param name="parNameWorkbook">Имя рабочей книги, содержащей VBA скрипт</param>
        /// <returns>Возращаемое значение (если оно конечно есть)</returns>
        public object RunVbaProc(string parNameProcVba, string parNameWorkbook)
        {
            object returnVal = null;
            returnVal = RunVbaProc(parNameProcVba, parNameWorkbook, null);
            return returnVal;
        }
        /// <summary>
        /// Запуск VBA процедуры или функции
        /// </summary>
        /// <param name="parNameProcVba">Имя VBA процедуры или функции</param>
        /// <param name="parNameWorkbook">Имя рабочей книги, содержащей VBA скрипт</param>
        /// <param name="parArgsProcVba">Аргументы для VBA процедуры или функции</param>
        /// <returns>Возращаемое значение (если оно конечно есть)</returns>
        public object RunVbaProc(string parNameProcVba, string parNameWorkbook,
            object[] parArgsProcVba)
        {
            object returnVal = null;
            if (isConnectExcel)
            {
                //Вычисляем длину массива - колличество аргументов
                int dlina = 1;
                if (parArgsProcVba != null)
                    dlina = parArgsProcVba.Length + 1;
                //Фомируем массив аргументов
                object[] argsMacro = new object[dlina];
                argsMacro[0] = parNameProcVba;
                if (parNameWorkbook != null)
                    argsMacro[0] = parNameWorkbook + "!" + argsMacro[0];
                for (int i = 1; i < dlina; i++)
                    argsMacro[i] = parArgsProcVba[i - 1];
                //Запускаем VBA скрипт
                returnVal = oApplication.GetType().InvokeMember("Run",
                    BindingFlags.InvokeMethod, null, oApplication, argsMacro);
            }
            return returnVal;
        }

        /// <summary>
        /// Вызов членов объектной модели Екселя
        /// </summary>
        /// <param name="parInvokeType">Ссылка на вызываемый член</param>
        /// <param name="parNameMember">Имя вызываемого члена </param>
        /// <param name="parBindingFlags">Определяет флаги, управляющие связыванием, и метод, используемый при отражении для поиска членов и типов.</param>
        /// <returns>Объект Object, представляющий значение, возвращаемое указанным членом.</returns>
        public object ExcelInvokeMember(object parInvokeType, string parNameMember,
            BindingFlags parBindingFlags)
        {
            object returnVal = null;
            if (isConnectExcel)
            {
                returnVal = ExcelInvokeMember(parInvokeType, parNameMember,
                    parBindingFlags, null);
            }
            return returnVal;
        }
        /// <summary>
        /// Вызов членов объектной модели Екселя
        /// </summary>
        /// <param name="parInvokeType">Ссылка на вызываемый член</param>
        /// <param name="parNameMember">Имя вызываемого члена </param>
        /// <param name="parBindingFlags">Определяет флаги, управляющие связыванием, и метод, используемый при отражении для поиска членов и типов.</param>
        /// <param name="parArgs">Массив с аргументами, которые передаются вызываемому члену.</param>
        /// <returns>Объект Object, представляющий значение, возвращаемое указанным членом.</returns>
        public object ExcelInvokeMember(object parInvokeType, string parNameMember,
            BindingFlags parBindingFlags, object[] parArgs)
        {
            object returnVal = null;
            if (isConnectExcel)
            {
                returnVal = parInvokeType.GetType().InvokeMember(parNameMember, parBindingFlags,
                    null, parInvokeType, parArgs);
            }
            return returnVal;
        }
        #endregion

        #region Экпорт данных в Ексель

        #region ExportDataToSheets - экспорт данных в Excel через массив
        /// <summary>
        /// Экспорт данных из массива DataView в новую рабочую книгу Екселя
        /// </summary>
        /// <param name="parDvReports">DataView c данными для экспорта</param>
        /// <returns>Возвращает имя созданной рабочей книги с данными</returns>
        public string ExportDataToSheets(DataView parDvReports)
        {
            DataView[] dvArr = new DataView[1];
            dvArr[0] = parDvReports;
            return ExportDataToSheets(dvArr);
        }
        /// <summary>
        /// Экспорт данных из массива DataView в новую рабочую книгу Екселя
        /// </summary>
        /// <param name="parDvReports">Массив DataView c данными для экспорта</param>
        /// <returns>Возвращает имя созданной рабочей книги с данными</returns>
        public string ExportDataToSheets(DataView[] parDvReports)
        {
            string getWorkbookName = null;
            if (isConnectExcel)
            {
                //На некоторые команды VBA Excel выбрасывает свои диалоги
                //(предупреждения, запросы на подтверждения и т.д.)
                //Чтобы этого избежать, устанавливаем свойство DisplayAlerts
                //объекта Application равным False
                object[] args1 = new object[1];
                args1[0] = false;
                //Запоминаем как было:
                bool appDisplayAlerts = (bool)oApplication.GetType().InvokeMember("DisplayAlerts",
                    BindingFlags.GetProperty, null, oApplication, null);
                //Устанавливаем DisplayAlerts = False
                oApplication.GetType().InvokeMember("DisplayAlerts",
                    BindingFlags.SetProperty, null, oApplication, args1);

                object xlsWorkbookData = oWorkbooks.GetType().InvokeMember("Add",
                    BindingFlags.InvokeMethod, null, oWorkbooks, null);
                object xlsSheets = xlsWorkbookData.GetType().InvokeMember("Sheets",
                    BindingFlags.GetProperty, null, xlsWorkbookData, null);
                int xlsSheetCount = (int)xlsSheets.GetType().InvokeMember("Count",
                    BindingFlags.GetProperty, null, xlsSheets, null);

                //Для отслеживания индекса в рабочем массиве DataView
                int iNumberView = 0;
                //Для ссылки на рабочий лист
                object xlsSheet;
                //Параметры
                object[] args2 = new object[2];

                foreach (DataView dv in parDvReports)
                {
                    //Получаем ссылку на рабочий лист,
                    //если надо добавляем
                    if (iNumberView < xlsSheetCount)
                    {
                        args1[0] = ++iNumberView;
                        xlsSheet = xlsSheets.GetType().InvokeMember("Item",
                            BindingFlags.GetProperty, null, xlsSheets, args1);
                    }
                    else
                    {
                        iNumberView++;
                        xlsSheet = xlsSheets.GetType().InvokeMember("Add",
                            BindingFlags.InvokeMethod, null, xlsSheets, null);
                    }

                    //Переименовываем лист - ИмяЛиста = iNumberView+TableName
                    args1[0] = iNumberView.ToString() + "._" + dv.Table.TableName;
                    xlsSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                        null, xlsSheet, args1);

                    //Готовлю массив для передачи
                    object[,] dim = new object[dv.Count + 1, dv.Table.Columns.Count];
                    //В первую строку название колонок
                    for (int j = 0; j < dv.Table.Columns.Count; j++)
                        dim[0, j] = dv.Table.Columns[j].ColumnName;
                    //Далее сами данные
                    int k = 1;
                    foreach (DataRowView dr in dv)
                    {
                        for (int j = 0; j < dv.Table.Columns.Count; j++)
                        {
                            if (dr[j] is System.String)
                                dim[k, j] = "'" + dr[j];
                            else
                                dim[k, j] = dr[j];
                            if (dr[j] is System.Enum)
                                dim[k, j] = "'" + dr[j].ToString();
                        }
                        k++;
                    }

                    //Переносим данные на рабочий лист
                    dataToSheet(dim, xlsSheet);
                }
                //Удаляем лишние листы если они есть!
                if (iNumberView < xlsSheetCount)
                {
                    for (int i = iNumberView + 1; i <= xlsSheetCount; i++)
                    {
                        args1[0] = iNumberView + 1;
                        xlsSheet = xlsSheets.GetType().InvokeMember("Item",
                            BindingFlags.GetProperty, null, xlsSheets, args1);
                        xlsSheet.GetType().InvokeMember("Delete",
                            BindingFlags.InvokeMethod, null, xlsSheet, null);
                    }
                }

                //Восстанавливаем значение свойства DisplayAlerts
                args1[0] = appDisplayAlerts;
                oApplication.GetType().InvokeMember("DisplayAlerts",
                    BindingFlags.SetProperty, null, oApplication, args1);

                //Получаем имя рабочей книги с данными
                getWorkbookName = (string)xlsWorkbookData.GetType().InvokeMember("Name",
                    BindingFlags.GetProperty, null, xlsWorkbookData, null);

            }
            return getWorkbookName;
        }

        /// <summary>
        /// Экспорт массива на рабочий лист
        /// </summary>
        /// <param name="parDim">Массив для переноса</param>
        /// <param name="parXlsSheet">Ссылка рабочий лист</param>
        private void dataToSheet(object[,] parDim, object parXlsSheet)
        {
            //Получаем объекты Cells - начало и конец диапазона
            object[] args2 = new object[2];
            args2[0] = 1;
            args2[1] = 1;
            object oCells11 = parXlsSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args2[0] = parDim.GetLength(0);
            args2[1] = parDim.GetLength(1);
            object oCellsYX = parXlsSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args2[0] = oCells11;
            args2[1] = oCellsYX;
            //Получаем объект Range, на который положим данные
            object oRangeTable = parXlsSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, parXlsSheet, args2);

            //Экспортируем данные на лист
            object[] args1 = new object[1];
            args1[0] = parDim;
            oRangeTable.GetType().InvokeMember("Value", BindingFlags.SetProperty,
                null, oRangeTable, args1);

            //Получаем имя рабочего листа
            string sXlsSheetName = (string)parXlsSheet.GetType().InvokeMember("Name", BindingFlags.GetProperty,
                null, parXlsSheet, null);

            //Присвоим имя строке заголовка, содержащем название столбцов
            args2[0] = 1;
            args2[1] = parDim.GetLength(1);
            object oCells1X = parXlsSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args2[0] = oCells11;
            args2[1] = oCells1X;
            //Получаем объект Range, которому будем присваивать имя
            object oRangeColumnsName = parXlsSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args1 = new object[1];
            args1[0] = "ColName_" + sXlsSheetName;
            oRangeColumnsName.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                null, oRangeColumnsName, args1);

            //Присвоим имя диапазону данных
            args2[0] = 2;
            args2[1] = 1;
            object oCells21 = parXlsSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args2[0] = oCells21;
            args2[1] = oCellsYX;
            //Получаем объект Range, которому будем присваивать имя
            oRangeColumnsName = parXlsSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args1 = new object[1];
            args1[0] = "Data_" + sXlsSheetName;
            oRangeColumnsName.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                null, oRangeColumnsName, args1);
        }
        #endregion

        #region ExportDataViewToCsv - экспорт данных в csv-формат
        /// <summary>
        /// Процедура - экспорт данных из DataView
        /// в csv-файлик
        /// </summary>
        /// <param name="parNameCsv">путь к csv-файлику</param>
        /// <param name="parDv">DataView с данными, которые надо выгрузить</param>
        public static void ExportDataViewToCsv(string parNameCsv, DataView parDv)
        {
            ExportDataViewToCsv(parNameCsv, parDv, null);
        }

        /// <summary>
        /// Процедура - экспорт данных из DataView
        /// в csv-файлик
        /// </summary>
        /// <param name="parNameCsv">путь к csv-файлику</param>
        /// <param name="parDv">DataView с данными, которые надо выгрузить</param>
        /// <param name="parColName">Массив названий выборочных столбцов (ColumnName)</param>
        public static void ExportDataViewToCsv(string parNameCsv, DataView parDv, string[] parColName)
        {
            StreamWriter fs = new StreamWriter(parNameCsv, false, Encoding.GetEncoding(1251));
            string repRow;

            if (parColName == null)
            {
                parColName = new string[parDv.Table.Columns.Count];
                for (int i = 0; i < parDv.Table.Columns.Count; i++)
                    parColName[i] = parDv.Table.Columns[i].ColumnName;
            }

            //В первую строку положу название полей из DataTable
            repRow = "";
            for (int j = 0; j < parColName.Length; j++)
                repRow += parColName[j] + ";";
            fs.WriteLine(repRow);

            //Далее заполняю данными из DataView
            string cells;
            for (int i = 0; i < parDv.Count; i++)
            {
                repRow = "";
                for (int j = 0; j < parColName.Length; j++)
                {
                    cells = parDv[i][parColName[j]].ToString().Trim();
                    if (cells.IndexOf(";") >= 0)
                    {
                        cells = cells.Replace("\"", "\"\"");
                        cells = "\"" + cells + "\"";
                    }
                    repRow += cells + ";";
                }
                fs.WriteLine(repRow);
            }
            fs.Close();
        }
        #endregion

        #endregion

        #region Импорт данных

        public static DataTable ImportData(System.IO.FileInfo fileInfo)
        {
            DataTable dt = null;
            string ext = fileInfo.Extension.ToLower();
            switch (ext)
            { 
                case ".xls":
                    dt = GetExcelWorkSheet(fileInfo.FullName, 0);
                    break;

                case ".dbf":
                    dt = ImportDBF(fileInfo.DirectoryName, fileInfo.Name);
                    break;
            }

            return dt;
        }

        public static List<DataTable> GetExcelWorkSheets(string filePath)
        {
            var list = new List<DataTable>();

            OleDbConnection excelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");
            OleDbCommand excelCommand = new OleDbCommand();
            excelCommand.Connection = excelConnection;
            OleDbDataAdapter excelAdapter = new OleDbDataAdapter(excelCommand);
            excelConnection.Open();
            DataTable excelSheets = excelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            List<string> tables = new List<string>();

            foreach (DataRow dr in excelSheets.Rows)
            {
                string spreadSheetName = dr["TABLE_NAME"].ToString();
                int length = spreadSheetName.IndexOf('$') + (spreadSheetName.Contains(" ") ? 2 : 1);
                if (spreadSheetName.Length == length)
                {
                    DataTable dt = GetExcelWorkSheet(spreadSheetName, excelCommand, excelAdapter);
                    list.Add(dt);
                }
                tables.Add(spreadSheetName);
            }

            excelConnection.Close();

            return list;
        }

        private static DataTable GetExcelWorkSheet(string spreadSheetName,
                                                   OleDbCommand excelCommand, OleDbDataAdapter excelAdapter)
        {
            DataSet ExcelDataSet = new DataSet();
            excelCommand.CommandText = @"SELECT * FROM [" + spreadSheetName + "]";
            excelAdapter.Fill(ExcelDataSet);

            OleDbDataReader reader = excelCommand.ExecuteReader();

            DataTable tab = ExcelDataSet.Tables[0];
            DataTable dt = new DataTable(spreadSheetName);
            foreach (DataColumn col in tab.Columns)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = col.ColumnName;
                dc.Caption = col.Caption;
                dc.DataType = Type.GetType("System.String");
                dt.Columns.Add(dc);
            }

            while (reader.Read())
            {
                DataRow row = dt.NewRow();
                foreach (DataColumn col in tab.Columns)
                {
                    try
                    {
                        row[col.Ordinal] = Convert.ToString(reader.GetValue(col.Ordinal));
                    }
                    catch { }
                }
                dt.Rows.Add(row);
            }
            reader.Close();

            return dt;
        }

        public static DataTable GetExcelWorkSheet(string filePath, int workSheetNumber)
        {
            OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=Excel 8.0;");
            OleDbCommand ExcelCommand = new OleDbCommand();
            ExcelCommand.Connection = ExcelConnection;
            OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
            ExcelConnection.Open();
            DataTable ExcelSheets = ExcelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            string SpreadSheetName = "[" + ExcelSheets.Rows[workSheetNumber]["TABLE_NAME"].ToString() + "]";
            DataSet ExcelDataSet = new DataSet();
            ExcelCommand.CommandText = @"SELECT * FROM " + SpreadSheetName;
            ExcelAdapter.Fill(ExcelDataSet);

            OleDbDataReader reader = ExcelCommand.ExecuteReader();

            DataTable tab = ExcelDataSet.Tables[0];
            DataTable t = new DataTable("Results");
            foreach (DataColumn col in tab.Columns)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = col.ColumnName;
                dc.Caption = col.Caption;
                dc.DataType = Type.GetType("System.String");
                t.Columns.Add(dc);
            }

            while (reader.Read())
            {
                DataRow row = t.NewRow();
                foreach (DataColumn col in tab.Columns)
                {
                    try
                    {
                        row[col.Ordinal] = Convert.ToString(reader.GetValue(col.Ordinal));
                    }
                    catch { }
                }
                t.Rows.Add(row);
            }
            reader.Close();

            ExcelConnection.Close();
            return t;
        }

        public static DataTable ImportDBF(string filePath, string fileName)
        {
            string tmpName = "tmp" + fileName;
            string tmpPath = Path.GetTempPath() + "tmp" + fileName;

            byte[] file = File.ReadAllBytes(filePath + "\\" + fileName);
            file[29] = 101;
            File.WriteAllBytes(tmpPath, file);

            System.Data.Odbc.OdbcConnection con = new System.Data.Odbc.OdbcConnection();
            con.ConnectionString = @"Driver={Microsoft dBASE Driver (*.dbf)};DriverID=277;Dbq=" + Path.GetTempPath() + ";";
            con.Open();
            DataTable dt = new DataTable();
            System.Data.Odbc.OdbcCommand cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT * FROM " + tmpName;
            dt.Load(cmd.ExecuteReader());

            File.Delete(tmpPath);

            /*
            // это одна из версий, пусть полежит, вдруг то, что сверху, окажется еще более косячным ))
            OleDbConnection con = new OleDbConnection(@"Provider=VFPOLEDB.1;Data Source=" + filePath + ";");
            OleDbCommand cmd = new OleDbCommand(@"SELECT * FROM '" + fileName + "'", con);
            con.Open();
            OleDbDataAdapter DBFDataAdapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            OleDbDataReader rdr = cmd.ExecuteReader();
            //DBFDataAdapter.Fill(dt);
            
            bool isSchemaCreate = false;
            DataRow row = null;
            while (rdr.Read())
            {
                int filedCount = rdr.FieldCount;

                if (!isSchemaCreate)
                {
                    for (int i = 0; i < filedCount; i++)
                    {
                        dt.Columns.Add("Column " + (i + 1).ToString(), typeof(string));
                    }
                    isSchemaCreate = true;
                }

                row = dt.NewRow();
                for (int i = 0; i < filedCount; i++)
                {
                    object o = rdr[i];
                    string str = EncodingConvert(rdr[i].ToString(), Encoding.GetEncoding(866), Encoding.Default);
                    //string str = EncodingConvert(rdr[i].ToString(), Encoding.GetEncoding(1251), Encoding.Default);
                    row[i] = str;
                }
                dt.Rows.Add(row);
            }

            con.Close();
            */

            return dt;
        }

        public static string EncodingConvert(string value, Encoding src, Encoding trg)
        {
            Decoder dec = src.GetDecoder();
            byte[] ba = trg.GetBytes(value);
            int len = dec.GetCharCount(ba, 0, ba.Length);
            char[] ca = new char[len];
            dec.GetChars(ba, 0, ba.Length, ca, 0);
            return new string(ca);
        }

        #endregion

        #endregion

        #region Завершение и освобождение ресурсов
        /// <summary>
        /// Рабочая книга-призрак.
        /// Наличие живой ссылки на нее означает,
        /// что пользователь не закрыл Ексель с момента последнего обращения
        /// </summary>
        private object _emptyBook = null;
        /// <summary>
        /// Создание рабочей книги-призрака
        /// </summary>
        private void emptyBook()
        {
            //Создаем новую рабочую книгу
            _emptyBook = oWorkbooks.GetType().InvokeMember("Add",
                BindingFlags.InvokeMethod, null, oWorkbooks, null);
            //Делаем ее невидимой
            object[] args1 = new object[1];
            args1[0] = true;
            _emptyBook.GetType().InvokeMember("IsAddin",
                BindingFlags.SetProperty, null, _emptyBook, args1);
            //Выставляем Saved в True, чтобы при закрытии Екселя
            //пользователь не получал предложений о сохранеии изменений в
            //документе о наличии которого он и не подозревает
            _emptyBook.GetType().InvokeMember("Saved",
                BindingFlags.SetProperty, null, _emptyBook, args1);
        }

        /// <summary>
        /// Признак того, что объект "завершен"
        /// </summary>
        private bool _isDisposed = false;
        /// <summary>
        ///  Закрытие и высвобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                //Выставляем культуру прежнюю
                if (_oldCI != null)
                    Thread.CurrentThread.CurrentCulture = _oldCI;
                //Освобождаем ресурсы
                ClearComReferens();
                // Устанавливаем флаг - объект "завершен"
                _isDisposed = true;
                // Подавляем вызов деструктора
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~ExcelHelper()
        {
            Dispose();
        }

        /// <summary>
        /// Отключение от Excel
        /// </summary>
        public void ClearComReferens()
        {
            if (_oExcel != null)
            {
                //Выставляем культуру прежнюю
                Thread.CurrentThread.CurrentCulture = _oldCI;
                // Освобождаем ссылку на Com интерфейс Екселя
                Marshal.ReleaseComObject(_oExcel);
                _oExcel = null;
                _oApplication = null;
                _oWorkbooks = null;
                _emptyBook = null;
                _isStartExcel = false;
                // Очищаем память
                GC.GetTotalMemory(true);
            }
        }
        #endregion

    }
}
