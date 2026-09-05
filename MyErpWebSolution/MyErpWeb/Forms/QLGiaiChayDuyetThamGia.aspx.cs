using DevExpress.Web;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.Config;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class QLGiaiChayDuyetThamGia : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLGiaiChayDuyetThamGia));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
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
               

              //  cbGiaiChay.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.cd_race_and_run_group + "_dangdienra", Utils.UserUtil.GetSessionUserId());
              //  cbGiaiChay.DataBind();

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
        protected void gridQLGiaiChayDuyetThamGia_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLGiaiChayDuyetThamGia_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            //Session["ssKey4Search"] = " ";
            //Session["ssgridQLGiaiChayDuyetThamGia"] = Utils.UserUtil.GetSessionUserId();
            //this.gridQLGiaiChayDuyetThamGia.DataBind();

            //  gridQLGiaiChayDuyetThamGia.DataSource = DataProcess.ProcessTinTuc.getInstance().TraCuuTinTuc(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //  gridQLGiaiChayDuyetThamGia.DataBind();

            btnSearch.Enabled = false;
            var dt = PropcessGiaiChayDuyetThamGia.getInstance().TraCuuGiaiChayDuyetThamGia("", Utils.UserUtil.GetSessionUserId() + "");
            Session["ssgridQLGiaiChayDuyetThamGia"] = dt;
            gridQLGiaiChayDuyetThamGia.DataBind();
            btnSearch.Enabled = true;

        }
        protected void gridQLGiaiChayDuyetThamGia_DataBinding(object sender, EventArgs e)
        {
            gridQLGiaiChayDuyetThamGia.DataSource = Session["ssgridQLGiaiChayDuyetThamGia"];
            // Session["ssDMTaiSan"] = null;
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
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
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLGiaiChayDuyetThamGia).Name.ToUpper()))
                        ret = SysConfig.EnableTrue;
                    else
                        ret = SysConfig.EnableFalse;
                    break;
            }
            return ret;
        }

        protected void gridLichSuKyDuyet_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {

                this.gridLichSuKyDuyet.DataSource = DataProcess.PropcessGiaiChayDuyetThamGia.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
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