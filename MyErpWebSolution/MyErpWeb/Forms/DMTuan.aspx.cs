using System;
using System;
using System.Web.UI;
using WebRunDragon.Config;

namespace WebRunDragon.Forms
{
    public partial class DMTuan : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DMTuan));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Session["DMTuan_cbNhanVienValue"] = cbNhanVien.Value;
            }

            // Luôn bind combo giải chạy để UI hiển thị items
            cbNhanVien.DataSource = DataProcess.ProcessTuan.getInstance().LoadDanhSachGiaiChay(Utils.UserUtil.GetSessionUserId());
            cbNhanVien.DataBind();

            if (IsPostBack && Session["DMTuan_cbNhanVienValue"] != null)
            {
                cbNhanVien.Value = Session["DMTuan_cbNhanVienValue"];
            }

            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper()))
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

        protected void gridTuan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }

        protected void gridTuan_PageIndexChanged(object sender, EventArgs e)
        {
            gridTuan.DataBind();
        }

        protected void gridTuan_DataBinding(object sender, EventArgs e)
        {
            gridTuan.DataSource = Session["ssDMTuan"];
        }

        private void LoadDataSource()
        {
            var dt = DataProcess.ProcessTuan.getInstance().TraCuuTuan(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId());
            Session["ssDMTuan"] = dt;
            gridTuan.DataBind();
        }

        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";
            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case SysConfig.ValueRead:
                    ret = Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    ret = Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    ret = Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    ret = Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTuan).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }
    }
}
