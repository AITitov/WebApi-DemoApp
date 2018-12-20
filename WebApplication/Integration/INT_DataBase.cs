using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication.Integration
{
    public class INT_DataBase
    {
        #region Соединение с БД
        public static SqlConnection getConnection()
        {
            string connectionStr = Properties.Settings.Default.ConectionString;
            SqlConnection connection = new SqlConnection(connectionStr);

            connection.Open();

            return connection;
        }
        #endregion

        #region Универсальные CRUD методы для работы с БД и метод ExecuteSQL

        /// <summary>
        /// Чтение таблицы
        /// </summary>
        /// <param name="connection">Соединение SqlConnection</param>
        /// <param name="tableName">Полное название таблицы</param>
        /// <param name="query">Фильтр вида "<название поля>=<значение поля>"</param>
        /// <param name="sort">Сортировка вида "<название поля>" </param>
        /// 
        public static DataTable Read(SqlConnection connection, string tableName, string query, string sort)
        {
            try
            {
                string commandText = String.Format("SELECT * FROM {0} {1} {2}", tableName, (query != "" ? " WHERE " + query : ""), (sort != "" ? " ORDER BY " + sort : ""));
                return ExecuteSQL(connection, commandText);
            }
            catch (Exception exc)
            {
                throw new Exception("INT_DataBase read error - " + exc.Message);
            }
        }

        /// <summary>
        ///  Добавление строки в таблицу
        /// </summary>
        /// <param name="connection">Соединение SqlConnection</param>
        /// <param name="tableName">Полное название таблицы</param>
        /// <param name="param">Список параметров команды вставки где Dictionary.Key - название поля, а Dictionary.Value - значение</param>
        /// 
        public static DataTable Insert(SqlConnection connection, string tableName, Dictionary<string, string> param)
        {
            try
            {
                string fieldNames = "";
                string fieldValues = "";
                for (int i = 0; i < param.Keys.Count; i++)
                {
                    fieldNames += param.Keys.ElementAt<string>(i);
                    fieldValues += param.Values.ElementAt<string>(i);
                    if (i != param.Keys.Count - 1)
                    {
                        fieldNames += ",";
                        fieldValues += ",";
                    }
                }
                string commandText = String.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, fieldNames, fieldValues);
                return ExecuteSQL(connection, commandText);
            }
            catch (Exception exc)
            {
                throw new Exception("INT_DataBase insert error - " + exc.Message);
            }
        }

        internal static DataTable get_list(SqlConnection connection, string v_NAV_Item, string v1, string v2)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Изменение строки в таблице
        /// </summary>
        /// <param name="connection">Соединение SqlConnection</param>
        /// <param name="tableName">Полное название таблицы</param>
        /// <param name="param">Список параметров, где key - название поля, а value - значение</param>
        /// <param name="query">Фильтр вида "<название поля>=<значение поля>"</param>
        /// 
        public static DataTable Update(SqlConnection connection, string tableName, Dictionary<string, string> param, string query)
        {
            try
            {
                string paramStr = "";
                for (int i = 0; i < param.Keys.Count; i++)
                {
                    paramStr += param.Keys.ElementAt<string>(i) + " = " + param.Values.ElementAt<string>(i);
                    if (i != param.Keys.Count - 1)
                        paramStr += ",";
                }
                string commandText = String.Format("UPDATE {0} SET {1} {2}", tableName, paramStr, (query != "" ? "WHERE " + query : ""));
                return ExecuteSQL(connection, commandText);
            }
            catch (Exception exc)
            {
                throw new Exception("INT_DataBase update error - " + exc.Message);
            }
        }

        /// <summary>
        /// Удаление строки из таблицы
        /// </summary>
        /// <param name="connection">Соединение SqlConnection</param>
        /// <param name="tableName">Полное название таблицы</param>
        /// <param name="query">Фильтр вида "<название поля>=<значение поля>"</param>
        /// 
        public static DataTable Delete(SqlConnection connection, string tableName, string query)
        {
            try
            {
                string commandText = String.Format("DELETE FROM {0} {1}", tableName, (query != "" ? "WHERE " + query : ""));
                return ExecuteSQL(connection, commandText);
            }
            catch (Exception exc)
            {
                throw new Exception("INT_DataBase delete error - " + exc.Message);
            }
        }

        public static DataTable ExecuteSQL(SqlConnection connection, string commandText)
        {
            try
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.CommandTimeout = 600;
                command.Dispose();
                SqlDataAdapter sqlDtAdptr = new SqlDataAdapter(command);
                DataTable result = new DataTable();
                sqlDtAdptr.Fill(result);
                result.Dispose();
                connection.Close();
                sqlDtAdptr.Dispose();
                return result;
            }
            catch (Exception exc)
            {
                throw new Exception("executeSQL error - " + exc.Message + ", commandText - " + commandText);
            }
        }

        #endregion
    }
}