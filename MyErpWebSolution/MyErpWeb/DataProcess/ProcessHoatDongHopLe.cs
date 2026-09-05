using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessHoatDongHopLe
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessHoatDongHopLe));
        static string className = typeof(ProcessHoatDongHopLe).Name;
        private static ProcessHoatDongHopLe _instance;
        public static ProcessHoatDongHopLe getInstance()
        {
            if (_instance == null)
                _instance = new ProcessHoatDongHopLe();
            return _instance;
        }
        public DataTable TraCuuHoatDongHopLe(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cd_activities_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuHoatDongHopLe() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetHoatDongHopLeByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.cd_activities + "");
        }
        internal bool Delete(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);

                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CDHoatDongHopLe_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.CDHoatDongHopLe + "", userLoginId, out message);
        }
        internal bool SendApprovalHoatDongHopLe(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CDHoatDongHopLe_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".SendApprovalHoatDongHopLe Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool AddOrUpdate(
            int id,
            string sportType,
            int distanceMin,
            int distanceMaxInDay,
            int activitiesMinInWeek,
            int averageSpeedFrom,
            int averageSpeedTo,
            decimal moneyPerKilomet,
            DateTime lamViecBuoiSangTu,
            DateTime lamViecBuoiSangDen,
            DateTime lamViecBuoiChieuTu,
            DateTime lamViecBuoiChieuDen,
            int maxMinuteInDayOfLaixe,
            string description,
            string userLogin,
            out string message,
            out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            try
            {
                // Khai báo các tham số truyền vào cho stored procedure
                Dictionary<string, object> param = new Dictionary<string, object>
                    {
                        { "ID", id },
                        { "SportType", sportType },
                        { "DistanceMin", distanceMin },
                        { "DistanceMaxInDay", distanceMaxInDay },
                        { "ActivitiesMinInWeek", activitiesMinInWeek },
                        { "AverageSpeedFrom", averageSpeedFrom },
                        { "AverageSpeedTo", averageSpeedTo },
                        { "MoneyPerKilomet", moneyPerKilomet },
                        { "LamViecBuoiSangTu", lamViecBuoiSangTu },
                        { "LamViecBuoiSangDen", lamViecBuoiSangDen },
                        { "LamViecBuoiChieuTu", lamViecBuoiChieuTu },
                        { "LamViecBuoiChieuDen", lamViecBuoiChieuDen },
                        { "MaxMinuteInDayOfLaixe", maxMinuteInDayOfLaixe },
                        { "Description", description },
                        { "UserLogin", userLogin }
                    };

                // Gọi stored procedure bằng DatabaseManager
                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_sport_type_AddOrUpdate", System.Data.CommandType.StoredProcedure, param
                ) + "";

                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");

                // Xử lý kết quả trả về
                if (retCode == -2)
                {
                    message = "Đã tồn tại mã loại thể thao.";
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
                {
                    idInsertNew = retCode; // Trả về ID khi thêm mới thành công
                }

                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdateSportType Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalHoatDongHopLe(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_CDHoatDongHopLe_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
                logger.Error(className + ".ApprovalHoatDongHopLe Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;


        }
        internal bool RejectHoatDongHopLe(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CDHoatDongHopLe_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".RejectHoatDongHopLe Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }

        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDHoatDongHopLe", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CDHoatDongHopLe_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoHoatDongHopLe(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idKho, int idTaiSan, string soPhieuHoatDongHopLe, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKho", idKho);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("SoPhieuHoatDongHopLe", soPhieuHoatDongHopLe);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_HoatDongHopLe", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoHoatDongHopLe() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

    }
}