using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;
using Newtonsoft.Json.Linq;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for QLDonHangBanAction
    /// </summary>
    public class QLDonHangBanAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLDonHangBanAction));
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool success = false;
            int retCode = -1;
            string message = "";
            int id = 0;
            JObject retObject = new JObject();
            try
            {
                string sessionUserId = Utils.UserUtil.GetSessionUserId();

                id = Utils.NumberUtil.ParseToInt(context.Request.Form["ID"]);

                string mode = context.Request.Form["mode"].Trim().ToUpper();
                if (mode == "EDIT")
                {
                    string subMode = context.Request.Form["subMode"].Trim().ToUpper();

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessDonHangBan.getInstance().GetDonHangBanByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }

                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "SAVETEMP".ToUpper())
                {
                    // Save header + details from session into DB via ProcessDonHangBan
                    int idKhachHang = Utils.NumberUtil.ParseToInt(context.Request.Form["idKhachHang"] + "");
                    string ghiChu = context.Request.Form["ghiChu"] + "";
                    int idInsertNew = 0;
                    string trangThai = "NEW";
                    success = false;
                    try
                    {
                       // DataTable dtDetails = context.Session["ssInvoiceDetails"] as DataTable;
                        if (idKhachHang<=0)
                        {
                            success = false;
                            message = "Vui lòng chọn khách hàng";
                        }
                        else
                        {
                            string msg = "";
                            success = DataProcess.ProcessDonHangBan.getInstance().SaveHoaDonBan(Utils.NumberUtil.ParseToInt(context.Request.Form["id"] + ""), idKhachHang, ghiChu, trangThai, sessionUserId, out msg, out idInsertNew);
                            message = msg;
                            if (success)
                            {
                                id = idInsertNew;
                                // refresh session-stored details from DB so session remains valid after save
                                try
                                {
                                    DataTable dtReload = DataProcess.ProcessDonHangBan.getInstance().TraCuuDonHangBanChiTiet(idInsertNew, sessionUserId);
                                    context.Session["ssInvoiceDetails"] = dtReload;
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        message = ex.Message;
                        logger.Error(ex);
                    }
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper()) && id == 0))
                    {
                        //if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoNhap"]) <= 0)
                        //{
                        //    message = "Chưa chọn kho điều chỉnh";
                        //    success = false;
                        //}
                        //else 
                        if (0 == 1 && Utils.NumberUtil.ParseToInt(context.Request.Form["IDChiNhanh"]) <= 0)
                        {
                            message = "Chưa nhập tên chi nhánh";
                            success = false;
                        }
                        //else if (context.Request.Form["DoiTuongDuocChi"] + "" == "")
                        //{
                        //    message = "Chưa nhập đối tượng được chi";
                        //    success = false;
                        //}
                        else
                        {
                            string content = context.Request.Form["Contents"] + "";
                            int idInsertUpdate = 0;
                            success = AddOrUdateDonHangBan(id

                                , context.Request.Form["Code"] + ""
                                , context.Request.Form["Subject"] + ""
                                , Utils.NumberUtil.ParseToDate(context.Request.Form["NewsDate"])

                                , context.Request.Form["Description"] + ""
                                , content
                                 , Utils.NumberUtil.ParseToInt(context.Request.Form["IdRace"])
                                , sessionUserId, out message, out idInsertUpdate);
                            if (success) { message = "Lưu thành công"; }
                            // truong hop them moi thi tra ve lại ID mới thêm vào database
                            if (id <= 0) id = idInsertUpdate;
                        }
                    }
                    else
                        message = "Bạn không có quyền thêm hoặc chỉnh sửa chức năng này";

                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        success = DeleteDonHangBan(id, sessionUserId, out message);
                        if (success) { message = "Xóa thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "SendApproval".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(QLDonHangBan).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        success = SendApprovalDonHangBan(id, sessionUserId, out message);
                        if (success) { message = "Gửi duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "Approval".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        success = ApprovalDonHangBan(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "Reject".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        success = RejectDonHangBan(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Từ chối thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "AddOrUpDateDetail".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper()) && id == 0))
                    {
                        if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"]) <= 0)
                        {
                            message = "Chưa chọn tài sản";
                            success = false;
                        }
                        else
                        {
                            int idInsertUpdate = 0;
                            int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);

                            success = AddOrUdateDonHangBanChiTiet(id, idMaster
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDTrangTaiSan"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["SoLuong"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["DonGiaMua"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaTriConLai"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaTriThuHoi"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoXuat"])
                                , context.Request.Form["PhuongAnXuLy"] + ""
                                , context.Request.Form["GhiChuChiTiet"] + ""
                                , sessionUserId, out message, out idInsertUpdate);
                            if (success) { message = "Lưu chi tiết thành công"; }

                            // truong hop them moi thi tra ve lại ID mới thêm vào database
                            if (id <= 0) id = idInsertUpdate;
                        }
                    }
                    else
                        message = "Bạn không có quyền thêm hoặc chỉnh sửa chức năng này";

                }
                else if (mode == "DELETEDETAIL")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                        success = DeleteDonHangBanChiTiet(id, idMaster, sessionUserId, out message);
                        if (success) { message = "Xóa chi tiết tài sản thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "EDITDETAIL")
                {
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLDonHangBan).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessDonHangBan.getInstance().GetDonHangBanChiTietByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                #region lien quan tap tin
                else if (mode == "deleteAttachment".ToUpper())
                {
                    int attId = int.Parse(context.Request.Params["attId"]);
                    DataTable dtbAttach = DataProcess.ProcessAttachment.GetAttachmentById(attId, Utils.UserUtil.GetSessionUserId());
                    if (dtbAttach != null && dtbAttach.Rows.Count > 0)
                    {
                        string filepath = dtbAttach.Rows[0]["FilePath"] + "";
                        if (System.IO.File.Exists(filepath))
                        {
                            string recycleBinFolder = Config.SysConfig.ATTACHMENTS_RECYCLEBIN_FOLDER_CO_ODIA_NOT_YYYYMM + "\\" + DateTime.Now.ToString("yyyyMM");
                            if (!System.IO.Directory.Exists(recycleBinFolder)) System.IO.Directory.CreateDirectory(recycleBinFolder);
                            string deletedPath = recycleBinFolder + "\\" + new FileInfo(filepath).Name;

                            // thực hiện xóa nè
                            success = DataProcess.ProcessAttachment.Remove(attId, deletedPath, Utils.UserUtil.GetSessionUserId(), ref retCode);
                            if (!success)
                            {
                                if (retCode == -1)
                                    message = "Không tồn tại tập tin đính kèm.";
                                else if (retCode == -2)
                                    message = "Trạng thái hiện tại không cho phép xóa.";
                                else if (retCode == -3)
                                    message = "Bạn không thể xóa những tập tin đã tải lên trước khi bị trả lại yêu cầu.";
                                else if (retCode == -4)
                                    message = "Trạng thái chờ duyệt hoặc đang chờ phân công thực hiện yêu cầu, bạn không thể xóa tập tin.";
                                else if (retCode == -5)
                                    message = "Bạn không thể xóa tập tin đã tải lên trước khi yêu cầu đã duyệt.";
                                else if (retCode == -6)
                                    message = "Yêu cầu hỗ trợ đã bị từ chối, bạn không thể xóa tập tin đã tải lên trong này.";
                                else
                                    message = "Xóa tập tin bị lỗi, bạn vui lòng thử lại hoặc liên hệ phòng CNTT để được hỗ trợ.";

                                logger.Error(Utils.UserUtil.GetSessionUserId() + " không thể xóa tập tin. Id = " + attId + " | Lý do (" + retCode + "): " + message);
                            }
                            else
                            {
                                System.IO.File.Move(filepath, deletedPath);
                            }
                        }
                        else
                            message = "Tập tin đã bị xóa khỏi ổ đĩa lưu trữ, bạn vui lòng thử lại hoặc liên hệ phòng CNTT để được hỗ trợ.";
                    }
                    else
                        message = "Tập tin không tồn tại hoặc bạn không có quyền truy cập.";
                }
                else if (mode == "downloadAtt")
                {
                    int attId = int.Parse(context.Request.Params["ID"]);
                    DataTable dtbAttach = DataProcess.ProcessAttachment.GetAttachmentById(attId, Utils.UserUtil.GetSessionUserId());
                    if (dtbAttach != null && dtbAttach.Rows.Count > 0)
                    {
                        string filePath = dtbAttach.Rows[0]["FilePath"] + "";
                        if (!System.IO.File.Exists(filePath))
                        {
                            logger.Error(Utils.UserUtil.GetSessionUserId() + " không thể tải tập tin không tồn tại. Id = " + attId + " | Đường dẫn" + filePath);
                        }
                        else
                        {
                            context.Response.Clear();
                            context.Response.AddHeader("Content-Disposition", "filename=" + (dtbAttach.Rows[0]["AttachName"] + "" + "").Replace(" ", ""));
                            context.Response.AddHeader("Content-Length", dtbAttach.Rows[0]["FileSize"] + "");
                            context.Response.ContentType = dtbAttach.Rows[0]["ContentType"] + "";
                            context.Response.TransmitFile(dtbAttach.Rows[0]["FilePath"] + "");
                            context.Response.Flush();
                            return;
                        }

                    }
                }
                else if (mode == "checkAtt".ToUpper())
                {
                    message = "";
                    success = true;
                }
                #endregion ket thuc lien quan tap tin 
                else
                    message = "Invalid mode !";

            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error("ProcessRequest Error");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                retObject["success"] = success;
                retObject["retCode"] = retCode;
                retObject["message"] = success ? (message == "" ? "Thực hiện thành công" : message) : (message == "" ? "Thực hiện thất bại" : message);
                retObject["id"] = id;
            }
            context.Response.Write(retObject.ToString());
        }
        private bool AddOrUdateDonHangBan(int id, string ma, string ten, DateTime ngayLap, string ghiChu, string NoiDungDonHangBan, int idRace, string userLogin, out string message, out int idInsertNew)
        {

            return DataProcess.ProcessDonHangBan.getInstance().AddOrUpdate(id, ma, ten, ngayLap, ghiChu, NoiDungDonHangBan, idRace, userLogin, out message, out idInsertNew);
        }
        private bool DeleteDonHangBan(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDonHangBan.getInstance().Delete(id, userLogin, out message);
        }
        private bool SendApprovalDonHangBan(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDonHangBan.getInstance().SendApprovalDonHangBan(id, userLogin, out message);
        }
        private bool ApprovalDonHangBan(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDonHangBan.getInstance().ApprovalDonHangBan(id, ghiChu, userLogin, out message);
        }
        private bool RejectDonHangBan(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDonHangBan.getInstance().RejectDonHangBan(id, ghiChu, userLogin, out message);
        }
        private bool AddOrUdateDonHangBanChiTiet(int id, int IDDonHangBan, int idTaiSanChiTiet, int idTrangThaiTS, decimal soLuong, decimal DonGiaMua, decimal GiaTriConLai
           , decimal GiaTriThuHoi, int idKhoXuat, string PhuongAnXuLy, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {

            return DataProcess.ProcessDonHangBan.getInstance().AddOrUpdateDonHangBanChiTiet(id, IDDonHangBan, idTaiSanChiTiet, idTrangThaiTS, soLuong, DonGiaMua, GiaTriConLai
           , GiaTriThuHoi, idKhoXuat, PhuongAnXuLy, GhiChuChiTiet, userLogin, out message, out idInsertNew);
        }
        private bool DeleteDonHangBanChiTiet(int idCT, int idMaster, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDonHangBan.getInstance().DeleteChiTietDonHangBan(idCT, idMaster, userLogin, out message);

        }

        //private DataTable DSTaiSanThayDoiKhiDuyet(int idMaster, string userLogin)
        //{

        //    return DataProcess.ProcessDonHangBan.getInstance().DSTaiSanThayDoiSoLuongKhiDuyet(idMaster, userLogin);
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}