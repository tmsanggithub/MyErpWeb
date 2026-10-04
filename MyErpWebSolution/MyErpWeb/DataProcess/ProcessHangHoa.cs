using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessHangHoa
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessHangHoa));
        static string className = typeof(ProcessHangHoa).Name;
        private static ProcessHangHoa _instance;
        public static ProcessHangHoa getInstance()
        {
            if (_instance == null)
                _instance = new ProcessHangHoa();
            return _instance;
        }

        public DataTable TraCuuHangHoa(string key4Search, string userLogin)
        {
            try
            {
                string sql = @"SELECT * FROM dm_hang_hoa WHERE ISNULL(IsDeleted,0)=0
                                AND (@Key4Search = '' OR ma_hang_hoa LIKE '%'+@Key4Search+'%' OR ten_hang_hoa LIKE '%'+@Key4Search+'%' OR ma_vach LIKE '%'+@Key4Search+'%')
                                ORDER BY id DESC";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@Key4Search", key4Search ?? "");

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                logger.Error(className + ".TraCuuHangHoa() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal JObject GetHangHoaByID(int id)
        {
            try
            {
                string sql = "SELECT * FROM dm_hang_hoa WHERE id=@id";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                DataTable dtb = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    JObject obj = new JObject();
                    DataRow row = dtb.Rows[0];
                    foreach (DataColumn col in dtb.Columns)
                    {
                        obj[col.ColumnName] = row[col.ColumnName] == DBNull.Value ? null : row[col.ColumnName] + "";
                    }
                    return obj;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".GetHangHoaByID() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal bool AddOrUpdate(int id, string maHangHoa, string maVach, string tenHangHoa, string imageUrl,
            int idNhomHangHoa, int idThuongHieu, int dinhMucTonKhoThapNhat, int dinhMucTonCaoThapNhat, int tenViTri,
            int trongLuong, int idDonViTrongLuong, int kichThuotRong, int kichThuotDai, int idDonViKichThuot,
            string moTaChiTietHangHoa, string userLogin, out string message, out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            SqlConnection cnn = null;
            SqlTransaction tran = null;
            try
            {
                if (string.IsNullOrWhiteSpace(tenHangHoa))
                {
                    message = "Ch?a nh?p Tên hàng hóa";
                    return false;
                }

                cnn = DatabaseManager.OpenSqlConnection(DatabaseManager.CNN_STRING_HELPDESK);
                tran = cnn.BeginTransaction();

                if (id <= 0)
                {
                    string insertSql = @"INSERT INTO dm_hang_hoa
                        (ma_hang_hoa, ma_vach, ten_hang_hoa, image_url, id_nhom_hang_hoa, id_thuong_hieu,
                        dinh_muc_ton_kho_thap_nhat, dinh_muc_ton_cao_thap_nhat, ten_vi_tri, trong_luong, id_don_vi_trong_luong,
                        kich_thuot_rong, kich_thuot_dai, id_don_vi_kich_thuot, mo_ta_chi_tiet_hang_hoa, IsDeleted, CreatedBy, CreatedTime)
                        OUTPUT INSERTED.id
                        VALUES
                        (@ma_hang_hoa, @ma_vach, @ten_hang_hoa, @image_url, @id_nhom_hang_hoa, @id_thuong_hieu,
                        @dinh_muc_ton_kho_thap_nhat, @dinh_muc_ton_cao_thap_nhat, @ten_vi_tri, @trong_luong, @id_don_vi_trong_luong,
                        @kich_thuot_rong, @kich_thuot_dai, @id_don_vi_kich_thuot, @mo_ta_chi_tiet_hang_hoa, 0, @CreatedBy, GETDATE())";

                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@ma_hang_hoa", maHangHoa ?? ""),
                        new SqlParameter("@ma_vach", maVach ?? ""),
                        new SqlParameter("@ten_hang_hoa", tenHangHoa ?? ""),
                        new SqlParameter("@image_url", imageUrl ?? ""),
                        new SqlParameter("@id_nhom_hang_hoa", idNhomHangHoa),
                        new SqlParameter("@id_thuong_hieu", idThuongHieu),
                        new SqlParameter("@dinh_muc_ton_kho_thap_nhat", dinhMucTonKhoThapNhat),
                        new SqlParameter("@dinh_muc_ton_cao_thap_nhat", dinhMucTonCaoThapNhat),
                        new SqlParameter("@ten_vi_tri", tenViTri),
                        new SqlParameter("@trong_luong", trongLuong),
                        new SqlParameter("@id_don_vi_trong_luong", idDonViTrongLuong),
                        new SqlParameter("@kich_thuot_rong", kichThuotRong),
                        new SqlParameter("@kich_thuot_dai", kichThuotDai),
                        new SqlParameter("@id_don_vi_kich_thuot", idDonViKichThuot),
                        new SqlParameter("@mo_ta_chi_tiet_hang_hoa", moTaChiTietHangHoa ?? ""),
                        new SqlParameter("@CreatedBy", userLogin ?? "")
                    };

                    object objId = DatabaseManager.SQLExecScalar(insertSql, CommandType.Text, ref tran, paras);
                    int newId = Utils.NumberUtil.ParseToInt(objId + "");
                    if (newId <= 0)
                    {
                        tran.Rollback();
                        message = "Th?t b?i trong quá trình l?u d? li?u";
                        return false;
                    }

                    tran.Commit();
                    idInsertNew = newId;
                    message = "L?u thành công";
                    return true;
                }
                else
                {
                    string updateSql = @"UPDATE dm_hang_hoa SET
                        ma_hang_hoa=@ma_hang_hoa, ma_vach=@ma_vach, ten_hang_hoa=@ten_hang_hoa, image_url=@image_url,
                        id_nhom_hang_hoa=@id_nhom_hang_hoa, id_thuong_hieu=@id_thuong_hieu,
                        dinh_muc_ton_kho_thap_nhat=@dinh_muc_ton_kho_thap_nhat, dinh_muc_ton_cao_thap_nhat=@dinh_muc_ton_cao_thap_nhat,
                        ten_vi_tri=@ten_vi_tri, trong_luong=@trong_luong, id_don_vi_trong_luong=@id_don_vi_trong_luong,
                        kich_thuot_rong=@kich_thuot_rong, kich_thuot_dai=@kich_thuot_dai, id_don_vi_kich_thuot=@id_don_vi_kich_thuot,
                        mo_ta_chi_tiet_hang_hoa=@mo_ta_chi_tiet_hang_hoa, UpdatedBy=@UpdatedBy, UpdatedTime=GETDATE()
                        WHERE id=@id";

                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@id", id),
                        new SqlParameter("@ma_hang_hoa", maHangHoa ?? ""),
                        new SqlParameter("@ma_vach", maVach ?? ""),
                        new SqlParameter("@ten_hang_hoa", tenHangHoa ?? ""),
                        new SqlParameter("@image_url", imageUrl ?? ""),
                        new SqlParameter("@id_nhom_hang_hoa", idNhomHangHoa),
                        new SqlParameter("@id_thuong_hieu", idThuongHieu),
                        new SqlParameter("@dinh_muc_ton_kho_thap_nhat", dinhMucTonKhoThapNhat),
                        new SqlParameter("@dinh_muc_ton_cao_thap_nhat", dinhMucTonCaoThapNhat),
                        new SqlParameter("@ten_vi_tri", tenViTri),
                        new SqlParameter("@trong_luong", trongLuong),
                        new SqlParameter("@id_don_vi_trong_luong", idDonViTrongLuong),
                        new SqlParameter("@kich_thuot_rong", kichThuotRong),
                        new SqlParameter("@kich_thuot_dai", kichThuotDai),
                        new SqlParameter("@id_don_vi_kich_thuot", idDonViKichThuot),
                        new SqlParameter("@mo_ta_chi_tiet_hang_hoa", moTaChiTietHangHoa ?? ""),
                        new SqlParameter("@UpdatedBy", userLogin ?? "")
                    };

                    int rows = DatabaseManager.SQLExecuteNonQuery(updateSql, CommandType.Text, ref tran, paras);
                    if (rows <= 0)
                    {
                        tran.Rollback();
                        message = "Th?t b?i trong quá trình l?u d? li?u";
                        return false;
                    }

                    tran.Commit();
                    idInsertNew = id;
                    message = "L?u thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                logger.Error(className + ".AddOrUpdate Error: " + ex.Message + "\n" + ex.StackTrace);
                message = ex.Message;
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
        }

        internal bool Delete(int id, string userLogin, out string message)
        {
            message = "";
            try
            {
                string sql = "UPDATE dm_hang_hoa SET IsDeleted=1, DeletedBy=@DeletedBy, DeletedTime=GETDATE() WHERE id=@id";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);
                param.Add("@DeletedBy", userLogin ?? "");

                int rows = DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                if (rows <= 0)
                {
                    message = "Th?t b?i trong quá trình xóa d? li?u";
                    return false;
                }
                message = "Xóa thành công";
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".Delete Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                message = ex.Message;
                return false;
            }
        }

        // ===================== ??n v? tính hàng hóa (chi ti?t) =====================

        internal DataTable TraCuuHangHoaDonViTinh(int idMaster, string userLogin)
        {
            try
            {
                string sql = @"SELECT * FROM dm_hang_hoa_don_vi_tinh WHERE ISNULL(IsDeleted,0)=0 AND Id_hang_hoa=@Id_hang_hoa ORDER BY id";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@Id_hang_hoa", idMaster);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                logger.Error(className + ".TraCuuHangHoaDonViTinh() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal JObject GetHangHoaDonViTinhByID(int id)
        {
            try
            {
                string sql = "SELECT * FROM dm_hang_hoa_don_vi_tinh WHERE id=@id";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                DataTable dtb = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                if (dtb != null && dtb.Rows.Count > 0)
                {
                    JObject obj = new JObject();
                    DataRow row = dtb.Rows[0];
                    foreach (DataColumn col in dtb.Columns)
                    {
                        obj[col.ColumnName] = row[col.ColumnName] == DBNull.Value ? null : row[col.ColumnName] + "";
                    }
                    return obj;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".GetHangHoaDonViTinhByID() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        // T??ng ?ng v?i AddOrUpdateGiaiChayChiTiet c?a QLGiaiChay
        internal bool AddOrUpdateHangHoaDonViTinh(int id, int idHangHoa, string tenDonViTinh, decimal giaTriQuiDoi,
            decimal giaBan, int isDonViCoBan, string userLogin, out string message, out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            SqlConnection cnn = null;
            SqlTransaction tran = null;
            try
            {
                if (idHangHoa <= 0)
                {
                    message = "Ch?a xác ??nh hàng hóa cha";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(tenDonViTinh))
                {
                    message = "Ch?a nh?p Tên ??n v? tính";
                    return false;
                }

                cnn = DatabaseManager.OpenSqlConnection(DatabaseManager.CNN_STRING_HELPDESK);
                tran = cnn.BeginTransaction();

                // n?u ?ánh d?u là ??n v? c? b?n, b? c? c? b?n c?a các ??n v? khác thu?c cùng hàng hóa
                if (isDonViCoBan == 1)
                {
                    string clearSql = "UPDATE dm_hang_hoa_don_vi_tinh SET is_don_vi_co_ban=0 WHERE Id_hang_hoa=@Id_hang_hoa AND ISNULL(IsDeleted,0)=0";
                    SqlParameter[] clearParas = new SqlParameter[] { new SqlParameter("@Id_hang_hoa", idHangHoa) };
                    DatabaseManager.SQLExecuteNonQuery(clearSql, CommandType.Text, ref tran, clearParas);
                }

                if (id <= 0)
                {
                    string insertSql = @"INSERT INTO dm_hang_hoa_don_vi_tinh
                        (Id_hang_hoa, ten_don_vi_tinh, gia_tri_qui_doi, gia_ban, is_don_vi_co_ban, IsDeleted, CreatedBy, CreatedTime)
                        OUTPUT INSERTED.id
                        VALUES (@Id_hang_hoa, @ten_don_vi_tinh, @gia_tri_qui_doi, @gia_ban, @is_don_vi_co_ban, 0, @CreatedBy, GETDATE())";

                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@Id_hang_hoa", idHangHoa),
                        new SqlParameter("@ten_don_vi_tinh", tenDonViTinh ?? ""),
                        new SqlParameter("@gia_tri_qui_doi", giaTriQuiDoi),
                        new SqlParameter("@gia_ban", giaBan),
                        new SqlParameter("@is_don_vi_co_ban", isDonViCoBan),
                        new SqlParameter("@CreatedBy", userLogin ?? "")
                    };

                    object objId = DatabaseManager.SQLExecScalar(insertSql, CommandType.Text, ref tran, paras);
                    int newId = Utils.NumberUtil.ParseToInt(objId + "");
                    if (newId <= 0)
                    {
                        tran.Rollback();
                        message = "Th?t b?i trong quá trình l?u d? li?u";
                        return false;
                    }

                    tran.Commit();
                    idInsertNew = newId;
                    message = "L?u chi ti?t thành công";
                    return true;
                }
                else
                {
                    string updateSql = @"UPDATE dm_hang_hoa_don_vi_tinh SET
                        ten_don_vi_tinh=@ten_don_vi_tinh, gia_tri_qui_doi=@gia_tri_qui_doi, gia_ban=@gia_ban,
                        is_don_vi_co_ban=@is_don_vi_co_ban, UpdatedBy=@UpdatedBy, UpdatedTime=GETDATE()
                        WHERE id=@id";

                    SqlParameter[] paras = new SqlParameter[] {
                        new SqlParameter("@id", id),
                        new SqlParameter("@ten_don_vi_tinh", tenDonViTinh ?? ""),
                        new SqlParameter("@gia_tri_qui_doi", giaTriQuiDoi),
                        new SqlParameter("@gia_ban", giaBan),
                        new SqlParameter("@is_don_vi_co_ban", isDonViCoBan),
                        new SqlParameter("@UpdatedBy", userLogin ?? "")
                    };

                    int rows = DatabaseManager.SQLExecuteNonQuery(updateSql, CommandType.Text, ref tran, paras);
                    if (rows <= 0)
                    {
                        tran.Rollback();
                        message = "Th?t b?i trong quá trình l?u d? li?u";
                        return false;
                    }

                    tran.Commit();
                    idInsertNew = id;
                    message = "L?u chi ti?t thành công";
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                logger.Error(className + ".AddOrUpdateHangHoaDonViTinh Error: " + ex.Message + "\n" + ex.StackTrace);
                message = ex.Message;
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
        }

        internal bool DeleteHangHoaDonViTinh(int id, int idMaster, string userLogin, out string message)
        {
            message = "";
            try
            {
                string sql = "UPDATE dm_hang_hoa_don_vi_tinh SET IsDeleted=1, DeletedBy=@DeletedBy, DeletedTime=GETDATE() WHERE id=@id";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);
                param.Add("@DeletedBy", userLogin ?? "");

                int rows = DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                if (rows <= 0)
                {
                    message = "Th?t b?i trong quá trình xóa d? li?u";
                    return false;
                }
                message = "Xóa chi ti?t thành công";
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".DeleteHangHoaDonViTinh Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                message = ex.Message;
                return false;
            }
        }
    }
}
