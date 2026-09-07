using System;
using System.Data;
using System.Web.UI;
using WebRunDragon.Config;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class BCXepHang : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BCXepHang));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                Session["BCXepHang_cbGiaiChayValue"] = cbGiaiChay.Value;
            }

            cbGiaiChay.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.ql_race + "", Utils.UserUtil.GetSessionUserId());
            cbGiaiChay.DataBind();

            if (IsPostBack && Session["BCXepHang_cbGiaiChayValue"] != null)
            {
                cbGiaiChay.Value = Session["BCXepHang_cbGiaiChayValue"];
            }

            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper()))
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
                // LoadDataSourceDoi();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
            }
        }

        protected void btnSearchSum_Click(object sender, EventArgs e)
        {
            switch (ASPxTabControl1.ActiveTabIndex)
            {
                case 0:
                    LoadDataSourceDoi();
                    break;
                case 1:
                    LoadDataSourceNu();
                    break;
                case 2:
                    LoadDataSourceNam();
                    break;
                case 3:
                    LoadDataSourceAllVDV();
                    break;
                case 4:
                    LoadDataSourceTuan();
                    break;
                default:
                    LoadDataSourceDoi();
                    break;
            }
        }

        private void LoadDataSourceDoi()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbGiaiChay.Value + "");
            DataTable dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhXepHangTheoDoi(Utils.UserUtil.GetSessionUserId() + "", idGiaiChay);
            Session["ssBCXepHangDoi"] = dt;
            gridHoatDongDoi.DataBind();
        }

        protected void gridHoatDongDoi_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceDoi();
        }

        protected void gridHoatDongDoi_PageIndexChanged(object sender, EventArgs e)
        {
            gridHoatDongDoi.DataBind();
        }

        protected void gridHoatDongDoi_DataBinding(object sender, EventArgs e)
        {
            gridHoatDongDoi.DataSource = Session["ssBCXepHangDoi"];
        }

        #region Nu
        private void LoadDataSourceNu()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbGiaiChay.Value + "");
            DataTable dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhXepHangTheoNu(Utils.UserUtil.GetSessionUserId() + "", idGiaiChay);
            Session["ssBCXepHangNu"] = dt;
            gridHoatDongNu.DataBind();
        }

        protected void gridHoatDongNu_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceNu();
        }

        protected void gridHoatDongNu_PageIndexChanged(object sender, EventArgs e)
        {
            gridHoatDongNu.DataBind();
        }

        protected void gridHoatDongNu_DataBinding(object sender, EventArgs e)
        {
            gridHoatDongNu.DataSource = Session["ssBCXepHangNu"];
        }
        #endregion
        #region Nam
        private void LoadDataSourceNam()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbGiaiChay.Value + "");
            DataTable dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhXepHangTheoNam(Utils.UserUtil.GetSessionUserId() + "", idGiaiChay);
            Session["ssBCXepHangNam"] = dt;
            gridHoatDongNam.DataBind();
        }

        protected void gridHoatDongNam_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceNam();
        }

        protected void gridHoatDongNam_PageIndexChanged(object sender, EventArgs e)
        {
            gridHoatDongNam.DataBind();
        }

        protected void gridHoatDongNam_DataBinding(object sender, EventArgs e)
        {
            gridHoatDongNam.DataSource = Session["ssBCXepHangNam"];
        }
        #endregion
        #region vdv
        private void LoadDataSourceAllVDV()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbGiaiChay.Value + "");
            DataTable dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhXepHangTheoAllVDV(Utils.UserUtil.GetSessionUserId() + "", idGiaiChay);
            Session["ssBCXepHangAllVDV"] = dt;
            gridHoatDongVDV.DataBind();
        }

        protected void gridHoatDongVDV_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceAllVDV();
        }

        protected void gridHoatDongVDV_PageIndexChanged(object sender, EventArgs e)
        {
            gridHoatDongVDV.DataBind();
        }

        protected void gridHoatDongVDV_DataBinding(object sender, EventArgs e)
        {
            gridHoatDongVDV.DataSource = Session["ssBCXepHangAllVDV"];
        }
        #endregion

        #region vdv
        private void LoadDataSourceTuan()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbGiaiChay.Value + "");
            DataTable dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhXepHangTheoTuan(Utils.UserUtil.GetSessionUserId() + "", idGiaiChay);
            Session["ssBCXepHangTuan"] = dt;
            gridHoatDongTuan.DataBind();
        }

        protected void gridHoatDongTuan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceTuan();
        }

        protected void gridHoatDongTuan_PageIndexChanged(object sender, EventArgs e)
        {
            gridHoatDongTuan.DataBind();
        }

        protected void gridHoatDongTuan_DataBinding(object sender, EventArgs e)
        {
            gridHoatDongTuan.DataSource = Session["ssBCXepHangTuan"];
        }
        #endregion


        protected void btnExcel_Click(object sender, EventArgs e)
        {
            switch (ASPxTabControl1.ActiveTabIndex)
            {
                case 0:
                    LoadDataSourceDoi();
                    gridExporter.GridViewID = "gridHoatDongDoi";
                    break;
                case 1:
                    LoadDataSourceNu();
                    gridExporter.GridViewID = "gridHoatDongNu";
                    break;
                case 2:
                    LoadDataSourceNam();
                    gridExporter.GridViewID = "gridHoatDongNam";
                    break;
                case 3:
                    LoadDataSourceAllVDV();
                    gridExporter.GridViewID = "gridHoatDongAllVDV";
                    break;
                case 4:
                    LoadDataSourceTuan();
                    gridExporter.GridViewID = "gridHoatDongTuan";
                    break;
                default:
                    LoadDataSourceDoi();
                    gridExporter.GridViewID = "gridHoatDongDoi";
                    break;
            }
            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }

        public string GetRight(object value)
        {
            string ret = "";
            switch (value + "")
            {
                case SysConfig.ValueRead:
                    ret = Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    ret = Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    ret = Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    ret = Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCXepHang).Name.ToUpper()) ? SysConfig.displayButton : SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }
    }
}
