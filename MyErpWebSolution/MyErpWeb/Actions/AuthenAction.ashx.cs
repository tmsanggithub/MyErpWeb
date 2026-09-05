using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for AuthenAction
    /// </summary>
    public class AuthenAction : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AuthenAction));
        public void ProcessRequest(HttpContext context)
        {
            logger.Error("ProcessRequest authen begin");
            context.Response.ContentType = "application/json";
            bool success = false;
            int retCode = -1;
            string message = "";
            JObject retObject = new JObject();
            try
            {
                logger.Error("ProcessRequest authen begin 2222");

                string mode = context.Request.Form["mode"].Trim().ToUpper();
                if (mode == "LOGIN")
                {
                    string userId = context.Request.Form["userId"].Trim();
                    string userPwd = context.Request.Form["userPwd"];

                    logger.Error("ProcessRequest authen begin 3333");

                    JObject retAuthen = DataProcess.ProcessCfgUser.DoAuthen(userId, userPwd, 100); // 100 = mode DNS

                    logger.Error("ProcessRequest authen begin 4444");

                    if (retAuthen != null)
                    {
                        string authenMessgae = retAuthen["message"] + "";
                        retCode = int.Parse(retAuthen["retCode"] + "");
                        if (retCode == 101)
                            message = "Tên đăng nhập không tồn tại.";
                        else if (retCode == 102)
                            message = "Thông tin đăng nhập không hợp lệ.";
                        else if (retCode == 103)
                            message = "Tạm thời tài khoản này không được phép đăng nhập trong thời gian này.";
                        else if (retCode == 104)
                            message = "Tạm thời tài khoản này không được phép đăng nhập tại máy trạm này.";
                        else if (retCode == 105)
                            message = "Mật khẩu của tài khoản đã hết hạn.";
                        else if (retCode == 106)
                            message = "Tài khoản này đã vô hiệu hóa (diabled).";
                        else if (retCode == 107)
                            message = "Tài khoản này chưa được phân quyền để đăng nhập với loại chứng thực {simple} trên máu trạm này.";
                        else if (retCode == 108)
                            message = "Tài khoản hết hạn sử dụng.";
                        else if (retCode == 109)
                            message = "Tài khoản phải đổi mật khẩu trước khi sử dụng.";
                        else if (retCode == 110)
                            message = "Tài khoản bị khóa (locked).";
                        else if (retCode == 201)
                            message = "Thông tin đăng nhập không hợp lệ.";
                        else if (retCode == 202)
                            message = "Tài khoản bị khóa hoặc không tồn tại";

                        if ((Boolean)retAuthen["success"] || Config.SysConfig.IS_BYPASS_PASSWORD)
                        {
                            Entity.EnUserInfo userInfo = DataProcess.ProcessCfgUser.GetUserInfoByUserName(userId);
                            if (userInfo != null)
                            {
                                if (userInfo.IsLocked > 0)
                                    message = "Bạn đang bị tạm dừng sử dụng hệ thống, vui lòng liên hệ với QTHT để được hỗ trợ.";
                                else if (userInfo.ListAccessRight == null || userInfo.ListAccessRight.Count <= 0)
                                    message = "Bạn chưa được cấp quyền sử dụng, vui lòng liên hệ QTHT để được hỗ trợ.";
                                else
                                {
                                    success = true;
                                    context.Session[Config.SysConfig.SESSION_USERINFO] = userInfo;
                                    Config.SysConfig.USERDEPARTMENTCODE = userInfo.DeptCode;
                                    context.Session[Config.SysConfig.SESSION_BRANCHID] = userInfo.BranchId;
                                    logger.Info("Người dùng " + userId + "@" + userInfo.StaffName + " đăng nhập thành công từ máy " + context.Request.UserHostAddress);
                                    retObject["lastUrl"] = "~/Forms/CfgHome.aspx";// Utils.ActionUtil.GetUrl4FirstLoginSuccess(context.Session[Config.SysConfig.SESSION_LASTURL_BEFORE_AUTHEN] + "", userInfo.ListAccessRight);

                                }
                            }
                            else
                            {
                                retCode = 1; // không lấy được thông tin nhân viên của user này
                                message = "Không tồn tại thông tin nhân viên có tên đăng nhập " + userId + ". Bạn vui lòng liên hệ với IT để được hỗ trợ.";
                            }
                        }
                    }
                    else
                        message = "Lỗi không xác định, bạn vui lòng liên hệ với IT để được hỗ trợ.";
                }
                else if (mode == "LOGOUT")
                {
                    if (context.Session[Config.SysConfig.SESSION_USERINFO] != null)
                    {
                        Entity.EnUserInfo userInfo = (Entity.EnUserInfo)context.Session[Config.SysConfig.SESSION_USERINFO];
                        logger.Warn("Người dùng " + userInfo.UserName + "@" + userInfo.StaffName + " đăng xuất hệ thống bằng cách nhấn nút Thoát.");
                    }
                    context.Session.Remove(Config.SysConfig.SESSION_USERINFO);
                    success = true;
                }
                else
                    message = "You are a bot !";
            }
            catch (Exception ex)
            {
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