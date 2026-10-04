using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class BCNhapKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BCNhapKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCNhapKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCNhapKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCNhapKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCNhapKho).Name.ToUpper()))
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
                deNgayNhapTu.Date = DateTime.Now.AddDays(-10);
                deNgayNhapDen.Date = DateTime.Now;

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
                DataTable dtCN = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                DataTable dtCNClone = dtCN.Clone();
                if (dtCN != null)
                {
                    DataRow radd = dtCNClone.NewRow();
                    radd["BranchName"] = "---Tất cả---";
                    radd["Code"] = "All";
                    radd["ID"] = 0;
                    dtCNClone.Rows.Add(radd);
                    foreach (DataRow r in dtCN.Rows)
                    {
                        DataRow rclone = dtCNClone.NewRow();
                        rclone.ItemArray = r.ItemArray;
                        dtCNClone.Rows.Add(rclone);
                    }
                }
                cbMaChiNhanh.DataSource = dtCNClone;
                cbMaChiNhanh.DataBind();

                DataTable dtKho = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMKho + "", Utils.UserUtil.GetSessionUserId());
                DataTable dtKhoClone = dtKho.Clone();
                if (dtKho != null)
                {
                    DataRow radd = dtKhoClone.NewRow();
                    radd["TenKho"] = "---Tất cả---";
                    radd["MaKho"] = "All";
                    radd["ID"] = 0;
                    dtKhoClone.Rows.Add(radd);
                    foreach (DataRow r in dtKho.Rows)
                    {
                        DataRow rclone = dtKhoClone.NewRow();
                        rclone.ItemArray = r.ItemArray;
                        dtKhoClone.Rows.Add(rclone);
                    }
                }
                cbKhoNhap.DataSource = dtKhoClone;//
                cbKhoNhap.DataBind();

                //---------------------------------------------------------------------------------------------------------
                DataTable dtTS = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTaiSan + "", Utils.UserUtil.GetSessionUserId());
                DataTable dtTSClone = dtTS.Clone();
                if (dtTS != null)
                {
                    DataRow radd = dtTSClone.NewRow();
                    radd["TenTaiSan"] = "---Tất cả---";
                    radd["MaTaiSan"] = "All";
                    radd["ID"] = 0;
                    dtTSClone.Rows.Add(radd);
                    foreach (DataRow r in dtTS.Rows)
                    {
                        DataRow rclone = dtTSClone.NewRow();
                        rclone.ItemArray = r.ItemArray;
                        dtTSClone.Rows.Add(rclone);
                    }
                }
                cbTaiSanNhapKho.DataSource = dtTSClone;// DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTaiSanNhapKho.DataBind();
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
        protected void gridNhapKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridNhapKho_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }

        private void LoadDataSource()
        {
            Session["ssTuNgay"] = Utils.NumberUtil.ParseToDate(deNgayNhapTu.Value);
            Session["ssDenNgay"] = Utils.NumberUtil.ParseToDate(deNgayNhapDen.Value);
            Session["ssIDChiNhanh"] = Utils.NumberUtil.ParseToInt(cbMaChiNhanh.Value);
            Session["ssIDKho"] = Utils.NumberUtil.ParseToInt(cbKhoNhap.Value);
            Session["ssIDTaiSan"] = Utils.NumberUtil.ParseToInt(cbTaiSanNhapKho.Value);
            Session["ssSoPhieuNhapKho"] = txtSoPhieuNhapKho.Text + "" == "" ? " " : txtSoPhieuNhapKho.Text;
            Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            gridNhapKho.DataBind();

            //gridNhapKho.DataSource = DataProcess.ProcessNhapKho.getInstance().BaoCaoNhapKho(deNgayNhapTu.Date, deNgayNhapDen.Date, Utils.NumberUtil.ParseToInt(cbMaChiNhanh.Value)
            //    , Utils.NumberUtil.ParseToInt(cbKhoNhap.Value), Utils.NumberUtil.ParseToInt(cbTaiSanNhapKho.Value), txtSoPhieuNhapKho.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //gridNhapKho.DataBind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadDataSource();
            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }
    }
}