using DevExpress.Web;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class WebUploadFile : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(WebUploadFile));

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    LoadDataSource();
                }
                else
                {

                }

            }
            catch (Exception er)
            {
                logger.Error("DMTaiSan.PageLoad.Message:" + er.Message + "\nTrace:" + er.StackTrace, er);
            }

        }

        protected void gridAttachments_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }

        protected void gridAttachments_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }

        protected void gridAttachments_DataBinding(object sender, EventArgs e)
        {
            gridAttachments.DataSource = Session["ssWebUploadFileSession"];
        }

        private void LoadDataSource()
        {
           // DataTable dtbAttachments = DataProcess.ProcessThanhToan.getInstance().GetTapTinDinhKemUploadFile(0, Utils.UserUtil.GetSessionUserId());
           // Session["ssWebUploadFileSession"] = dtbAttachments;
          //  gridAttachments.DataBind();

        }

        protected void UploadControl_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject result = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray();
            bool success = false;
            string message = "";
            try
            {

                //UploadControl
                foreach (UploadedFile file in uplAttachment.UploadedFiles)
                {
                    if (!string.IsNullOrEmpty(file.FileName) && file.IsValid)
                    {
                        string folderSaveyyyyMM = string.Empty;
                        string folderSaveDataBaseFull = string.Empty;
                        string folderSaveDataBaseExt = string.Empty;
                        string fileNameSaved = "";
                        ProcessFiles.getInstance().GetFolderSaveFile(file.FileName, ProcessDanhMuc.eTenDanhMuc.WebUploadFile.ToString(), out folderSaveyyyyMM, out folderSaveDataBaseFull, out folderSaveDataBaseExt, out fileNameSaved);

                        if (!Directory.Exists(folderSaveyyyyMM))
                        {
                            Directory.CreateDirectory(folderSaveyyyyMM);
                        }
                        file.SaveAs(folderSaveDataBaseFull, false);

                        //replace text----------
                        //string folderSaveDataBaseFull_AfterReplace= folderSaveDataBaseFull.Replace(".pdf", "_2.pdf");
                        //VerySimpleReplaceText(folderSaveDataBaseFull, folderSaveDataBaseFull_AfterReplace, "1 Kg", "13 Kg");
                        //-----------------------

                        
                        int retCode = 0;
                        bool flag = DataProcess.ProcessAttachment.AddOrUpdate(0, ProcessDanhMuc.eTenDanhMuc.WebUploadFile.ToString(), 0, file.FileName, folderSaveDataBaseFull, file.ContentType, file.ContentLength, file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), Utils.UserUtil.GetSessionUserId(), "", folderSaveDataBaseExt, ref retCode);

                        // json return
                        Newtonsoft.Json.Linq.JObject row = new Newtonsoft.Json.Linq.JObject();
                        row["id"] = flag ? retCode : 0;
                        row["file"] = file.FileName;
                        row["retCode"] = retCode;
                        row["success"] = flag;
                        array.Add(row);

                        
                    }
                }
                success = true;
         
            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error(ex);
            }
            finally
            {
                result["list"] = array;
                result["success"] = success;
                result["message"] = message;
                e.CallbackData = result.ToString();
            }

        }

        void VerySimpleReplaceText(string OrigFile, string ResultFile, string origText, string replaceText)
        {

            using (PdfReader reader = new PdfReader(OrigFile))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    byte[] contentBytes = reader.GetPageContent(i);
                    string contentString = PdfEncodings.ConvertToString(contentBytes, PdfObject.TEXT_PDFDOCENCODING);
                    contentString = contentString.Replace(origText, replaceText);
                    reader.SetPageContent(i, PdfEncodings.ConvertToBytes(contentString, PdfObject.TEXT_PDFDOCENCODING));
                }
                new PdfStamper(reader, new FileStream(ResultFile, FileMode.Create, FileAccess.Write)).Close();
                
            }
        }
    }
}