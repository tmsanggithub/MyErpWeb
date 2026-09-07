using DevExpress.Xpo.DB.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebRunDragon.Forms;
using WebRunDragon.Utils;

namespace WebRunDragon.DataProcess
{
    public class ProcessDanhMuc
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessDanhMuc));
        static string className = typeof(ProcessDanhMuc).Name;
        private static ProcessDanhMuc _instance;
        public static ProcessDanhMuc getInstance()
        {
            if (_instance == null)
                _instance = new ProcessDanhMuc();
            return _instance;
        }
        public DataTable TraCuuDanhMuc(string key4Search, string tableName, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("KeySearch", key4Search);
                param.Add("TableName", tableName);
                param.Add("UserLogin", userLogin);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_Utils_DanhMucTraCuu", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMuc() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public DataTable LoadUserDangKy4Combo(string userLogin)
        {
            try
            {
                

                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_app_user_registed";
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }

                string sql = @"select r.id as id_runner, r.id_strava, r.staff_no_vdsc as code, r.full_name as name
                               from app_user_registed r
                               where (r.is_deleted = 0 or r.is_deleted is null)
                               order by r.full_name";

                var dtRet =DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, null);

                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }

                return dtRet;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LoadUserDangKy4Combo() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhMucHoatDongBK(DateTime tuNgay, DateTime denNgay, string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_ql_activities_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public bool AddHoatDongThuCong(int idRace, string idStrava, int idRunner, DateTime startDate, decimal totalDistanceMet, decimal totalDistanceMetValid, string idActivities, int valid, out string message, out int idInertNew)
        {
            message = "";
            idInertNew = 0;
            try
            {
                string sql = @"insert into ql_activities
                                (id_race, id_strava, id_runner,date, start_date, total_distance_met, total_distance_met_valid, id_activities, created_by, created_time, valid, isdeleted, total_moving_time, average_speed_second_on_km,external_id)
                               values
                                (@id_race, @id_strava, @id_runner,@date, @start_date, @total_distance_met, @total_distance_met_valid, @id_activities, 'admin_import', getdate(), @valid, 0,0, 0,'web_created');
                               select scope_identity();";

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("id_race", idRace);
                param.Add("id_strava", idStrava);
                param.Add("id_runner", idRunner);
                param.Add("date", startDate);
                param.Add("start_date", startDate);
                param.Add("total_distance_met", totalDistanceMet);
                param.Add("total_distance_met_valid", totalDistanceMetValid);
                param.Add("id_activities", idActivities);
                param.Add("valid", valid);

                idInertNew = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param) + "");
                if (idInertNew <= 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }

                return idInertNew > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddHoatDongThuCong Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lỗi Exception: " + ex.Message;
                return false;
            }
        }
        public DataTable TraCuuDanhMucHoatDong(string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                //  param.Add("TuNgay", tuNgay);
                //  param.Add("DenNgay", denNgay);
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_ql_activities_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhMucHoatDongSum(DateTime tuNgay, DateTime denNgay, string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_ql_activities_Search_sum", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhXepHangTheoDoi(string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_bc_xephang_doi_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhXepHangTheoAllVDV(string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_bc_xephang_allvdv_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhXepHangTheoNam(string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_bc_xephang_nam_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhXepHangTheoNu(string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_bc_xephang_nu_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhXepHangTheoTuan(string userLogin, int idGiaiChay)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_bc_xephang_tuan", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhXepHangTheoTuan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable TraCuuDanhMucFromCache(string key4Search, string tableName, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "TraCuuDanhMucFromCache_" + tableName + "_" + key4Search;
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("KeySearch", key4Search);
                param.Add("TableName", tableName);
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_Utils_DanhMucTraCuu", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }
                return dtRet;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMuc() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        //public DataTable LoadDanhMuc4Combo(string tableName, string userLogin)
        //{
        //    try
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("TableName", tableName);
        //        param.Add("UserLogin", userLogin);
        //        return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_Utils_LoadDanhMuc", CommandType.StoredProcedure, param);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(className.ToString() + ".LoadDanhMuc4ComboFromCache() Error: ");
        //        logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
        //        return null;
        //    }
        //}
        public DataTable LoadDanhMuc4ComboFromCache(string tableName, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_" + tableName + "_";
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TableName", tableName);
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_Utils_LoadDanhMuc", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }
                return dtRet;

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LoadDanhMuc4ComboFromCache() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable LoadDanhMucPhongBanTheoBranchId(string branchId, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LoadDanhMucPhongBanTheoBranchId" + "_" + branchId;
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("BranchId", Utils.NumberUtil.ParseToInt(branchId));
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_cfgDepartments_LoadByBrachId", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }
                return dtRet;

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LoadDanhMucPhongBanTheoBranchId() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable LoadDanhMucKhoTheoBranchId(string branchId, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LoadDanhMucKhoTheoBranchId" + "_" + branchId;
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("BranchId", Utils.NumberUtil.ParseToInt(branchId));
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_DMKho_LoadKhoByBrachId", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }
                return dtRet;

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LoadDanhMucKhoTheoUser() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public DataTable TraCuuDanhMucTestImg( string userLogin)
        {
            try
            {
                //Dictionary<string, object> param = new Dictionary<string, object>();
                //param.Add("TuNgay", tuNgay);
                //param.Add("DenNgay", denNgay);
                //param.Add("UserLogin", userLogin);
                //param.Add("IdGiaiChay", idGiaiChay);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "select id,ma_hang_hoa, ten_hang_hoa, image_url, don_gia from dm_hang_hoa", CommandType.Text,null);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuDanhMucHoatDong() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }


        public JObject GetDanhMucByID(int ID, string tableName)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", ID);            // param.Add("TableName", tableName);
            string sql = "select * from @TableName where id=@Id and (IsDeleted=0 or IsDeleted is null)".Replace("@TableName", tableName);
            if (tableName.ToUpper() == "QL_ACTIVITIES")
            {
                sql = @"select u.[staff_no_vdsc],u.[full_name],ac.*
                    from QL_ACTIVITIES ac
                    inner join  [dbo].[app_user_registed] u on ac.[id_runner]=u.id
                    where ac.id=@Id and (ac.IsDeleted=0 or ac.IsDeleted is null)";
            }
            else if (tableName.ToUpper() == "DMCONFIG")
            {
                sql = @"select lh.ID, lh.para_type NhomConfig, lh.para_code MaConfig,lh.para_value GiaTriConfig ,lh.Description GhiChu
                    	from app_params_config lh	
                    where lh.id=@Id and (lh.is_deleted=0 or lh.is_deleted is null)";
            }
            else if (tableName.ToUpper() == "APP_USER_REGISTED")
            {
                sql = "select * from @TableName where id=@Id and (Is_Deleted=0 or Is_Deleted is null)".Replace("@TableName", tableName);
            }
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }
        public JObject GetDanhMucByCode(string codeName, string codeValue, string tableName)
        {
            string sql = "select * from " + tableName + " where  (IsDeleted=0 or IsDeleted is null) and " + codeName + "=N'" + codeValue + "'";
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, null);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }
        public bool DeleteDanhMucByIDCheckConstrainKey(int id, string tableName, string userLoginId, out string message)
        {
            message = "";
            try
            {
                if (tableName == "DMHOATDONG") { tableName = "ql_activities"; }
                string contrainKeyOfTable = GetTableInfo(tableName).MaDM;
                if (contrainKeyOfTable + "" == "")
                {
                    message = "Thiếu cài đặt khóa chính trong Code. Không cho phép xóa"; // cac danh muc phai co Cónstrain key
                    return false;
                }

                string sqlDelete = "update " + tableName + " set IsDeleted=1 , " + contrainKeyOfTable + " = " + contrainKeyOfTable + " + CONVERT(varchar(10),id), DeletedBy=@UserLogin, DeletedTime=GETDATE() where ID=@Id";
                if (tableName.ToUpper() == "ql_activities".ToUpper())
                {
                    sqlDelete = @"
INSERT INTO [dbo].[ql_activities_his]
           (id_master,[id_race], [id_strava], [id_runner], [date], [total_moving_time], [total_distance_met], [average_speed_second_on_km], [valid], [sport_type], [time_zone], [splits], [visibility], [cheat], [id_activities], [start_date], [created_by], [created_time], [updated_by], [updated_time], [status], [reason], [total_distance_met_valid], [moving_time_str], [name], [external_id], [IsDeleted], [description_change], [map], [invalid_week], [total_elapsed_time])
SELECT id, [id_race], [id_strava], [id_runner], [date], [total_moving_time], [total_distance_met], [average_speed_second_on_km], [valid], [sport_type], [time_zone], [splits], [visibility], [cheat], [id_activities], [start_date], [created_by], [created_time], [updated_by], [updated_time], [status], [reason], [total_distance_met_valid], [moving_time_str], [name], [external_id], [IsDeleted], [description_change], [map], [invalid_week], [total_elapsed_time]
FROM ql_activities_his
WHERE id = @Id;
delete from " + tableName + "  where ID=@Id ;";

                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("UserLogin", userLoginId);

                int retCode = DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, sqlDelete, System.Data.CommandType.Text, param);


                if (Utils.NumberUtil.ParseToInt(retCode) > 0)
                {
                    message = "Xóa thành công";
                    return true;
                }
                else
                {
                    message = "Xóa thất bại";
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DeleteDanhMucByID Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool DeleteDanhMucByIDNotCheckConstrainKey(int id, string tableName, string userLoginId, out string message)
        {
            message = "";
            try
            {

                string sqlDelete = "update " + tableName + " set IsDeleted=1 , DeletedBy=@UserLogin, DeletedTime=GETDATE() where ID=@Id";

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("UserLogin", userLoginId);

                int retCode = DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, sqlDelete, System.Data.CommandType.Text, param);


                if (Utils.NumberUtil.ParseToInt(retCode) > 0)
                {
                    message = "Xóa thành công";
                    return true;
                }
                else
                {
                    message = "Xóa thất bại";
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DeleteDanhMucByID Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }

        public bool AddOrUpdateDanhMucKho(int id, string maKho, string tenKho, int idChiNhanh, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaKho", maKho);
                param.Add("TenKho", tenKho);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMKho_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã kho " + maKho;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucDonViTinh(int id, string maDonViTinh, string tenDonViTinh, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaDonViTinh", maDonViTinh);
                param.Add("TenDonViTinh", tenDonViTinh);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMDonViTinh_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã đơn vị tính " + maDonViTinh;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucDonViTinh Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucNhomChay(int id, string maDonViTinh, string tenDonViTinh, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaNhomChay", maDonViTinh);
                param.Add("TenNhomChay", tenDonViTinh);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMNhomChay_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã đơn vị tính " + maDonViTinh;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucNhomChay Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool UpdateDanhMucVanDongVien(int id, string maVanDongVien, string tenHienThi, string tenDayDu, string gioiTinh, string userType, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("staff_no_vdsc", maVanDongVien);
                param.Add("display_name", tenHienThi);
                param.Add("full_name", tenDayDu);
                param.Add("admin_description", ghiChu);
                param.Add("gender", gioiTinh);
                param.Add("user_type", userType);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_app_user_registed_update", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    //  message = "Đã tồn tại mã đơn vị tính " + maDonViTinh;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucVanDongVien Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucLoaiTaiSan(int id, string maLoaiTaiSan, string tenLoaiTaiSan, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaLoaiTaiSan", maLoaiTaiSan);
                param.Add("TenLoaiTaiSan", tenLoaiTaiSan);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMLoaiTaiSan_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã loại tài sản" + maLoaiTaiSan;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucLoaiTaiSan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucNhomTaiSan(int id, string ma, string ten, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaNhomTaiSan", ma);
                param.Add("TenNhomTaiSan", ten);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMNhomTaiSan_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã nhóm tài sản " + ma;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucNhomTaiSan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucNhaCungCap(int id, string maNhaCungCap, string tenNhaCungCap, string diaChi, string dienThoaiCty, string website, string fax, int idNhomNhaCungCap, string ghiChu, string dienThoaiNguoiDaiDien, string emailNguoiDaiDien, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaNhaCungCap", maNhaCungCap);
                param.Add("TenNhaCungCap", tenNhaCungCap);
                param.Add("DiaChi", diaChi);
                param.Add("DienThoaiCty", dienThoaiCty);
                param.Add("Website", website);
                param.Add("Fax", fax);
                param.Add("IDNhomNhaCungCap", idNhomNhaCungCap);
                param.Add("GhiChu", ghiChu);
                param.Add("DienThoaiNguoiDaiDien", dienThoaiNguoiDaiDien);
                param.Add("EmailNguoiDaiDien", emailNguoiDaiDien);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMNhaCungCap_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã nhà cung cấp " + maNhaCungCap;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucDonViTinh Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucNhaSanXuat(int id, string maNhaSanXuat, string tenNhaSanXuat, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaNhaSanXuat", maNhaSanXuat);
                param.Add("TenNhaSanXuat", tenNhaSanXuat);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMNhaSanXuat_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã nhà sản xuất " + maNhaSanXuat;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucDonViTinh Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucTaiSan(int id, string maTaiSan, string TenTaiSan, int IDDonViTinh, int IDNhomTaiSan,
            int IDLoaiTaiSan, string SoSerialNumber, decimal giaTriTaiSan, string CauHinh, int IDNhaSanXuat, DateTime NamSanXuat, string SoPhieuBaoHanh, int SoThangBaoHanh
            , DateTime ngayHetHan, string TyLeHaoMon, DateTime NamDuaVaoSuDung, int IDNhaCungCap, string ChungNhanCOCQ, string GhiChu, int IDLoaiBaoHiem, string userLoginId,
            out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaTaiSan", maTaiSan);
                param.Add("TenTaiSan", TenTaiSan);
                param.Add("IDDonViTinh", IDDonViTinh);
                param.Add("IDNhomTaiSan", IDNhomTaiSan);
                param.Add("IDLoaiTaiSan", IDLoaiTaiSan);
                param.Add("SoSerialNumber", SoSerialNumber);
                param.Add("GiaTriTaiSan", giaTriTaiSan);
                param.Add("CauHinh", CauHinh);
                param.Add("IDNhaSanXuat", IDNhaSanXuat);
                param.Add("NamSanXuat", NamSanXuat);
                param.Add("SoPhieuBaoHanh", SoPhieuBaoHanh);
                param.Add("SoThangBaoHanh", SoThangBaoHanh);
                //  param.Add("SoThangHanSuDung", SoThangHanSuDung);
                param.Add("TyLeHaoMon", TyLeHaoMon);
                param.Add("NamDuaVaoSuDung", NamDuaVaoSuDung);
                param.Add("IDNhaCungCap", IDNhaCungCap);
                param.Add("ChungNhanCOCQ", ChungNhanCOCQ);
                param.Add("GhiChu", GhiChu);
                param.Add("IDLoaiBaoHiem", IDLoaiBaoHiem);
                param.Add("NgayHetHan", ngayHetHan);


                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã tài sản " + maTaiSan;
                }
                else if (retCode == -5)
                {
                    message = "Đã tồn tại số S/N " + SoSerialNumber;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucTaiSan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        internal bool DeleteTaiSan(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);

                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLNhapKho + "", userLoginId, out message);
        }
        internal bool SendApprovalTaiSan(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);


                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_SendApproval", System.Data.CommandType.StoredProcedure, param);
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
                //--gui pushnotify đến người duyệt--------------------------------------
                if (retCode > 0 && !string.IsNullOrEmpty(pushId))
                {
                    Task.Run(() =>
                    {
                        ProcessNotifyFCM.GetInstance().SendNotifyByFunction("vi-VN", StringUtil.enumFCMFunction.DMTAISAN_CHODUYET_YC.ToString(), pushId);
                    }
                   ).ConfigureAwait(false);
                }
                //---------------------------------------------------------------------------------------------------
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".SendApprovalNhapKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalTaiSan(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_DMTaiSan_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
                logger.Error(className + ".ApprovalNhapKho Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;



        }
        internal bool RejectTaiSan(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".RejectNhapKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool CapNhatGiaTriTaiSan(int id, decimal GiaTriTaiSan, int idLoaiBaoHiem, int idNhomTaiSan, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GiaTriTaiSan", GiaTriTaiSan);
                param.Add("IDLoaiBaoHiem", idLoaiBaoHiem);
                param.Add("IDNhomTaiSan", idNhomTaiSan);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_UpdateGiaTriTS", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Bạn không có quyền cập nhật giá trị tài sản";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".CapNhatGiaTriTaiSan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal DataTable TraCuuLichSuKyDuyetTaiSan(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTaiSan", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyetTaiSan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public bool AddOrUpdateDanhMucLoaiBaoHiem(int id, string ma, string ten, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaLoaiBaoHiem", ma);
                param.Add("TenLoaiBaoHiem", ten);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMLoaiBaoHiem_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã " + ma;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucLoaiBaoHiem Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucNhomNhaCungCap(int id, string ma, string ten, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaNhomNhaCungCap", ma);
                param.Add("TenNhomNhaCungCap", ten);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMNhaCungCapNhom_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã " + ma;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucNhomNhaCungCap Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }

        public bool UpdaeDanhMucConfig(int id, string ma, string ten, string ghiChu, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("MaConfig", ma);
                param.Add("GiaTriConfig", ten);
                param.Add("GhiChu", ghiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMConfig_Update", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã " + ma;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".UpdaeDanhMucConfig Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }

        public bool UpdateHoatDong(int id, int hopLe, decimal quangDuongHopLe, string ghiChuThayDoi, string userLogin, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("HopLe", hopLe);
                param.Add("QuangDuongHopLe", quangDuongHopLe);
                param.Add("GhiChuThayDoi", ghiChuThayDoi);
                param.Add("ID", id);
                param.Add("UserLogin", userLogin);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QL_Activities_Update", System.Data.CommandType.StoredProcedure, param) + "");

                if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".UpdateHoatDong Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucSanPham(int id, string loaiSP, int idNhomSP, string ma, string ten, decimal donGia, int idDonViTinh, string ghiChu, string ghiChuHighLight, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("LoaiSanPham", loaiSP);
                param.Add("IDNhomSanPham", idNhomSP);
                param.Add("MaSanPham", ma);
                param.Add("TenSanPham", ten);
                param.Add("DonGia", donGia);
                param.Add("IDDonViTinh", idDonViTinh);
                param.Add("GhiChu", ghiChu);
                param.Add("GhiChuHighLight", ghiChuHighLight);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMSanPham_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại mã " + ma;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucNhomSanPham Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public bool AddOrUpdateDanhMucQLDangKyGuiThuChiPhi(int id, int IDTinhThanhTu,
                int IDTinhThanhDen,
                int TrongLuongTu,
                int TrongLuongDen,
                string HinhThucGui,
                decimal ChiPhiGuiThu,
                decimal PhuPhiVungSau,
                decimal PhuPhiXangDau,
                decimal PhuPhiKhac,
                decimal PhiVAT,
                decimal TongChiPhi,
                string GhiChu,
                string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("IDTinhThanhTu", IDTinhThanhTu);
                param.Add("IDTinhThanhDen", IDTinhThanhDen);
                param.Add("TrongLuongTu", TrongLuongTu);
                param.Add("TrongLuongDen", TrongLuongDen);
                param.Add("HinhThucGui", HinhThucGui);
                param.Add("ChiPhiGuiThu", ChiPhiGuiThu);
                param.Add("PhuPhiVungSau", PhuPhiVungSau);
                param.Add("PhuPhiXangDau", PhuPhiXangDau);
                param.Add("PhuPhiKhac", PhuPhiKhac);
                param.Add("PhiVAT", PhiVAT);
                param.Add("TongChiPhi", TongChiPhi);
                param.Add("GhiChu", GhiChu);
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLDangKyGuiThuChiPhi_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại tỉnh thành từ đến ";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateDanhMucNhomSanPham Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        internal DataTable DMThanhToan4Copy(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "Sp_QLThanhToan_DMSaoChep", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMThanhToan4Copy() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable DMTamUng4Copy(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "Sp_QLTamUng_DMSaoChep", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMTamUng4Copy() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        #region suport ProcessDanhMuc
        public enum eTenDanhMuc
        {
            cfgBranches, cfgDepartments, cfgUsersHCQT, cfgUsers, DMTrangThaiTaiSan, DMTrangThaiTaiSanNhapKhoThuHoi,
            dm_run_group, app_user_registed,
            ql_race, ql_news, cd_race_and_run_group, cd_race_and_run_group_and_user,
            cd_activities, ql_activities,DmHoatDong,
            DMTaiSan, DMLoaiTaiSan, DMNhomTaiSan, DMDonViTinh,
            DMNhaSanXuat, DMNhaCungCap, DMNhaCungCapNhom,
            DMHinhThucSuDungTaiSan, DMLoaiThanhToan, DMLoaiHopDong, CfThoiHanHopDong,
            DMViTri, DMLoaiBaoHiem, DMDoiTuong,
            DMTinhThanh, DMQuanHuyen, DMPhuongXa, DMPhongBan_HCQT,
            QLNhapKho, QLNhapKhoCT,
            QLBanGiaoTaiSan, QLBanGiaoTaiSanCT,
            QLThuHoiTaiSan, QLThuHoiTaiSanCT,
            QLDieuChinhKho, QLDieuChinhKhoCT,
            QLDieuChinhKhoNhanVien, QLDieuChinhKhoNhanVienCT,
            QLHopDong, QLThanhToan, QLTamUng,
            QLThanhLy, QLThanhLyCT, QLThanhToanCT,
            QLKiemKeKho, QLKiemKeKhoCT,
            QLChuyenKho, QLChuyenKhoCT,
            IssueType,// loai yeu cau
            QLYeuCau,
            TrangThaiBanGiao,
            UserManager,
            PermisionReferIssue,
            PermisionTaskIssue,
            IssueTrangThaiCapNhat,
            IssueTrangThaiThucHien,
            IssueTrangThaiAll,
            // PDF Replace
            ReplaceTextPDF,
            DMSanPham, QLDangKyVpp, QLDangKyVppCT, WebUploadFile,
            DMNhomSanPham,
            QLNhapKhoSanPham, QLNhapKhoSanPhamCT,
            QLBanGiaoSanPham, QLBanGiaoSanPhamCT,
            QLDangKyGuiThu, QLDangKyGuiThuChiPhi,
            ql_race_group_user_register,
            ql_nghi_phep
        }

        public enum eTenColumMasterID
        {
            IDKiemKe
            /*
        cfgBranches, cfgDepartments, cfgUsersHCQT, DMTrangThaiTaiSan, cfgUsers,
        DMKho,
        DMTaiSan, DMLoaiTaiSan, DMNhomTaiSan, DMDonViTinh,
        DMNhaSanXuat, DMNhaCungCap, DMNhaCungCapNhom,
        DMHinhThucSuDungTaiSan, DMLoaiThanhToan, DMLoaiHopDong,
        DMViTri, DMLoaiBaoHiem,
        QLNhapKho, QLNhapKhoCT,
        QLBanGiaoTaiSan, QLBanGiaoTaiSanCT,
        QLThuHoiTaiSan, QLThuHoiTaiSanCT,
        QLDieuChinhKho, QLDieuChinhKhoCT,
        QLDieuChinhKhoNhanVien, QLDieuChinhKhoNhanVienCT,
        QLHopDong, QLThanhToan, QLTamUng,
        QLThanhLy, QLThanhLyCT, QLThanhToanCT,
        QLKiemKeKho, QLKiemKeKhoCT
        */
        }


        public TableInfo GetTableInfo(string tenDanhMuc)
        {
            TableInfo info = new TableInfo();
            if (tenDanhMuc.ToUpper() == "DMKho".ToUpper())
            {
                info.TableName = "DMKho"; info.ID = "ID"; info.MaDM = "MaKho"; info.TenDM = "TenKho";
            }

            else if (tenDanhMuc.ToUpper() == "dm_run_group".ToUpper())
            {
                info.TableName = "dm_run_group"; info.ID = "ID"; info.MaDM = "GroupCode"; info.TenDM = "GroupName";
            }

            else if (tenDanhMuc.ToUpper() == "cfgBranches".ToUpper())
            {
                info.TableName = "cfgBranches"; info.ID = "ID"; info.MaDM = "Code"; info.TenDM = "BranchName";
            }
            else if (tenDanhMuc.ToUpper() == "cfgDepartments".ToUpper())
            {
                info.TableName = "cfgDepartments"; info.ID = "ID"; info.MaDM = "Code"; info.TenDM = "DeptName";
            }
            else if (tenDanhMuc.ToUpper() == "cfgUsers".ToUpper())
            {
                info.TableName = "cfgUsers"; info.ID = "ID"; info.MaDM = "UserName"; info.TenDM = "FullName";
            }
            else if (tenDanhMuc.ToUpper() == "cfgUsersHCQT".ToUpper())
            {
                info.TableName = "cfgUsers"; info.ID = "ID"; info.MaDM = "UserName"; info.TenDM = "FullName";
            }
            else if (tenDanhMuc.ToUpper() == "ql_activities".ToUpper())
            {
                info.TableName = "ql_activities"; info.ID = "ID"; info.MaDM = "id_strava"; info.TenDM = "total_distance_met_valid";
            }

            return info;
        }
        public class TableInfo
        {
            public string TableName;
            public string ID;
            public string MaDM;
            public string TenDM;
            public string FormPhanQuyen;
        }
        #endregion

        private DataTable dtDanhMucGio = null;
        public DataTable getDanhMucGio()
        {
            if (dtDanhMucGio == null)
            {
                dtDanhMucGio = new DataTable("dtGio");
                dtDanhMucGio.Columns.Add("TenGio", typeof(int));
                for (int i = 1; i <= 24; i++)
                {
                    DataRow radd = dtDanhMucGio.NewRow();
                    radd["TenGio"] = i;
                    dtDanhMucGio.Rows.Add(radd);
                }
            }
            return dtDanhMucGio;
        }
        private DataTable dtDanhMucPhut = null;
        public DataTable getDanhMucPhut()
        {
            if (dtDanhMucPhut == null)
            {
                dtDanhMucPhut = new DataTable("dtPhut");
                dtDanhMucPhut.Columns.Add("TenPhut", typeof(int));
                for (int i = 0; i <= 12; i++)
                {
                    DataRow radd = dtDanhMucPhut.NewRow();
                    radd["TenPhut"] = i * 5;
                    dtDanhMucPhut.Rows.Add(radd);
                }
            }
            return dtDanhMucPhut;
        }
        private DataTable dtThuTrongTuan = null;
        public DataTable getThuTrongTuan()
        {
            if (dtThuTrongTuan == null)
            {
                dtThuTrongTuan = new DataTable("dtThu");
                dtThuTrongTuan.Columns.Add("MaThu", typeof(string));
                dtThuTrongTuan.Columns.Add("TenThu", typeof(string));
                for (int i = 2; i <= 7; i++)
                {
                    DataRow radd = dtThuTrongTuan.NewRow();
                    radd["MaThu"] = "T" + i.ToString();
                    radd["TenThu"] = "Thứ " + i.ToString();
                    dtThuTrongTuan.Rows.Add(radd);
                }
                DataRow raddCN = dtThuTrongTuan.NewRow();
                raddCN["MaThu"] = "CN";
                raddCN["TenThu"] = "Chủ nhật";
                dtThuTrongTuan.Rows.Add(raddCN);
            }
            return dtThuTrongTuan;
        }

        private DataTable dtHinhThucDongTien = null;
        public DataTable getHinhThucDongTien()
        {
            if (dtHinhThucDongTien == null)
            {
                try
                {

                    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "select ParaCode Code,ParaValue Name from cfgParams where ParaType='HTDongTien'", CommandType.Text, null);
                }
                catch (Exception ex)
                {
                    logger.Error(className.ToString() + ".LoadDanhMuc4ComboFromCache() Error: ");
                    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                    return null;
                }
            }
            return dtThuTrongTuan;
        }

        public DataTable BaoCaoTonKho(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_TonKho_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoTonKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoTonKhoSanPham(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_TonKhoSanPham_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoTonKhoSanPham() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public DataTable BaoCaoTonKhoTongHopFromCache(string key4Search, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "BaoCaoTonKhoTongHopFromCache_" + userLogin + "_";
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_TonKhoTongHop_Search", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCacheAutoRemove(keyCache, dtRet, MyCachePriority.Default, 2 * 60); // 2 phut
                }
                return dtRet;

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoTonKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable DMTaiSanSearchFromCache(string key4Search, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var key = "DMTaiSan";
                var objCache = cache.GetMemCachedItem(key);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                var dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Search", CommandType.StoredProcedure, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cache.AddToMemCache(key, dt, MyCachePriority.Default);
                }
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMTaiSanSearch() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public DataTable DMTaiSan4ComboFromCache(string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var key = "DMTaiSan4ComboFromCache";
                var objCache = cache.GetMemCachedItem(key);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                var dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Load4Combo", CommandType.StoredProcedure, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cache.AddToMemCache(key, dt, MyCachePriority.Default);
                }
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMTaiSan4ComboFromCache() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable DMSanPham4ComboFromCache(string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var key = "DMSanPham4ComboFromCache";
                var objCache = cache.GetMemCachedItem(key);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                var dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMSanPham_Load4Combo", CommandType.StoredProcedure, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cache.AddToMemCache(key, dt, MyCachePriority.Default);
                }
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMSanPham4ComboFromCache() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable DMTaiSanSearchThayDoiFromCache(string key4Search, string userLogin)
        {
            try
            {
                //BusinessMemCache cache = new BusinessMemCache();
                //var key = "DMTaiSan_ThayDoi";
                //var objCache = cache.GetMemCachedItem(key);
                //if (objCache != null)
                //{
                //    return (DataTable)objCache;
                //}
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                var dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_Search_ThayDoi", CommandType.StoredProcedure, param);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    cache.AddToMemCache(key, dt, MyCachePriority.Default);
                //}
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMTaiSanSearchThayDoiFromCache() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable DMTaiSanSearchDuyetThayDoiFromCache(string key4Search, string userLogin)
        {
            try
            {
                //BusinessMemCache cache = new BusinessMemCache();
                //var key = "DMTaiSan_Duyet_ThayDoi";
                //var objCache = cache.GetMemCachedItem(key);
                //if (objCache != null)
                //{
                //    return (DataTable)objCache;
                //}
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                var dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSan_SearchDuyet_ThayDoi", CommandType.StoredProcedure, param);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    cache.AddToMemCache(key, dt, MyCachePriority.Default);
                //}
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".DMTaiSanSearchDuyetThayDoiFromCache() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal bool DeleteThayDoiTaiSan(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);

                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSanCT_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLNhapKho + "", userLoginId, out message);
        }

        internal bool SendApprovalThayDoiTaiSan(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSanCT_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".SendApprovalNhapKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }

        public bool AddOrUpdateThayDoiTaiSan(int id, int idTaiSan, string noiDung, string ghiChu, DateTime ngayThayDoi, string userLoginId, out string message, out int idInertNew)
        {
            message = ""; idInertNew = id;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("NoiDungThayDoi", noiDung);
                param.Add("GhiChu", ghiChu);
                param.Add("NgayThayDoi", ngayThayDoi);
                param.Add("ID", id);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("UserLogin", userLoginId);
                int retCode = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSanCT_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu";
                }
                else if (retCode == -3)
                {
                    message = "Đã tồn tại nội dung " + noiDung;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInertNew = retCode;
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".AddOrUpdateThayDoiTaiSan Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }

        internal DataTable TraCuuChiTietThayDoiTS(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDTaiSan", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSanCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuNhapKhoChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal bool ApprovalThayDoiTaiSan(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_DMTaiSanCT_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
                logger.Error(className + ".ApprovalThayDoiTaiSan Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;



        }
        internal bool RejectThayDoiTaiSan(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_DMTaiSanCT_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".RejectNhapKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }


        public DataTable LoadDanhMucQuanHuyenTheoTinhThanh(string IdTinhThanh, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LoadDanhMucQuanHuyenTheoTinhThanh" + "_" + IdTinhThanh;
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IdTinhThanh", Utils.NumberUtil.ParseToInt(IdTinhThanh));
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_DMQuanHuyenByMaTinhThanh", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }
                return dtRet;

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LoadDanhMucQuanHuyenTheoTinhThanh() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        public DataTable LoadDanhMucPhuongXaTheoQuanHuyen(string IdQuanHuyen, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LoadDanhMucPhuongXaTheoQuanHuyen" + "_" + IdQuanHuyen;
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IdQuanHuyen", Utils.NumberUtil.ParseToInt(IdQuanHuyen));
                param.Add("UserLogin", userLogin);
                var dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "sp_DMPhuongXaByMaQuanHuyen", CommandType.StoredProcedure, param);
                if (dtRet != null)
                {
                    cache.AddToMemCache(keyCache, dtRet, MyCachePriority.Default);
                }
                return dtRet;

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LoadDanhMucPhuongXaTheoQuanHuyen() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
    }

}