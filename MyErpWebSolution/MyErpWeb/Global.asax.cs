using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using WebRunDragon.Utils;

namespace WebRunDragon
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Global));
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                logger.Info("********************* Application_Start ok *********************");
                logger.Error("CNN_STRING_HELPDESK_1111 :" + ConfigurationManager.ConnectionStrings["assetCnnString"].ConnectionString.Trim());
                WebRunDragon.DataProcess.DatabaseManager.CNN_STRING_HELPDESK = ConfigurationManager.ConnectionStrings["assetCnnString"].ConnectionString.Trim();

                logger.Error("CNN_STRING_HELPDESK_ set:" + WebRunDragon.DataProcess.DatabaseManager.CNN_STRING_HELPDESK);
                int assetCnnStringIsEncrypted = 0;
                int.TryParse(ConfigurationManager.AppSettings["assetCnnStringIsEncrypted"], out assetCnnStringIsEncrypted);
                if (assetCnnStringIsEncrypted == 0)
                {
                    logger.Warn("Chuỗi kết nối CSDL Helpdesk ok");
   
                       Config.SysConfig.ITHELPDESK_CNNSTRING_IS_ENCRYPTED = false;
                    //  logger.Warn("Test helpdesk connection: " + DataProcess.DatabaseManager.TestHelpDeskConnection());
                }
                else
                {
                    Config.SysConfig.ITHELPDESK_CNNSTRING_IS_ENCRYPTED = true;
                    logger.Warn("Chuỗi kết nối CSDL Helpdesk bị mã hóa, vui lòng nhập Mã để sử dụng hệ thống.");
                }
                // Config.EmailConfig.UseSendMail = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSendMail"].ToString());
                //Config.EmailConfig.FolderPath4EmailTemplate = Server.MapPath("~/App_Data/MailTemplate");
                string pathAtt = "~/" + ConfigurationManager.AppSettings["AttachmentsFolder"];
                string pathAttRecycle = "~/" + ConfigurationManager.AppSettings["AttachmentsRecycleBinFolder"];
                Config.SysConfig.ATTACHMENTS_FOLDER_CO_ODIA_NOT_YYYYMM = Server.MapPath(pathAtt);
                Config.SysConfig.ATTACHMENTS_RECYCLEBIN_FOLDER_CO_ODIA_NOT_YYYYMM = Server.MapPath(pathAttRecycle);
                Config.SysConfig.ATTACHMENTS_FOLDER_ONLY_NOT_ODIA_NOT_YYYYMM = ConfigurationManager.AppSettings["AttachmentsFolder"];


                //Config.SysConfig.ATTACHMENTS_FOLDER = ConfigurationManager.AppSettings["AttachmentsFolder"] + "" == "" ? Server.MapPath("~/App_Data/Attachments") : ConfigurationManager.AppSettings["AttachmentsFolder"].Trim();
                //Config.SysConfig.ATTACHMENTS_RECYCLEBIN_FOLDER = ConfigurationManager.AppSettings["AttachmentsRecycleBinFolder"] + "" == "" ? Server.MapPath("~/App_Data/Attachments") : ConfigurationManager.AppSettings["AttachmentsRecycleBinFolder"].Trim(); ;

                Config.SysConfig.IS_BYPASS_PASSWORD = int.Parse(ConfigurationManager.AppSettings["byPassPassword"].Trim()) > 0;

                //logger.Info("Thư mục hiện tại: " + Directory.GetCurrentDirectory() + "./.");
                //logger.Info("Thư mục tập tin đính kèm: " + Config.SysConfig.ATTACHMENTS_FOLDER);
                //logger.Info("Thư mục thùng rác: " + Config.SysConfig.ATTACHMENTS_RECYCLEBIN_FOLDER);

                try
                {
                  //  ResourceUtil.PutResourceToMemory();

                }
                catch (Exception er)
                {

                }

                logger.Info("Application.StartUpPath: " + HttpContext.Current.Server.MapPath("Forms") + "./.");
                logger.Info("Server.MapPath(): " + Server.MapPath("Forms/") + "./.");
                logger.Info("********************* Application_Start success *********************");
            }
            catch (Exception er)
            {
                logger.Info("********************* Application_Start Erro:Message:" + er.Message + "\nTrace:" + er.StackTrace);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            logger.Info("********************* Session_Start *********************");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            logger.Info("********************* Application_Error *********************");
        }

        protected void Session_End(object sender, EventArgs e)
        {
            logger.Info("********************* Session_End *********************");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            logger.Info("---------------- Application_End begin -------------------");

            HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);

            if (runtime == null)
            {
                return;
            }

            string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);

            string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);

            ApplicationShutdownReason shutdownReason = System.Web.Hosting.HostingEnvironment.ShutdownReason;

            logger.Debug(String.Format("\r\n\r\nAPPLICATION END\r\n\r\n_shutDownReason = {2}\r\n\r\n _shutDownMessage = {0}\r\n\r\n_shutDownStack = {1}\r\n\r\n",
                            shutDownMessage, shutDownStack, shutdownReason));

            logger.Info(" ---------------- Application_End end ------------------- ");
        }
        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            //if (Config.SysConfig.ITHELPDESK_CNNSTRING_IS_ENCRYPTED)
            //{
            //    //if (!(context.Request.RawUrl.IndexOf("_CnnString_Forbidden") > 0))
            //    //{
            //    //    logger.Warn("Kết nối bị từ chối từ IP = " + HttpContext.Current.Request.UserHostAddress);
            //    //    context.Response.Redirect("~/Forms/_CnnString_Forbidden.aspx");
            //    //}
            //}
            //else
            //{

            if (context.Request.RawUrl.IndexOf("Login") > 0
                || context.Request.RawUrl.IndexOf("Logout") > 0
                || context.Request.RawUrl.IndexOf("AuthenAction") > 0
                || context.Request.RawUrl.IndexOf("_Error_Forbidden") > 0
                || context.Request.RawUrl.IndexOf("_CnnString_Forbidden") > 0
                || context.Request.RawUrl.IndexOf("WebReplaceTextPDF") > 0
            )
            {
                // ko làm gì cả
            }
            else
            {
                if (context.Session != null)
                {
                    if (context.Session[Config.SysConfig.SESSION_USERINFO] == null)
                    {
                        logger.Debug("a Man deo cho quyen");
                        context.Session[Config.SysConfig.SESSION_LASTURL_BEFORE_AUTHEN] = context.Request.RawUrl;
                        context.Response.Redirect("~/Forms/CfgLogin.aspx");
                    }
                }
            }




        }
    }
}