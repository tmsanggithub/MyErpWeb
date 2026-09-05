using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for CDHoatDongHopLeAction
    /// </summary>
    public class CDHoatDongHopLeAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CDHoatDongHopLeAction));
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

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessHoatDongHopLe.getInstance().GetHoatDongHopLeByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper()) && id == 0))
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
                            string content = context.Request.Form["RaceInfoHtml"] + "";
                            int idInsertUpdate = 0;
                            success = AddOrUdateHoatDongHopLe(id,
                                context.Request.Form["sportType"] + "",
                                 Utils.NumberUtil.ParseToInt(context.Request.Form["distanceMin"]),
                                    Utils.NumberUtil.ParseToInt(context.Request.Form["distanceMaxInDay"]),
                                      Utils.NumberUtil.ParseToInt(context.Request.Form["activitiesMinInWeek"]),
                                        Utils.NumberUtil.ParseToInt(context.Request.Form["averageSpeedFrom"]),
                                          Utils.NumberUtil.ParseToInt(context.Request.Form["averageSpeedTo"]),
                                            Utils.NumberUtil.ParseToDecimal(context.Request.Form["moneyPerKilomet"]),
                                             Utils.NumberUtil.ParseToDate(context.Request.Form["lamViecBuoiSangTu"]),
                                               Utils.NumberUtil.ParseToDate(context.Request.Form["lamViecBuoiSangDen"]),
                                                 Utils.NumberUtil.ParseToDate(context.Request.Form["lamViecBuoiChieuTu"]),
                                                   Utils.NumberUtil.ParseToDate(context.Request.Form["lamViecBuoiChieuDen"]),
              Utils.NumberUtil.ParseToInt(context.Request.Form["maxMinuteInDayOfLaixe"]),
                                context.Request.Form["Description"] + "",
                                sessionUserId, out message, out idInsertUpdate);
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
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        success = DeleteHoatDongHopLe(id, sessionUserId, out message);
                        if (success) { message = "Xóa thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "SendApproval".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(CDHoatDongHopLe).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        success = SendApprovalHoatDongHopLe(id, sessionUserId, out message);
                        if (success) { message = "Gửi duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "Approval".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        success = ApprovalHoatDongHopLe(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "Reject".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        success = RejectHoatDongHopLe(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Từ chối thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "AddOrUpDateDetail".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper()) && id == 0))
                    {
                        if (Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhomChay"]) <= 0)
                        {
                            message = "Chưa chọn nhóm chạy";
                            success = false;
                        }
                        else
                        {
                            int idInsertUpdate = 0;
                            int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);

                            success = false;
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
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                        success = false;
                        if (success) { message = "Xóa chi tiết tài sản thành công"; }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "EDITDETAIL")
                {
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CDHoatDongHopLe).Name.ToUpper()))
                    {
                        JObject entity = null;
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }

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
        private bool AddOrUdateHoatDongHopLe(int id,
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

            return DataProcess.ProcessHoatDongHopLe.getInstance().AddOrUpdate(id,
             sportType,
             distanceMin,
             distanceMaxInDay,
             activitiesMinInWeek,
             averageSpeedFrom,
             averageSpeedTo,
             moneyPerKilomet,
             lamViecBuoiSangTu,
             lamViecBuoiSangDen,
             lamViecBuoiChieuTu,
             lamViecBuoiChieuDen,
             maxMinuteInDayOfLaixe,
             description,
             userLogin,
            out message,
            out idInsertNew);
        }
        private bool DeleteHoatDongHopLe(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessHoatDongHopLe.getInstance().Delete(id, userLogin, out message);
        }
        private bool SendApprovalHoatDongHopLe(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessHoatDongHopLe.getInstance().SendApprovalHoatDongHopLe(id, userLogin, out message);
        }
        private bool ApprovalHoatDongHopLe(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessHoatDongHopLe.getInstance().ApprovalHoatDongHopLe(id, ghiChu, userLogin, out message);
        }
        private bool RejectHoatDongHopLe(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessHoatDongHopLe.getInstance().RejectHoatDongHopLe(id, ghiChu, userLogin, out message);
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