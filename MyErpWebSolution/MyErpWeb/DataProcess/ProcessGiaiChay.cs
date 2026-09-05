using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Linq;
using System.Web; 
using System.Drawing; 
using System.IO;
using System.Diagnostics;

namespace WebRunDragon.DataProcess
{
    public class ProcessGiaiChay
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessGiaiChay));
        static string className = typeof(ProcessGiaiChay).Name;
        private static ProcessGiaiChay _instance;
        public static ProcessGiaiChay getInstance()
        {
            if (_instance == null)
                _instance = new ProcessGiaiChay();
            return _instance;
        }
        public DataTable TraCuuGiaiChay(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuGiaiChay() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetGiaiChayByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.ql_race + "");
        }
        internal bool AddOrUpdate(int id, string ma, string ten, DateTime ngayLap, DateTime tuNgay, DateTime denNgay, string ghiChu, string noiDungGiaiChay, string userLogin, int DangDienRa, out string message, out int idInsertNew)
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
                param.Add("RaceInfoHtml", noiDungGiaiChay);
                param.Add("DangDienRa", DangDienRa);
                param.Add("UserLogin", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_AddOrUdate", System.Data.CommandType.StoredProcedure, param) + "";
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

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLGiaiChay + "", userLoginId, out message);
        }
        internal bool SendApprovalGiaiChay(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_SendApproval", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".SendApprovalGiaiChay Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalGiaiChay(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_QLGiaiChay_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);


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
                logger.Error(className + ".ApprovalGiaiChay Error: ");
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

            //    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_Approval", System.Data.CommandType.StoredProcedure, param) + "";
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
            //    logger.Error(className + ".ApprovalGiaiChay Error: ");
            //    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            //    return false;
            //}
        }
        internal bool RejectGiaiChay(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                logger.Error(className + ".RejectGiaiChay Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool AddOrUpdateGiaiChayChiTiet(int ID, int IDGiaiChay, int idNhomChay, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDGiaiChay", IDGiaiChay);
                param.Add("IDNhomChay", idNhomChay);
                param.Add("UserLogin", userLogin);


                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChayCT_AddOrUpdateNhomChay", System.Data.CommandType.StoredProcedure, param);
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
                logger.Error(className + ".AddOrUpdateGiaiChayChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietGiaiChay(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetGiaiChayByID(idMaster);
            if (entity != null)
            {
                //if (entity["status"] + "" == "NEW" || entity["status"] + "" == "EDIT" || entity["status"] + "" == "RETURNED")
                //{
                return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "", userLoginId, out message);
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
        internal DataTable TraCuuGiaiChayChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDGiaiChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChayCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuGiaiChayChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable LayDanhSachTaiSan4ThemGiaiChay(int idGiaiChay, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDGiaiChay", idGiaiChay);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChayCT_DSTaiSan4ThemGiaiChay", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemGiaiChay() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetGiaiChayChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDGiaiChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoGiaiChay(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idKho, int idTaiSan, string soPhieuGiaiChay, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKho", idKho);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("SoPhieuGiaiChay", soPhieuGiaiChay);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_GiaiChay", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoGiaiChay() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal DataTable InGiaiChayTaiSan(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDGiaiChay", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_In", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".InGiaiChayTaiSan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal DataTable GetTapTinDinhKem(int idHopDong, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDGiaiChay", idHopDong);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLGiaiChay_DSTapTinDinhKem", CommandType.StoredProcedure, param);
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
            return ProcessAttachment.AddOrUpdate(attId, "QLGiaiChay", issueId, attName, filePath, contentType, fileSize, extension, userId, "", folderExt, ref retCode);
        }
        // ...existing code...

        /// <summary>
        /// Converts an HTML file to a PNG image using wkhtmltoimage.exe.
        /// </summary>
        /// <param name="htmlFilePath">Path to the HTML file.</param>
        /// <param name="outputPngPath">Path to save the PNG file.</param>
        /// <param name="width">Width of the output image.</param>
        /// <param name="height">Height of the output image.</param>
        public void ConvertHtmlToPng(string htmlFilePath, string outputPngPath, int width = 800, int height = 600)
        {
            // Đường dẫn tới wkhtmltoimage.exe, cần đảm bảo tool này đã được cài đặt trên server
            string wkhtmltoimagePath = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltoimage.exe";
            if (!File.Exists(wkhtmltoimagePath))
                throw new FileNotFoundException("Không tìm thấy wkhtmltoimage.exe tại: " + wkhtmltoimagePath);

            var psi = new ProcessStartInfo
            {
                FileName = wkhtmltoimagePath,
                Arguments = $"--width {width} --height {height} \"{htmlFilePath}\" \"{outputPngPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("Lỗi khi convert HTML sang PNG. ExitCode: " + process.ExitCode);
                }
            }
        }
        // ...existing code...

        /// <summary>
        /// Converts an HTML file to a PDF file using wkhtmltopdf.exe.
        /// </summary>
        /// <param name="htmlFilePath">Path to the HTML file.</param>
        /// <param name="outputPdfPath">Path to save the PDF file.</param>
        /// <param name="width">(Optional) Page width in pixels.</param>
        /// <param name="height">(Optional) Page height in pixels.</param>
        public void ConvertHtmlToPdf(string htmlFilePath, string outputPdfPath, int width = 0, int height = 0)
        {
            // Đường dẫn tới wkhtmltopdf.exe, cần đảm bảo tool này đã được cài đặt trên server
            string wkhtmltopdfPath = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe";
            if (!File.Exists(wkhtmltopdfPath))
                throw new FileNotFoundException("Không tìm thấy wkhtmltopdf.exe tại: " + wkhtmltopdfPath);

            // Tùy chọn thêm --enable-local-file-access nếu HTML có tham chiếu file cục bộ
            string options = "--enable-local-file-access";
            // Nếu muốn set kích thước trang, có thể thêm các option sau (không bắt buộc)
            if (width > 0) options += $" --page-width {width}px";
            if (height > 0) options += $" --page-height {height}px";

            var psi = new ProcessStartInfo
            {
                FileName = wkhtmltopdfPath,
                Arguments = $"{options} \"{htmlFilePath}\" \"{outputPdfPath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("Lỗi khi convert HTML sang PDF. ExitCode: " + process.ExitCode);
                }
            }
        }
    }
}