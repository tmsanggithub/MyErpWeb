using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session[Config.SysConfig.SESSION_USERINFO] != null)
                {
                    Entity.EnUserInfo staff = (Entity.EnUserInfo)Session[Config.SysConfig.SESSION_USERINFO];
                    this.lblUserFullName.Text = staff.StaffName;

                    this.nvbMain.Groups.Clear();
                    Dictionary<string, string> listMenuGroup = staff.GetMenuGroups();
                    foreach (KeyValuePair<string, string> kvp in listMenuGroup)
                    {
                        NavBarGroup group = new NavBarGroup(kvp.Value.ToUpper(), "grp" + kvp.Key);

                        foreach (KeyValuePair<string, Entity.EnAccessRightInfo> kvpAccess in staff.ListAccessRight)
                        {
                            Entity.EnAccessRightInfo access = kvpAccess.Value;
                            if (access.ModuleId == kvp.Key)
                            {
                                if (access.ShowMenuLeft > 0 && access.FunctionUrl.Length > 3)
                                {
                                    NavBarItem item = new NavBarItem(access.FunctionName, "item" + access.FunctionId);
                                    item.NavigateUrl = access.FunctionUrl;
                                    group.Items.Add(item);
                                }
                            }
                        }
                        this.nvbMain.Groups.Add(group);
                    }
                }
            }
        }
    }
}