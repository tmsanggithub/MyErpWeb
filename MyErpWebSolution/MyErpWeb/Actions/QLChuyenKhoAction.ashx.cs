using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for QLChuyenKhoAction
    /// </summary>
    public class QLChuyenKhoAction : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLChuyenKhoAction));
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool success = false;
            int retCode = -1;//2 waring
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

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanSpecialAction(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessChuyenKho.getInstance().GetChuyenKhoByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper()) && id == 0))
                    {
                        if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViChuyen"]) <= 0)
                        {
                            message = "Chưa có đơn vị chuyển tài sản";
                            success = false;
                        }
                        else if (context.Request.Form["IDNguoiChuyen"] + "" == "" || context.Request.Form["IDNguoiChuyen"] + "" == "null")
                        {
                            message = "Chưa nhập người giao tài sản";
                            success = false;
                        }
                        else if (context.Request.Form["IDTruongDonViChuyen"] + "" == "" || context.Request.Form["IDTruongDonViChuyen"] + "" == "null")
                        {
                            message = "Chưa nhập trưởng đơn vị giao tài sản";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViNhan"]) <= 0)
                        {
                            message = "Chưa có đơn vị nhận tài sản";
                            success = false;
                        }
                        else if (context.Request.Form["IDNguoiNhan"] + "" == "" || context.Request.Form["IDNguoiNhan"] + "" == "null")
                        {
                            message = "Chưa nhập người nhận tài sản";
                            success = false;
                        }
                        else if (context.Request.Form["IDTruongDonViNhan"] + "" == "" || context.Request.Form["IDTruongDonViNhan"] + "" == "null")
                        {
                            message = "Chưa nhập trưởng đơn vị nhận tài sản";
                            success = false;
                        }

                        else
                        {
                            int idInsertUpdate = 0;
                            success = AddOrUdateChuyenKho(id, Utils.NumberUtil.ParseToInt(context.Request.Form["IDChiNhanh"])
                                , Utils.NumberUtil.ParseToDate(context.Request.Form["NgayChuyen"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViChuyen"]), context.Request.Form["IDNguoiChuyen"] + "", context.Request.Form["IDTruongDonViChuyen"] + ""
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViNhan"]), context.Request.Form["IDNguoiNhan"] + "", context.Request.Form["IDTruongDonViNhan"] + ""
                                , context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertUpdate);
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
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = DeleteChuyenKho(id, sessionUserId, out message);
                        if (success) { message = "Xóa thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "SendApproval".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        // lay thong tin nguoi duyet: gui notify, check xem co nguoi duyet ko

                        success = SendApprovalChuyenKho(id, sessionUserId, out message);
                        if (success)
                        {
                            message = "Gửi duyệt thành công";

                            // gui notify
                        }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "Approval".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = ApprovalChuyenKho(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "Reject".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = RejectChuyenKho(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Từ chối thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "ApprovalAcceptAsset".ToUpper())
                {
                    if (Utils.ActionUtil.CanSpecialAction(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = ApprovalNhanTaiSanBanGiao(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "RejectAcceptAsset".ToUpper())
                {
                    if (Utils.ActionUtil.CanSpecialAction(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = RejectNhanTaiSanBanGiao(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "NhapKhoTaiSanTraLai".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = NhapKhoTaiSanTraLai(id, context.Request.Form["GhiChuNhapKhoTSTraLai"] + "", sessionUserId, out message);
                        if (success) { message = "Nhập kho thành công"; }
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này (quyền duyệt)";
                }
                /*
                else if (mode == "AddOrUpdateDetailCheck".ToUpper())
                {
                    success = true;
                    int idKhoXuat = Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoXuat"]);
                    int soLuongBanGiao = Utils.NumberUtil.ParseToInt(context.Request.Form["SoLuong"]);
                    int idTrangThai = Utils.NumberUtil.ParseToInt(context.Request.Form["IDTrangThaiTaiSanXuatKho"]);
                    int idTaiSan = Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"]);
                    DataTable dtAmKho = DataProcess.ProcessChuyenKho.getInstance().TraCuuAmKhoKhiBanGiao(idKhoXuat, idTaiSan, idTrangThai, soLuongBanGiao, sessionUserId);
                    if (dtAmKho != null && dtAmKho.Rows.Count > 0)
                    {
                        retCode = 2;// warning
                        message = "Số lượng tồn kho sẽ bị âm khi bàn giao tài sản này";
                    }
                }
				*/
                else if (mode == "AddOrUpDateDetail".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper()) && id == 0))
                    {
                        if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"]) <= 0)
                        {
                            message = "Chưa chọn tài sản bàn giao";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["SoLuong"]) <= 0)
                        {
                            message = "Chưa nhập số lượng";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["TrangThaiCu"]) <= 0)
                        {
                            message = "Chưa chọn trạng thái tài sản Xuất kho";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["TrangThaiMoi"]) <= 0)
                        {
                            message = "Chưa chọn trạng thái tài sản Nhân viên";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToDecimal(context.Request.Form["IDKhoXuat"]) <= 0)
                        {
                            message = "Chưa chọn kho xuất";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoNhap"]) <= 0)
                        {
                            message = "Chưa chọn vị trí";
                            success = false;
                        }
                        else
                        {
                            int idInsertUpdate = 0;
                            int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                            success = AddOrUdateChuyenKhoChiTiet(id, idMaster
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoXuat"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["SoLuong"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["TrangThaiCu"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["TrangThaiMoi"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoNhap"])
                                , context.Request.Form["GhiChuChiTiet"], sessionUserId, out message, out idInsertUpdate);
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
                    int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        success = DeleteChuyenKhoChiTiet(id, idMaster, sessionUserId, out message);
                        if (success) { message = "Xóa chi tiết tài sản thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "EDITDETAIL")
                {
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLChuyenKho).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessChuyenKho.getInstance().GetChuyenKhoChiTietByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                // danh cho import-------------------------------------------------------------------------
                //else if (mode.ToUpper() == "DELETEIMPORT".ToUpper())
                //{
                //    success = DeleteImport(out message);
                //    if (success) { message = "Xóa thành công"; }
                //    else
                //        message = "Xóa thất bại";
                //}
                //else if (mode.ToUpper() == "SaveTaiSanImport".ToUpper())
                //{
                //    success = SaveImport(id, out message);
                //    if (success) { message = "Lưu chi tiết các tài sản import thành công"; }
                //    else
                //        message = message + "" == "" ? "Lưu thất bại" : message;
                //}
                else if (mode.ToUpper() == "TraCuuImport".ToUpper())
                {
                    success = true; message = "";
                }
                //----------------------------------------------------------------------------------------
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
        private bool AddOrUdateChuyenKho(int id, int idChiNhanh, DateTime NgayGiao, int IDDonViChuyen, string IDNguoiChuyen, string IDTruongDonViChuyen, int idDonViNhan, string IDNguoiNhan, string IDTruongDonViNhan, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessChuyenKho.getInstance().AddOrUpdate(id, idChiNhanh, NgayGiao, IDDonViChuyen, IDNguoiChuyen, IDTruongDonViChuyen
                , idDonViNhan, IDNguoiNhan, IDTruongDonViNhan
                , ghiChu, userLogin, out message, out idInsertNew);
        }
        private bool DeleteChuyenKho(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().Delete(id, userLogin, out message);
        }
        private bool SendApprovalChuyenKho(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().SendApprovalChuyenKho(id, userLogin, out message);
        }
        private bool ApprovalChuyenKho(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().ApprovalChuyenKho(id, ghiChu, userLogin, out message);
        }
        private bool RejectChuyenKho(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().RejectChuyenKho(id, ghiChu, userLogin, out message);
        }
        private bool ApprovalNhanTaiSanBanGiao(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().ApprovalNhanTaiSanBanGiao(id, ghiChu, userLogin, out message);
        }
        private bool RejectNhanTaiSanBanGiao(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().RejectNhanTaiSanBanGiao(id, ghiChu, userLogin, out message);
        }
        private bool NhapKhoTaiSanTraLai(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().NhapKhoTaiSanTraLai(id, ghiChu, userLogin, out message);
        }
        private bool AddOrUdateChuyenKhoChiTiet(int id, int IDChuyenKho, int idkhoXuat, int idTaiSan, int SoLuong, int idTrangThaiTaiSanXuatKho, int idTrangThaiTaiSanNhapKho, int IDKhoNhap, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessChuyenKho.getInstance().AddOrUpdateChuyenKhoChiTiet(id, IDChuyenKho, idkhoXuat, idTaiSan, SoLuong
           , idTrangThaiTaiSanXuatKho, idTrangThaiTaiSanNhapKho, IDKhoNhap, GhiChuChiTiet, userLogin, out message, out idInsertNew);
        }
        private bool DeleteChuyenKhoChiTiet(int idCT, int idMaster, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().DeleteChiTietChuyenKho(idCT, idMaster, userLogin, out message);

        }

        // danh cho import----------------------------------------------------------------------------------------------------------------------------
        /*
        private bool DeleteImport(out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().DeleteAllTaiSanImport(Utils.UserUtil.GetSessionUserId());
        }
        private bool SaveImport(int idBanGiao, out string message)
        {
            message = "";
            return DataProcess.ProcessChuyenKho.getInstance().SaveAllTaiSanImport(Utils.UserUtil.GetSessionUserId(), idBanGiao, out message);
        }
        */
        //---------------------------------------------------------------------------------------------------------------------------------------------
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}