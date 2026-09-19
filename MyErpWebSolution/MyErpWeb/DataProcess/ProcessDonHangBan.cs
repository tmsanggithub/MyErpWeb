using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessDonHangBan
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessDonHangBan));
        static string className = typeof(ProcessDonHangBan).Name;
        private static ProcessDonHangBan _instance;
        public static ProcessDonHangBan getInstance()
        {
            if (_instance == null)
                _instance = new ProcessDonHangBan();
            return _instance;
        }

        internal bool SaveHoaDonBan(int id, int idKhachHang, string ghiChu, string trangThai, string userLogin, out string message, out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            SqlConnection cnn = null;
            SqlTransaction tran = null;
            try
            {
                cnn = DatabaseManager.OpenSqlConnection(DatabaseManager.CNN_STRING_HELPDESK);
                tran = cnn.BeginTransaction();

                // Insert header
                string insertHeader = "INSERT INTO ql_hoa_don_ban (ngay_ban, id_khach_hang, ghi_chu,trang_thai, isdeleted, createdby, createdtime) OUTPUT INSERTED.id VALUES (@ngay_ban, @id_khach_hang, @ghi_chu, @trang_thai, 0, @createdby, GETDATE())";
                SqlParameter[] parasHeader = new SqlParameter[] {
                    new SqlParameter("@ngay_ban", DateTime.Now),
                    new SqlParameter("@id_khach_hang", idKhachHang),
                    new SqlParameter("@ghi_chu", ghiChu ?? ""),
                       new SqlParameter("@trang_thai", trangThai),
                    new SqlParameter("@createdby", userLogin)
                };

                object objId = DatabaseManager.SQLExecScalar(insertHeader, System.Data.CommandType.Text, ref tran, parasHeader);
                int idHoaDon = Utils.NumberUtil.ParseToInt(objId + "");
                if (idHoaDon <= 0)
                {
                    tran.Rollback();
                    message = "Không thể tạo hóa đơn";
                    return false;
                }

                // Insert details
                //if (chiTiets != null)
                //{
                //    foreach (DataRow dr in chiTiets.Rows)
                //    {
                //        int idHangHoa = Utils.NumberUtil.ParseToInt(dr["id_hang_hoa"] + "");
                //        decimal soLuong = Utils.NumberUtil.ParseToDecimal(dr["so_luong"]);
                //        decimal donGia = Utils.NumberUtil.ParseToDecimal(dr["don_gia"]);
                //        decimal thanhTien = soLuong * donGia;

                //        string insertDetail = "INSERT INTO ql_hoa_don_ban_ct (id_hoa_don_ban, id_hang_hoa, so_luong, don_gia, thanh_tien, ghi_chu, isdeleted, createdby, createdtime) VALUES (@id_hoa_don_ban, @id_hang_hoa, @so_luong, @don_gia, @thanh_tien, @ghi_chu, 0, @createdby, GETDATE())";
                //        SqlParameter[] parasDet = new SqlParameter[] {
                //            new SqlParameter("@id_hoa_don_ban", idHoaDon),
                //            new SqlParameter("@id_hang_hoa", idHangHoa),
                //            new SqlParameter("@so_luong", soLuong),
                //            new SqlParameter("@don_gia", donGia),
                //            new SqlParameter("@thanh_tien", thanhTien),
                //            new SqlParameter("@ghi_chu", dr.Table.Columns.Contains("ghi_chu") ? (dr["ghi_chu"] + "") : ""),
                //            new SqlParameter("@createdby", userLogin)
                //        };

                //        DatabaseManager.SQLExecuteNonQuery(insertDetail, System.Data.CommandType.Text, ref tran, parasDet);
                //    }
                //}

                tran.Commit();
                idInsertNew = idHoaDon;
                message = "Lưu thành công";
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                logger.Error(className + ".SaveHoaDonBan Error: " + ex.Message + "\n" + ex.StackTrace);
                message = ex.Message;
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
        }
        public DataTable TraCuuDonHangBan(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDonHangBan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetDonHangBanByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.ql_hoa_don_ban + "");
        }
        internal bool AddOrUpdate(int id, string ma, string ten, DateTime ngayLap, string ghiChu, string noiDungDonHangBan, int idRace, string userLogin, out string message, out int idInsertNew)
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
                param.Add("Content", noiDungDonHangBan);
                param.Add("IdRace", idRace);
                param.Add("UserLogin", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_AddOrUdate", System.Data.CommandType.StoredProcedure, param) + "";
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

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLDonHangBan + "", userLoginId, out message);
        }
        internal bool SendApprovalDonHangBan(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".SendApprovalDonHangBan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalDonHangBan(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_QLDonHangBan_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
                logger.Error(className + ".ApprovalDonHangBan Error: ");
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

            //    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_Approval", System.Data.CommandType.StoredProcedure, param) + "";
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
            //    logger.Error(className + ".ApprovalDonHangBan Error: ");
            //    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            //    return false;
            //}
        }
        internal bool RejectDonHangBan(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".RejectDonHangBan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool AddOrUpdateDonHangBanChiTiet(int ID, int IDDonHangBan, int idTaiSanChiTiet, int idTrangThaiTS, decimal soLuong, decimal DonGiaMua, decimal GiaTriConLai
           , decimal GiaTriThuHoi, int idKhoXuat, string PhuongAnXuLy, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                // Validate master
                var master = this.GetDonHangBanByID(IDDonHangBan);
                if (master == null)
                {
                    message = "Không tồn tại đơn hàng cha";
                    return false;
                }
                string trangThai = master["TrangThai"] + "";
                //if (!(trangThai == "NEW" || trangThai == "EDIT" || trangThai == "RETURNED"))
                //{
                //    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo' hoặc 'chỉnh sửa'";
                //    return false;
                //}


                var pInsert = new Dictionary<string, object>() {
                        { "@id_hoa_don_ban", IDDonHangBan },
                        { "@id_hang_hoa", idTaiSanChiTiet },
                        { "@so_luong", soLuong },
                        { "@don_gia", DonGiaMua },
                        { "@thanh_tien", soLuong * DonGiaMua },
                        { "@ghi_chu", GhiChuChiTiet ?? "" },
                        { "@createdby", userLogin }
                    };
                string sqlInsert = "INSERT INTO ql_hoa_don_ban_ct (id_hoa_don_ban, id_hang_hoa, so_luong, don_gia, thanh_tien, ghi_chu, isdeleted, createdby, createdtime) OUTPUT INSERTED.id VALUES (@id_hoa_don_ban, @id_hang_hoa, @so_luong, @don_gia, @thanh_tien, @ghi_chu, 0, @createdby, GETDATE())";
                object objId = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, sqlInsert, System.Data.CommandType.Text, pInsert);
                int newId = Utils.NumberUtil.ParseToInt(objId + "");
                if (newId > 0)
                {
                    idInsertNew = newId;
                    return true;
                }
                else
                {
                    message = "Thất bại trong quá trình thêm chi tiết";
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.Error(className + ".AddOrUpdateDonHangBanChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietDonHangBan(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetDonHangBanByID(idMaster);
            if (entity != null)
            {
                if (entity["TrangThai"] + "" == "" || entity["TrangThai"] + "" == "NEW" || entity["TrangThai"] + "" == "EDIT" || entity["TrangThai"] + "" == "RETURNED")
                {
                    return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.ql_hoa_don_ban_ct + "", userLoginId, out message);
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
        internal DataTable TraCuuDonHangBanChiTiet(int idMaster, string userLogin)
        {
            try
            {
                // Use inline SQL instead of stored procedure to return detail rows joined with product info
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", idMaster);

                string sqlDetail = @"SELECT ct.id
                                    ,ct.id_hoa_don_ban
                                    ,ct.id_hang_hoa
                                    ,ct.so_luong
                                    ,ct.don_gia
                                    ,ct.thanh_tien
                                    ,ct.ghi_chu
                                    ,hh.ma_hang_hoa
                                    ,hh.ten_hang_hoa
                                FROM [dbo].[ql_hoa_don_ban_ct] ct
                                LEFT JOIN [dbo].[dm_hang_hoa] hh ON hh.id = ct.id_hang_hoa
                                WHERE ct.id_hoa_don_ban = @id AND ISNULL(ct.isdeleted,0)=0";

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sqlDetail, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDonHangBanChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable LayDanhSachTaiSan4ThemDonHangBan(int idDonHangBan, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDonHangBan", idDonHangBan);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBanCT_DSTaiSan4ThemDonHangBan", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemDonHangBan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetDonHangBanChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDonHangBan", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoDonHangBan(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idKho, int idTaiSan, string soPhieuDonHangBan, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKho", idKho);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("SoPhieuDonHangBan", soPhieuDonHangBan);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_DonHangBan", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoDonHangBan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal DataTable InDonHangBanTaiSan(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDDonHangBan", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDonHangBan_In", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".InDonHangBanTaiSan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }


    }
}