using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for logout
    /// </summary>
    public class logout : IHttpHandler, IRequiresSessionState
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(logout));
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session[Config.SysConfig.SESSION_USERINFO] != null)
            {
                Entity.EnUserInfo userInfo = (Entity.EnUserInfo)context.Session[Config.SysConfig.SESSION_USERINFO];
                logger.Warn("Người dùng " + userInfo.UserName + "@" + userInfo.StaffName + " đăng xuất hệ thống bằng cách nhấn nút Thoát.");
            }
            context.Session.Remove(Config.SysConfig.SESSION_USERINFO);
            context.Response.Redirect("~/Forms/CfgLogin.aspx");
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