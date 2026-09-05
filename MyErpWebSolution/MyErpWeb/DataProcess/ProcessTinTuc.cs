using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessTinTuc
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTinTuc));
        static string className = typeof(ProcessTinTuc).Name;
        private static ProcessTinTuc _instance;
        public static ProcessTinTuc getInstance()
        {
            if (_instance == null)
                _instance = new ProcessTinTuc();
            return _instance;
        }
        public DataTable TraCuuTinTuc(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuTinTuc() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetTinTucByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.ql_news + "");
        }
        internal bool AddOrUpdate(int id, string ma, string ten, DateTime ngayLap ,string ghiChu, string noiDungTinTuc,int idRace, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("Code", ma);
                param.Add("Subject", ten);
                param.Add("NewsDate", ngayLap);
                param.Add("Description", ghiChu);
                param.Add("Content", noiDungTinTuc);
                param.Add("IdRace", idRace);
                param.Add("UserLogin", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_AddOrUdate", System.Data.CommandType.StoredProcedure, param) + "";
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

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLTinTuc + "", userLoginId, out message);
        }
        internal bool SendApprovalTinTuc(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".SendApprovalTinTuc Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
            internal bool ApprovalTinTuc(int id, string ghiChu, string userLoginId, out string message)
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
                    object objRet = DatabaseManager.SQLExecScalar("SP_QLTinTuc_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
            internal bool RejectTinTuc(int id, string ghiChu, string userLoginId, out string message)
            {
                message = "";

                try
                {
                    Dictionary<string, object> param = new Dictionary<string, object>();
                    param.Add("ID", id);
                    param.Add("GhiChu", ghiChu);
                    param.Add("UserLogin", userLoginId);

                    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
        internal bool AddOrUpdateTinTucChiTiet(int ID, int IDTinTuc, int idTaiSanChiTiet, int idTrangThaiTS, decimal soLuong, decimal DonGiaMua, decimal GiaTriConLai
           , decimal GiaTriThuHoi, int idKhoXuat, string PhuongAnXuLy, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDTinTuc", IDTinTuc);
                param.Add("IDKhoXuat", idKhoXuat);
                param.Add("IDTaiSanChiTiet", idTaiSanChiTiet);
                param.Add("IDTrangThaiTS", idTrangThaiTS);
                param.Add("SoLuong", soLuong);
                param.Add("DonGiaMua", DonGiaMua);
                param.Add("GiaTriConLai", GiaTriConLai);
                param.Add("GiaTriThuHoi", GiaTriThuHoi);
                param.Add("PhuongAnXuLy", PhuongAnXuLy);
                param.Add("GhiChuChiTiet ", GhiChuChiTiet + "");
                param.Add("UserLogin", userLogin);


                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTucCT_AddOrUpdate", System.Data.CommandType.StoredProcedure, param);
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
                    message = "Tài sản đang được điều chỉnh tại 'phiếu điều chỉnh kho' (id=" + retId + ")";
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
                logger.Error(className + ".AddOrUpdateTinTucChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietTinTuc(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetTinTucByID(idMaster);
            if (entity != null)
            {
                if (entity["TrangThai"] + "" == "NEW" || entity["TrangThai"] + "" == "EDIT" || entity["TrangThai"] + "" == "RETURNED")
                {
                    return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "", userLoginId, out message);
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
        internal DataTable TraCuuTinTucChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTinTuc", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTucCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuTinTucChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable LayDanhSachTaiSan4ThemTinTuc(int idTinTuc, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTinTuc", idTinTuc);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTucCT_DSTaiSan4ThemTinTuc", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemTinTuc() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetTinTucChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTinTuc", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoTinTuc(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idKho, int idTaiSan, string soPhieuTinTuc, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKho", idKho);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("SoPhieuTinTuc", soPhieuTinTuc);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_TinTuc", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoTinTuc() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal DataTable InTinTucTaiSan(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTinTuc", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_In", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".InTinTucTaiSan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        #region Dành cho tập tin
        internal DataTable GetTapTinDinhKem(int idHopDong, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTinTuc", idHopDong);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTinTuc_DSTapTinDinhKem", CommandType.StoredProcedure, param);
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
            return ProcessAttachment.AddOrUpdate(attId, "QLTinTuc", issueId, attName, filePath, contentType, fileSize, extension, userId, "", folderExt, ref retCode);
        }
        #endregion
    }
}