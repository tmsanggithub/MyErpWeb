using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class CfgLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session[Config.SysConfig.SESSION_USERINFO] != null)
                {
                    Entity.EnUserInfo userInfo = (Entity.EnUserInfo)Session[Config.SysConfig.SESSION_USERINFO];
                    Response.Redirect(Utils.ActionUtil.GetUrl4FirstLoginSuccess(Session[Config.SysConfig.SESSION_LASTURL_BEFORE_AUTHEN] + "", userInfo.ListAccessRight));
                }

            }
        }
    }
}