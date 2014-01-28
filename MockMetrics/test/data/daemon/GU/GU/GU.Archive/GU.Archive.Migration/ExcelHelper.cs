/*
 *  ����� ����������� ����� ������: http://contest2005.gotdotnet.ru/Request/Tools/UtilitiesLib/209099.aspx
 *
 *  UPD: ...�� ����������� �������
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
    /// ����� - ����-������� ��� �������
    /// </summary>
    public class ExcelHelper : IDisposable
    {

        #region ���� � ��������

        #region ������ �� ������ Excel
        /// <summary>
        /// ������� ����, ��� ��������� ������ ������ �
        /// ���������� ������ ������
        /// </summary>
        private bool _isStartExcel = false;
        /// <summary>
        /// ������ �� ������ Excel
        /// </summary>
        private object _oExcel = null;
        /// <summary>
        /// ��� ����������� ������� ��������.
        /// ��� ����� ������ ���: http://support.microsoft.com/kb/320369
        /// </summary>
        private CultureInfo _oldCI;
        /// <summary>
        /// ������ �� ������ Excel
        /// </summary>
        protected object oExcel
        {
            get
            {
                if (_isStartExcel && !isConnectExcel)
                {
                    //���� ��������� ��� ������ ������ � ������ �� ���� ������� ���
                    //� ��� ����������� ��������� �����-�������, �.�. ������������ ������ ������,
                    //� ������� ��� ��������� (������������ � ������ ���������, ��� ���������� ��� ������ ���� 2003),
                    //����� ��������� ���.
                    ClearComReferens();
                }
                if (_oExcel == null)
                {
                    string sAppProgID = "Excel.Application";
                    // �������� ������ �� ��������� IDispatch
                    Type tExcelObj = Type.GetTypeFromProgID(sAppProgID);
                    // ��������� Excel
                    _oExcel = Activator.CreateInstance(tExcelObj);
                    //���������� �������� ������� �������� ������
                    _oldCI = Thread.CurrentThread.CurrentCulture;
                    //���������� � ������������
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    //��������� � ������ ������� ����� � ������ �� ���������.
                    //�� �� ������� ����� ������ �������� �� ���-������ ������.
                    //��� ����� �������� � ��������� ��������� � �������� ������ ���� 2003.
                    //� ����� ������� �� ������ �������, �� ����� ������� � �� �����.
                    emptyBook();
                    //������������� ��������� ������� ������
                    _isStartExcel = true;
                }
                return _oExcel;
            }
        }
        #endregion
        #region	������ �� ������ Application
        /// <summary>
        /// ������ �� ������ Application
        /// </summary>
        private object _oApplication;
        /// <summary>
        /// ������ �� ������ Application
        /// </summary>
        protected object oApplication
        {
            get
            {
                if (_oApplication == null)
                {
                    //�������� ������ �� ������ Application
                    _oApplication = oExcel.GetType().InvokeMember("Application",
                        BindingFlags.GetProperty, null, oExcel, null);
                }
                return _oApplication;
            }
        }
        #endregion
        #region ������ �� ������ Workbooks
        /// <summary>
        /// ������ �� ������ Workbooks
        /// </summary>
        private object _oWorkbooks;
        /// <summary>
        /// ������ �� ������ Workbooks
        /// </summary>
        protected object oWorkbooks
        {
            get
            {
                if (_oWorkbooks == null)
                {
                    //�������� ������ �� ��������� ������� ���� -
                    //������ Workbooks
                    _oWorkbooks = oExcel.GetType().InvokeMember("Workbooks",
                        BindingFlags.GetProperty, null, oExcel, null);
                }
                return _oWorkbooks;
            }
        }
        #endregion

        #region ��������� Excel
        /// <summary> 
        /// ��������� Excel
        /// </summary>
        public bool Visible
        {
            get
            {
                if (!_isStartExcel)
                    return false;
                try
                {
                    //�������� �������� �������� Visible ������ Application
                    return (bool)oExcel.GetType().InvokeMember("Visible",
                        BindingFlags.GetProperty, null, oApplication, null);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    //���� ������ �� ��������,
                    //�������� ������ �� ����, �������, ��� � ���� ������ �� ���������
                    ClearComReferens();
                }
                return false;
            }
            set
            {
                //������������� �������� �������� Visible ������ Application
                object[] args1 = new object[1];
                args1[0] = value;
                oExcel.GetType().InvokeMember("Visible",
                    BindingFlags.SetProperty, null, oApplication, args1);
            }
        }
        #endregion

        #region ������� ���������� ������
        /// <summary>
        /// ������� ���������� ������ �� ������
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
                        //���� ������ �� ��������� ����� ����, �������� �� �������.
                        //��������� �������� �������� Name, ���� ��� ���� �� ��� ���������.
                        _emptyBook.GetType().InvokeMember("Name",
                            BindingFlags.GetProperty, null, _emptyBook, null);
                        return true;
                    }
                    else
                        return false;
                }
                catch
                {
                    //���� ��������� ������, ������� ������ ������.
                    ClearComReferens();
                    return false;
                }
            }
        }
        #endregion

        #endregion

        #region ������

        #region �������� � �������� ������� ����
        /// <summary>
        /// �������� ������� �����
        /// </summary>
        /// <returns>���������� ��� ��������� ������� �����</returns>
        public string WorkbookAdd()
        {
            return WorkbookAdd(null);
        }

        /// <summary>
        /// �������� ������� �����
        /// </summary>
        /// <param name="parTemplate">���� � �������</param>
        /// <returns>���������� ��� ��������� ������� �����</returns>
        public string WorkbookAdd(string parTemplate)
        {
            string getWorkbookName = null;
            if (isConnectExcel)
            {
                //��������� ��������� ������ ��� ������
                //Workbooks.Add
                object[] args1 = null;
                if (parTemplate != null)
                {
                    args1 = new object[1];
                    args1[0] = parTemplate;
                }
                //������� ����� ������� �����
                object addWorkbook = oWorkbooks.GetType().InvokeMember("Add",
                    BindingFlags.InvokeMethod, null, oWorkbooks, args1);
                //�������� �� ���
                getWorkbookName = (string)addWorkbook.GetType().InvokeMember("Name",
                    BindingFlags.GetProperty, null, addWorkbook, null);
            }
            return getWorkbookName;
        }
        /// <summary>
        /// �������� ������� �����
        /// </summary>
        /// <param name="parFullPath">���� � ����� ������� �����</param>
        /// <returns>���������� ��� ������� �����</returns>
        public string WorkbookOpen(string parFullPath)
        {
            string getWorkbookName = null;
            if (isConnectExcel)
            {
                //��������� ��������� ������ ��� ������
                //Workbooks.Open
                object[] args1 = new object[1];
                args1[0] = parFullPath;
                //��������� ������� �����
                object openWorkbook = oWorkbooks.GetType().InvokeMember("Open",
                    BindingFlags.InvokeMethod, null, oWorkbooks, args1);
                //�������� �� ���
                getWorkbookName = (string)openWorkbook.GetType().InvokeMember("Name",
                    BindingFlags.GetProperty, null, openWorkbook, null);
            }
            return getWorkbookName;
        }
        #endregion

        #region ������ VBA �������� ��� �������, ����� ������ ��������� ������ ������
        /// <summary>
        /// ������ VBA ��������� ��� �������
        /// </summary>
        /// <param name="parNameProcVba">��� VBA ��������� ��� �������</param>
        /// <returns>����������� �������� (���� ��� ������� ����)</returns>
        public object RunVbaProc(string parNameProcVba)
        {
            object returnVal = null;
            returnVal = RunVbaProc(parNameProcVba, null, null);
            return returnVal;
        }
        /// <summary>
        /// ������ VBA ��������� ��� �������
        /// </summary>
        /// <param name="parNameProcVba">��� VBA ��������� ��� �������</param>
        /// <param name="parArgsProcVba">��������� ��� VBA ��������� ��� �������</param>
        /// <returns>����������� �������� (���� ��� ������� ����)</returns>
        public object RunVbaProc(string parNameProcVba, object[] parArgsProcVba)
        {
            object returnVal = null;
            returnVal = RunVbaProc(parNameProcVba, null, parArgsProcVba);
            return returnVal;
        }
        /// <summary>
        /// ������ VBA ��������� ��� �������
        /// </summary>
        /// <param name="parNameProcVba">��� VBA ��������� ��� �������</param>
        /// <param name="parNameWorkbook">��� ������� �����, ���������� VBA ������</param>
        /// <returns>����������� �������� (���� ��� ������� ����)</returns>
        public object RunVbaProc(string parNameProcVba, string parNameWorkbook)
        {
            object returnVal = null;
            returnVal = RunVbaProc(parNameProcVba, parNameWorkbook, null);
            return returnVal;
        }
        /// <summary>
        /// ������ VBA ��������� ��� �������
        /// </summary>
        /// <param name="parNameProcVba">��� VBA ��������� ��� �������</param>
        /// <param name="parNameWorkbook">��� ������� �����, ���������� VBA ������</param>
        /// <param name="parArgsProcVba">��������� ��� VBA ��������� ��� �������</param>
        /// <returns>����������� �������� (���� ��� ������� ����)</returns>
        public object RunVbaProc(string parNameProcVba, string parNameWorkbook,
            object[] parArgsProcVba)
        {
            object returnVal = null;
            if (isConnectExcel)
            {
                //��������� ����� ������� - ����������� ����������
                int dlina = 1;
                if (parArgsProcVba != null)
                    dlina = parArgsProcVba.Length + 1;
                //�������� ������ ����������
                object[] argsMacro = new object[dlina];
                argsMacro[0] = parNameProcVba;
                if (parNameWorkbook != null)
                    argsMacro[0] = parNameWorkbook + "!" + argsMacro[0];
                for (int i = 1; i < dlina; i++)
                    argsMacro[i] = parArgsProcVba[i - 1];
                //��������� VBA ������
                returnVal = oApplication.GetType().InvokeMember("Run",
                    BindingFlags.InvokeMethod, null, oApplication, argsMacro);
            }
            return returnVal;
        }

        /// <summary>
        /// ����� ������ ��������� ������ ������
        /// </summary>
        /// <param name="parInvokeType">������ �� ���������� ����</param>
        /// <param name="parNameMember">��� ����������� ����� </param>
        /// <param name="parBindingFlags">���������� �����, ����������� �����������, � �����, ������������ ��� ��������� ��� ������ ������ � �����.</param>
        /// <returns>������ Object, �������������� ��������, ������������ ��������� ������.</returns>
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
        /// ����� ������ ��������� ������ ������
        /// </summary>
        /// <param name="parInvokeType">������ �� ���������� ����</param>
        /// <param name="parNameMember">��� ����������� ����� </param>
        /// <param name="parBindingFlags">���������� �����, ����������� �����������, � �����, ������������ ��� ��������� ��� ������ ������ � �����.</param>
        /// <param name="parArgs">������ � �����������, ������� ���������� ����������� �����.</param>
        /// <returns>������ Object, �������������� ��������, ������������ ��������� ������.</returns>
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

        #region ������ ������ � ������

        #region ExportDataToSheets - ������� ������ � Excel ����� ������
        /// <summary>
        /// ������� ������ �� ������� DataView � ����� ������� ����� ������
        /// </summary>
        /// <param name="parDvReports">DataView c ������� ��� ��������</param>
        /// <returns>���������� ��� ��������� ������� ����� � �������</returns>
        public string ExportDataToSheets(DataView parDvReports)
        {
            DataView[] dvArr = new DataView[1];
            dvArr[0] = parDvReports;
            return ExportDataToSheets(dvArr);
        }
        /// <summary>
        /// ������� ������ �� ������� DataView � ����� ������� ����� ������
        /// </summary>
        /// <param name="parDvReports">������ DataView c ������� ��� ��������</param>
        /// <returns>���������� ��� ��������� ������� ����� � �������</returns>
        public string ExportDataToSheets(DataView[] parDvReports)
        {
            string getWorkbookName = null;
            if (isConnectExcel)
            {
                //�� ��������� ������� VBA Excel ����������� ���� �������
                //(��������������, ������� �� ������������� � �.�.)
                //����� ����� ��������, ������������� �������� DisplayAlerts
                //������� Application ������ False
                object[] args1 = new object[1];
                args1[0] = false;
                //���������� ��� ����:
                bool appDisplayAlerts = (bool)oApplication.GetType().InvokeMember("DisplayAlerts",
                    BindingFlags.GetProperty, null, oApplication, null);
                //������������� DisplayAlerts = False
                oApplication.GetType().InvokeMember("DisplayAlerts",
                    BindingFlags.SetProperty, null, oApplication, args1);

                object xlsWorkbookData = oWorkbooks.GetType().InvokeMember("Add",
                    BindingFlags.InvokeMethod, null, oWorkbooks, null);
                object xlsSheets = xlsWorkbookData.GetType().InvokeMember("Sheets",
                    BindingFlags.GetProperty, null, xlsWorkbookData, null);
                int xlsSheetCount = (int)xlsSheets.GetType().InvokeMember("Count",
                    BindingFlags.GetProperty, null, xlsSheets, null);

                //��� ������������ ������� � ������� ������� DataView
                int iNumberView = 0;
                //��� ������ �� ������� ����
                object xlsSheet;
                //���������
                object[] args2 = new object[2];

                foreach (DataView dv in parDvReports)
                {
                    //�������� ������ �� ������� ����,
                    //���� ���� ���������
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

                    //��������������� ���� - �������� = iNumberView+TableName
                    args1[0] = iNumberView.ToString() + "._" + dv.Table.TableName;
                    xlsSheet.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                        null, xlsSheet, args1);

                    //������� ������ ��� ��������
                    object[,] dim = new object[dv.Count + 1, dv.Table.Columns.Count];
                    //� ������ ������ �������� �������
                    for (int j = 0; j < dv.Table.Columns.Count; j++)
                        dim[0, j] = dv.Table.Columns[j].ColumnName;
                    //����� ���� ������
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

                    //��������� ������ �� ������� ����
                    dataToSheet(dim, xlsSheet);
                }
                //������� ������ ����� ���� ��� ����!
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

                //��������������� �������� �������� DisplayAlerts
                args1[0] = appDisplayAlerts;
                oApplication.GetType().InvokeMember("DisplayAlerts",
                    BindingFlags.SetProperty, null, oApplication, args1);

                //�������� ��� ������� ����� � �������
                getWorkbookName = (string)xlsWorkbookData.GetType().InvokeMember("Name",
                    BindingFlags.GetProperty, null, xlsWorkbookData, null);

            }
            return getWorkbookName;
        }

        /// <summary>
        /// ������� ������� �� ������� ����
        /// </summary>
        /// <param name="parDim">������ ��� ��������</param>
        /// <param name="parXlsSheet">������ ������� ����</param>
        private void dataToSheet(object[,] parDim, object parXlsSheet)
        {
            //�������� ������� Cells - ������ � ����� ���������
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
            //�������� ������ Range, �� ������� ������� ������
            object oRangeTable = parXlsSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, parXlsSheet, args2);

            //������������ ������ �� ����
            object[] args1 = new object[1];
            args1[0] = parDim;
            oRangeTable.GetType().InvokeMember("Value", BindingFlags.SetProperty,
                null, oRangeTable, args1);

            //�������� ��� �������� �����
            string sXlsSheetName = (string)parXlsSheet.GetType().InvokeMember("Name", BindingFlags.GetProperty,
                null, parXlsSheet, null);

            //�������� ��� ������ ���������, ���������� �������� ��������
            args2[0] = 1;
            args2[1] = parDim.GetLength(1);
            object oCells1X = parXlsSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args2[0] = oCells11;
            args2[1] = oCells1X;
            //�������� ������ Range, �������� ����� ����������� ���
            object oRangeColumnsName = parXlsSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args1 = new object[1];
            args1[0] = "ColName_" + sXlsSheetName;
            oRangeColumnsName.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                null, oRangeColumnsName, args1);

            //�������� ��� ��������� ������
            args2[0] = 2;
            args2[1] = 1;
            object oCells21 = parXlsSheet.GetType().InvokeMember("Cells", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args2[0] = oCells21;
            args2[1] = oCellsYX;
            //�������� ������ Range, �������� ����� ����������� ���
            oRangeColumnsName = parXlsSheet.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                null, parXlsSheet, args2);
            args1 = new object[1];
            args1[0] = "Data_" + sXlsSheetName;
            oRangeColumnsName.GetType().InvokeMember("Name", BindingFlags.SetProperty,
                null, oRangeColumnsName, args1);
        }
        #endregion

        #region ExportDataViewToCsv - ������� ������ � csv-������
        /// <summary>
        /// ��������� - ������� ������ �� DataView
        /// � csv-������
        /// </summary>
        /// <param name="parNameCsv">���� � csv-�������</param>
        /// <param name="parDv">DataView � �������, ������� ���� ���������</param>
        public static void ExportDataViewToCsv(string parNameCsv, DataView parDv)
        {
            ExportDataViewToCsv(parNameCsv, parDv, null);
        }

        /// <summary>
        /// ��������� - ������� ������ �� DataView
        /// � csv-������
        /// </summary>
        /// <param name="parNameCsv">���� � csv-�������</param>
        /// <param name="parDv">DataView � �������, ������� ���� ���������</param>
        /// <param name="parColName">������ �������� ���������� �������� (ColumnName)</param>
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

            //� ������ ������ ������ �������� ����� �� DataTable
            repRow = "";
            for (int j = 0; j < parColName.Length; j++)
                repRow += parColName[j] + ";";
            fs.WriteLine(repRow);

            //����� �������� ������� �� DataView
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

        #region ������ ������

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
            // ��� ���� �� ������, ����� �������, ����� ��, ��� ������, �������� ��� ����� �������� ))
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

        #region ���������� � ������������ ��������
        /// <summary>
        /// ������� �����-�������.
        /// ������� ����� ������ �� ��� ��������,
        /// ��� ������������ �� ������ ������ � ������� ���������� ���������
        /// </summary>
        private object _emptyBook = null;
        /// <summary>
        /// �������� ������� �����-��������
        /// </summary>
        private void emptyBook()
        {
            //������� ����� ������� �����
            _emptyBook = oWorkbooks.GetType().InvokeMember("Add",
                BindingFlags.InvokeMethod, null, oWorkbooks, null);
            //������ �� ���������
            object[] args1 = new object[1];
            args1[0] = true;
            _emptyBook.GetType().InvokeMember("IsAddin",
                BindingFlags.SetProperty, null, _emptyBook, args1);
            //���������� Saved � True, ����� ��� �������� ������
            //������������ �� ������� ����������� � ��������� ��������� �
            //��������� � ������� �������� �� � �� �����������
            _emptyBook.GetType().InvokeMember("Saved",
                BindingFlags.SetProperty, null, _emptyBook, args1);
        }

        /// <summary>
        /// ������� ����, ��� ������ "��������"
        /// </summary>
        private bool _isDisposed = false;
        /// <summary>
        ///  �������� � ������������� ��������
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                //���������� �������� �������
                if (_oldCI != null)
                    Thread.CurrentThread.CurrentCulture = _oldCI;
                //����������� �������
                ClearComReferens();
                // ������������� ���� - ������ "��������"
                _isDisposed = true;
                // ��������� ����� �����������
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// ����������
        /// </summary>
        ~ExcelHelper()
        {
            Dispose();
        }

        /// <summary>
        /// ���������� �� Excel
        /// </summary>
        public void ClearComReferens()
        {
            if (_oExcel != null)
            {
                //���������� �������� �������
                Thread.CurrentThread.CurrentCulture = _oldCI;
                // ����������� ������ �� Com ��������� ������
                Marshal.ReleaseComObject(_oExcel);
                _oExcel = null;
                _oApplication = null;
                _oWorkbooks = null;
                _emptyBook = null;
                _isStartExcel = false;
                // ������� ������
                GC.GetTotalMemory(true);
            }
        }
        #endregion

    }
}
