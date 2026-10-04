using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebRunDragon.Utils;

namespace WebRunDragon.DataProcess
{
    public class ProcessDieuChinhKho
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessDieuChinhKho));
        static string className = typeof(ProcessDieuChinhKho).Name;
        private static ProcessDieuChinhKho _instance;
        public static ProcessDieuChinhKho getInstance()
        {
            if (_instance == null)
                _instance = new ProcessDieuChinhKho();
            return _instance;
        }
        public DataTable TraCuuDieuChinhKho(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDieuChinhKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetDieuChinhKhoByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLDieuChinhKho + "");
        }
        internal bool AddOrUpdate(int id, int idChiNhanh, int idKhoNhap, DateTime ngayPhieu, string NhanSuPhieu, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKhoNhap", idKhoNhap);
                param.Add("NgayPhieu", ngayPhieu);
                param.Add("NhanSuPhieu", NhanSuPhieu);
                //param.Add("NhanSuKhac", nhanSuKhac);
                param.Add("GhiChu", ghiChu + "");
                param.Add("UserLogin", userLogin);

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_AddOrUdate", System.Data.CommandType.StoredProcedure, param);
                int retCode = 0; int retId = 0;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    retId = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][1]);
                }


                if (retCode == -2)
                {
                    message = "Đã tồn tại mã số phiếu nhập kho.";
                }
                else if (retCode == -22)
                {
                    message = "'Kho điều chỉnh' có phiếu điều chỉnh đang xử lý";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo' hoặc 'chỉnh sửa'";
                }
                else if (retCode == -201)
                {
                    message = "Tài sản đang nhập kho tại 'phiếu nhập Kho' (ID=" + retId + ")";
                }
                else if (retCode == -202)
                {
                    message = "Tài sản đang bàn giao tại 'phiếu bàn giao' (ID=" + retId + ")";
                }
                else if (retCode == -203)
                {
                    message = "Tài sản đang thu hồi tại 'phiếu thu hồi' (ID=" + retId + ")";
                }
                else if (retCode == -205)
                {
                    message = "Tài sản đang thanh lý tại 'phiếu thanh lý' (ID=" + retId + ")";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInsertNew = retCode;

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "DieuChinhKho_AddOrUpdate_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------
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

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_Delete", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không cho phép xóa trạng thái đã 'gửi duyệt' hoặc 'đã duyệt'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình xóa dữ liệu";
                }
                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "DieuChinhKho_Delete_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------
                return retCode > 0;

            }
            catch (Exception ex)
            {
                logger.Error(className + ".Delete Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLDieuChinhKho + "", userLoginId, out message);
        }
        internal bool SendApprovalDieuChinhKho(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                 

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_SendApproval", System.Data.CommandType.StoredProcedure, param);
                int retCode = 0;
                string pushId = string.Empty;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    pushId = dtRet.Rows[0][1]?.ToString();
                }
                 

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

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "DieuChinhKho_SendApproval_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------
                //--gui pushnotify đến người duyệt--------------------------------------
                if (retCode > 0 && !string.IsNullOrEmpty(pushId))
                {
                    Task.Run(() =>
                    {
                        ProcessNotifyFCM.GetInstance().SendNotifyByFunction("vi-VN", StringUtil.enumFCMFunction.DIEUCHINHTAISANKHO_CHODUYET_YC.ToString(), pushId);
                    }
                   ).ConfigureAwait(false);
                }
                //---------------------------------------------------------------------------------------------------
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".SendApprovalDieuChinhKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalDieuChinhKho(int id, string ghiChu, string userLoginId, out string message)
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

                //Dictionary<string, object> param = new Dictionary<string, object>();
                //param.Add("ID", id);
                //param.Add("GhiChu", ghiChu);
                //param.Add("UserLogin", userLoginId);


                int retCode = 0; int retId = 0;
                DataTable dt = DatabaseManager.SQLGetDataTable("SP_QLDieuChinhKho_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);
                if (dt != null && dt.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dt.Rows[0][0]);
                    retId = Utils.NumberUtil.ParseToInt(dt.Rows[0][1]);
                }


                if (retCode == -2)
                {
                    message = "Chỉ cho duyệt khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == -3)
                {
                    message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
                }
                else if (retCode == -201)
                {
                    message = "Tài sản đang nhập kho tại 'phiếu nhập Kho' (ID=" + retId + ")";
                }
                else if (retCode == -202)
                {
                    message = "Tài sản đang thanh lý tại 'phiếu thanh lý' (ID=" + retId + ")";
                }
                else if (retCode == -203)
                {
                    message = "Tài sản đang bàn giao tại 'phiếu bàn giao' (ID=" + retId + ")";
                }
                else if (retCode == -205)
                {
                    message = "Tài sản đang thu hồi tại 'phiếu thu hồi' (ID=" + retId + ")";
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

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "DieuChinhKho_Approval_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------
            }
            catch (Exception er)
            {
                if (tran != null) tran.Rollback();
                logger.Error(className + ".ApprovalDieuChinhKho Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;


        }
        internal bool RejectDieuChinhKho(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "DieuChinhKho_Reject_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".RejectDieuChinhKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool AddOrUpdateDieuChinhKhoChiTiet(int ID, int IDDieuChinhKho, int idTaiSanChiTiet, int IDTonKhoCu, int SoLuongCu, int TrangThaiCu, int TrangThaiMoi
           , decimal SoLuongMoi, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {



                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDDieuChinhKho", IDDieuChinhKho);
                param.Add("IDTaiSanChiTiet", idTaiSanChiTiet);
                param.Add("IDTonKhoCu", IDTonKhoCu);
                param.Add("SoLuongCu", SoLuongCu);
                param.Add("TrangThaiCu", TrangThaiCu);
                param.Add("TrangThaiMoi", TrangThaiMoi);
                param.Add("SoLuongMoi", SoLuongMoi);
                param.Add("GhiChuChiTiet ", GhiChuChiTiet + "");
                param.Add("UserLogin", userLogin);

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKhoCT_AddOrUpdate", System.Data.CommandType.StoredProcedure, param);
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
                else if (retCode == -201)
                {
                    message = "Tài sản đang nhập kho tại 'phiếu nhập Kho' (ID=" + retId + ")";
                }
                else if (retCode == -202)
                {
                    message = "Tài sản đang bàn giao tại 'phiếu bàn giao' (ID=" + retId + ")";
                }
                else if (retCode == -203)
                {
                    message = "Tài sản đang thu hồi tại 'phiếu thu hồi' (ID=" + retId + ")";
                }
                else if (retCode == -205)
                {
                    message = "Tài sản đang thanh lý tại 'phiếu thanh lý' (ID=" + retId + ")";
                }
                else if (retCode == -301)
                {
                    message = "Tài sản đang điều chỉnh Kho tại 'phiếu điều chỉnh' (ID=" + retId + ")";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInsertNew = retCode;

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "DieuChinhKho_AddOrUpdateCT_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------
                return retCode > 0;

            }
            catch (Exception ex)
            {
                logger.Error(className + ".AddOrUpdateDieuChinhKhoChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietDieuChinhKho(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetDieuChinhKhoByID(idMaster);
            if (entity != null)
            {
                if (entity["TrangThai"] + "" == "NEW" || entity["TrangThai"] + "" == "EDIT" || entity["TrangThai"] + "" == "RETURNED")
                {
                    return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLDieuChinhKhoCT + "", userLoginId, out message);

                }
                else
                {
                    message = "Chỉ cho phép xóa khi trạng thái là: mới tạo, chỉnh sử và trả về";
                    return false;
                }

            }
            else
                return false;
        }
        internal DataTable TraCuuDieuChinhKhoChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDieuChinhKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKhoCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDieuChinhKhoChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable LayDanhSachTaiSan4ThemDieuChinhKhoFromCache(int idKho, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var key = "LayDanhSachTaiSan4ThemDieuChinhKho" + idKho;
                var objCache = cache.GetMemCachedItem(key);

                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDKho", idKho);
                param.Add("UserLogin", userLogin);

                DataTable dtret = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKhoCT_DSTaiSan4ThemDieuChinhKho", CommandType.StoredProcedure, param);

                if (dtret != null && dtret.Rows.Count > 0)
                {
                    cache.AddToMemCacheAutoRemove(key, dtret, MyCachePriority.Default, 5 * 60);
                }
                return dtret;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemDieuChinhKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetDieuChinhKhoChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLDieuChinhKhoCT + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDieuChinhKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable GetTapTinDinhKem(int idDieuChinhKho, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDieuChinhKho", idDieuChinhKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_DSTapTinDinhKem", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetTapTinDinhKem() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public bool AddOrUpdateAttachment(int attId, int issueId, string attName, string filePath,
            string contentType, long fileSize, string extension, string userId, string folderExt, ref int retCode)
        {
            return ProcessAttachment.AddOrUpdate(attId, "QLDieuChinhKho", issueId, attName, filePath, contentType, fileSize, extension, userId, "", folderExt, ref retCode);
        }
        internal DataTable InDieuChinhKho(int idDieuChinhKho, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDieuChinhKho", idDieuChinhKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKho_InPhieuKiemKe", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".InDieuChinhKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

    }
}