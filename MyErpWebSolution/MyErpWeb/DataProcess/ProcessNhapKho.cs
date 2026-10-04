using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebRunDragon.Utils;
using System.Threading.Tasks;

namespace WebRunDragon.DataProcess
{
    public class ProcessNhapKho
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessNhapKho));
        static string className = typeof(ProcessNhapKho).Name;
        private static ProcessNhapKho _instance;
        public static ProcessNhapKho getInstance()
        {
            if (_instance == null)
                _instance = new ProcessNhapKho();
            return _instance;
        }
        public DataTable TraCuuNhapKho(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuNhapKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetNhapKhoByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLNhapKho + "");
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
                param.Add("GhiChu", ghiChu + "");
                param.Add("UserLogin", userLogin);

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_AddOrUdate", System.Data.CommandType.StoredProcedure, param);
                int retCode = 0; int retID = 0;
                if (dtRet != null && dtRet.Rows.Count >= 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    retID = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][1]);
                }
                //int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Đã tồn tại mã số phiếu nhập kho.";
                }
                else if (retCode == -100)
                {
                    message = "Kho nhập có phiếu đang xử lý (ID=" + retID + "). Vui lòng xử lý trước khi làm phiếu mới.";
                }
                else if (retCode == -101)
                {
                    message = "Kho nhập có 'phiếu kiểm kê Kho' đang xử lý (id=" + retID + ").\n Vui thực hiện 'kiểm kê kho' trước khi tạo phiếu mới.";
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
                    var strCode = "NhapKho_AddOrUpdate_" + Math.Abs(retCode).ToString();
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

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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
                    var strCode = "NhapKho_Delete_" + Math.Abs(retCode).ToString();
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLNhapKho + "", userLoginId, out message);
        }
        internal bool SendApprovalNhapKho(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

               
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_SendApproval", System.Data.CommandType.StoredProcedure, param);
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
                    var strCode = "NhapKho_SendApproval_" + Math.Abs(retCode).ToString();
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
                        ProcessNotifyFCM.GetInstance().SendNotifyByFunction("vi-VN", StringUtil.enumFCMFunction.NHAPKHO_CHODUYET_YC.ToString(), pushId);
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
        internal bool ApprovalNhapKho(int id, string ghiChu, string userLoginId, out string message)
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
                DataTable dtRet = DatabaseManager.SQLGetDataTable("SP_QLNhapKho_Approval", System.Data.CommandType.StoredProcedure, ref tran, paras);
                int retCode = 0; int retId = 0;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    retId = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][1]);
                }



                if (retCode == -2)
                {
                    message = "Chỉ cho duyệt khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == -3)
                {
                    message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
                }
                else if (retCode == -101)
                {
                    message = "Tài sản đang được điều chỉnh tại 'phiếu điều chỉnh kho' ID=" + retId;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "NhapKho_Approval_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //----------------------------------------------------------------------

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



            //message = "";

            //try
            //{
            //    Dictionary<string, object> param = new Dictionary<string, object>();
            //    param.Add("ID", id);
            //    param.Add("GhiChu", ghiChu);
            //    param.Add("UserLogin", userLoginId);

            //    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_Approval", System.Data.CommandType.StoredProcedure, param) + "";
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
            //    logger.Error(className + ".ApprovalNhapKho Error: ");
            //    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            //    return false;
            //}
        }
        internal bool RejectNhapKho(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_Reject", System.Data.CommandType.StoredProcedure, param) + "";
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
                    var strCode = "NhapKho_Rject_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".RejectNhapKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool AddOrUpdateNhapKhoChiTiet(int ID, int IDNhapKho, int idTaiSanChiTiet, int SoLuong, decimal DonGia
           , decimal ThanhTien, int idTrangThaiTaiSan, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDNhapKho", IDNhapKho);
                param.Add("IDTaiSanChiTiet", idTaiSanChiTiet);
                param.Add("SoLuong", SoLuong);
                param.Add("DonGia", DonGia);
                param.Add("ThanhTien", ThanhTien);
                param.Add("IDTrangThaiTaiSan", idTrangThaiTaiSan);
                param.Add("GhiChuChiTiet ", GhiChuChiTiet + "");
                param.Add("UserLogin", userLogin);

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKhoCT_AddOrUpdate", System.Data.CommandType.StoredProcedure, param);
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
                else if (retCode == -5)
                {
                    message = "Đã tồn tại tài sản này, vui lòng chọn tài sản khác";
                }
                else if (retCode == -101)
                {
                    message = "Tài sản đang được điều chỉnh tại 'phiếu điều chỉnh kho' ID=" + retId;
                }
                else if (retCode == -102)
                {
                    message = "Tài sản đã được thêm trong phiếu nhập kho hiện tại";
                }
                else if (retCode == -103)
                {
                    message = "Tài sản đang được nhập kho tại 'phiếu nhập kho' ID=" + retId;
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
                    var strCode = "NhapKho_AddOrUpdateChiTiet_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".AddOrUpdateNhapKhoChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietNhapKho(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetNhapKhoByID(idMaster);
            if (entity != null)
            {
                if (entity["TrangThai"] + "" == "NEW" || entity["TrangThai"] + "" == "EDIT" || entity["TrangThai"] + "" == "RETURNED")
                {
                    return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLNhapKhoCT + "", userLoginId, out message);
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
        internal DataTable TraCuuNhapKhoChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDNhapKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKhoCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuNhapKhoChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal DataTable LayDanhSachTaiSan4ThemNhapKho(int idNhapKho, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDNhapKho", idNhapKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKhoCT_DSTaiSan4ThemNhapKho", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemNhapKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetNhapKhoChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLNhapKhoCT + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDNhapKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKho_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public DataTable BaoCaoNhapKho(DateTime tuNgay, DateTime denNgay, int idChiNhanh, int idKho, int idTaiSan, string soPhieuNhapKho, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("TuNgay", tuNgay);
                param.Add("DenNgay", denNgay);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("IDKho", idKho);
                param.Add("IDTaiSan", idTaiSan);
                param.Add("SoPhieuNhapKho", soPhieuNhapKho);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_NhapKho", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".BaoCaoNhapKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        internal DataTable GetDMTaiSanImportNhapKho(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKhoCT_Import_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImportNhapKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        //internal bool UploadImport( out string msg)
        //{
        //    bool ret = false;
        //    msg = string.Empty;

        //       DataTable dt = new DataTable();
        //    string conStr = "";
        //    switch (Extension)
        //    {
        //        case ".xls": //
        
        
        //            conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
        //            break;
        //        case ".xlsx": //Excel 07  
        //            conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
        //            break;
        //    }
        //    OleDbConnection connExcel = null;
        //    try
        //    {
        //        // kiem tra ton tai data da import thì ko cho thuc hien, phai xoa het truoc khi import
        //        DataTable dtImported = ProcessQR.getInstance().GetDMTaiSanImportQR(Utils.UserUtil.GetSessionUserId());
        //        if (dtImported != null && dtImported.Rows.Count > 0)
        //        {
        //            msg = "Đã tồn tại dữ liệu import. Bạn không được phép tiếp tục import.Vui lòng xóa dữ liệu hiện tại nếu muốn import khác\n(Tra cứu để xem dữ liệu hiện tại)";
        //            return false;
        //        }

        //        conStr = String.Format(conStr, FilePath);
        //        connExcel = new OleDbConnection(conStr);
        //        OleDbCommand cmdExcel = new OleDbCommand();
        //        OleDbDataAdapter oda = new OleDbDataAdapter();
        //        cmdExcel.Connection = connExcel;
        //        //Get the name of First Sheet  
        //        connExcel.Open();
        //        DataTable dtExcelSchema;
        //        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //        connExcel.Close();
        //        //Read Data from First Sheet  
        //        connExcel.Open();
        //        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        //        oda.SelectCommand = cmdExcel;
        //        oda.Fill(dt);
        //        connExcel.Close();
        //        //Bind Data to GridView  
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Error("Lỗi xãy ra khi import thông tin cổ đông. Mã cổ đông cuối cùng đọc được: " + lastShareId, ex);
        //        // MessageBox.Show(this, "Có lỗi khi mở tập tin " + filePath + ", vui lòng thử lại. Mã cổ đông cuối cùng trước khi xãy ra lổi: " + lastShareId, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        if (connExcel != null) connExcel.Close();
        //    }

        //    // autosave table

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        dt.TableName = "QLNhapKhoCT_Import";
        //        DataTable dtSave = new DataTable("QLNhapKhoCT_Import");
        //        dtSave.Columns.Add("ID", typeof(int));
        //        dtSave.Columns.Add("MaTaiSan", typeof(string));
        //        dtSave.Columns.Add("MaTrangThaiTaiSan", typeof(string));
        //        dtSave.Columns.Add("SoLuong", typeof(int));
        //        dtSave.Columns.Add("DonGia", typeof(decimal));
        //        dtSave.Columns.Add("GhiChu", typeof(string));
        //        dtSave.Columns.Add("TrangThai", typeof(string));
        //        foreach (DataRow r in dt.Rows)
        //        {
        //            DataRow radd = dtSave.NewRow();
        //            radd["ID"] = -1;
        //            radd["MaTaiSan"] = r["MaTaiSan"];
        //            radd["MaTrangThaiTaiSan"] = r["MaTrangThaiTaiSan"];
        //            radd["SoLuong"] = r["SoLuong"];
        //            radd["DonGia"] = r["DonGia"];
        //            radd["GhiChu"] = r["GhiChu"];
        //            radd["TrangThai"] = "NEW";

        //            dtSave.Rows.Add(radd);
        //        }
        //        ret = DatabaseManager.AutoSaveDataTable(DatabaseManager.CNN_STRING_HELPDESK, dtSave, "QLNhapKhoCT_Import");
        //        if (ret)
        //        {                
        //            //FileUpload1.PostedFile.InputStream.Dispose();
        //            //FileUpload1.Dispose();
        //        }
        //        else
        //        {
        //            msg = "import thất bại";
        //        }

        //    }
        //    return ret;
        //}

        public bool DeleteAllTaiSanImport(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin);
                return DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKhoCT_Import_DeleteAll", CommandType.StoredProcedure, param) + "" != "";
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        public DataTable DSMaTaiSanHopLe()
        {
            try
            {
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "select mataisan from dmtaisan where isdeleted=0", CommandType.Text,null);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        public bool SaveAllTaiSanImport(string userLogin, int IDNhapKho, out string message)
        {
            message = string.Empty;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin + "");
                param.Add("IDNhapKho", IDNhapKho);
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLNhapKhoCT_Import_SaveAll", CommandType.StoredProcedure, param);
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
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
    }
}