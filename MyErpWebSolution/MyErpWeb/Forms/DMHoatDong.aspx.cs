using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.Config;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class DMHoatDong : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(DMHoatDong));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Lưu lại giá trị user đang chọn TRƯỚC KHI DataBind() reset nó
                Session["DMHoatDong_cbGiaiChayValue"] = cbGiaiChay.Value;
                Session["DMHoatDong_cbGiaiChayPopupValue"] = cbGiaiChayPopup.Value;
                Session["DMHoatDong_cbNguoiDungDangKyValue"] = cbNguoiDungDangKy.Value;
            }

            // Luôn bind DataSource để UI hiển thị items (kể cả PostBack)
            cbGiaiChay.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.ql_race + "", Utils.UserUtil.GetSessionUserId());
            cbGiaiChay.DataBind();

            cbGiaiChayPopup.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.ql_race + "", Utils.UserUtil.GetSessionUserId());
            cbGiaiChayPopup.DataBind();

            cbNguoiDungDangKy.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadUserDangKy4Combo(Utils.UserUtil.GetSessionUserId());
            cbNguoiDungDangKy.DataBind();

            if (IsPostBack && Session["DMHoatDong_cbGiaiChayValue"] != null)
            {
                // Khôi phục lại giá trị đã chọn sau khi DataBind()
                cbGiaiChay.Value = Session["DMHoatDong_cbGiaiChayValue"];
            }

            if (IsPostBack && Session["DMHoatDong_cbGiaiChayPopupValue"] != null)
            {
                cbGiaiChayPopup.Value = Session["DMHoatDong_cbGiaiChayPopupValue"];
            }

            if (IsPostBack && Session["DMHoatDong_cbNguoiDungDangKyValue"] != null)
            {
                cbNguoiDungDangKy.Value = Session["DMHoatDong_cbNguoiDungDangKyValue"];
            }

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
        }

        private void BindControl()
        {
            try
            {
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
            LoadDataSource();
        }

        protected void gridHoatDong_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridHoatDong_PageIndexChanged(object sender, EventArgs e)
        {
            // Chỉ bind lại từ Session, không query DB lại
            gridHoatDong.DataBind();
        }
        protected void gridHoatDong_DataBinding(object sender, EventArgs e)
        {
            gridHoatDong.DataSource = Session["ssDMHoatDong"];
            // Session["ssDMTaiSan"] = null;
        }
        private void LoadDataSource()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbGiaiChay.Value + "");
            if (idGiaiChay > 0)
            {
                var dt = DataProcess.ProcessDanhMuc.getInstance().TraCuuDanhMucHoatDong(Utils.UserUtil.GetSessionUserId() + "", idGiaiChay);
                Session["ssDMHoatDong"] = dt;
            }
            else
            {
                Session["ssDMHoatDong"] = null;
            }
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
