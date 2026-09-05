using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class CfgDataRightInfo : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CfgDataRightInfo));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(CfgDataRightInfo).Name.ToUpper()))
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);

                BindControl();
            }
        }

        protected void gridAccessGroups_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            this.gridAccessGroups.DataBind();
        }

        protected void gridAccessGroups_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridAccessGroups_DataBinding(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbDbList = DataProcess.ProcessCfgDataRightAccess.SearchDataAccessGroup(Utils.UserUtil.GetSessionUserId());
                this.gridAccessGroups.DataSource = dtbDbList;
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace, ex);
                logger.Error(ex);
            }
        }
        private void BindControl()
        {
            try
            {
                this.gridAccessGroups.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace, ex);
                logger.Error(ex);
            }
        }

        protected void lsbAccess_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                int accessId = int.Parse(e.Parameter);
                if (accessId == 0)
                    this.lsbAccess.Items.Clear();
                else
                {
                    DataTable dtbStaffs = DataProcess.ProcessCfgDataRightAccess.GetPrivilegeStaffByAccessGroup(accessId, Utils.UserUtil.GetSessionUserId());
                    if (dtbStaffs == null || dtbStaffs.Rows.Count <= 0)
                        this.lsbAccess.Items.Clear();
                    else
                    {
                        this.lsbAccess.DataSource = dtbStaffs;
                        this.lsbAccess.DataBind();
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        protected void lsbUnAccess_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string[] param = e.Parameter.Split("|".ToCharArray());
                int accessId = int.Parse(param[0]);
                string filterName = param[1].Trim();
                if (accessId == 0)
                    this.lsbUnAccess.Items.Clear();
                else
                {
                    DataTable dtbUnStaffs = DataProcess.ProcessCfgDataRightAccess.GetUnPrivilegeStaffByAccessGroup(accessId, filterName, Utils.UserUtil.GetSessionUserId());
                    if (dtbUnStaffs == null || dtbUnStaffs.Rows.Count <= 0)
                        this.lsbUnAccess.Items.Clear();
                    else
                    {
                        this.lsbUnAccess.DataSource = dtbUnStaffs;
                        this.lsbUnAccess.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }
    }
}