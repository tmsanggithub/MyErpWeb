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

                    this.nvbMain.Items.Clear();
                    Dictionary<string, string> listMenuGroup = staff.GetMenuGroups();
                    foreach (KeyValuePair<string, string> kvp in listMenuGroup)
                    {
                        DevExpress.Web.MenuItem group = new DevExpress.Web.MenuItem(kvp.Value.ToUpper(), "grp" + kvp.Key);

                        foreach (KeyValuePair<string, Entity.EnAccessRightInfo> kvpAccess in staff.ListAccessRight)
                        {
                            Entity.EnAccessRightInfo access = kvpAccess.Value;
                            if (access.ModuleId == kvp.Key)
                            {
                                if (access.ShowMenuLeft > 0 && access.FunctionUrl.Length > 3)
                                {
                                    DevExpress.Web.MenuItem item = new DevExpress.Web.MenuItem(access.FunctionName, "item" + access.FunctionId);
                                    item.NavigateUrl = access.FunctionUrl;
                                    group.Items.Add(item);
                                }
                            }
                        }
                        this.nvbMain.Items.Add(group);
                    }

                    // Set breadcrumb dựa trên URL hiện tại
                    SetBreadcrumb(staff);
                }
            }
        }

        private void SetBreadcrumb(Entity.EnUserInfo staff)
        {
            string currentUrl = Request.AppRelativeCurrentExecutionFilePath + "";
            string groupName = "";
            string itemName = "";

            foreach (KeyValuePair<string, string> kvp in staff.GetMenuGroups())
            {
                foreach (KeyValuePair<string, Entity.EnAccessRightInfo> kvpAccess in staff.ListAccessRight)
                {
                    Entity.EnAccessRightInfo access = kvpAccess.Value;
                    if (access.ModuleId == kvp.Key && access.ShowMenuLeft > 0)
                    {
                        string menuUrl = access.FunctionUrl + "";
                        // So sánh url không phân biệt hoa thường
                        if (menuUrl.Length > 0 && currentUrl.ToLower().Contains(
                            System.IO.Path.GetFileName(menuUrl).ToLower()))
                        {
                            groupName = kvp.Value;
                            itemName = access.FunctionName;
                            break;
                        }
                    }
                }
                if (itemName.Length > 0) break;
            }

            if (groupName.Length > 0 && itemName.Length > 0)
                this.lblBreadcrumb.Text = string.Format(
                    "<a href='../'>Trang chủ</a><span class='bc-separator'>›</span>{0}<span class='bc-separator'>›</span>{1}",
                    groupName, itemName);
            else
                this.lblBreadcrumb.Text = "<a href='../'>Trang chủ</a>";
        }
    }
}