using WebRunDragon.Forms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for CfgDataRightAction
    /// </summary>
    public class CfgDataRightAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CfgDataRightAction));

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool success = false;
            int retCode = -1;
            string message = "";
            JObject retObject = new JObject();
            try
            {
                int id = int.Parse(context.Request.Form["id"]);
                string mode = context.Request.Form["mode"].Trim().ToUpper();
                if (mode == "SELECT")
                {
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(CfgDataRightInfo).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessCfgDataRightAccess.GetDataAccessGroupById(id, Utils.UserUtil.GetSessionUserId());
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "UPDATE")
                {
                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CfgDataRightInfo).Name.ToUpper()) && id > 0) ||
                            (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CfgDataRightInfo).Name.ToUpper()) && id == 0))
                    {
                        string tblName = context.Request.Form["tblName"].Trim();

                        if (tblName == "M")
                        {
                            string groupCode = context.Request.Form["code"].Trim();
                            string groupName = context.Request.Form["groupName"].Trim();
                            string description = context.Request.Form["description"].Trim();

                            if (string.IsNullOrEmpty(groupCode))
                                message += "Bạn vui lòng nhập Mã nhóm quyền dữ liệu" + Environment.NewLine;

                            if (string.IsNullOrEmpty(groupName))
                                message += "Bạn vui lòng nhập Tên nhóm quyền dữ liệu" + Environment.NewLine;

                            if (string.IsNullOrEmpty(message))
                            {
                                retCode = DataProcess.ProcessCfgDataRightAccess.AddOrUpdateDataAccessGroup(ref id, groupCode, groupName, description, Utils.UserUtil.GetSessionUserId());
                                success = retCode == 0;
                                retObject["id"] = id;
                                message = success ? "Thêm dữ liệu nhóm truy cập thành công." : "Nhóm dữ liệu đã tồn tại hoặc phát sinh lỗi khi cập nhật dữ liệu, vui lòng liên hệ với QTHT để được hỗ trợ.";
                            }
                        }
                        else if (tblName == "D")
                        {
                            string action = context.Request.Form["action"].Trim().ToUpper();
                            string staffId = context.Request.Form["domainUser"].Trim().ToUpper();
                            if (action == "ADD")
                            {
                                retCode = DataProcess.ProcessCfgDataRightAccess.AddStaff(staffId, id, Utils.UserUtil.GetSessionUserId());
                                success = retCode > 0;
                            }
                            else if (action == "REMOVE")
                            {
                                retCode = DataProcess.ProcessCfgDataRightAccess.RemoveStaff(staffId, id, Utils.UserUtil.GetSessionUserId());
                                success = retCode > 0;
                            }
                        }
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";

                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(CfgDataRightInfo).Name.ToUpper()))
                    {
                        retCode = DataProcess.ProcessCfgDataRightAccess.DeleteDataAccessGroup(id, Utils.UserUtil.GetSessionUserId());
                        success = retCode == 0;
                        message = success ? "Xóa thành công nhóm quyền dữ liệu thành công." : "Nhóm quyền dữ liệu này đã có cấu hình chi tiết hoặc phát sinh lỗi khi xóa, vui lòng liên hệ với QTHT để được hỗ trợ.";
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "APPROVE")
                {
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(CfgDataRightInfo).Name.ToUpper()))
                    {

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
                retObject["message"] = message;
            }
            context.Response.Write(retObject.ToString());
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