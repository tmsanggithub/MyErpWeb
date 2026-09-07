using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;

namespace WebRunDragon.Actions
{
    public class QLNghiPhepAction : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLNghiPhepAction));

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool success = false;
            int retCode = -1;
            string message = "";
            int id = Utils.NumberUtil.ParseToInt(context.Request.Form["id"]);
            JObject retObject = new JObject();

            try
            {
                string sessionUserId = Utils.UserUtil.GetSessionUserId();
                string mode = (context.Request.Form["mode"] + "").Trim().ToUpper();

                if (mode == "EDIT")
                {
                    string subMode = (context.Request.Form["subMode"] + "").Trim().ToUpper();
                    if ((subMode == "EDITONLY" && Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMNghiPhep).Name.ToUpper()))
                        || (subMode == "READONLY" && Utils.ActionUtil.CanRead(sessionUserId, typeof(DMNghiPhep).Name.ToUpper())))
                    {
                        JObject entity = DataProcess.ProcessNghiPhep.getInstance().GetNghiPhepByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                    {
                        message = "Bạn không có quyền thực hiện chức năng này";
                    }
                }
                else if (mode == "ADDORUPDATE")
                {
                    if ((Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMNghiPhep).Name.ToUpper()) && id > 0)
                        || (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMNghiPhep).Name.ToUpper()) && id <= 0))
                    {
                        string idStrava = context.Request.Form["id_strava"] + "";
                        DateTime ngayNghiTu = Utils.NumberUtil.ParseToDate(context.Request.Form["ngay_nghi_tu"]);
                        DateTime ngayNghiDen = Utils.NumberUtil.ParseToDate(context.Request.Form["ngay_nghi_den"]);
                        string ghiChu = context.Request.Form["GhiChu"] + "";
                        int idInsertUpdate;

                        success = DataProcess.ProcessNghiPhep.getInstance().AddOrUpdate(id, idStrava, ngayNghiTu, ngayNghiDen, ghiChu, sessionUserId, out message, out idInsertUpdate);
                        if (success)
                        {
                            message = "L?u thành công";
                            if (id <= 0) id = idInsertUpdate;
                        }
                    }
                    else
                    {
                        message = "Bạn không có quyền thực hiện chức năng này";
                    }
                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(DMNghiPhep).Name.ToUpper()))
                    {
                        success = DataProcess.ProcessNghiPhep.getInstance().Delete(id, sessionUserId, out message);
                        if (success) message = "Xóa thành công";
                    }
                    else
                    {
                        message = "Bạn không có quyền xóa chức năng này";
                    }
                }
                else
                {
                    message = "Invalid mode !";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error("QLNghiPhepAction.ProcessRequest Error", ex);
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

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
