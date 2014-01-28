using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace ServiceSpecLoader
{
    public class SQLScriptHelper
    {
        public static void ExportDatasetToSqlScript(DataSet ds, int serviceId, string[] tableNames, Boolean deleteStatements, Boolean commitStatement, string note, string filePath, Boolean allowCreateDir, Boolean allowEmptyFile)
        {
            StringBuilder result = new StringBuilder();
            Boolean hasData = false;

            result.AppendLine("/*==========================================================================================");
            result.AppendLine(note);
            result.AppendLine("==========================================================================================*/");
            foreach (string tableName in tableNames)
            {
                if (ds.Tables.Contains(tableName))
                    result.AppendLine(String.Format("delete from {0} where service_id = {1};", tableName, serviceId));
            }
            result.AppendLine("--==============================");

            foreach (DataTable dt in ds.Tables)
            {
                if (tableNames.Contains(dt.TableName))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        StringBuilder values = new StringBuilder();
                        hasData = true;
                        result.Append(String.Format("insert into {0} (", dt.TableName));
                        for (var i = 0; i < dt.Columns.Count; i++)
                        {
                            if (i != 0)
                            {
                                result.Append(", ");
                                values.Append(", ");
                            }
                            result.Append(dt.Columns[i].ColumnName);
                            values.Append(FormatValue(row[i]));
                        }
                        result.AppendLine(String.Format(") values ({0});", values.ToString()));
                    }
                    result.AppendLine("------------------------------");
                }
            }
            if (commitStatement)
                result.AppendLine("commit;");

            result.AppendLine();
            result.AppendLine();

            if (!allowEmptyFile && !hasData)
                return;

            if (allowCreateDir)
            {
                string dirPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
            }

            File.WriteAllText(filePath, result.ToString());
        }


        private static string FormatValue(int val)
        {
            return val.ToString();
        }

        private static string FormatValue(decimal val)
        {
            return val.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
        
        private static string FormatValue(object val)
        {
            if (val.Equals(DBNull.Value))
                return "NULL";
            if (val is int)
                return FormatValue((int)val);
            if (val is decimal)
                return FormatValue((decimal) val);

            return "'"+val.ToString().Replace("'","''")+"'";
        }
    }
}
