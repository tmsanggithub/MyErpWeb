using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebRunDragon.Utils;

namespace WebRunDragon.DataProcess
{
    public class ProcessChuyenKho
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessChuyenKho));
        static string className = typeof(ProcessChuyenKho).Name;
        private static ProcessChuyenKho _instance;
        public static ProcessChuyenKho getInstance()
        {
            if (_instance == null)
                _instance = new ProcessChuyenKho();
            return _instance;
        }
        public DataTable TraCuuChuyenKho(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuChuyenKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetChuyenKhoByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLChuyenKho + "");
        }
        internal bool AddOrUpdate(int id, int idChiNhanh, DateTime NgayChuyen, int idDonViChuyen, string IDNguoiChuyen, string IDTruongDonViChuyen, int idDonViNhan, string IDNguoiNhan, string IDTruongDonViNhan
            , string ghiChu, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("IDChiNhanh", idChiNhanh);
                param.Add("NgayChuyen", NgayChuyen);

                param.Add("IDDonViChuyen", idDonViChuyen);
                param.Add("IDNguoiChuyen", IDNguoiChuyen);
                param.Add("IDTruongDonViChuyen", IDTruongDonViChuyen);

                param.Add("IDDonViNhan", idDonViNhan);
                param.Add("IDNguoiNhan", IDNguoiNhan);
                param.Add("IDTruongDonViNhan", IDTruongDonViNhan);

                param.Add("GhiChu", ghiChu + "");
                param.Add("UserLogin", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_AddOrUdate", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Đã tồn tại mã số phiếu chuyển tài sản";
                }
                //else if (retCode == -22)
                //{
                //    message = "Người nhận này đã có phiếu bàn chuyển đang xử lý";
                //}
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo','chỉnh sửa' hoặc 'trả về'";
                }
                //else if (retCode == -102)
                //{
                //    message = "Nhân viên nhận chuyển đang có 'phiếu điều chỉnh tài sản NV' cần xử lý";
                //}
                //else if (retCode == -101)
                //{
                //    message = "Nhân viên nhận bàn chuyển đang có 'phiếu điều chỉnh Kho' cần xử lý";
                //}
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInsertNew = retCode;


                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "ChuyenKho_AddOrUpdate_" + Math.Abs(retCode).ToString();
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

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_Delete", System.Data.CommandType.StoredProcedure, param) + "";
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
                    var strCode = "ChuyenKho_Delete_" + Math.Abs(retCode).ToString();
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

            //  return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLChuyenKho + "", userLoginId, out message);
        }
        internal bool SendApprovalChuyenKho(int id, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("UserLogin", userLoginId);

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_SendApproval", System.Data.CommandType.StoredProcedure, param);
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
                    var strCode = "ChuyenKho_SendApproval_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //--gui pushnotify đến người duyệt--------------------------------------------------------------------
                /*
                if (retCode > 0 && !string.IsNullOrEmpty(pushId))
                {
                    Task.Run(() =>
                    {
                        ProcessNotifyFCM.GetInstance().SendNotifyByFunction("vi-VN", StringUtil.enumFCMFunction.BANGIAO_CHODUYET_YC.ToString(), pushId);
                    }
                   ).ConfigureAwait(false);
                }
                */
                //---------------------------------------------------------------------------------------------------
                return retCode > 0;
            }
            catch (Exception ex)
            {
                logger.Error(className + ".SendApprovalChuyenKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalChuyenKho(int id, string ghiChu, string userLoginId, out string message)
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
                DataTable dtRet = DatabaseManager.SQLGetDataTable("SP_QLChuyenKho_ApprovalChuyenKho", System.Data.CommandType.StoredProcedure, ref tran, paras);

                int retCode = 0;
                string pushId = string.Empty;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    pushId = dtRet.Rows[0][1]?.ToString();
                }

                if (retCode == -2)
                {
                    message = "Chỉ cho duyệt khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == -3)
                {
                    message = "Không đúng nhân sự được gán duyệt./n(Không phải là trưởng đơn vị của người gửi duyệt)";
                }
                else if (retCode == -5)
                {
                    message = "Số lượng tồn kho không đủ để chuyển kho\nVui lòng kiểm tra tồn kho sản phẩm";
                }
                //else if (retCode == -101)
                //{
                //    message = "Nhân viên nhận bàn giao đang có 'phiếu điều chỉnh Kho' cần xử lý";
                //}
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

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "BanGiao_Approval_" + Math.Abs(retCode).ToString();
                    message = ResourceUtil.GetString(strCode, Constants.DEFAULT_LANGUAGE);
                }
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    message += "\nMã lổi:" + retCode;
                }
                //--gui pushnotify đến người duyet nhan tai san--------------------------------------------------------------------
                /*
                if (result && !string.IsNullOrEmpty(pushId))
                {
                    Task.Run(() =>
                    {
                        ProcessNotifyFCM.GetInstance().SendNotifyByFunction("vi-VN", StringUtil.enumFCMFunction.BANGIAO_CHODUYET_NHAN_TS.ToString(), pushId);
                    }
                   ).ConfigureAwait(false);
                }
                */
                //---------------------------------------------------------------------------------------------------
                //----------------------------
            }
            catch (Exception er)
            {
                if (tran != null) tran.Rollback();
                logger.Error(className + ".ApprovalChuyenKho Error: ");
                logger.Error("Exception message: " + er.Message + ". Stack trace: " + er.StackTrace);
                return false;
            }
            finally
            {
                DatabaseManager.CloseDbConnection(cnn);
            }
            return result;


        }
        internal bool RejectChuyenKho(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_Reject", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không đúng nhân sự được gán duyệt. (Không phải là trưởng đơn vị giao)";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho duyệt(từ chối) khi trạng thái là 'gửi duyệt'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "BanGiao_Reject_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".RejectChuyenKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool ApprovalNhanTaiSanBanGiao(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_ApprovalAccept", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không đúng nhân sự được gán duyệt (Không phải là trưởng đơn vị của người gửi duyệt)";

                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho duyệt(từ chối) khi trạng thái là 'Chờ duyệt nhận tài sản'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "ChuyenKho_ApprovalNhanTS_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".RejectChuyenKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool RejectNhanTaiSanBanGiao(int id, string ghiChu, string userLoginId, out string message)
        {
            message = "";

            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", id);
                param.Add("GhiChu", ghiChu);
                param.Add("UserLogin", userLoginId);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_RejectAccept", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không đúng nhân sự được gán duyệt nhận tài sản (Không phải là trưởng đơn vị nhận)";

                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho duyệt(từ chối) khi trạng thái là 'Chờ duyệt nhận tài sản'";
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "BanGiao_RejectNhanTS_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".RejectChuyenKho Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }

        internal bool NhapKhoTaiSanTraLai(int id, string ghiChu, string userLoginId, out string message)
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
                object objRet = DatabaseManager.SQLExecScalar("SP_QLChuyenKho_NhapKhoTaiSanTraLai", System.Data.CommandType.StoredProcedure, ref tran, paras);


                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không đúng nhân sự được quyền nhập kho (Không phải là trưởng đơn vị giao)";

                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho nhập kho khi trạng thái là 'Từ chối nhận tài sản'";
                }
                else if (retCode == -101)
                {
                    message = "Nhân viên nhận bàn giao đang có 'phiếu điều chỉnh Kho' cần xử lý";
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

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "BanGiao_NhapKhoTSTraLai_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".ApprovalChuyenKho Error: ");
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

            //    object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_NhapKhoTaiSanTraLai", System.Data.CommandType.StoredProcedure, param) + "";
            //    int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
            //    if (retCode == -2)
            //    {
            //        message = "Không đúng nhân sự được gán duyệt nhận tài sản (Không phải là trưởng đơn vị nhận)";

            //    }
            //    else if (retCode == -3)
            //    {
            //          message = "Chỉ cho nhập kho khi trạng thái là 'Từ chối nhận tài sản'";
            //    }
            //    else if (retCode == 0)
            //    {
            //        message = "Thất bại trong quá trình lưu dữ liệu";
            //    }
            //    return retCode > 0;
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(className + ".RejectChuyenKho Error: ");
            //    logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            //    return false;
            //}
        }
        internal bool AddOrUpdateChuyenKhoChiTiet(int ID, int IDChuyenKho, int IDKhoXuat, int idTaiSanChiTiet, int SoLuong
           , int idTrangThaiTaiSanXuatKho, int idTrangThaiTaiSanNhapKho, int IDKhoNhap, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            message = "";
            idInsertNew = 0;
            try
            {
                /*
                @ID int ,
                @IDChuyenKho int ,
                @IDKhoXuat int ,
                @IDTaiSanChiTiet int ,
                @TrangThaiCu int ,
                @TrangThaiMoi int ,
                @SoLuong int ,
                @IDKhoNhan int ,
                @GhiChuChiTiet nvarchar(250),
                @UserLogin nvarchar(50)
                 */

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("ID", ID);
                param.Add("IDChuyenKho", IDChuyenKho);
                param.Add("IDKhoXuat", IDKhoXuat);
                param.Add("IDTaiSanChiTiet", idTaiSanChiTiet);
                param.Add("SoLuong", SoLuong);
                param.Add("TrangThaiCu", idTrangThaiTaiSanXuatKho);
                param.Add("TrangThaiMoi", idTrangThaiTaiSanNhapKho);
                param.Add("IDKhoNhan", IDKhoNhap);
                param.Add("GhiChuChiTiet ", GhiChuChiTiet + "");
                param.Add("UserLogin", userLogin);

                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKhoCT_AddOrUpdate", System.Data.CommandType.StoredProcedure, param);
                int retCode = 0; int retId = 0;
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    retCode = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][0]);
                    retId = Utils.NumberUtil.ParseToInt(dtRet.Rows[0][1]);
                }
                if (retCode == -1)
                {
                    message = "Kho xuất và kho nhập không được giống nhau";
                }
                else if (retCode == -2)
                {
                    message = "Không tồn tại ID để cập nhật dữ liệu";
                }
                else if (retCode == -22)
                {
                    message = "Đã tồn tại tài sản chuyển kho này, vui lòng chọn tài sản khác";
                }
                else if (retCode == -3)
                {
                    message = "Chỉ cho phép lưu khi trạng thái 'mới tạo' hoặc 'chỉnh sửa'";
                }
                //else if (retCode == -102)
                //{
                //    message = "Tài sản đang được điều chỉnh tại 'phiếu điều chỉnh tài sản NV' (id=" + retId + ")";
                //}
                //else if (retCode == -101)
                //{
                //    message = "Tài sản đang được điều chỉnh tại 'phiếu điều chỉnh Kho' (id=" + retId + ")";
                //}
                //else if (retCode == 0)
                //{
                //    message = "Thất bại trong quá trình lưu dữ liệu";
                //}
                else
                    idInsertNew = retCode;

                // false: lay message tu resource--------------------------------------
                if (retCode < 0 && string.IsNullOrEmpty(message))
                {
                    var strCode = "BanGiao_AddOrUpdateCT_" + Math.Abs(retCode).ToString();
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
                logger.Error(className + ".AddOrUpdateChuyenKhoChiTiet Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        internal bool DeleteChiTietChuyenKho(int id, int idMaster, string userLoginId, out string message)
        {
            message = "";
            JObject entity = this.GetChuyenKhoByID(idMaster);
            if (entity != null)
            {
                if (entity["TrangThai"] + "" == "NEW" || entity["TrangThai"] + "" == "EDIT" || entity["TrangThai"] + "" == "RETURNED")
                {
                    return DataProcess.ProcessCommonEntities.getInstance().DeleteEntityByIDNotCheckConstrainKey(id, ProcessDanhMuc.eTenDanhMuc.QLChuyenKhoCT + "", userLoginId, out message);
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
        internal DataTable TraCuuChuyenKhoChiTiet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDChuyenKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKhoCT_SearchByIDMaster", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuChuyenKhoChiTiet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        //internal DataTable LayDanhSachTaiSan4ThemChuyenKho(int idChuyenKho, string userLogin)
        //{
        //    try
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("IDBanGiao", idChuyenKho);
        //        param.Add("UserLogin", userLogin);

        //        return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKhoCT_DSTaiSan4ThemBanGiao", CommandType.StoredProcedure, param);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(className.ToString() + ".LayDanhSachTaiSan4ThemChuyenKho() Error: ");
        //        logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
        //        return null;
        //    }
        //}
        internal DataTable LayDanhSachTaiSanChuyenKhoFromCache(int IDKhoTraCuu, string userLogin)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                var keyCache = "LayDanhSachTaiSanChuyenKho_" + IDKhoTraCuu;
                var objCache = cache.GetMemCachedItem(keyCache);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDKhoTraCuu", IDKhoTraCuu);
                param.Add("UserLogin", userLogin);
                DataTable dt = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLTonKho_TraCuu2BanGiaoTS", CommandType.StoredProcedure, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cache.AddToMemCacheAutoRemove(keyCache, dt, MyCachePriority.Default, 2 * 60);// 3 phut
                }
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".LayDanhSachTaiSanChuyenKho() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        internal JObject GetChuyenKhoChiTietByID(int id)
        {
            return DataProcess.ProcessCommonEntities.getInstance().GetEntityByID(id, ProcessDanhMuc.eTenDanhMuc.QLChuyenKhoCT + "");
        }
        internal DataTable TraCuuLichSuKyDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDChuyenKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_SearchApprovalLog", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        //public DataTable BaoCaoBanGiao(DateTime tuNgay, DateTime denNgay, int idDonViNhan, string nguoiNhan, int taiSan, string soPhieu, string trangThaiBanGiao, string userLogin)
        //{
        //    try
        //    {
        //        Dictionary<string, object> param = new Dictionary<string, object>();
        //        param.Add("TuNgay", tuNgay);
        //        param.Add("DenNgay", denNgay);
        //        param.Add("IDDonViNhan", idDonViNhan);
        //        param.Add("IDNguoiNhan", nguoiNhan);
        //        param.Add("IDTaiSanBanGiao", taiSan);
        //        param.Add("SoPhieuBanGiao", soPhieu);
        //        param.Add("IDTrangThaiBanGiao", trangThaiBanGiao);
        //        param.Add("UserLogin", userLogin);

        //        return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_BC_ChuyenKho", CommandType.StoredProcedure, param);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(className.ToString() + ".BaoCaoBanGiao() Error: ");
        //        logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
        //        return null;
        //    }
        //}


        internal DataSet TraCuuThongTinCanDuyet(int idMaster, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("IDChuyenKho", idMaster);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataSetSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKho_GetInfoApproval", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuLichSuKyDuyet() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        /*

        // danh cho import tai san ban giao-----------------------------------------------------------------------------------------
        internal DataTable GetDMTaiSanImportBanGiao(string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKhoCT_Import_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImportBanGiao() Error: ");
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
                return DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKhoCT_Import_DeleteAll", CommandType.StoredProcedure, param) + "" != "";
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }
        public bool SaveAllTaiSanImport(string userLogin, int IDBanGiao, out string message)
        {
            message = string.Empty;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserLogin", userLogin + "");
                param.Add("IDBanGiao", IDBanGiao);
                DataTable dtRet = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_QLChuyenKhoCT_Import_SaveAll", CommandType.StoredProcedure, param);
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
                
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".GetDMTaiSanImport() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return false;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        */
    }
}