using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class DatabaseManager
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DatabaseManager));

        public static string CNN_STRING_HELPDESK= "";

        #region Kết nối

        public static SqlConnection OpenSqlConnection(string cnnString)
        {
            SqlConnection cnn = null;
            try
            {
                cnn = new SqlConnection(cnnString);
                cnn.Open();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            return cnn;
        }



        public static void CloseDbConnection(SqlConnection cnn)
        {
            if (cnn != null) cnn.Close();
        }

        public static bool TestHelpDeskConnection()
        {
            SqlConnection cnn = OpenSqlConnection(CNN_STRING_HELPDESK);
            if (cnn != null)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    logger.Warn("Database = " + cnn.Database);
                    logger.Warn("DataSource = " + cnn.DataSource);
                    logger.Warn("ServerVersion = " + cnn.ServerVersion);
                    CloseDbConnection(cnn);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Execute SQL Server

        public static int ExecuteUpdateSql(string cnnString, string cmmText, CommandType cmmType, Dictionary<string, object> param)
        {
            SqlConnection cnn = null;
            try
            {
                cnn = OpenSqlConnection(cnnString);
                SqlCommand cmm = new SqlCommand(cmmText, cnn);
                cmm.CommandType = cmmType;
                if (param != null)
                {
                    foreach (KeyValuePair<string, object> pair in param)
                        cmm.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
                int rowEffected = cmm.ExecuteNonQuery();
                return rowEffected;
            }
            catch (Exception ex)
            {
                logger.Error("ExecuteUpdate Sql Statement Error: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                CloseDbConnection(cnn);
            }
            return 0;
        }

        public static object ExecuteScalarSql(string cnnString, string cmmText, CommandType cmmType, Dictionary<string, object> param)
        {
            SqlConnection cnn = null;
            try
            {
                cnn = OpenSqlConnection(cnnString);
                SqlCommand cmm = new SqlCommand(cmmText, cnn);
                cmm.CommandType = cmmType;
                if (param != null)
                {
                    foreach (KeyValuePair<string, object> pair in param)
                        cmm.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
                return cmm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.Error("ExecuteScalar Sql Statement Error: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                CloseDbConnection(cnn);
            }
            return null;
        }

        public static DataTable GetDataTableSql(string cnnString, string cmmText, CommandType cmmType, Dictionary<string, object> param)
        {
            DataTable dtb = null;
            SqlConnection cnn = null;
            try
            {
                cnn = OpenSqlConnection(cnnString);
                SqlCommand cmm = new SqlCommand(cmmText, cnn);
                cmm.CommandType = cmmType;
                if (param != null)
                {
                    foreach (KeyValuePair<string, object> pair in param)
                        cmm.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                SqlDataAdapter dta = new SqlDataAdapter(cmm);
                dtb = new DataTable();
                dta.Fill(dtb);
                return dtb;
            }
            catch (Exception ex)
            {
                logger.Error("GetDataTable Sql Statement Error: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                CloseDbConnection(cnn);
            }
            return null;
        }

        public static DataSet GetDataSetSql(string cnnString, string cmmText, CommandType cmmType, Dictionary<string, object> param)
        {
            DataSet dts = null;
            SqlConnection cnn = null;
            try
            {
                cnn = OpenSqlConnection(cnnString);
                SqlCommand cmm = new SqlCommand(cmmText, cnn);
                cmm.CommandType = cmmType;
                if (param != null)
                {
                    foreach (KeyValuePair<string, object> pair in param)
                        cmm.Parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }

                SqlDataAdapter dta = new SqlDataAdapter(cmm);
                dts = new DataSet();
                dta.Fill(dts);
                return dts;
            }
            catch (Exception ex)
            {
                logger.Error("GetDataSetSql Sql Statement Error: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                CloseDbConnection(cnn);
            }
            return null;
        }


        public static Newtonsoft.Json.Linq.JArray GetJsonArraySql(string cnnString, string cmmText, CommandType cmmType, Dictionary<string, object> param)
        {
            Newtonsoft.Json.Linq.JArray array = null;
            try
            {
                DataTable dtb = GetDataTableSql(cnnString, cmmText, cmmType, param);
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    array = new Newtonsoft.Json.Linq.JArray();
                    foreach (DataRow row in dtb.Rows)
                    {
                        Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                        foreach (DataColumn col in dtb.Columns)
                        {
                            if (col.DataType == typeof(int))
                                jObject[col.ColumnName] = Utils.NumberUtil.ParseToInt(row[col.ColumnName] + "");
                            if (col.DataType == typeof(decimal) || col.DataType == typeof(double))
                                jObject[col.ColumnName] = Utils.NumberUtil.ParseToDecimal(row[col.ColumnName] + "");
                            else
                                jObject[col.ColumnName] = row[col.ColumnName] + "";
                        }
                        array.Add(jObject);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("GetJsonArray Sql Statement Error: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            return array;
        }

        public static bool AutoSaveDataTable(string cnnString, DataTable dataTable, string TableName)
        {
            SqlConnection cnn = null; SqlDataAdapter data = null;
            try
            {
                cnn = OpenSqlConnection(cnnString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = string.Format("select * from {0} where 0=1", TableName);

                data = new SqlDataAdapter(cmd);
                DataTable dtTemp = new DataTable(TableName);
                data.Fill(dtTemp);
                SqlCommandBuilder builder = new SqlCommandBuilder();
                builder.DataAdapter = data;


                //foreach (DataRow r in dataTable.Rows)
                //{
                //    r.SetModified();
                //}
                data.Update(dataTable);
                return true;
            }
            catch (Exception ex)
            {
                if (cnn != null) cnn.Close();
                logger.Error("SQLAutoSaveDataTable()Message:" + ex.Message + ". Trace: " + ex.StackTrace);
                return false;
            }
            finally
            {
                //if (cnn != null)
                CloseDbConnection(cnn);
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region có transaction

        public static int SQLExecuteNonQuery(string cmmText, CommandType cmmType, ref SqlTransaction vTran, SqlParameter[] paras)
        {
            int ret = 0;
            try
            {
                SqlCommand cmm = new SqlCommand(cmmText, vTran.Connection, vTran);
                cmm.CommandType = cmmType;
                if (paras != null && paras.Length > 0)
                {
                    foreach (SqlParameter para in paras)
                        cmm.Parameters.Add(para);
                }
                ret = cmm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error("SQLExecuteNonQuery: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                throw ex;
            }
            finally
            {
                //   clsConnectionManager.CloseConnection(cnn);
            }
            return ret;
        }

        public static object SQLExecScalar(string cmmText, CommandType cmmType, ref SqlTransaction vTran, SqlParameter[] paras)
        {
            object ret = null;
            try
            {
                SqlCommand cmm = new SqlCommand(cmmText, vTran.Connection, vTran);
                cmm.CommandType = cmmType;
                if (paras != null && paras.Length > 0)
                {
                    foreach (SqlParameter para in paras)
                        cmm.Parameters.Add(para);
                }
                ret = cmm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                logger.Error("SQLExecScalar: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                throw ex;
            }
            finally
            {

            }
            return ret;
        }

        public static DataTable SQLGetDataTable(string cmmText, CommandType cmmType, ref SqlTransaction vTran, SqlParameter[] paras)
        {
            DataTable dtb = new DataTable();
            SqlDataAdapter dta = null;
            try
            {
                SqlCommand cmm = new SqlCommand(cmmText, vTran.Connection, vTran);
                cmm.CommandType = cmmType;
                if (paras != null && paras.Length > 0)
                {
                    foreach (SqlParameter para in paras)
                        cmm.Parameters.Add(para);
                }
                dta = new SqlDataAdapter();
                dta.SelectCommand = cmm;
                dta.Fill(dtb);
            }
            catch (Exception ex)
            {
                logger.Error("SQLGetDataTable: " + cmmText);
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                throw ex;
            }
            finally
            {
                if (dta != null) dta.Dispose();
                // clsConnectionManager.CloseConnection(cnn);
            }
            return dtb;
        }

        public static bool AutoSaveDataTable(DataTable dataTable, string TableName, ref SqlTransaction tran)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = tran.Connection;
                cmd.CommandText = string.Format("select * from {0} where 0=1", TableName);
                cmd.Transaction = tran;

                SqlDataAdapter data = new SqlDataAdapter(cmd);
                DataTable dtTemp = new DataTable(TableName);
                data.Fill(dtTemp);
                SqlCommandBuilder builder = new SqlCommandBuilder();
                builder.DataAdapter = data;
                data.Update(dataTable);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("AutoSaveDataTable() ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                return false;
            }
        }

        #endregion

    }
}