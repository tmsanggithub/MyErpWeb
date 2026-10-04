using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessImportTaiSan
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessImportTaiSan));
        static string className = typeof(ProcessImportTaiSan).Name;
        private static ProcessImportTaiSan _instance;
        public static ProcessImportTaiSan getInstance()
        {
            if (_instance == null)
                _instance = new ProcessImportTaiSan();
            return _instance;
        }

        public DataTable GetDMTaiSanImport(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Import_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable GetDMTaiSanImportPhongBan(int idKiemKe, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                param.Add("IdKiemKe", idKiemKe);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_Import_Search_ByIDKiemKe", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImportPhongBan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }


        public DataTable GetDMTaiSanImport4KyDuyet(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Import_Search4Approval", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public bool DeleteAllTaiSanImport(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Import_DeleteAll", CommandType.StoredProcedure, param) > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }

        }

        public bool SendApprovalAllTaiSanImport(string userLogin, out string message)
        {
            message = string.Empty;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Import_SendApproval", CommandType.StoredProcedure, param);
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    int errorCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0]["RetCode"]);
                    if (errorCode > 0)
                    {
                        return true;
                    }
                    else
                    {
                        message = dtRet.Rows[0]["MessageFail"] + "";
                        return false;
                    }
                }
                else
                {
                    message = "Có lổi trong quá trình xử lý dữ liệu";
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                message = ex.Message;
                return false;
            }

        }

        public bool ApprovalAllTaiSanImport(string userLogin, out string message)
        {
            message = string.Empty;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Import_Approval", CommandType.StoredProcedure, param);
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    int errorCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0]["RetCode"]);
                    if (errorCode > 0)
                    {
                        return true;
                    }
                    else
                    {
                        message = dtRet.Rows[0]["MessageFail"] + "";
                        return false;
                    }
                }
                else
                {
                    message = "Có lổi trong quá trình xử lý dữ liệu";
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                message = ex.Message;
                return false;
            }

        }

        public bool RejectAllTaiSanImport(string userLogin, out string message)
        {
            message = string.Empty;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Import_Reject", CommandType.StoredProcedure, param);
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    int errorCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0]["RetCode"]);
                    if (errorCode > 0)
                    {
                        return true;
                    }
                    else
                    {
                        message = dtRet.Rows[0]["MessageFail"] + "";
                        return false;
                    }
                }
                else
                {
                    message = "Có lổi trong quá trình xử lý dữ liệu";
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                message = ex.Message;
                return false;
            }

        }

    }
}