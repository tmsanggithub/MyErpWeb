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
    /// Summary description for CfgFunctionRightAction
    /// </summary>
    public class CfgFunctionRightAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CfgFunctionRightAction));

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
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(CfgFunctionRightInfo).Name.ToUpper()))
                    {
                        string tblName = context.Request.Form["tblName"];
                        if (tblName == "M")
                        {
                            JObject entity = DataProcess.ProcessCfgFunctionRightAccess.GetFunctionRightAccessGroupById(id, Utils.UserUtil.GetSessionUserId());
                            success = entity != null;
                            retObject["entity"] = entity;
                        }
                        else if (tblName == "D")
                        {
                            // bảng FunctionAndRoles
                            JObject entity = DataProcess.ProcessCfgFunctionRightAccess.GetRoleDetailById(id, Utils.UserUtil.GetSessionUserId());
                            success = entity != null;
                            retObject["entity"] = entity;
                        }
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "UPDATE")
                {
                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CfgFunctionRightInfo).Name.ToUpper()) && id > 0) ||
                            (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CfgFunctionRightInfo).Name.ToUpper()) && id == 0))
                    {
                        string tblName = context.Request.Form["tblName"];
                        if (tblName == "M")
                        {
                            string code = context.Request.Form["code"];
                            string roleName = context.Request.Form["roleName"];
                            string description = context.Request.Form["description"];
                            if (string.IsNullOrEmpty(code))
                                message += "Bạn vui lòng cho biết Mã nhóm quyền" + Environment.NewLine;

                            if (string.IsNullOrEmpty(roleName))
                                message += "Bạn vui lòng cho biết Tên nhóm quyền" + Environment.NewLine;

                            if (string.IsNullOrEmpty(message))
                            {
                                retCode = DataProcess.ProcessCfgFunctionRightAccess.AddOrUpdateFunctionRightAccessGroup(ref id, code, roleName, description, Utils.UserUtil.GetSessionUserId());
                                success = retCode == 0;
                                retObject["id"] = id;
                                message = success ? "Cập nhật thông tin nhóm quyền chức năng thành công" : "Cập nhật lỗi, vui lòng thử lại hoặc liên hệ QTHT để được hỗ trợ.";
                            }
                        }
                        else if (tblName == "D")
                        {
                            #region bảng FunctionAndRoles

                            if (Utils.StringUtil.IsNullOrBlank(context.Request.Form["functionId"]))
                                message += "Bạn vui lòng cho biết chức năng cần phân quyền" + Environment.NewLine;

                            if (Utils.StringUtil.IsNullOrBlank(message))
                            {
                                int functionId = int.Parse(context.Request.Form["functionId"]);
                                int roleId = int.Parse(context.Request.Form["roleId"]);
                                int canRead = int.Parse(context.Request.Form["canRead"]);
                                int canCreate = int.Parse(context.Request.Form["canCreate"]);
                                int canEdit = int.Parse(context.Request.Form["canEdit"]);
                                int canDelete = int.Parse(context.Request.Form["canDelete"]);
                                int canPrint = int.Parse(context.Request.Form["canPrint"]);
                                int canApprove = int.Parse(context.Request.Form["canApprove"]);
                                int canSpecial = int.Parse(context.Request.Form["canSpecial"]);

                                retCode = DataProcess.ProcessCfgFunctionRightAccess.AddOrUpdateRoleDetail(id, roleId, functionId, canRead, canCreate, canEdit, canDelete, canPrint, canApprove, canSpecial, Utils.UserUtil.GetSessionUserId());
                                if (retCode == 0)
                                {
                                    success = true;
                                    message = "Cập nhật thành công phân quyền chức năng";
                                }
                                else
                                {
                                    message = retCode == 2 ? "Bạn không thể sửa chức năng hiện tại thành chức năng đã tồn tại trước đó" : "Sửa phân quyền chức năng bị lỗi, vui lòng thử lại hoặc liên hệ với Quản trị hệ thống.";
                                }
                            }

                            #endregion
                        }
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";

                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(CfgFunctionRightInfo).Name.ToUpper()))
                    {
                        string tblName = context.Request.Form["tblName"];
                        if (tblName == "M")
                        {
                            retCode = DataProcess.ProcessCfgFunctionRightAccess.DeleteFunctionRightAccessGroup(id, Utils.UserUtil.GetSessionUserId());
                            success = retCode == 0;
                            message = success ? "Xóa thông tin quyền chức năng thành công." : "Nhóm quyền chức năng này đã có chi tiết phân quyền hoặc phát sinh lỗi khi xóa, bạn vui lòng liên hệ với QTHT để được hỗ trợ.";
                        }
                        else if (tblName == "D")
                        {
                            success = DataProcess.ProcessCfgFunctionRightAccess.DeleteRoleDetail(id, Utils.UserUtil.GetSessionUserId());
                            message = success ? "Xóa chi tiết quyền thành công." : "Phát sinh lỗi khi xóa chi tiết quyền, vui lòng thử lại hoặc liên hệ QTHT để được hỗ trợ.";
                        }
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "APPROVE")
                {
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(CfgFunctionRightInfo).Name.ToUpper()))
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