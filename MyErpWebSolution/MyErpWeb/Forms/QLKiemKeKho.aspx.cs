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
    public partial class QLKiemKeKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLKiemKeKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
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

                deNgayChotDanhSach.Date = DateTime.Now;
                //cbMaChiNhanh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                //cbMaChiNhanh.DataBind();
                cbMaNhanVienHCQT.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgUsersHCQT + "", Utils.UserUtil.GetSessionUserId());
                cbMaNhanVienHCQT.DataBind();
                cbMaNhanVienKeToan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgUsers + "", Utils.UserUtil.GetSessionUserId());
                cbMaNhanVienKeToan.DataBind();

                // Load danh sách trạng thái - chỉ bind 1 lần cho mỗi combo
                cbTrangThaiCu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTrangThaiCu.DataBind();

                cbTrangThaiMoi.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTrangThaiMoi.DataBind();

                cbTaiSanChiTiet.DataSource = DataProcess.ProcessDanhMuc.getInstance().DMTaiSan4ComboFromCache(Utils.UserUtil.GetSessionUserId());
                cbTaiSanChiTiet.DataBind();
                //  LoadDataSource();
                cbMaPhongBanDuocKiemKe.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgDepartments + "", Utils.UserUtil.GetSessionUserId());
                cbMaPhongBanDuocKiemKe.DataBind();
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
        protected void gridQLKiemKeKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLKiemKeKho_PageIndexChanged(object sender, EventArgs e)
        {
            // Không cần load lại data khi chuyển trang, data đã có trong session
            // Chỉ cần rebind grid từ session
            gridQLKiemKeKho.DataBind();
        }
        private void LoadDataSource()
        {
            try
            {
                btnSearch.Enabled = false;
                var dt = DataProcess.ProcessKiemKeKho.getInstance().TraCuuKiemKeKho(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
                Session["ssTraCuuKiemKeKho"] = dt;
                gridQLKiemKeKho.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error("LoadDataSource Error: " + ex.Message);
            }
            finally
            {
                btnSearch.Enabled = true;
            }
        }

        protected void gridQLKiemKeKho_DataBinding(object sender, EventArgs e)
        {
            gridQLKiemKeKho.DataSource = Session["ssTraCuuKiemKeKho"];
        }


        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
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
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLKiemKeKho).Name.ToUpper()))
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
        protected void gridDetails_DataBinding(object sender, EventArgs e)
        {
            gridDetails.DataSource = Session["ssTraCuuKiemKeKhoDetail"];
        }
        private void SearchDetails(int idMaster)
        {
            try
            {


                //DataTable dtbDetails = DataProcess.ProcessKiemKeKho.getInstance().TraCuuKiemKeKhoChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                //this.gridDetails.DataSource = dtbDetails;
                //gridDetails.DataBind();


                var dtDetail = DataProcess.ProcessKiemKeKho.getInstance().TraCuuKiemKeKhoChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                Session["ssTraCuuKiemKeKhoDetail"] = dtDetail;
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

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessKiemKeKho.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void gridDanhSachTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchTaiSan4ThemKiemKeKho(int.Parse(e.Parameters));
        }
        protected void gridDanhSachTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            //SearchTaiSan4ThemKiemKeKho(int.Parse(cbKhoNhap.Value + ""));
        }
        protected void gridDanhSachTaiSan_DataBinding(object sender, EventArgs e)
        {
            if (Session["ssDanhSachTaiSanKiemKeKho"] != null)
                gridDanhSachTaiSan.DataSource = Session["ssDanhSachTaiSanKiemKeKho"];

        }
        private void SearchTaiSan4ThemKiemKeKho(int IDKho)
        {
            try
            {
                // Sửa: Lưu vào session key đúng với gridDanhSachTaiSan_DataBinding
                DataTable dtSource = DataProcess.ProcessDieuChinhKho.getInstance().LayDanhSachTaiSan4ThemDieuChinhKhoFromCache(IDKho, Utils.UserUtil.GetSessionUserId());
                if (dtSource != null)
                {
                    Session["ssDanhSachTaiSanKiemKeKho"] = dtSource;
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
                            string folderSaveyyyyMM = string.Empty;
                            string folderSaveDataBaseFull = string.Empty;
                            string folderSaveDataBaseExt = string.Empty;
                            string fileNameSaved = "";
                            ProcessFiles.getInstance().GetFolderSaveFile(file.FileName, ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho.ToString(), out folderSaveyyyyMM, out folderSaveDataBaseFull, out folderSaveDataBaseExt, out fileNameSaved);

                            if (!Directory.Exists(folderSaveyyyyMM)) Directory.CreateDirectory(folderSaveyyyyMM);
                            file.SaveAs(folderSaveDataBaseFull, false);
                            int retCode = 0;
                            bool flag = DataProcess.ProcessAttachment.AddOrUpdate(0, ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho.ToString(), issueId, file.FileName, folderSaveDataBaseFull, file.ContentType, file.ContentLength, file.FileName.Substring(file.FileName.LastIndexOf(".") + 1), Utils.UserUtil.GetSessionUserId(), "", folderSaveDataBaseExt, ref retCode);

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
                DataTable dtbAttachments = DataProcess.ProcessAttachment.GetAttachments(ProcessDanhMuc.eTenDanhMuc.QLKiemKeKho.ToString(), Utils.NumberUtil.ParseToInt(txtObjectId["hidden_value"]), Utils.UserUtil.GetSessionUserId());
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
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {/*
                if (FileUpload1.HasFile)
                {
                    logger.Error("btnUpload_Click.OK tim thay file");
                    string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string FolderPath = "";// ConfigurationManager.AppSettings["FolderPath"];
                    string FilePath = Server.MapPath(FolderPath + "/FilesImport/" + FileName + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                    FileUpload1.SaveAs(FilePath);
                    Import_To_Grid(FilePath, Extension);
                }
                else
                {
                    logger.Error("btnUpload_Click.Ko tim thay file");
                }
                */
            }
            catch (Exception ex)
            {
                logger.Error($"btnUpload_Click.Loi exception {ex.Message}");
            }
        }
        DataTable dtImportTemp = new DataTable();
        private void Import_To_Grid(string FilePath, string Extension)
        {/*
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03  
                    conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07  
                    conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
            }
            OleDbConnection connExcel = null;
            try
            {
                conStr = String.Format(conStr, FilePath);
                connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                //Get the name of First Sheet  
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();
                //Read Data from First Sheet  
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dtImportTemp);
                connExcel.Close();
                //Bind Data to GridView  
            }
            catch (Exception ex)
            {
                // logger.Error("Lỗi xãy ra khi import thông tin cổ đông. Mã cổ đông cuối cùng đọc được: " + lastShareId, ex);
                // MessageBox.Show(this, "Có lỗi khi mở tập tin " + filePath + ", vui lòng thử lại. Mã cổ đông cuối cùng trước khi xãy ra lổi: " + lastShareId, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connExcel != null) connExcel.Close();
            }

            // autosave table

            if (dtImportTemp != null && dtImportTemp.Rows.Count > 0)
            {
                dtImportTemp.TableName = "QLKiemKeKhoCT_Import";
                DataTable dtSave = new DataTable("QLKiemKeKhoCT_Import");
                dtSave.Columns.Add("ID", typeof(int));
                //dtSave.Columns.Add("MaChiNhanh", typeof(string));
                //dtSave.Columns.Add("MaPhongBan", typeof(string));
                //dtSave.Columns.Add("MaKho", typeof(string));
                dtSave.Columns.Add("MaNhanVien", typeof(string));
                dtSave.Columns.Add("MaTaiSan", typeof(string));
                dtSave.Columns.Add("MaTrangThaiTaiSan", typeof(string));
                dtSave.Columns.Add("DonGiaConLai", typeof(string));
                dtSave.Columns.Add("SoLuongSoSach", typeof(string));
                dtSave.Columns.Add("SoLuongKiemKe", typeof(string));
                dtSave.Columns.Add("GhiChu", typeof(string));
                dtSave.Columns.Add("TrangThai", typeof(string));
                dtSave.Columns.Add("CreatedBy", typeof(string));
                dtSave.Columns.Add("CreadDate", typeof(DateTime));
                foreach (DataRow r in dtImportTemp.Rows)
                {
                    DataRow radd = dtSave.NewRow();
                    radd["ID"] = -1;
                    // radd["MaChiNhanh"] = r["MaChiNhanh"];
                    //radd["MaPhongBan"] = r["MaPhongBan"];
                    //radd["MaKho"] = r["MaKho"];
                    radd["MaNhanVien"] = r["MaNhanVien"];
                    radd["MaTaiSan"] = r["MaTaiSan"];
                    radd["MaTrangThaiTaiSan"] = r["MaTrangThaiTaiSan"];

                    radd["DonGiaConLai"] = r["DonGiaConLai"];
                    radd["SoLuongSoSach"] = r["SoLuongSoSach"];
                    radd["SoLuongKiemKe"] = r["SoLuongKiemKe"];
                    radd["CreatedBy"] = Utils.UserUtil.GetSessionUserId();
                    radd["CreadDate"] = DateTime.Now;
                    radd["TrangThai"] = "NEW";

                    dtSave.Rows.Add(radd);
                }
                bool save = DatabaseManager.AutoSaveDataTable(DatabaseManager.CNN_STRING_HELPDESK, dtSave, "QLKiemKeKhoCT_Import");
                if (save)
                {
                    LoadDataSourceGridView(-1);
                    lbThongBao.Text = "import thành công";
                    // txtObjectResult["hide_Value"] = "ImportThanhCong";
                    FileUpload1.PostedFile.InputStream.Dispose();
                    FileUpload1.Dispose();

                }
            }
            */
        }
        private void LoadDataSourceGridView(int idKiemKe)
        {
            try
            {
                DataTable dtSource = DataProcess.ProcessImportTaiSan.getInstance().GetDMTaiSanImportPhongBan(idKiemKe, Utils.UserUtil.GetSessionUserId());
                if (dtSource != null)
                {
                    Session["ssDanhSachTaiSanImportPhongBan"] = dtSource;
                }
                this.gridImportTaiSan.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void gridImportTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceGridView(int.Parse(e.Parameters));
        }
        protected void gridImportTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            //  LoadDataSourceGridView(int.Parse(e.Parameters));
        }
        protected void gridImportTaiSan_DataBinding(object sender, EventArgs e)
        {
            // LoadDataSourceGridView();

            if (Session["ssDanhSachTaiSanImportPhongBan"] != null)
                gridImportTaiSan.DataSource = Session["ssDanhSachTaiSanImportPhongBan"];


        }
        //protected void btnXoa_Click(object sender, EventArgs e)
        //{
        //    bool delele = ProcessImportTaiSan.getInstance().DeleteAllTaiSanImport(Utils.UserUtil.GetSessionUserId());
        //    if (delele)
        //    {
        //        LoadDataSourceGridView(int.Parse(e.Parameters));
        //    }
        //}


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