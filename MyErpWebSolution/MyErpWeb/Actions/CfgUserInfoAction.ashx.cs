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
    /// Summary description for CfgUserInfoAction
    /// </summary>
    public class CfgUserInfoAction : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CfgUserInfoAction));
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

                id = Utils.NumberUtil.ParseToInt(context.Request.Form["id"]);

                string mode = context.Request.Form["mode"].Trim().ToUpper();
                if (mode == "EDIT")
                {
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()))
                    {

                        JObject entity = DataProcess.ProcessCfgUser.GetUserByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()) && id == 0))
                    {
                        if (context.Request.Form["UserName"] + "" == "")
                        {
                            message = "Chưa có tên đăng nhập";
                            success = false;
                        }
                        else if (context.Request.Form["BranchId"] + "" == "")
                        {
                            message = "Chưa nhập tên chi nhánh";
                            success = false;
                        }
                        else
                        {
                            int idInsertUpdate = 0;
                            success = AddOrUdateUserInfo(id, context.Request.Form["UserName"], context.Request.Form["StaffNo"], context.Request.Form["StaffName"], context.Request.Form["Email"], context.Request.Form["BranchId"], context.Request.Form["DepartmentId"], "", "", sessionUserId, out message, out idInsertUpdate);
                            if (success)
                            {
                                message = "Lưu thành công";
                            }
                            else
                            {

                            }
                            // truong hop them moi thi tra ve lại ID mới thêm vào database
                            if (id <= 0) id = idInsertUpdate;
                        }
                    }
                    else
                        message = "Bạn không có quyền thêm hoặc chỉnh sửa chức năng này";

                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(CfgUserInfo).Name))
                    {
                        success = Delete(id, sessionUserId, out message);
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "AddRemoveRole".ToUpper())
                {
                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()) && id > 0) ||
                            (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()) && id == 0))
                    {
                        string userName = context.Request.Form["userName"];
                        string action = context.Request.Form["action"].ToUpper();
                        if (action == "AddRole".ToUpper())
                        {
                            int roleId = int.Parse(context.Request.Form["roleId"]);
                            success = DataProcess.ProcessCfgUser.AddRole(userName, roleId, Utils.UserUtil.GetSessionUserId());
                        }
                        else if (action == "REMOVEROLE")
                        {
                            int roleId = int.Parse(context.Request.Form["roleId"]);
                            success = DataProcess.ProcessCfgUser.RemoveRole(userName, roleId, Utils.UserUtil.GetSessionUserId());
                        }
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";

                }
                else if (mode == "AddRemoveAccessGroup".ToUpper())
                {
                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()) && id > 0) ||
                            (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()) && id == 0))
                    {
                        string userName = context.Request.Form["userName"];
                        string action = context.Request.Form["action"].ToUpper();

                        if (action == "ADDAccessGroup".ToUpper())
                        {

                            int groupId = int.Parse(context.Request.Form["groupId"]);
                            success = DataProcess.ProcessCfgUser.AddDataAccessGroup(userName, groupId, Utils.UserUtil.GetSessionUserId());

                        }
                        else if (action == "REMOVEAccessGroup".ToUpper())
                        {
                            int groupId = int.Parse(context.Request.Form["groupId"]);
                            success = DataProcess.ProcessCfgUser.RemoveDataAccessGroup(userName, groupId, Utils.UserUtil.GetSessionUserId());
                        }

                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";

                }
                //else if (action == "UNLOCK")
                //        {
                //            success = DataProcess.ProcessUser.UnlockUser(userName, Utils.UserUtil.GetSessionUserId());
                //        }
                //else if (mode == "DELETE")
                //{
                //    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name))
                //    {

                //    }
                //    else
                //        message = "Bạn không có quyền thực hiện chức năng này";
                //}
                //else if (mode == "APPROVE")
                //{
                //    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name))
                //    {

                //    }
                //    else
                //        message = "Bạn không có quyền thực hiện chức năng này";
                //}
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
                retObject["id"] = id;
            }
            context.Response.Write(retObject.ToString());
        }


        private bool AddOrUdateUserInfo(int ID, string UserName, string StaffNo, string StaffName, string Email, string BranchId, string DepartmentId, string ManagerId, string Description, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessCfgUser.AddOrUpdateUserInfo(ID, UserName, StaffNo, StaffName, Email, BranchId, DepartmentId, ManagerId, Description, userLogin, out message, out idInsertNew);
        }
        private bool Delete(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessCfgUser.Delete(id, userLogin, out message);

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