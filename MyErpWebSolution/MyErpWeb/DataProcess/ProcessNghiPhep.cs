using System;
using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;

namespace WebRunDragon.DataProcess
{
    public class ProcessNghiPhep
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessNghiPhep));
        static string className = typeof(ProcessNghiPhep).Name;

        private static ProcessNghiPhep _instance;
        public static ProcessNghiPhep getInstance()
        {
            if (_instance == null)
                _instance = new ProcessNghiPhep();
            return _instance;
        }

        /// <summary>
        /// Load danh sách nhân viên ?ã ??ng ký Strava cho ComboBox
        /// select r.id_strava as id, u.StaffNo code, u.StaffName name
        /// from app_user_registed r inner join cfgUsers u on r.staff_no_vdsc=u.StaffNo
        /// where u.IsDeleted=0 and r.is_deleted=0
        /// </summary>
        public DataTable LoadDanhSachNhanVien(string userLogin)
        {
            try
            {
                string sql = @"select r.id_strava as id, u.StaffNo code, u.StaffName name
                               from app_user_registed r
                               inner join cfgUsers u on r.staff_no_vdsc = u.StaffNo
                               where u.IsDeleted = 0 and r.is_deleted = 0
                               order by u.StaffName";

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, null);
            }
            catch (Exception ex)
            {
                logger.Error(className + ".LoadDanhSachNhanVien Error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Tìm ki?m danh sách ngh? phép
        /// </summary>
        public DataTable TraCuuNghiPhep(string keyword, string userLogin)
        {
            try
            {
                string sql = @"select p.id, p.id_strava
                                 ,convert(nvarchar(10), p.ngay_nghi_tu,103) ngay_nghi_tu, convert(nvarchar(10),p.ngay_nghi_den,103) ngay_nghi_den
                                  , p.ghi_chu,
                                      u.staff_no_vdsc, us.StaffName
                               from ql_nghi_phep p
                               inner join app_user_registed u on p.id_strava = u.id_strava
                               inner join cfgUsers us on us.StaffNo = u.staff_no_vdsc
                               where p.isdeleted = 0
                               and (@keyword = '' or us.StaffName like '%' + @keyword + '%' or us.StaffNo like '%' + @keyword + '%')
                               order by p.ngay_nghi_tu desc";

                var param = new Dictionary<string, object>();
                param.Add("keyword", keyword + "");
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
            }
            catch (Exception ex)
            {
                logger.Error(className + ".TraCuuNghiPhep Error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// L?y thông tin ngh? phép theo ID
        /// </summary>
        public JObject GetNghiPhepByID(int id)
        {
            try
            {
                string sql = @"select p.id, p.id_strava, 
                                      convert(varchar(10), p.ngay_nghi_tu, 103) as ngay_nghi_tu,
                                      convert(varchar(10), p.ngay_nghi_den, 103) as ngay_nghi_den,
                                      p.ghi_chu
                               from ql_nghi_phep p
                               where p.id = @id and p.isdeleted = 0";

                var param = new Dictionary<string, object>();
                param.Add("id", id);
                var dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    var obj = new JObject();
                    foreach (DataColumn col in dt.Columns)
                        obj[col.ColumnName] = row[col.ColumnName] + "";
                    return obj;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".GetNghiPhepByID Error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Thêm m?i ho?c c?p nh?t ngh? phép
        /// </summary>
        public bool AddOrUpdate(int id, string idStrava, DateTime ngayNghiTu, DateTime ngayNghiDen, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            try
            {
                string sql;
                var param = new Dictionary<string, object>();
                param.Add("id_strava", idStrava);
                param.Add("ngay_nghi_tu", ngayNghiTu);
                param.Add("ngay_nghi_den", ngayNghiDen);
                param.Add("ghi_chu", ghiChu);
                param.Add("userLogin", userLogin);

                if (id <= 0)
                {
                    sql = @"insert into ql_nghi_phep (id_strava, ngay_nghi_tu, ngay_nghi_den, ghi_chu, isdeleted, createdby, createdtime)
                            values (@id_strava, @ngay_nghi_tu, @ngay_nghi_den, @ghi_chu, 0, @userLogin, getdate());
                            select scope_identity();";
                }
                else
                {
                    param.Add("id", id);
                    sql = @"update ql_nghi_phep set id_strava=@id_strava, ngay_nghi_tu=@ngay_nghi_tu, ngay_nghi_den=@ngay_nghi_den,
                                    ghi_chu=@ghi_chu, updatedby=@userLogin, updatedtime=getdate()
                            where id=@id and isdeleted=0;
                            select @id;";
                }

                object ret = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, sql, CommandType.Text, param);
                idInsertNew = Utils.NumberUtil.ParseToInt(ret + "");
                if (idInsertNew <= 0)
                    message = "Lưu dữ liệu thất bại";
                return idInsertNew > 0;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error(className + ".AddOrUpdate Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Xóa m?m ngh? phép
        /// </summary>
        public bool Delete(int id, string userLogin, out string message)
        {
            message = "";
            try
            {
                string sql = @"update ql_nghi_phep set isdeleted=1, deletedby=@userLogin, deletedtime=getdate() where id=@id;
                               select @@rowcount;";
                var param = new Dictionary<string, object>();
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
