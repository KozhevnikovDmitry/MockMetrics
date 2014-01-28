using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Common.Types.Exceptions;

namespace GU.MZ.Import
{
    public class ImportException : BLLException
    {
        public DataRow Row { get; set; }

        public string ParseString { get; set; }

        public ImportException(string message, Exception ex)
            :base(string.Format("Ошибка импорта данных: {0} \n {1}", message, ex), ex)
        {
            
        }

        public ImportException(string message)
            : base(string.Format("Ошибка импорта данных: {0}", message))
        {
            
        }

        public override string Message
        {
            get
            {
                var msg = base.Message;

                if (!string.IsNullOrEmpty(ParseString))
                {
                    msg += string.Format("\n Ошибка разбора строки [{0}]", ParseString);
                }

                if (Row != null)
                {
                    msg += string.Format("\n Ошибка разбора строки таблицы номер [{0}]", Row.Table.Rows.IndexOf(Row));
                }

                return msg;
            }
        }

        private string ArrayToString(IEnumerable<object> array)
        {
            return array.Aggregate(string.Empty, (current, item) => current + string.Format("[{0}]", item));
        }
    }
}