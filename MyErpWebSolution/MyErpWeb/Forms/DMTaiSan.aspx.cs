using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.Config;

namespace WebRunDragon.Forms
{
    public partial class DMTaiSan : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DMTaiSan));
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper())
                        || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper())
                        || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                    {
                        BindControl();
                    }
                    else
                    {
                        Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                    }
                }
                else
                {

                }
                //GridViewFeaturesHelper.SetupGlobalGridViewBehavior(gridTaiSan);

                //gridTaiSan.SettingsResizing.ColumnResizeMode = (ColumnResizeMode)Enum.Parse(typeof(ColumnResizeMode), ddlResizingMode.Text, true);
                //gridTaiSan.SettingsResizing.Visualization = (ResizingMode)Enum.Parse(typeof(ResizingMode), ddlResizingVisualization.Text, true);

                //ddlResizingVisualization.Theme = DemoHelper.Instance.Theme;
                //ddlResizingMode.Theme = DemoHelper.Instance.Theme;
            }
            catch(Exception er)
            {
                logger.Error("DMTaiSan.PageLoad.Message:" + er.Message+"\nTrace:"+er.StackTrace,er);
            }
        }

        private void BindControl()
        {
            try
            {
                //InitData();
                cbDonViTinhCT.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMDonViTinh + "", Utils.UserUtil.GetSessionUserId());
                cbDonViTinhCT.DataBind();
                cbDonViTinh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMDonViTinh + "", Utils.UserUtil.GetSessionUserId());
                cbDonViTinh.DataBind();
                cbDonViTinhTrongLuong.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMDonViTinh + "", Utils.UserUtil.GetSessionUserId());
                cbDonViTinhTrongLuong.DataBind();
                cbDonViTinhKichThuot.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMDonViTinh + "", Utils.UserUtil.GetSessionUserId());
                cbDonViTinhKichThuot.DataBind();
                cbNhaSanXuat.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMNhaSanXuat + "", Utils.UserUtil.GetSessionUserId());
                cbNhaSanXuat.DataBind();
                cbNhomTaiSan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMNhomTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbNhomTaiSan.DataBind();
                cbViTri.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMViTri + "", Utils.UserUtil.GetSessionUserId());
                cbViTri.DataBind();
                cbNhomTaiSan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMViTri + "", Utils.UserUtil.GetSessionUserId());
              
                LoadDataSource(1); // luc init ma load thi no nang lam

            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDataSource(1);
        }
        protected void gridTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource(1);
        }
        protected void gridTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource(1);
        }
        private void LoadDataSource(int TS1ThayDoi2DuyetThayDoi3)
        {
            btnSearch.Enabled = false;
            //Session["ssKey4Search"] = txtKeyword.Text + "" == "" ? " " : txtKeyword.Text;
            //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            //gridTaiSan.DataBind();
            if (TS1ThayDoi2DuyetThayDoi3 == 1)
            { 
                var dt = DataProcess.ProcessDanhMuc.getInstance().DMTaiSanSearchFromCache("", Utils.UserUtil.GetSessionUserId() + "");
                Session["ssDMTaiSan"] = dt;                
            }
            else if (TS1ThayDoi2DuyetThayDoi3 == 2)
            { 
                var dt = DataProcess.ProcessDanhMuc.getInstance().DMTaiSanSearchThayDoiFromCache( "", Utils.UserUtil.GetSessionUserId() + "");
                Session["ssDMTaiSan"] = dt;              
            }
            else if (TS1ThayDoi2DuyetThayDoi3 == 3)
            {                
                var dt = DataProcess.ProcessDanhMuc.getInstance().DMTaiSanSearchDuyetThayDoiFromCache("", Utils.UserUtil.GetSessionUserId() + "");
                Session["ssDMTaiSan"] = dt;
                
            }
            gridTaiSan.DataBind();
            btnSearch.Enabled = true;
        }

        protected void btnSearchThayDoi_Click(object sender, EventArgs e)
        {
            LoadDataSource(2);
        }
        protected void btnSearchThayDoi_Duyet_Click(object sender, EventArgs e)
        {
            LoadDataSource(3);
        }
        protected void gridTaiSan_DataBinding(object sender, EventArgs e)
        {
            gridTaiSan.DataSource = Session["ssDMTaiSan"];
            // Session["ssDMTaiSan"] = null;
        }

        protected void gridLichSuKyDuyet_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessDanhMuc.getInstance().TraCuuLichSuKyDuyetTaiSan(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadDataSource(1);
            gridTaiSan.Columns["Xem"].Visible = false;
            gridTaiSan.Columns["Sửa"].Visible = false;
            gridTaiSan.Columns["Xóa"].Visible = false;
            gridTaiSan.Columns["Duyệt"].Visible = false;
            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }

        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMTaiSan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }




        protected void gridDetails_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchDetails(int.Parse(e.Parameters));
        }
        protected void gridDetails_PageIndexChanged(object sender, EventArgs e)
        {
            SearchDetails(int.Parse(this.txtObjectId.Get("hidden_value") + ""));
        }
        private void SearchDetails(int idMaster)
        {
            try
            {
                DataTable dtbDetails = DataProcess.ProcessDanhMuc.getInstance().TraCuuChiTietThayDoiTS(idMaster, Utils.UserUtil.GetSessionUserId());
                this.gridDetails.DataSource = dtbDetails;
                gridDetails.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
    }
}