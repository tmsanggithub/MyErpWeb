using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;

namespace WebRunDragon.Actions
{
    public class QLTuanAction : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLTuanAction));

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
                    if ((subMode == "EDITONLY" && Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMTuan).Name.ToUpper()))
                        || (subMode == "READONLY" && Utils.ActionUtil.CanRead(sessionUserId, typeof(DMTuan).Name.ToUpper())))
                    {
                        JObject entity = DataProcess.ProcessTuan.getInstance().GetTuanByID(id);
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
                    if ((Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMTuan).Name.ToUpper()) && id > 0)
                        || (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMTuan).Name.ToUpper()) && id <= 0))
                    {
                        int idRace = Utils.NumberUtil.ParseToInt(context.Request.Form["id_race"]);
                        string tuan = context.Request.Form["tuan"] + "";
                        DateTime fromDate = Utils.NumberUtil.ParseToDate(context.Request.Form["from_date"]);
                        DateTime toDate = Utils.NumberUtil.ParseToDate(context.Request.Form["to_date"]);
                        string ghiChu = context.Request.Form["GhiChu"] + "";
                        int idInsertUpdate;

                        success = DataProcess.ProcessTuan.getInstance().AddOrUpdate(id, idRace, tuan, fromDate, toDate, ghiChu, sessionUserId, out message, out idInsertUpdate);
                        if (success)
                        {
                            message = "Lưu thành công";
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
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(DMTuan).Name.ToUpper()))
                    {
                        success = DataProcess.ProcessTuan.getInstance().Delete(id, sessionUserId, out message);
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
                logger.Error("QLTuanAction.ProcessRequest Error", ex);
            }
            finally
            {
                retObject["success"] = success;
                retObject["retCode"] = retCode;
                retObject["message"] = success ? (message == "" ? "Thực hiện thành công" : message) : (message == "" ? "Th?c hi?n th?t b?i" : message);
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
