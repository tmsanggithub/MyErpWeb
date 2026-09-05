using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class HuongDanSuDung : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"600px\">";

                // ltEmbed.Text = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"800px\"> </object>";
                ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/HuongDanSuDung.pdf"));


                //if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.HelpDeskGuide).Name)  )
                //{

                //}
                //else
                //{
                //    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                //}                
            }
            else
            {

            }
        }
    }
}