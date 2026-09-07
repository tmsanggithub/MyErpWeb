using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebRunDragon.DataProcess
{
    public class ProcessTuan
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTuan));
        static string className = typeof(ProcessTuan).Name;

        private static ProcessTuan _instance;
        public static ProcessTuan getInstance()
        {
            if (_instance == null)
                _instance = new ProcessTuan();
            return _instance;
        }

        public DataTable LoadDanhSachGiaiChay(string userLogin)
        {
            try
            {
                string sql = @"select id, race_code as code, race_name as name
                               from ql_race
                               where isdeleted = 0
                               order by id";
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                logger.Error(className + ".LoadDanhSachGiaiChay Error: " + ex.Message);
                return null;
            }
        }

        public DataTable TraCuuTuan(string keyword, string userLogin)
        {
            try
            {
                string sql = @"select t.id, t.id_race, r.race_name as race_name, t.tuan,
                                      convert(varchar(10), t.from_date, 103) as from_date,
                                      convert(varchar(10), t.to_date, 103) as to_date,
                                      t.ghi_chu
                               from dm_config_tuan t
                               inner join ql_race r on t.id_race = r.id
                               where t.IsDeleted = 0
                               and (@keyword = '' or t.tuan like N'%' + @keyword + '%' or r.race_name like N'%' + @keyword + '%')
                               order by t.id desc";
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("keyword", keyword + "");
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                logger.Error(className + ".TraCuuTuan Error: " + ex.Message);
                return null;
            }
        }

        public JObject GetTuanByID(int id)
        {
            try
            {
                string sql = @"select id, id_race, tuan,
                                      convert(varchar(10), from_date, 103) as from_date,
                                      convert(varchar(10), to_date, 103) as to_date,
                                      ghi_chu
                               from dm_config_tuan
                               where id = @id and IsDeleted = 0";

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("id", id);
                JArray arr = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                if (arr != null && arr.Count > 0) return (JObject)arr[0];
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".GetTuanByID Error: " + ex.Message);
                return null;
            }
        }

        public bool AddOrUpdate(int id, int idRace, string tuan, DateTime fromDate, DateTime toDate, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            try
            {
                if (idRace <= 0)
                {
                    message = "Chưa chọn giải chạy";
                    return false;
                }

                if ((tuan + "").Trim() == "")
                {
                    message = "Chưa nhập tuần";
                    return false;
                }

                if (fromDate > toDate)
                {
                    message = "Từ ngày không được lớn hơn đến ngày";
                    return false;
                }

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("id_race", idRace);
                param.Add("tuan", tuan);
                param.Add("from_date", fromDate);
                param.Add("to_date", toDate);
                param.Add("ghi_chu", ghiChu);
                param.Add("userLogin", userLogin);

                string sql;
                if (id <= 0)
                {
                    sql = @"insert into dm_config_tuan (id_race, tuan, from_date, to_date, ghi_chu, IsDeleted, CreatedBy, CreatedTime)
                            values (@id_race, @tuan, @from_date, @to_date, @ghi_chu, 0, @userLogin, getdate());
                            select scope_identity();";
                }
                else
                {
                    param.Add("id", id);
                    sql = @"update dm_config_tuan
                            set id_race = @id_race,
                                tuan = @tuan,
                                from_date = @from_date,
                                to_date = @to_date,
                                ghi_chu = @ghi_chu,
                                UpdatedBy = @userLogin,
                                UpdatedTime = getdate()
                            where id = @id and IsDeleted = 0;
                            select @id;";
                }

                idInsertNew = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param) + "");
                if (idInsertNew <= 0) message = "Lưu dữ liệu thất bại";
                return idInsertNew > 0;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error(className + ".AddOrUpdate Error: " + ex.Message);
                return false;
            }
        }

        public bool Delete(int id, string userLogin, out string message)
        {
            message = "";
            try
            {
                string sql = @"update dm_config_tuan
                               set IsDeleted = 1,
                                   DeletedBy = @userLogin,
                                   DeletedTime = getdate()
                               where id = @id and IsDeleted = 0;
                               select @@rowcount;";

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("id", id);
                param.Add("userLogin", userLogin);
                int rows = Utils.NumberUtil.ParseToInt(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param) + "");
                if (rows <= 0) message = "Xóa thất bại";
                return rows > 0;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error(className + ".Delete Error: " + ex.Message);
                return false;
            }
        }
    }
}
