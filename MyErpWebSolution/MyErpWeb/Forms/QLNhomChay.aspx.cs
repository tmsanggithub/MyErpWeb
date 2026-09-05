using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.Config;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class QLNhomChay : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLNhomChay));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                {
                    BindControl();
                }
                else
                {
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                }
                //  txtMasterRightCurrent["hidden_value"] = "";
                //  txtObjStatus["hidden_value"] = "";
            }
            else
            {

            }
        }
        private void BindControl()
        {
            try
            {
                //InitData();
                LoadDataSource();

                cbGiaiChay.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.ql_race + "", Utils.UserUtil.GetSessionUserId());
                cbGiaiChay.DataBind();

                cbNhomChay.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.dm_run_group + "", Utils.UserUtil.GetSessionUserId());
                cbNhomChay.DataBind();


                cbRunner.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.app_user_registed + "", Utils.UserUtil.GetSessionUserId());
                cbRunner.DataBind();

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
        protected void gridQLNhomChay_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLNhomChay_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            //Session["ssKey4Search"] = " ";
            //Session["ssgridQLNhomChay"] = Utils.UserUtil.GetSessionUserId();
            //this.gridQLNhomChay.DataBind();

            //  gridQLNhomChay.DataSource = DataProcess.ProcessNhomChay.getInstance().TraCuuNhomChay(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //  gridQLNhomChay.DataBind();

            btnSearch.Enabled = false;
            var dt = ProcessNhomChay.getInstance().TraCuuNhomChay("", Utils.UserUtil.GetSessionUserId() + "");
            Session["ssgridQLNhomChay"] = dt;
            gridQLNhomChay.DataBind();
            btnSearch.Enabled = true;

        }
        protected void gridQLNhomChay_DataBinding(object sender, EventArgs e)
        {
            gridQLNhomChay.DataSource = Session["ssgridQLNhomChay"];
            // Session["ssDMTaiSan"] = null;
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;

            }
            return ret;
        }
        public bool GetEnable(object Xem0Them1Sua2Xoa3Duyet5)
        {
            bool ret = false;

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {

                case Config.SysConfig.EnableApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhomChay).Name.ToUpper()))
                        ret = SysConfig.EnableTrue;
                    else
                        ret = SysConfig.EnableFalse;
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
                //DataTable dtbDetails = ProcessNhomChay.getInstance().TraCuuNhomChayChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                //this.gridDetails.DataSource = dtbDetails;
                //gridDetails.DataBind();


                var dtbDetails = ProcessNhomChay.getInstance().TraCuuNhomChayChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                Session["ssTraCuuNhomChayChiTiet"] = dtbDetails;
                gridDetails.DataBind();
             

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void gridDetails_DataBinding(object sender, EventArgs e)
        {
            gridDetails.DataSource = Session["ssTraCuuNhomChayChiTiet"];
            // Session["ssDMTaiSan"] = null;
        }


        protected void gridLichSuKyDuyet_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessNhomChay.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        protected void gridDanhSachRunner_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchTaiSan4ThemNhomChay(Utils.NumberUtil.ParseToInt(this.txtObjectId.Get("hidden_value") + ""));
        }
        protected void gridDanhSachRunner_PageIndexChanged(object sender, EventArgs e)
        {
            SearchTaiSan4ThemNhomChay(Utils.NumberUtil.ParseToInt(this.txtObjectId.Get("hidden_value") + ""));
        }
        private void SearchTaiSan4ThemNhomChay(int IDNhomChay)
        {
            try
            {
                Session["ssrace_group_id"] = IDNhomChay;
                Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
                this.gridDanhSachRunner.DataBind();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        public int GetBranchID()
        {
            return Utils.NumberUtil.ParseToInt(Session[Config.SysConfig.SESSION_BRANCHID]);
        }
    }
}