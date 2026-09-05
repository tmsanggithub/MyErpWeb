using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace WebRunDragon.DataProcess
{
    public class PropcessGiaiChayDuyetThamGia
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PropcessGiaiChayDuyetThamGia));
        static string className = typeof(PropcessGiaiChayDuyetThamGia).Name;
        private static PropcessGiaiChayDuyetThamGia _instance;
        public static PropcessGiaiChayDuyetThamGia getInstance()
        {
            if (_instance == null)
                _instance = new PropcessGiaiChayDuyetThamGia();
            return _instance;
        }
        public DataTable TraCuuGiaiChayDuyetThamGia(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_ql_race_group_user_register_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuTinTuc() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetGiaiChayDuyetThamGiaByID(int id)
        {


            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);            // param.Add("TableName", tableName);
            string sql = @"select g.id, g.description, g.id_race_and_group, concat( r.race_name , ' / ', gr.GroupName ) giai_chay_dang_ky
, u.full_name, u.email, u.staff_no_vdsc, u.user_type,u.mobile_phone, g.status
from ql_race_group_user_register g 
left join app_user_registed u on u.id=g.id_runner
left join cd_race_and_run_group rg on rg.id=g.id_race_and_group
left join ql_race r on r.id=rg.race_id
left join dm_run_group gr on gr.id=rg.run_group_id
                             where g.id=@Id and (g.IsDeleted=0 or g.IsDeleted is null)";
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
            if (array != null && array.Count > 0)
            {
                return (JObject)array[0];
            }
            return null;
        }

        internal bool Approval(int id, string ghiChu, string userLoginId, out string message)
        {
            // chổ này dùng transaction vì trong store có nhiều câu update, insert
            message = "";

            bool result = false;
            SqlTransaction tran = null;
            SqlConnection cnn = null;
            try
            {
                cnn = DatabaseManager.OpenSqlConnection(DatabaseManager.CNN_STRING_HELPDESK);
                tran = cnn.BeginTransaction();

                SqlParameter para01 = new SqlParameter("ID", id);
                SqlParameter para02 = new SqlParameter("GhiChu", ghiChu);
                SqlParameter para03 = new SqlParameter("UserLogin", userLoginId);
                SqlParameter[] paras = new SqlParameter[3] { para01, para02, para03 };
                object objRet = DatabaseManager.SQLExecScalar("SP_ql_race_group_user_register_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Chỉ cho duyệt khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == -3)
                {
                    message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
                }
                else if (retCode == -4)
                {
                    message = "Không đủ số lượng để thanh lý. \nVui lòng kiểm tra lại tồn kho của tài sản (theo trạng thái TS))";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                result = retCode > 0;

                if (result)
                {
                    tran.Commit();
                    result = true;
                }
                else
                {
                    tran.Rollback();
                    result = false;
                }
            }
            catch (Exception er)
            {
                if (tran != null) tran.Rollback();
                logger.Error(className + ".ApprovalTinTuc Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;



            //message = "";

            //try
            //{
            //    Dictionary<string, object> param = new Dictionary<string, object>();
            //    param.Add("ID", id);
            //    param.Add("GhiChu", ghiChu);
            //    param.Add("UserLogin", userLoginId);

            //    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_Approval", System.Data.CommandType.StoredProcedure, param) + "";
            //    int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
            //    if (retCode == -2)
            //    {
            //        message = "Chỉ cho duyệt khi trạng thái là 'gửi duyệt'";
            //    }
            //    else if (retCode == -3)
            //    {
            //        message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
            //    }
            //    else if (retCode == 0)
            //    {
            //        message = "Thất bại trong quá trình lưu dữ liệu";
            //    }
            //    return retCode > 0;
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(className + ".ApprovalTinTuc Error: ");
            //    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            //    return false;
            //}
        }
        internal bool Reject(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_ql_race_group_user_register_Reject", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Chỉ cho duyệt(từ chối) khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == -3)
                {
                    message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".RejectTinTuc Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDangKyGiaiChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_ql_race_group_user_register_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
    }
}
