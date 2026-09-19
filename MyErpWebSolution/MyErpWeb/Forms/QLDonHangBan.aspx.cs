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
    public partial class QLDonHangBan : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLDonHangBan));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
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
        protected void gridQLDonHangBan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string param = (e.Parameters ?? "").ToString();
                if (!string.IsNullOrEmpty(param))
                {
                    string[] parts = param.Split(new char[] { '|' }, 2);
                    if (parts.Length > 0 && parts[0] == "PAGESIZE")
                    {
                        int pageSize = 0;
                        int.TryParse(parts.Length > 1 ? parts[1] : "0", out pageSize);
                        if (pageSize > 0)
                        {
                            gridQLDonHangBan.SettingsPager.PageSize = pageSize;
                            Session["ssgridQLDonHangBan_PageSize"] = pageSize;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("gridQLDonHangBan_CustomCallback param error: " + ex.Message + "\n" + ex.StackTrace);
            }
            LoadDataSource();
        }
        protected void gridQLDonHangBan_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            //Session["ssKey4Search"] = " ";
            //Session["ssgridQLDonHangBan"] = Utils.UserUtil.GetSessionUserId();
            //this.gridQLDonHangBan.DataBind();

            //  gridQLDonHangBan.DataSource = DataProcess.ProcessDonHangBan.getInstance().TraCuuDonHangBan(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //  gridQLDonHangBan.DataBind();

            btnSearch.Enabled = false;
            var dt = ProcessDonHangBan.getInstance().TraCuuDonHangBan("", Utils.UserUtil.GetSessionUserId() + "");
            Session["ssgridQLDonHangBan"] = dt;
            gridQLDonHangBan.DataBind();
            btnSearch.Enabled = true;

        }
        protected void gridQLDonHangBan_DataBinding(object sender, EventArgs e)
        {
            // restore page size from previous client calculation if present
            try
            {
                var psObj = Session["ssgridQLDonHangBan_PageSize"];
                if (psObj != null)
                {
                    int ps = 0;
                    if (int.TryParse(psObj + "", out ps) && ps > 0)
                    {
                        gridQLDonHangBan.SettingsPager.PageSize = ps;
                    }
                }
            }
            catch { }
            gridQLDonHangBan.DataSource = Session["ssgridQLDonHangBan"];
            // Session["ssDMTaiSan"] = null;
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
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
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.EnableTrue;
                    else
                        ret = SysConfig.EnableFalse;
                    break;
            }
            return ret;
        }

     

    }
}