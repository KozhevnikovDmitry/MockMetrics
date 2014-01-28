using System;
using System.Data;
using System.IO;
using Excel;
using NUnit.Framework;

namespace GU.MZ.Import.Tests.Tests
{
#if Integration
    [TestFixture]
#else
    [TestFixture(Ignore = true)]
#endif
    public class RegistrStructureTests
    {
        [Test]
        public void PrintDataTxtTest()
        {
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var txt = Path.Combine(desktop, "med.txt");

            if (File.Exists(txt))
            {
                File.Delete(txt);
            }

            using (var stream = File.Open(Path.Combine(desktop, "med.xls"), FileMode.Open, FileAccess.Read))
            {
                var excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                DataSet data = excelReader.AsDataSet();
                excelReader.Close();
                var table = data.Tables[0];

                foreach (DataRow row in table.Rows)
                {
                    File.AppendAllText(txt, "Строка" + table.Rows.IndexOf(row) + ";");
                    for (int i = 0; i < row.ItemArray.Length; i++)
                    {
                        string itemView = string.Format("[[{0}][{1}]]; ", i, row.ItemArray[i]);
                        File.AppendAllText(txt, itemView.Replace('\n', ' '));
                    }
                    File.AppendAllText(txt, "\n");
                }
            }
        }
    }
}