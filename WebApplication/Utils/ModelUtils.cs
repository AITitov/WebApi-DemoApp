using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication.Utils
{
    public class ModelUtils
    {
        // Получение из объекта словаря (ключ - название параметра, значение - значение параметра)
        public static Dictionary<string, string> GetDictionaryFromObject(object obj)
        {
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if ((prop.GetValue(obj, null) == null) || (prop.Name == "ID"))
                        continue;
                    string value = "";
                    if ((prop.PropertyType == typeof(System.String)) // Добавление кавычек для текстовых типов полей
                        || (prop.PropertyType == typeof(System.Boolean))
                        || (prop.PropertyType == typeof(System.Guid))
                        || (prop.PropertyType == typeof(System.Guid?))
                        )
                        value = "N'" + prop.GetValue(obj, null).ToString() + "'";
                    else
                        value = prop.GetValue(obj, null).ToString();

                    result.Add(prop.Name, value);
                }
                return result;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetDictionaryFromObject error - " + exc.Message);
            }
        }
        /// <summary>
        /// Получение объекта из строки БД
        /// </summary>
        /// <param name="row">строка таблицы БД</param>
        /// <param name="objectType">тип возвращаемого объекта</param>
        public static object GetObjectFromRow(DataRow row, Type objectType)
        {
            try
            {
                object obj = Activator.CreateInstance(objectType);
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (row[column] == DBNull.Value)
                        continue;
                    var prop = obj.GetType().GetProperty(column.ToString());
                    if (prop != null)
                        prop.SetValue(obj, row[column]);
                }
                return obj;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetObjectFromRow error - " + exc.Message);
            }
        }
        /// <summary>
        /// Получение строки для вставки в БД списка объектов
        /// </summary>
        /// <param name="tableName">название таблицы БД</param>
        /// <param name="paramNames">список полей</param>
        /// /// <param name="paramValues">список значений</param>
        public static string GetInsertMultipleCommandString(string tableName, List<string> paramNames, List<string> paramValues)
        {
            try
            {
                string fieldNames = "";
                string fieldValues = "";
                for (int i = 0; i < paramNames.Count; i++)
                {
                    fieldNames += paramNames.ElementAt<string>(i);
                    if (i != paramNames.Count - 1)
                        fieldNames += ",";
                }
                for (int i = 0; i < paramValues.Count; i++)
                {
                    fieldValues += "(" + paramValues.ElementAt<string>(i) + ")";
                    if (i != paramValues.Count - 1)
                        fieldValues += ",";
                }
                string commandText = String.Format("INSERT INTO {0} ({1}) VALUES {2}", tableName, fieldNames, fieldValues);
                return commandText;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetInsertMultipleCommandString error - " + exc.Message);
            }
        }

        public static string GetUpdateCommandString(string tableName, long id, Dictionary<string, string> keyValues)
        {
            try
            {
                string commandText = "UPDATE " + tableName + " SET ";
                for (int i = 0; i < keyValues.Keys.Count; i++)
                {
                    commandText += keyValues.Keys.ElementAt<string>(i) + "=" + keyValues.Values.ElementAt<string>(i);
                    if (i != keyValues.Keys.Count - 1)
                        commandText += ",";
                }
                commandText += " WHERE ID=" + id;
                return commandText;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetUpdateCommandString error - " + exc.Message);
            }
        }

        // Удаление по списку ID
        public static string GetDeleteCommandString(string tableName, Guid parentGuid, string parentGuidFieldName, List<long> idList)
        {
            try
            {
                string commandText = "DELETE FROM " + tableName + " WHERE " + parentGuidFieldName + "='" + parentGuid.ToString() + "'";
                if (idList.Count != 0)
                    commandText += " AND ID NOT IN (";
                for (int i = 0; i < idList.Count; i++)
                {
                    commandText += idList[i];
                    if (i != idList.Count - 1)
                        commandText += ",";
                    else
                        commandText += ")";
                }

                return commandText;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetDeleteCommandString error - " + exc.Message);
            }
        }

        // Получение списка параметров из объекта
        public static List<string> GetParamListFromObject(object obj)
        {
            try
            {
                List<string> result = new List<string>();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if ((prop.PropertyType.Name.Contains("List") == true) || (prop.GetValue(obj, null) == null))
                        continue;
                    result.Add(prop.Name);
                }
                return result;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetParamListFromObject error - " + exc.Message);
            }
        }
        // Получение списка значений из объекта
        public static string GetValueListFromObject(object obj)
        {
            try
            {
                string result = "";
                foreach (var prop in obj.GetType().GetProperties())
                {
                    if ((prop.PropertyType.Name.Contains("List") == true) || (prop.GetValue(obj, null) == null))
                        continue;
                    string value = "";
                    if ((prop.PropertyType == typeof(System.String))
                        || ((prop.PropertyType == typeof(System.Boolean?))
                        || ((prop.PropertyType == typeof(System.Guid?)))))
                        value = "N'" + prop.GetValue(obj, null).ToString() + "'";
                    else
                        value = prop.GetValue(obj, null).ToString();

                    result += value + ",";
                }
                if (result[result.Length - 1] == ',')
                    result = result.Remove(result.Length - 1, 1);
                return result;
            }
            catch (Exception exc)
            {
                throw new Exception("ModelHelpers GetValueListFromObject error - " + exc.Message);
            }
        }
    }
}