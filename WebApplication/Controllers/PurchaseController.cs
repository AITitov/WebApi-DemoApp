using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebApplication.Integration;
using WebApplication.Utils;

namespace WebApplication.Controllers
{
    public class PurchaseController
    {
        #region Параметры
        // SupplyTickets (Заявки на закупку)
        const string V_SP_SupplyTickets = "[purchase].[V_SP_SupplyTickets]";

        // SupplyItems (Строки заявок на закупку)
        const string SupplyItems_Table = "[purchase].[T_SP_SupplyItems]";
        const string SupplyPositions_View = "[purchase].[V_SP_SupplyItems]";
        const string SupplyItems_UpdateProcedure = "[purchase].[P_T_SupplyItems_Update]";

        // Item (Товары)
        const string T_NAV_Item = "[mdm].[T_NAV_Item]";
        const string V_NAV_Item = "[purchase].[V_NAV_Items]";

        // ItemUnitOfMeasure (Единицы измерения)
        const string T_NAV_ItemUnitOfMeasure = "[mdm].[T_NAV_ItemUnitOfMeasure]";
        const string V_NAV_ItemUnitOfMeasure = "[purchase].[V_NAV_ItemUnitOfMeasure]";


        #endregion

        #region Категории
        [Route("api/Purchase/Category")]
        [HttpGet]
        public Models.M_JSON Category()
        {
            try
            {
                SqlConnection connection = INT_DataBase.getConnection();
                string commandText = @"SELECT Category FROM " + V_NAV_Item + @" WHERE Category <> '' GROUP BY Category ORDER BY Category asc";

                DataTable result = INT_DataBase.ExecuteSQL(connection, commandText);
                Models.ItemCategory[] Result_Array = new Models.ItemCategory[result.Rows.Count];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    Result_Array[i] = (Models.ItemCategory)ModelUtils.GetObjectFromRow(result.Rows[i], typeof(Models.ItemCategory));
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true, total = result.Rows.Count, data = Result_Array };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при получения списка категорий товаров - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }

        }
        #endregion

        #region Товары
        [Route("api/Purchase/Get_ItemsByCategory")]
        [HttpGet]
        public Models.M_JSON Get_ItemsByCategory(string category)
        {
            try
            {
                SqlConnection connection = INT_DataBase.getConnection();
                DataTable result = INT_DataBase.get_list(connection, V_NAV_Item, " Category = N'" + category + "'", "");
                Models.Item[] Result_Array = new Models.Item[result.Rows.Count];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    Result_Array[i] = (Models.Item)ModelUtils.GetObjectFromRow(result.Rows[i], typeof(Models.Item));
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true, total = result.Rows.Count, data = Result_Array };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при получении списка товаров по категории " + category + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }

        [Route("api/Purchase/Get_ItemsByTitle")]
        [HttpGet]
        public Models.M_JSON Get_ItemsByTitle(string title)
        {
            try
            {
                SqlConnection connection = INT_DataBase.getConnection();
                DataTable result = INT_DataBase.get_list(connection, V_NAV_Item, " Title like N'%" + title + "%'", "");
                Models.Item[] Result_Array = new Models.Item[result.Rows.Count];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    Result_Array[i] = (Models.Item)ModelUtils.GetObjectFromRow(result.Rows[i], typeof(Models.Item));
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true, total = result.Rows.Count, data = Result_Array };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при получении списка товаров по названию " + title + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }

        #endregion

        #region Единицы измерения
        [Route("api/Purchase/Measure")]
        [HttpGet]
        public Models.M_JSON Measure(string itemCode)
        {
            try
            {
                SqlConnection connection = INT_DataBase.getConnection();
                DataTable result = INT_DataBase.get_list(connection, V_NAV_ItemUnitOfMeasure, " ItemCode = N'" + itemCode + "'", "");
                Models.Measure[] Result_Array = new Models.Measure[result.Rows.Count];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    Result_Array[i] = (Models.Measure)ModelUtils.GetObjectFromRow(result.Rows[i], typeof(Models.Measure));
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true, total = result.Rows.Count, data = Result_Array };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при получении списка единиц измерения по товару " + itemCode + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }
        #endregion

        #region Строки заявки (Товары)
        [Route("api/Purchase/Get_SupplyItems")]
        [HttpGet]
        public Models.M_JSON Get_SupplyItems(string supplyTicketRefId)
        {
            try
            {
                SqlConnection connection = INT_DataBase.getConnection();
                DataTable result = INT_DataBase.get_list(connection, SupplyPositions_View, "[SupplyTicketRefId] = '" + supplyTicketRefId + "'", "Number");
                Models.SupplyItem[] Result_Array = new Models.SupplyItem[result.Rows.Count];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    Result_Array[i] = (Models.SupplyItem)ModelUtils.GetObjectFromRow(result.Rows[i], typeof(Models.SupplyItem));
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true, total = result.Rows.Count, data = Result_Array };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при получении списка строк по заявке " + supplyTicketRefId + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }

        [Route("api/Purchase/Create_SupplyItems")]
        [HttpPost]
        public Models.M_JSON Create_SupplyItems([FromBody] Models.SupplyTicket supplyTicket)
        {
            try
            {
                List<string> paramNames = new List<string>();
                List<string> paramValues = new List<string>();
                foreach (var position in supplyTicket.SupplyItems)
                {
                    if (paramNames.Count == 0)
                    {
                        paramNames = ModelUtils.GetParamListFromObject(position);
                        paramNames.Add("SupplyTicketRefId");
                    }
                    string valuesStr = ModelUtils.GetValueListFromObject(position) + ",'" + supplyTicket.SupplyTicketRefId + "'";
                    paramValues.Add(valuesStr);
                }
                string commandText = ModelUtils.GetInsertMultipleCommandString(SupplyItems_Table, paramNames, paramValues);
                SqlConnection connection = INT_DataBase.getConnection();
                DataTable result = INT_DataBase.ExecuteSQL(connection, commandText);

                Models.M_JSON JSONResponse = new Models.M_JSON { success = true };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при добавлении списка строк по заявке " + supplyTicket.SupplyTicketRefId + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }


        [Route("api/Purchase/Update_SupplyItems")]
        [HttpPost]
        public Models.M_JSON Update_SupplyItems([FromBody] Models.SupplyTicket supplyTicket)
        {
            try
            {
                var updatedPositions = from position in supplyTicket.SupplyItems where position.ID != null select position;
                var newPositions = from position in supplyTicket.SupplyItems where position.ID == null select position;

                List<string> paramNames = new List<string>();
                List<string> paramValues = new List<string>();

                if (updatedPositions.Count() != 0)
                {
                    foreach (var position in updatedPositions)
                    {
                        if (paramNames.Count == 0)
                        {
                            paramNames = ModelUtils.GetParamListFromObject(position);
                            paramNames.Add("SupplyTicketRefId");
                        }
                        string valuesStr = ModelUtils.GetValueListFromObject(position) + ",'" + supplyTicket.SupplyTicketRefId + "'";
                        paramValues.Add(valuesStr);
                    }
                    string insertCommandText = ModelUtils.GetInsertMultipleCommandString("@UpdatedPositions", paramNames, paramValues);
                    string commandText = String.Format("EXECUTE {0} '{1}'", SupplyItems_UpdateProcedure, insertCommandText.Replace("'", "''"));
                    SqlConnection connection = INT_DataBase.getConnection();
                    DataTable result = INT_DataBase.ExecuteSQL(connection, commandText);
                }
                else // При отправке пустого списка обновленных записей все существующие удаляются
                {
                    string commandText = String.Format("DELETE FROM {0} WHERE SupplyTicketRefId='{1}'", SupplyItems_Table, supplyTicket.SupplyTicketRefId);
                    SqlConnection connection = INT_DataBase.getConnection();
                    DataTable result = INT_DataBase.ExecuteSQL(connection, commandText);
                }
                if (newPositions.Count() != 0)
                {
                    paramNames = new List<string>();
                    paramValues = new List<string>();

                    foreach (var position in newPositions)
                    {
                        if (paramNames.Count == 0)
                        {
                            paramNames = ModelUtils.GetParamListFromObject(position);
                            paramNames.Add("SupplyTicketRefId");
                        }
                        string valuesStr = ModelUtils.GetValueListFromObject(position) + ",'" + supplyTicket.SupplyTicketRefId + "'";
                        paramValues.Add(valuesStr);
                    }
                    string commandText = ModelUtils.GetInsertMultipleCommandString(SupplyItems_Table, paramNames, paramValues);
                    SqlConnection connection = INT_DataBase.getConnection();
                    DataTable result = INT_DataBase.ExecuteSQL(connection, commandText);
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при обновлении списка строк по заявке " + supplyTicket.SupplyTicketRefId + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }

        #endregion

        #region Услуги
        [Route("api/Purchase/Get_SupplyServices")]
        [HttpGet]
        public Models.M_JSON Get_SupplyServices(string SupplyTicketRefId)
        {
            try
            {
                SqlConnection connection = INT_DataBase.getConnection();
                DataTable result = INT_DataBase.get_list(connection, SupplyPositions_View, "[SupplyTicketRefId] = '" + SupplyTicketRefId + "'", "Number");
                Models.SupplyItem[] Result_Array = new Models.SupplyItem[result.Rows.Count];
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    Result_Array[i] = (Models.SupplyItem)ModelUtils.GetObjectFromRow(result.Rows[i], typeof(Models.SupplyItem));
                }
                Models.M_JSON JSONResponse = new Models.M_JSON { success = true, total = result.Rows.Count, data = Result_Array };
                return JSONResponse;
            }
            catch (Exception exc)
            {
                string errorMsg = "Ошибка при получении списка строк по заявке " + SupplyTicketRefId + " - " + exc.Message;
                if (!UnitTestDetector.IsRunningFromNUnit)
                    ULSLogger.WriteLog(errorMsg);
                Models.M_JSON JSONResponse = new Models.M_JSON { success = false, message = errorMsg };
                return JSONResponse;
            }
        }

        #endregion
    }
}