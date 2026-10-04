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
    /// Summary description for QLNhapKhoAction
    /// </summary>
    public class QLNhapKhoAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLNhapKhoAction));
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

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessNhapKho.getInstance().GetNhapKhoByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper()) && id == 0))
                    {
                        if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoNhap"]) <= 0)
                        {
                            message = "Chưa chọn Kho nhập";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDChiNhanh"]) <= 0)
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
                            int idInsertUpdate = 0;
                            success = AddOrUdateNhapKho(id, Utils.NumberUtil.ParseToInt(context.Request.Form["IDChiNhanh"]), Utils.NumberUtil.ParseToInt(context.Request.Form["IDKhoNhap"]), Utils.NumberUtil.ParseToDate(context.Request.Form["NgayPhieu"]), context.Request.Form["NhanSuPhieu"] + "", context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertUpdate);
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
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLNhapKho).Name.ToUpper()))
                    {
                        success = DeleteNhapKho(id, sessionUserId, out message);
                        if (success) { message = "Xóa thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "SendApproval".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(QLNhapKho).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(QLNhapKho).Name.ToUpper()))
                    {
                        success = SendApprovalNhapKho(id, sessionUserId, out message);
                        if (success) { message = "Gửi duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "Approval".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLNhapKho).Name.ToUpper()))
                    {
                        success = ApprovalNhapKho(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "Reject".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLNhapKho).Name.ToUpper()))
                    {
                        success = RejectNhapKho(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Từ chối thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "AddOrUpDateDetail".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper()) && id == 0))
                    {
                        if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"]) <= 0)
                        {
                            message = "Chưa chọn tài sản nhập kho";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["SoLuong"]) <= 0)
                        {
                            message = "Chưa nhập số lượng";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToInt(context.Request.Form["TrangThaiTaiSan"]) <= 0)
                        {
                            message = "Chưa chọn trạng thái tài sản";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToDecimal(context.Request.Form["DonGia"]) < 0)
                        {
                            message = "Đơn giá không được nhỏ hơn 0";
                            success = false;
                        }
                        else if (Utils.NumberUtil.ParseToDecimal(context.Request.Form["ThanhTien"]) < 0)
                        {
                            message = "Thành tiền không được nhỏ hơn 0";
                            success = false;
                        }
                        else
                        {
                            int idInsertUpdate = 0;
                            int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                            success = AddOrUdateNhapKhoChiTiet(id, idMaster, Utils.NumberUtil.ParseToInt(context.Request.Form["IDTaiSanChiTiet"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["SoLuong"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["DonGia"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["ThanhTien"])
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["TrangThaiTaiSan"])
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
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLNhapKho).Name.ToUpper()))
                    {
                        int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                        success = DeleteNhapKhoChiTiet(id, idMaster, sessionUserId, out message);
                        if (success) { message = "Xóa chi tiết tài sản thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "EDITDETAIL")
                {
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLNhapKho).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessNhapKho.getInstance().GetNhapKhoChiTietByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                // danh cho import-----------------------------------------------------------------
                else if (mode.ToUpper() == "DELETEIMPORT".ToUpper())
                {
                    success = DeleteImport(out message);
                    if (success) { message = "Xóa thành công"; }
                    else
                        message = "Xóa thất bại";
                }
                else if (mode.ToUpper() == "SaveTaiSanImport".ToUpper())
                {
                    success = SaveImport(id, out message);
                    if (success) { message = "Lưu chi tiết các tài sản import thành công"; }
                    else
                        message = message + "" == "" ? "Lưu thất bại" : message;
                }
                else if (mode.ToUpper() == "TraCuuImport".ToUpper())
                {
                    success = true; message = "";
                }
                //-----------------------------------------------------------------------------------
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


        private bool AddOrUdateNhapKho(int id, int idChiNhanh, int idKhoNhap, DateTime ngayPhieu, string NhanSuPhieu, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessNhapKho.getInstance().AddOrUpdate(id, idChiNhanh, idKhoNhap, ngayPhieu, NhanSuPhieu, ghiChu, userLogin, out message, out idInsertNew);
        }
        private bool DeleteNhapKho(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessNhapKho.getInstance().Delete(id, userLogin, out message);
        }
        private bool SendApprovalNhapKho(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessNhapKho.getInstance().SendApprovalNhapKho(id, userLogin, out message);
        }
        private bool ApprovalNhapKho(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessNhapKho.getInstance().ApprovalNhapKho(id, ghiChu, userLogin, out message);
            if (ret)
            {
                //   DataProcess.ProcessDanhMuc.getInstance().clearCacheAndSessionDMTaiSan();..
            }
            return ret;
        }
        private bool RejectNhapKho(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessNhapKho.getInstance().RejectNhapKho(id, ghiChu, userLogin, out message);
        }
        private bool AddOrUdateNhapKhoChiTiet(int id, int IDNhapKho, int idTaiSan, int SoLuong, decimal DonGia
           , decimal ThanhTien, int idTrangThaiTaiSan, string GhiChuChiTiet, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessNhapKho.getInstance().AddOrUpdateNhapKhoChiTiet(id, IDNhapKho, idTaiSan, SoLuong, DonGia
           , ThanhTien, idTrangThaiTaiSan, GhiChuChiTiet, userLogin, out message, out idInsertNew);
        }
        private bool DeleteNhapKhoChiTiet(int idCT, int idMaster, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessNhapKho.getInstance().DeleteChiTietNhapKho(idCT, idMaster, userLogin, out message);

        }


        private bool DeleteImport(out string message)
        {
            message = "";
            return DataProcess.ProcessNhapKho.getInstance().DeleteAllTaiSanImport(Utils.UserUtil.GetSessionUserId());
        }
        private bool SaveImport(int IDNhapKho, out string message)
        {
            message = "";
            return DataProcess.ProcessNhapKho.getInstance().SaveAllTaiSanImport(Utils.UserUtil.GetSessionUserId(), IDNhapKho, out message);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }



    }
}