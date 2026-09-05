using WebRunDragon.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class CfgUserInfo : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CfgUserInfo));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(CfgUserInfo).Name.ToUpper()))
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);

                BindControl();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.gridUsers.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace, ex);
                logger.Error(ex);
            }
        }
        protected void gridUsers_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            this.gridUsers.DataBind();
        }

        protected void gridUsers_PageIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gridUsers_DataBinding(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbUsers = DataProcess.ProcessCfgUser.SearchStaffByKeyword4Grid((this.txtKeyword.Value + "").Trim(), Utils.UserUtil.GetSessionUserId());
                this.gridUsers.DataSource = dtbUsers;
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        private void BindControl()
        {
            try
            {
                this.cbDepartment.DataSource = DataProcess.ProcessCfgUser.GetListDepartment();
                this.cbDepartment.DataBind();

                this.cbBranch.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                this.cbBranch.DataBind();

                this.gridUsers.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        protected void lsbRoles_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                if (e.Parameter != null)
                {
                    DataTable dtbRoles = DataProcess.ProcessCfgUser.SearchRolesByUserName(e.Parameter, Utils.UserUtil.GetSessionUserId());

                    if (dtbRoles == null || dtbRoles.Rows.Count <= 0)
                        this.lsbRoles.Items.Clear();
                    else
                    {
                        this.lsbRoles.DataSource = dtbRoles;
                        this.lsbRoles.DataBind();
                    }
                }
                else
                    this.lsbRoles.Items.Clear();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        protected void lsbUnRoles_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter != null)
            {
                DataTable dtbUnRoles = DataProcess.ProcessCfgUser.SearchUnRolesByUserName((e.Parameter), Utils.UserUtil.GetSessionUserId());
                if (dtbUnRoles == null || dtbUnRoles.Rows.Count <= 0)
                    this.lsbUnRoles.Items.Clear();
                else
                {
                    this.lsbUnRoles.DataSource = dtbUnRoles;
                    this.lsbUnRoles.DataBind();
                }
            }
            else
                this.lsbRoles.Items.Clear();
        }

        protected void lsbUnAccess_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

            try
            {
                if (e.Parameter != null)
                {
                    DataTable dtbUnAccessGroups = DataProcess.ProcessCfgUser.SearchDataUnAccessGroupsByUserName((e.Parameter), Utils.UserUtil.GetSessionUserId());
                    if (dtbUnAccessGroups == null || dtbUnAccessGroups.Rows.Count <= 0)
                        this.lsbUnAccess.Items.Clear();
                    else
                    {
                        this.lsbUnAccess.DataSource = dtbUnAccessGroups;
                        this.lsbUnAccess.DataBind();
                    }
                }
                else
                    this.lsbRoles.Items.Clear();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }

        }

        protected void lsbAccess_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {

            try
            {
                if (e.Parameter != null)
                {
                    DataTable dtbAccessGroups = DataProcess.ProcessCfgUser.SearchDataAccessGroupsByUserName(e.Parameter, Utils.UserUtil.GetSessionUserId());
                    if (dtbAccessGroups == null || dtbAccessGroups.Rows.Count <= 0)
                        this.lsbAccess.Items.Clear();
                    else
                    {
                        this.lsbAccess.DataSource = dtbAccessGroups;
                        this.lsbAccess.DataBind();
                    }
                }
                else
                    this.lsbRoles.Items.Clear();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }

        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.CfgUserInfo).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }

    }
}