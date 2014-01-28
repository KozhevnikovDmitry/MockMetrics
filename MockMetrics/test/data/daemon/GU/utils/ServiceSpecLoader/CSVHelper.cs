using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace ServiceSpecLoader
{
    public class CSVHelper
    {
        // взято из http://stackoverflow.com/questions/1008556/c-sharp-export-listview-to-csv

        public static void ExportDatatableToCSV(DataTable dt, string filePath, Boolean allowCreateDir, Boolean allowEmptyFile)
        {
            if (!allowEmptyFile && dt.Rows.Count == 0)
                return;

            StringBuilder result = new StringBuilder();

            FormatCSVRow(result, dt.Columns.Count, i => true, i => dt.Columns[i].ColumnName);

            foreach (DataRow row in dt.Rows)
                FormatCSVRow(result, dt.Columns.Count, i => true, i => row[i].ToString());

            if (allowCreateDir)
            {
                string dirPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, result.ToString(), Encoding.UTF8);
        }


        private static void FormatCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                if (!isColumnNeeded(i))
                    continue;

                if (!isFirstTime)
                    result.Append(";");
                isFirstTime = false;

                result.Append(String.Format("\"{0}\"", columnValue(i)));
            }
            result.AppendLine();
        }
    }
}
