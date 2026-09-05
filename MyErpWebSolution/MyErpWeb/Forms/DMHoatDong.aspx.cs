using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.Config;

namespace WebRunDragon.Forms
{
    public partial class DMHoatDong : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DMHoatDong));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
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
        }

        private void BindControl()
        {
            try
            {
                deNgayDieuChinhTu.Date = DateTime.Now.AddDays(-30);
                deNgayDieuChinhDen.Date = DateTime.Now;
                //InitData();
                LoadDataSource();

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
        protected void btnSearchSum_Click(object sender, EventArgs e)
        {
            LoadDataSourceSum();
        }
        private void LoadDataSourceSum()
        {

            var dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhMucHoatDongSum(deNgayDieuChinhTu.Date, deNgayDieuChinhDen.Date, Utils.UserUtil.GetSessionUserId() + "");
            Session["ssDMHoatDong"] = dt;
            gridHoatDong.DataBind();
        }
        protected void gridHoatDong_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridHoatDong_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        protected void gridHoatDong_DataBinding(object sender, EventArgs e)
        {
            gridHoatDong.DataSource = Session["ssDMHoatDong"];
            // Session["ssDMTaiSan"] = null;
        }
        private void LoadDataSource()
        {

            var dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhMucHoatDong(deNgayDieuChinhTu.Date, deNgayDieuChinhDen.Date, Utils.UserUtil.GetSessionUserId() + "");
            Session["ssDMHoatDong"] = dt;
            gridHoatDong.DataBind();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadDataSource();
            //gridThanhToan.Columns["Xem"].Visible = false;

            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.DMHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }
    }
}