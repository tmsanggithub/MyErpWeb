using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class BCDieuChinhKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BCDieuChinhKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCDieuChinhKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCDieuChinhKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCDieuChinhKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCDieuChinhKho).Name.ToUpper()))
                {
                    BindControlNotCombo();
                }
                else
                {
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                }
            }
            else
            {

            }
            BindControlCombo();
        }

        private void BindControlNotCombo()
        {
            try
            {
                deNgayDieuChinhTu.Date = DateTime.Now.AddDays(-10);
                deNgayDieuChinhDen.Date = DateTime.Now;

            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }
        private void BindControlCombo()
        {
            try
            {
                cbMaChiNhanh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                cbMaChiNhanh.DataBind();
                cbKhoDieuChinh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMKho + "", Utils.UserUtil.GetSessionUserId());
                cbKhoDieuChinh.DataBind();
                cbTaiSanDieuChinhKho.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTaiSanDieuChinhKho.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        protected void gridDieuChinhKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridDieuChinhKho_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }

        private void LoadDataSource()
        {
            Session["ssTuNgay"] = Utils.NumberUtil.ParseToDate(deNgayDieuChinhTu.Value);
            Session["ssDenNgay"] = Utils.NumberUtil.ParseToDate(deNgayDieuChinhDen.Value);
            Session["ssIDChiNhanh"] = Utils.NumberUtil.ParseToInt(cbMaChiNhanh.Value);
            Session["ssIDKho"] = Utils.NumberUtil.ParseToInt(cbKhoDieuChinh.Value);
            Session["ssIDTaiSan"] = Utils.NumberUtil.ParseToInt(cbTaiSanDieuChinhKho.Value);
            Session["ssSoPhieuDieuChinhKho"] = txtSoPhieuDieuChinhKho.Text + "" == "" ? " " : txtSoPhieuDieuChinhKho.Text;
            Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            gridDieuChinhKho.DataBind();

            //gridDieuChinhKho.DataSource = DataProcess.ProcessDieuChinhKho.getInstance().BaoCaoDieuChinhKho(deNgayDieuChinhTu.Date, deNgayDieuChinhDen.Date, Utils.NumberUtil.ParseToInt(cbMaChiNhanh.Value)
            //    , Utils.NumberUtil.ParseToInt(cbKhoDieuChinh.Value), Utils.NumberUtil.ParseToInt(cbTaiSanDieuChinhKho.Value), txtSoPhieuDieuChinhKho.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //gridDieuChinhKho.DataBind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadDataSource();
            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }
    }
}