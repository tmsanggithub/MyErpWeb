using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;
using static WebRunDragon.DataProcess.PropcessGiaiChayDuyetThamGia;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for QLGiaiChayDuyetThamGiaAction
    /// </summary>
    public class QLGiaiChayDuyetThamGiaAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLGiaiChayDuyetThamGiaAction));
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

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLGiaiChayDuyetThamGia).Name.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(QLGiaiChayDuyetThamGia).Name.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(QLGiaiChayDuyetThamGia).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.PropcessGiaiChayDuyetThamGia.getInstance().GetGiaiChayDuyetThamGiaByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }

                else if (mode == "SendApproval".ToUpper())
                {
                    //if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(QLGiaiChayDuyetThamGia).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(QLGiaiChayDuyetThamGia).Name.ToUpper()))
                    //{
                    //    success = SendApprovalTinTuc(id, sessionUserId, out message);
                    //    if (success) { message = "Gửi duyệt thành công"; }
                    //}
                    //else
                    //    message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "Approval".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLGiaiChayDuyetThamGia).Name.ToUpper()))
                    {
                        success = ApprovalDangKyGiaiChay(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Duyệt thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "Reject".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(QLGiaiChayDuyetThamGia).Name.ToUpper()))
                    {
                        success = RejectDangKyGiaiChay(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success) { message = "Từ chối thành công"; }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
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

        private bool ApprovalDangKyGiaiChay(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.PropcessGiaiChayDuyetThamGia.getInstance().Approval(id, ghiChu, userLogin, out message);
        }
        private bool RejectDangKyGiaiChay(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.PropcessGiaiChayDuyetThamGia.getInstance().Reject(id, ghiChu, userLogin, out message);
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