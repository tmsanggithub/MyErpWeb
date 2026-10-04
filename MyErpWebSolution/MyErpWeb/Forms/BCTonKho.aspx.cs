using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class BCTonKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BCTonKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCTonKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCTonKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCTonKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCTonKho).Name.ToUpper()))
                {

                }
                else
                {
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                }
            }
            else
            {

            }
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        protected void gridTonKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridTonKho_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {

            //Session["ssKey4Search"] = txtKeyword.Text + "" == "" ? " " : txtKeyword.Text;
            //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            //gridTonKho.DataBind();
            //if (Session["bcTonKho"] == null)
            //{
                var dt = DataProcess.ProcessDanhMuc.getInstance().BaoCaoTonKho( "", Utils.UserUtil.GetSessionUserId() + "");
                Session["bcTonKho"] = dt;
            //}
            //else
            //{

            //}            

            gridTonKho.DataBind();
        }
        protected void gridTonKho_DataBinding(object sender, EventArgs e)
        {

            //gridTonKho.DataMember = "MyDataSource";
            gridTonKho.DataSource = Session["bcTonKho"];

            //gridTonKho.DataSource = DataProcess.ProcessDanhMuc.getInstance().BaoCaoTonKho(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //gridTonKho.DataMember = "MyDataSource";
            //gridTonKho.DataBind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadDataSource();
            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }
    }
}