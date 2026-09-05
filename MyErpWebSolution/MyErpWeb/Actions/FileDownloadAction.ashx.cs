using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for FileDownloadAction
    /// </summary>
    public class FileDownloadAction : IHttpHandler
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(FileDownloadAction));
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //string id = context.Request.QueryString["Id"];
                //id = id + "" == "" ? "0" : id;

                int attId = int.Parse(context.Request.Params["ID"]);
                DataTable dtbAttach = DataProcess.ProcessAttachment.GetAttachmentById(attId, "");
                if (dtbAttach != null && dtbAttach.Rows.Count > 0)
                {
                    string filePath = dtbAttach.Rows[0]["FilePath"] + "";
                    if (!System.IO.File.Exists(filePath))
                    {
                        logger.Error(Utils.UserUtil.GetSessionUserId() + " không thể tải tập tin không tồn tại. Id = " + attId + " | Đường dẫn" + filePath);
                    }
                    else
                    {
                        context.Response.Clear();
                        context.Response.AddHeader("Content-Disposition", "filename=" + (dtbAttach.Rows[0]["AttachName"] + "" + "").Replace(" ", ""));
                    //    context.Response.AddHeader("Content-Length", dtbAttach.Rows[0]["FileSize"] + "");
                        context.Response.ContentType = dtbAttach.Rows[0]["ContentType"] + "";
                        context.Response.TransmitFile(dtbAttach.Rows[0]["FilePath"] + "");
                        context.Response.Flush();
                        return;
                    }

                }
            }
            catch (Exception ex) { }


        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}