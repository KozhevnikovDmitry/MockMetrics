using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using GU.Trud.BL.Export.DataModel;
using GU.Trud.BL.Export.Interface;

namespace GU.Trud.BL.Export
{
    /// <summary>
    /// Класс осуществляющий выгрузку данных в dbase3(DBF) файл.
    /// </summary>
    internal class DbaseToFileDumper : IToFileDumper
    {
        private OleDbCommand _currentCommand;

        /// <summary>
        /// Осуществляет выгрузку данных в dbase3(DBF) файл.
        /// </summary>
        /// <param name="data">Данные для выгрузки</param>
        /// <param name="filename">Имя файла</param>
        public void Dump(List<IToItemArray> data, string filename)
        {
            using (OleDbConnection connection = GetConnection(Path.GetDirectoryName(filename)))
            {
                connection.Open();
                string commonCommandText = string.Format("INSERT INTO {0} {1} ", Path.GetFileName(filename), WrapItems(data.First().ToItemNameArray()));
                foreach (var row in data)
                {
                    string rowInsertCommand = string.Format("{0} VALUES {1}", commonCommandText, WrapItems(ParseValues(row.ToItemArray())));
                    _currentCommand = new OleDbCommand(rowInsertCommand, connection);
                    _currentCommand.ExecuteNonQuery();
                }
                connection.Close();
            }
            _currentCommand = null;
        }

        /// <summary>
        /// Отменяет процедуру выгрузки данных.
        /// </summary>
        public void CancelDump()
        {
            try
            {
                if (_currentCommand != null)
                {
                    _currentCommand.Cancel();
                }
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// Формирует строку формата "(item1, item2, item3, ... , itemN)" для массива строк.
        /// </summary>
        /// <param name="items">Массив элементов</param>
        /// <returns>Строка формата "(item1, item2, item3, ... , itemN)"</returns>
        private string WrapItems(string[] items)
        {
            string result = string.Empty;
            for (int i = 0; i < items.Count(); i++)
            {
                result += string.Format("{0}{1}{2},", "{", i, "}");
            }
            result = result.Substring(0, result.Length - 1);
            result = string.Format("({0})", result);
            return string.Format(result, items);
        }

        /// <summary>
        /// Возвращает объект-соединение с dbf-базой данных.
        /// </summary>
        /// <param name="path">Путь к файлы БД</param>
        /// <returns>Объект-соединение с dbf-базой данных</returns>
        private OleDbConnection GetConnection(string path)
        {
            string connectionString = string.Format(ConfigurationManager.AppSettings["ole_connectionstring"], path);
            return new OleDbConnection(connectionString);
        }

        /// <summary>
        /// Преобразовывает массив значений в формат пригодный для вставки в DBF-таблицу
        /// </summary>
        /// <param name="values">Входной массив данных</param>
        /// <returns>Отформатированный массив</returns>
        private string[] ParseValues(object[] values)
        {
            var result = new List<string>();
            foreach (var value in values)
            {
                if (value == null || value is DBNull || string.IsNullOrEmpty(value.ToString()))
                {
                    result.Add("NULL"); continue;
                }

                if (value is DateTime)
                {
                    result.Add(((DateTime)value).ToShortDateString().Replace('.', '/')); continue;
                }
                if (value is string)
                {
                    result.Add(string.Format("'{0}'", value.ToString())); continue;
                }
                result.Add(value.ToString());
            }
            return result.ToArray();
        }


    }
} 
