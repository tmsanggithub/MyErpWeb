using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebRunDragon.Utils;

namespace WebRunDragon.DataProcess
{
    public class ProcessKiemKeKho
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessKiemKeKho));
        static string className = typeof(ProcessKiemKeKho).Name;
        private static ProcessKiemKeKho _instance;
        public static ProcessKiemKeKho getInstance()
        {
            if (_instance == null)
                _instance = new ProcessKiemKeKho();
            return _instance;
        }
        public DataTable TraCuuKiemKeKho(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKho_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuKiemKeKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetKiemKeKhoByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho + "");
        }
        internal bool AddOrUpdate(int id, int idChiNhanh, DateTime NgayChotDanhSach, string MaNhanVienHCQT, string MaPhongBanNVHCQT
             , string MaNhanVienKeToan, string MaPhongBanNVKeToan, string MaPhongBanDuocKiemKe, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("NgayChotDanhSach", NgayChotDanhSach);
                param.Add("MaNhanVienHCQT", MaNhanVienHCQT);
                param.Add("MaPhongBanNVHCQT", MaPhongBanNVHCQT);
                param.Add("MaNhanVienKeToan", MaNhanVienKeToan);
                param.Add("MaPhongBanNVKeToan", MaPhongBanNVKeToan);
                param.Add("MaPhongBanDuocKiemKe", MaPhongBanDuocKiemKe);
                param.Add("GhiChu", ghiChu);

                param.Add("UserLogin", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKho_AddOrUdate", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Đã tồn tại mã số phiếu giao tài sản";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo','chỉnh sửa' hoặc 'trả về'";
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
                    var strCode = "ThanhToan_AddOrUpdate_" + Math.Abs(retCode).ToString();
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
                param.Add("TableName", ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho+"");
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Utils_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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
                    var strCode = "ThanhToan_Delete_" + Math.Abs(retCode).ToString();
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho + "", userLoginId, out message);
        }
        internal bool SendApprovalKiemKeKho(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("TableName", ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho+"");
                param.Add("TableNameCT", ProcessDanhMuc.eTenDanhMuc.QLKiemKeKhoCT+"");
                param.Add("IDMasterName", ProcessDanhMuc.eTenColumMasterID.IDKiemKe+"");
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Utils_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Chỉ cho phép gửi duyệt khi trạng thái là 'mới tạo' hoặc 'chỉnh sửa'";
                }
                else if (retCode == -3)
                {
                    message = "Vui lòng nhập thông tin chi tiết trước khi gửi duyệt";
                }
                else if (retCode == -5)
                {
                    message = "Không tồn tại người quản lý của user gửi duyệt.\nVui lòng kiểm tra lại thông tin nhân sự.";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình xử lý dữ liệu";
                }

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "ThanhToan_SendApproval_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".SendApprovalKiemKeKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalKiemKeKho(int id, string ghiChu, string userLoginId, out string message)
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
                SqlParameter para02 = new SqlParameter("TableName", ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho);
                SqlParameter para03 = new SqlParameter("GhiChu", ghiChu);
                SqlParameter para04 = new SqlParameter("UserLogin", userLoginId);
                SqlParameter[] paras = new SqlParameter[4] { para01, para02, para03, para04 };
                object objRet = DatabaseManager.SQLExecScalar("SP_Utils_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Chỉ cho duyệt khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == -3)
                {
                    message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình xử lý dữ liệu";
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
                    var strCode = "ThanhToan_Approval_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".ApprovalKiemKeKho Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;
        }
        internal bool RejectKiemKeKho(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("TableName", ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Utils_Reject", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không đúng nhân sự được gán duyệt.";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép từ chối khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình xử lý dữ liệu";
                }
                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "ThanhToan_Reject_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".RejectKiemKeKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }

        // Chi tiet-------------------------------------------------

        internal bool AddOrUpdateKiemKeKhoChiTiet(int ID, int IDKiemKeKho, int idTaiSanChiTiet, int SoLuong,DateTime NgayMua, decimal DonGiaMua
           , int DonGiaConLai, int SoLuongSoSach, int SoLuongKiemKe,   string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {

              

                Dictionary <string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDKiemKe", IDKiemKeKho);
                param.Add("IDTaiSanChiTiet", idTaiSanChiTiet);
                param.Add("SoLuong", SoLuong);
                param.Add("NgayMua", NgayMua);
                param.Add("DonGiaMua", DonGiaMua);
                param.Add("DonGiaConLai", DonGiaConLai);
                param.Add("SoLuongSoSach", SoLuongSoSach);
                param.Add("SoLuongKiemKe", SoLuongKiemKe);
                param.Add("GhiChuChiTiet ", GhiChuChiTiet + "");
                param.Add("UserLogin", userLogin);
                                
                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID để cập nhật dữ liệu";
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

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "ThanhToan_AddOrUpdateCT_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".AddOrUpdateKiemKeKhoChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietKiemKeKho(int id, int idMaster, string userLoginId, out string message)
        {

            message = "";
            JObject entity = this.GetKiemKeKhoByID(idMaster);
            if (entity != null)
            {
                if (entity["TrangThai"] + "" == "NEW" || entity["TrangThai"] + "" == "EDIT" || entity["TrangThai"] + "" == "RETURNED")
                {
                    return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLKiemKeKhoCT + "", userLoginId, out message);
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
        internal DataTable TraCuuKiemKeKhoChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDKiemKe", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuKiemKeKhoChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }/*
        internal DataTable LayDanhSachTaiSan4ThemKiemKeKho(int idKiemKeKho, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDThuHoi", idKiemKeKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_DSTaiSan4ThemThuHoi", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemKiemKeKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }*/
        internal JObject GetKiemKeKhoChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLKiemKeKhoCT + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDObjectValue", idMaster);
                param.Add("TableName", ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_Utils_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
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
                return DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_Import_DeleteAll", CommandType.StoredProcedure, param) + "" != "";
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DeleteAllTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        public bool SaveAllTaiSanImport(string userLogin, int idDieuChinhKho, out string message)
        {
            message = string.Empty;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin + "");
                param.Add("IDKiemKeKho", idDieuChinhKho);
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_Import_SaveAll", CommandType.StoredProcedure, param);
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


                //Dictionary<string, object> param = new Dictionary<string, object>();
                //param.Add("UserLogin", userLogin);
                //param.Add("IDDieuChinhKho", idDieuChinhKho);
                //return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDieuChinhKhoCT_Import_SaveAll", CommandType.StoredProcedure, param) > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".SaveAllTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }

        internal DataTable InKiemKeTsNV(int idKiemKeKho, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IdKiemKeKho", idKiemKeKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKho_InPhieuKiemKeTsNV", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".InKiemKeTsNV() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public DataTable BaoCaoKiemKeTSNhanVien(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idPhongBan, string nhanSuDuocKiemKe, int taiSan, string soPhieu, string userLogin)
        {
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDPhongBanDuocKiemKe", idPhongBan);
                param.Add("MaNhanSuDuocKiemKe", nhanSuDuocKiemKe);
                param.Add("IDTaiSan", taiSan);
                param.Add("SoPhieuKiemKe", soPhieu);

                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_KiemKeTaiSanNhanVien_TongHop", CommandType.StoredProcedure, param);
                  

                /*


                string sql = @"
select kk.ID, kk.NgayChotDanhSach, br.BranchName, pp.Code, pp.DeptName, nv.StaffNo, nv.StaffName
, nts.TenNhomTaiSan, ts.MaTaiSan, ts.TenTaiSan, tt.TenTrangThaiTaiSan
, ct.SoLuongSoSach, ct.SoLuongKiemKe, ct.DonGiaMua, ct.DonGiaConLai, ct.SoLuongKiemKe * ct.DonGiaConLai as GiaTriKiemKe
from QLKiemKeKho kk
inner join QLKiemKeKhoCT ct on kk.ID = ct.IDKiemKe
left join cfgDepartments pp on pp.Code = kk.MaPhongBanDuocKiemKe
left join (
    select distinct BranchId, DepartmentId 
    from cfgUsers u	
    where IsDeleted = 0 
) cn on pp.Id = cn.DepartmentId
left join cfgBranches br on br.ID = cn.BranchId
left join DMTaiSan ts on ts.id = ct.IDTaiSanChiTiet
left join cfgUsers nv on nv.StaffNo = ct.MaNhanVien
left join DMTrangThaiTaiSan tt on tt.ID = ct.IDTrangThaiTaiSan
left join DMNhomTaiSan nts on nts.ID = ts.IDNhomTaiSan
where kk.IsDeleted = 0 and ct.IsDeleted = 0";

                List<string> whereConditions = new List<string>();

                if (tuNgay != DateTime.MinValue)
                {
                    whereConditions.Add("kk.NgayChotDanhSach >= @TuNgay");
                }
                if (denNgay != DateTime.MinValue)
                {
                    whereConditions.Add("kk.NgayChotDanhSach <= @DenNgay");
                }
                if (idChiNhanh > 0)
                {
                    whereConditions.Add("br.ID = @IDChiNhanh");
                }
                if (!string.IsNullOrEmpty(nhanSuDuocKiemKe) && nhanSuDuocKiemKe.Trim() != "")
                {
                    whereConditions.Add("(nv.StaffNo = @IDNhanSuDuocKiemKe OR nv.UserName = @IDNhanSuDuocKiemKe)");
                }
                if (taiSan > 0)
                {
                    whereConditions.Add("ts.ID = @IDTaiSan");
                }
                if (!string.IsNullOrEmpty(soPhieu) && soPhieu.Trim() != "" && soPhieu.Trim() != " ")
                {
                    whereConditions.Add("(kk.Code LIKE @SoPhieuKiemKe OR kk.SoPhieu LIKE @SoPhieuKiemKe)");
                }

                if (whereConditions.Count > 0)
                {
                    sql += " AND " + string.Join(" AND ", whereConditions);
                }

                Dictionary<string, object> param = new Dictionary<string, object>();
                if (tuNgay != DateTime.MinValue)
                    param.Add("TuNgay", tuNgay);
                if (denNgay != DateTime.MinValue)
                    param.Add("DenNgay", denNgay);
                if (idChiNhanh > 0)
                    param.Add("IDChiNhanh", idChiNhanh);
                if (!string.IsNullOrEmpty(nhanSuDuocKiemKe) && nhanSuDuocKiemKe.Trim() != "")
                    param.Add("IDNhanSuDuocKiemKe", nhanSuDuocKiemKe.Trim());
                if (taiSan > 0)
                    param.Add("IDTaiSan", taiSan);
                if (!string.IsNullOrEmpty(soPhieu) && soPhieu.Trim() != "" && soPhieu.Trim() != " ")
                    param.Add("SoPhieuKiemKe", "%" + soPhieu.Trim() + "%");

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                */
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoKiemKeTSNhanVien() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoKiemKeTSNhanVienTheoPhongBan(DateTime tuNgay, DateTime denNgay, string userLogin)
        {
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
               

                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_KiemKeTaiSanNhanVien_TongHop_TheoPB", CommandType.StoredProcedure, param);
                
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoKiemKeTSNhanVien() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoKiemKeTSNhanVienTheoChiNhanh(DateTime tuNgay, DateTime denNgay, string userLogin)
        {
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);


                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_KiemKeTaiSanNhanVien_TongHop_TheoCN", CommandType.StoredProcedure, param);

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoKiemKeTSNhanVien() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        /*
        internal DataTable BaoCaoKiemKeKho(DateTime tuNgay, DateTime denNgay, int IDDonViTraLai, string IDNguoiTraLai, int IDTaiSanThuHoi, string SoPhieuThuHoi, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDDonViTraLai", IDDonViTraLai);
                param.Add("IDNguoiTraLai", IDNguoiTraLai);
                param.Add("IDTaiSanThuHoi", IDTaiSanThuHoi);
                param.Add("SoPhieuThuHoi", SoPhieuThuHoi);
                param.Add("UserLogin", userLogin);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_KiemKeKho", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoKiemKeKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataSet TraCuuThongTinCanDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDKiemKeKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataSetSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKho_GetInfoApproval", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuThongTinCanDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal bool SaoChepTaiSanThuHoi(int idMaster, int idKho, string sessionUserId, out string message)
        {
            message = "";
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                // param.Add("ID", idCT);
                param.Add("IDThuHoi", idMaster);
                param.Add("IDKhoNhap", idKho);
                param.Add("UserLogin", sessionUserId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLKiemKeKhoCT_SaoChepTaiSan", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID để cập nhật dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo' hoặc 'chỉnh sửa'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                return retCode > 0;

            }
            catch (Exception ex)
            {
                logger.Error(className + ".SaoChepTaiSanThuHoi Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }

        }
        */
    }
}