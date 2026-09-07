using System;
using System;
using System.Web.UI;
using WebRunDragon.Config;

namespace WebRunDragon.Forms
{
    public partial class DMNghiPhep : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DMNghiPhep));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Session["DMNghiPhep_cbNhanVienValue"] = cbNhanVien.Value;
            }

            // Luôn bind cbNhanVien để UI hiển thị items
            cbNhanVien.DataSource = DataProcess.ProcessNghiPhep.getInstance().LoadDanhSachNhanVien(Utils.UserUtil.GetSessionUserId());
            cbNhanVien.DataBind();

            if (IsPostBack && Session["DMNghiPhep_cbNhanVienValue"] != null)
            {
                cbNhanVien.Value = Session["DMNghiPhep_cbNhanVienValue"];
            }

            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper()))
                {
                    BindControl();
                }
                else
                {
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                }
            }
        }

        private void BindControl()
        {
            try
            {
                LoadDataSource();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDataSource();
        }

        protected void gridNghiPhep_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }

        protected void gridNghiPhep_PageIndexChanged(object sender, EventArgs e)
        {
            gridNghiPhep.DataBind();
        }

        protected void gridNghiPhep_DataBinding(object sender, EventArgs e)
        {
            gridNghiPhep.DataSource = Session["ssDMNghiPhep"];
        }

        private void LoadDataSource()
        {
            var dt = DataProcess.ProcessNghiPhep.getInstance().TraCuuNghiPhep(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId());
            Session["ssDMNghiPhep"] = dt;
            gridNghiPhep.DataBind();
        }

        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";
            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case SysConfig.ValueRead:
                    ret = Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    ret = Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    ret = Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    ret = Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMNghiPhep).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }
    }
}
