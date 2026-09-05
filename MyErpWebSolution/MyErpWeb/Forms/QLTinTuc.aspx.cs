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
    public partial class QLTinTuc : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLTinTuc));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
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
                de_news_date.Date = DateTime.Now;

                cbGiaiChay.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.ql_race+"", Utils.UserUtil.GetSessionUserId());
                cbGiaiChay.DataBind();

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
        protected void gridQLTinTuc_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLTinTuc_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            //Session["ssKey4Search"] = " ";
            //Session["ssgridQLTinTuc"] = Utils.UserUtil.GetSessionUserId();
            //this.gridQLTinTuc.DataBind();

            //  gridQLTinTuc.DataSource = DataProcess.ProcessTinTuc.getInstance().TraCuuTinTuc(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //  gridQLTinTuc.DataBind();

            btnSearch.Enabled = false;
            var dt = ProcessTinTuc.getInstance().TraCuuTinTuc("", Utils.UserUtil.GetSessionUserId() + "");
            Session["ssgridQLTinTuc"] = dt;
            gridQLTinTuc.DataBind();
            btnSearch.Enabled = true;

        }
        protected void gridQLTinTuc_DataBinding(object sender, EventArgs e)
        {
            gridQLTinTuc.DataSource = Session["ssgridQLTinTuc"];
            // Session["ssDMTaiSan"] = null;
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
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
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLTinTuc).Name.ToUpper()))
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

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessTinTuc.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        protected void gridDanhSachNhomChay_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            //   SearchTaiSan4ThemTinTuc(Utils.NumberUtil.ParseToInt(this.txtObjectIDKhoTraCuu.Get("hidden_value") + ""));
        }
        protected void gridDanhSachNhomChay_PageIndexChanged(object sender, EventArgs e)
        {
            // SearchTaiSan4ThemTinTuc(Utils.NumberUtil.ParseToInt(this.txtObjectIDKhoTraCuu.Get("hidden_value") + ""));
        }
        private void SearchTaiSan4ThemTinTuc(int IDKhoTraCuu)
        {
            try
            {
                Session["ssIDKhoTraCuu"] = IDKhoTraCuu;
                Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
                //  this.gridDanhSachTaiSan.DataBind();

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

        // dành cho upload file ======================
        #region dành cho upload file 
        protected void UploadControl_FilesUploadComplete(object sender, DevExpress.Web.FilesUploadCompleteEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject result = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JArray array = new Newtonsoft.Json.Linq.JArray();
            bool success = false;
            string message = "";
            try
            {
                if (this.txtObjectId == null || !this.txtObjectId.Contains("hidden_value"))
                {
                    return;
                }

                int issueId = Utils.NumberUtil.ParseToInt(this.txtObjectId.Get("hidden_value"));
                // int issueId =Utils.NumberUtil.ParseToInt(txtSoLuongCu.Text);
                //if (issueId > 0)
                //{
                //    // chỉ update mới kiểm tra quyền
                //    // thêm mới thì issueId = 0 vì lúc đó user chưa nhấn nút lưu
                //    DataTable dtbPermission = DataProcess.ProcessYeuCauHoTRo.GetPermission(issueId, Utils.UserUtil.GetSessionUserId());
                //    if ((int)dtbPermission.Rows[0]["CanUpload"] <= 0)
                //    {
                //        message = "Bạn không có quyền thực hiện chức năng này";
                //        return;
                //    }
                //}
                if (issueId > 0)
                {

                    JObject objHopDong = DataProcess.ProcessTinTuc.getInstance().GetTinTucByID(issueId);
                    string currentStatus = objHopDong["status"] + "";
                    if (currentStatus == "NEW" || currentStatus == "EDIT")
                    {
                        //UploadControl
                        foreach (UploadedFile file in uplAttachment.UploadedFiles)
                        {
                            if (!string.IsNullOrEmpty(file.FileName) && file.IsValid)
                            {
                                /*
                                 string folder = Config.SysConfig.ATTACHMENTS_FOLDER + "\\" + DateTime.Now.ToString("yyyyMM");
                                 if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                                 int lastIndexOf = file.FileName.LastIndexOf(".");
                                 string localStore = folder + "\\" + file.FileName.Substring(0, lastIndexOf) + "." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + "." + file.FileName.Substring(lastIndexOf + 1);
                                 //  localStore = localStore.Replace(" ", "");
                                 file.SaveAs(localStore, false);

                                 int retCode = 0;
                                 bool flag = DataProcess.ProcessHopDong.getInstance().AddOrUpdateAttachment(0, issueId, file.FileName, localStore, file.ContentType, file.ContentLength, file.FileName.Substring(lastIndexOf + 1), Utils.UserUtil.GetSessionUserId(), ref retCode);
                                 */

                                string folderSaveyyyyMM = string.Empty;
                                string folderSaveDataBaseFull = string.Empty;
                                string folderSaveDataBaseExt = string.Empty;
                                string fileNameSaved = "";
                                ProcessFiles.getInstance().GetFolderSaveFile(file.FileName, ProcessDanhMuc.eTenDanhMuc.ql_news.ToString(), out folderSaveyyyyMM, out folderSaveDataBaseFull, out folderSaveDataBaseExt, out fileNameSaved);

                                if (!Directory.Exists(folderSaveyyyyMM)) Directory.CreateDirectory(folderSaveyyyyMM);
                                file.SaveAs(folderSaveDataBaseFull, false);
                                int retCode = 0;
                                bool flag = DataProcess.ProcessAttachment.AddOrUpdate(0, ProcessDanhMuc.eTenDanhMuc.ql_news.ToString(), issueId, file.FileName, folderSaveDataBaseFull, file.ContentType, file.ContentLength, file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), Utils.UserUtil.GetSessionUserId(), "", folderSaveDataBaseExt, ref retCode);

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
                    else
                    {
                        message = "Chỉ cho phép thêm tập tin khi mới tạo hoặc chỉnh sửa";
                        success = false;
                    }
                }
                else
                {
                    message = "Vui lòng lưu thông tin trước khi đính kèm tập tin";
                    success = false;
                }
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
        protected void gridAttachments_DataBinding(object sender, EventArgs e)
        {
            try
            {
                DataTable dtbAttachments = DataProcess.ProcessTinTuc.getInstance().GetTapTinDinhKem(Utils.NumberUtil.ParseToInt(txtObjectId["hidden_value"]), Utils.UserUtil.GetSessionUserId());
                gridAttachments.DataSource = dtbAttachments;
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        protected void gridAttachments_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "FileSize")
                {
                    e.DisplayText = Utils.NumberUtil.FormatNumber(Math.Round(Utils.NumberUtil.ParseToDecimal(e.Value) / 1000, 0));
                }
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }

        protected void gridAttachments_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                this.gridAttachments.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }
        #endregion
        // =========================================
    }
}