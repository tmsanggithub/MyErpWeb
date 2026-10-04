using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRunDragon.Config;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class QLDieuChinhKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLDieuChinhKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
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

                deNgayNhap.Date = DateTime.Now;
                cbMaChiNhanh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                cbMaChiNhanh.DataBind();
                cbNhanVienPhieu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgUsersHCQT + "", Utils.UserUtil.GetSessionUserId());
                cbNhanVienPhieu.DataBind();
                //cbNhanVienKhac.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgUsers + "", Utils.UserUtil.GetSessionUserId());
                //cbNhanVienKhac.DataBind();
                cbKhoNhap.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucKhoTheoBranchId(Utils.UserUtil.GetSessionBranchIdOfUserLogin() + "", Utils.UserUtil.GetSessionUserId());
                cbKhoNhap.DataBind();
                //cbTrangThaiCu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                //cbTrangThaiCu.DataBind();
                cbTrangThaiMoi.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTrangThaiMoi.DataBind();
                //cbTaiSanDieuChinhKho.DataSource = DataProcess.ProcessDieuChinhKhoNhanVien.getInstance().LayDanhSachTaiSan4ThemDieuChinhKhoNhanVien(0, Utils.UserUtil.GetSessionUserId());
                //cbTaiSanDieuChinhKho.DataBind();
                cbTrangThaiCu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTrangThaiCu.DataBind();

                cbTaiSanDieuChinhKho.DataSource = DataProcess.ProcessDanhMuc.getInstance().DMTaiSan4ComboFromCache(Utils.UserUtil.GetSessionUserId());
                cbTaiSanDieuChinhKho.DataBind();
                // LoadDataSource();
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
        protected void gridQLDieuChinhKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLDieuChinhKho_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            Session["ssKey4Search"] = " ";
            Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            this.gridQLDieuChinhKho.DataBind();

            //  gridQLDieuChinhKho.DataSource = DataProcess.ProcessDieuChinhKho.getInstance().TraCuuDieuChinhKho(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //   gridQLDieuChinhKho.DataBind();

        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
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

                case SysConfig.EnableApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDieuChinhKho).Name.ToUpper()))
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
                DataTable dtbDetails = DataProcess.ProcessDieuChinhKho.getInstance().TraCuuDieuChinhKhoChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                this.gridDetails.DataSource = dtbDetails;
                gridDetails.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        protected void gridLichSuKyDuyet_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessDieuChinhKho.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void gridDanhSachTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchTaiSan4ThemDieuChinhKho(int.Parse(e.Parameters));
        }
        protected void gridDanhSachTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            SearchTaiSan4ThemDieuChinhKho(int.Parse(cbKhoNhap.Value + ""));
        }
        protected void gridDanhSachTaiSan_DataBinding(object sender, EventArgs e)
        {

            gridDanhSachTaiSan.DataSource = Session["ssDanhSachTaiSanDieuChinhKho"];

        }
        private void SearchTaiSan4ThemDieuChinhKho(int IDKho)
        {
            try
            {

                //Session["ssIDDieuChinhKho"] = idMasterDieuChinhKho;
                //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
                //this.gridDanhSachTaiSan.DataBind();

                DataTable dtSource = DataProcess.ProcessDieuChinhKho.getInstance().LayDanhSachTaiSan4ThemDieuChinhKhoFromCache(IDKho, Utils.UserUtil.GetSessionUserId()); ;
                if (dtSource != null)
                {
                    Session["ssDanhSachTaiSanDieuChinhKho"] = dtSource;

                }

                this.gridDanhSachTaiSan.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
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
                            bool flag = DataProcess.ProcessDieuChinhKho.getInstance().AddOrUpdateAttachment(0, issueId, file.FileName, localStore, file.ContentType, file.ContentLength, file.FileName.Substring(lastIndexOf + 1), Utils.UserUtil.GetSessionUserId(), ref retCode);
                            */
                            string folderSaveyyyyMM = string.Empty;
                            string folderSaveDataBaseFull = string.Empty;
                            string folderSaveDataBaseExt = string.Empty;
                            string fileNameSaved = "";
                            ProcessFiles.getInstance().GetFolderSaveFile(file.FileName,ProcessDanhMuc.eTenDanhMuc.QLDieuChinhKho.ToString(), out folderSaveyyyyMM, out folderSaveDataBaseFull, out folderSaveDataBaseExt, out fileNameSaved);

                            if (!Directory.Exists(folderSaveyyyyMM)) Directory.CreateDirectory(folderSaveyyyyMM);
                            file.SaveAs(folderSaveDataBaseFull, false);
                            int retCode = 0;
                            bool flag = DataProcess.ProcessAttachment.AddOrUpdate(0, ProcessDanhMuc.eTenDanhMuc.QLDieuChinhKho.ToString(), issueId
                                , file.FileName, folderSaveDataBaseFull, file.ContentType, file.ContentLength
                                , file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), Utils.UserUtil.GetSessionUserId(),"", folderSaveDataBaseExt, ref retCode);

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
                DataTable dtbAttachments = DataProcess.ProcessDieuChinhKho.getInstance().GetTapTinDinhKem(Utils.NumberUtil.ParseToInt(txtObjectId["hidden_value"]), Utils.UserUtil.GetSessionUserId());
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


        // danh cho import



        public int GetBranchID()
        {
            return Utils.NumberUtil.ParseToInt(Session[Config.SysConfig.SESSION_BRANCHID]);
        }
        public string GetStaffNo()
        {
            return Utils.UserUtil.GetSessionStaff().StaffNo;
        }
        public int GetUserLoginDept()
        {
            return Utils.NumberUtil.ParseToInt(Utils.UserUtil.GetSessionDeptIdOfUserLogin());
        }
    }
}