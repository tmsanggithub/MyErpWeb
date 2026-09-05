using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class CfgFunctionRightInfo : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CfgFunctionRightInfo));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(CfgFunctionRightInfo).Name.ToUpper()))
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);

                BindControl();
            }
        }

        protected void gridRoles_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            this.gridRoles.DataBind();
        }

        protected void gridRoles_PageIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gridRoles_DataBinding(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbRoles = DataProcess.ProcessCfgFunctionRightAccess.SearchFunctionRightAccessGroup(Utils.UserUtil.GetSessionUserId());
                this.gridRoles.DataSource = dtbRoles;
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
                this.gridRoles.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        protected void gridDetails_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchRoleDetails(int.Parse(e.Parameters));
        }

        protected void gridDetails_PageIndexChanged(object sender, EventArgs e)
        {
            SearchRoleDetails(int.Parse(this.txtObjectId.Get("hidden_value") + ""));
        }

        private void SearchRoleDetails(int roleId)
        {
            try
            {
                DataTable dtbDetails = DataProcess.ProcessCfgFunctionRightAccess.SearchRoleDetails(roleId, Utils.UserUtil.GetSessionUserId());
                this.gridDetails.DataSource = dtbDetails;
                this.gridDetails.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void cboFunctionId_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            string[] args = e.Parameter.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            DataTable dtb = new DataTable();
            if (args[0].ToLower() == "add")
            {
                dtb = DataProcess.ProcessCfgFunctionRightAccess.GetFunctionList4Adding(int.Parse(args[1]), Utils.UserUtil.GetSessionUserId());
            }
            if (args[0].ToLower() == "edit")
            {
                dtb = DataProcess.ProcessCfgFunctionRightAccess.GetFunctionList4Editing(int.Parse(args[1]), int.Parse(args[2]), Utils.UserUtil.GetSessionUserId());
            }
            this.cboFunctionId.DataSource = dtb;
            this.cboFunctionId.DataBind();
        }


    }
}