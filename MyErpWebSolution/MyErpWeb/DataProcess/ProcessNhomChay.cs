using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessNhomChay
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessNhomChay));
        static string className = typeof(ProcessNhomChay).Name;
        private static ProcessNhomChay _instance;
        public static ProcessNhomChay getInstance()
        {
            if (_instance == null)
                _instance = new ProcessNhomChay();
            return _instance;
        }
        public DataTable TraCuuNhomChay(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuNhomChay() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetNhomChayByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "");
        }
        internal bool AddOrUpdate(int id, string ma, string ten, DateTime ngayLap, DateTime tuNgay, DateTime denNgay, string ghiChu, string noiDungNhomChay, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("RaceCode", ma);
                param.Add("RaceName", ten);
                param.Add("FromDate", tuNgay);
                param.Add("ToDate", denNgay);
                param.Add("TranDate", ngayLap);
                param.Add("Description", ghiChu);
                param.Add("RaceInfoHtml", noiDungNhomChay);

                param.Add("UserLogin", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_AddOrUdate", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Đã tồn tại mã số phiếu nhập kho.";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo' hoặc 'chỉnh sửa'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInsertNew = retCode;
                return retCode > 0;

            }
            catch (Exception ex)
            {
                logger.Error(className + ".AddOrUpdate Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool Delete(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);

                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_Delete", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không cho phép xóa trạng thái đã 'gửi duyệt' hoặc 'đã duyệt'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình xóa dữ liệu";
                }

                return retCode > 0;

            }
            catch (Exception ex)
            {
                logger.Error(className + ".Delete Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLNhomChay + "", userLoginId, out message);
        }
        internal bool SendApprovalNhomChay(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Chỉ cho phép gửi duyệt khi trạng thái là 'mới tạo' hoặc 'chỉnh sửa'";
                }
                else if (retCode == -3)
                {
                    message = "Vui lòng nhập chi tiết sản phẩm";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".SendApprovalNhomChay Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalNhomChay(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_QLNhomChay_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
                logger.Error(className + ".ApprovalNhomChay Error: ");
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

            //    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_Approval", System.Data.CommandType.StoredProcedure, param) + "";
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
            //    logger.Error(className + ".ApprovalNhomChay Error: ");
            //    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            //    return false;
            //}
        }
        internal bool RejectNhomChay(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".RejectNhomChay Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool AddOrUpdateNhomChayChiTiet(int ID, int IDNhomChay, int idNhomChay, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDGiaiVaNhomChay", IDNhomChay);
                param.Add("IDRunner", idNhomChay);
                param.Add("UserLogin", userLogin);


                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChayCT_AddOrUpdateRunner", System.Data.CommandType.StoredProcedure, param);
                int retCode = 0; int retId = 0;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    retId = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][1]);
                }
                if (retCode == -2)
                {
                    message = "Không tồn tại ID để cập nhật dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo' hoặc 'chỉnh sửa'";
                }
                else if (retCode == -101)
                {
                    message = "Đã tồn tại người chạy này rồi";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu ";
                }
                else
                    idInsertNew = retCode;
                return retCode > 0;

            }
            catch (Exception ex)
            {
                logger.Error(className + ".AddOrUpdateNhomChayChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietNhomChay(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetNhomChayByID(idMaster);
            if (entity != null)
            {
                //if (entity["status"] + "" == "NEW" || entity["status"] + "" == "EDIT" || entity["status"] + "" == "RETURNED")
                //{
                return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group_and_user + "", userLoginId, out message);
                //}
                //else
                //{
                //    message = "Chỉ cho phép xóa khi trạng thái là: mới tạo, chỉnh sử và trả về";
                //    return false;
                //}

            }
            else
                return false;
        }
        internal DataTable TraCuuNhomChayChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDGiaiVaNhomChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChayCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuNhomChayChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable LayDanhSachTaiSan4ThemNhomChay(int idNhomChay, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDNhomChay", idNhomChay);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChayCT_DSTaiSan4ThemNhomChay", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemNhomChay() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetNhomChayChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group_and_user + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDNhomChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoNhomChay(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idKho, int idTaiSan, string soPhieuNhomChay, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKho", idKho);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("SoPhieuNhomChay", soPhieuNhomChay);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_NhomChay", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoNhomChay() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal DataTable InNhomChayTaiSan(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDNhomChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhomChay_In", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".InNhomChayTaiSan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
    }
}