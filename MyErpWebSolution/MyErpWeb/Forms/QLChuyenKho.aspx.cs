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
    public partial class QLChuyenKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLChuyenKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                    || Utils.ActionUtil.CanSpecialAction(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
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
        {//CHI NHÁNH BÌNH DƯƠNG
            try
            {
                //InitData();
                LoadDataSource();
                deNgayChuyen.Date = DateTime.Now;
                cbMaChiNhanh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                cbMaChiNhanh.DataBind();
                // ben giao load default theo userlogin
                cbPhongBanChuyen.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucPhongBanTheoBranchId(Utils.UserUtil.GetSessionBranchIdOfUserLogin(), Utils.UserUtil.GetSessionUserId());
                cbPhongBanChuyen.DataBind();
                cbNhanVienChuyen.DataSource = DataProcess.ProcessCfgUser.GetStaffInDepartment(Utils.NumberUtil.ParseToInt(Utils.UserUtil.GetSessionDeptIdOfUserLogin() + ""), Utils.UserUtil.GetSessionUserId());
                cbNhanVienChuyen.DataBind();
                DataTable dtDSNhanvien = DataProcess.ProcessCfgUser.GetStaffInDepartment(Utils.NumberUtil.ParseToInt(Utils.UserUtil.GetSessionDeptIdOfUserLogin() + ""), Utils.UserUtil.GetSessionUserId());
                cbTruongDonViChuyen.DataSource = dtDSNhanvien;
                cbTruongDonViChuyen.DataBind();

                // ben nhan
                cbPhongBanNhan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucPhongBanTheoBranchId(Utils.UserUtil.GetSessionBranchIdOfUserLogin(), Utils.UserUtil.GetSessionUserId());
                cbPhongBanNhan.DataBind();
                cbNhanVienNhan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgUsers + "", Utils.UserUtil.GetSessionUserId());
                cbNhanVienNhan.DataBind();

                // chi tiet

                cbTaiSanChuyenKho.DataSource = DataProcess.ProcessDanhMuc.getInstance().DMTaiSan4ComboFromCache(Utils.UserUtil.GetSessionUserId());
                cbTaiSanChuyenKho.DataBind();

                cbKhoXuat.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMKho + "", Utils.UserUtil.GetSessionUserId());
                cbKhoXuat.DataBind();
                cbKhoNhap.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMKho+"", Utils.UserUtil.GetSessionUserId());
                cbKhoNhap.DataBind();
                DataTable dtTrangThaiTS = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTrangThaiTaiSanXuatKho.DataSource = dtTrangThaiTS;
                cbTrangThaiTaiSanXuatKho.DataBind();

                cbTrangThaiTaiSanNhapKho.DataSource = dtTrangThaiTS;
                cbTrangThaiTaiSanNhapKho.DataBind();
              
                // Initialize warehouse lookup with warehouses from the selected branch (default to user's branch)
                cbKhoTraCuu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucKhoTheoBranchId(Utils.UserUtil.GetSessionBranchIdOfUserLogin(), Utils.UserUtil.GetSessionUserId());
                cbKhoTraCuu.DataBind();

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
        protected void gridQLChuyenKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLChuyenKho_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            //Session["ssKey4Search"] = txtKeyword.Text + "" == "" ? " " : txtKeyword.Text;
            //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            //this.gridQLChuyenKho.DataBind();

            //  gridQLChuyenKho.DataSource = DataProcess.ProcessChuyenKho.getInstance().TraCuuChuyenKho(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //   gridQLChuyenKho.DataBind();


            btnSearch.Enabled = false;
            var dt = DataProcess.ProcessChuyenKho.getInstance().TraCuuChuyenKho(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            Session["ssgridQLChuyenKho"] = dt;
            gridQLChuyenKho.DataBind();
            btnSearch.Enabled = true;
        }
        protected void gridTaiSan_DataBinding(object sender, EventArgs e)
        {
            gridQLChuyenKho.DataSource = Session["ssgridQLChuyenKho"];
            // Session["ssDMTaiSan"] = null;
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                        || Utils.ActionUtil.CanSpecialAction(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
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
                DataTable dtbDetails = DataProcess.ProcessChuyenKho.getInstance().TraCuuChuyenKhoChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
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

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessChuyenKho.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void gridDanhSachTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchTaiSan4ThemChuyenKho(Utils.NumberUtil.ParseToInt(this.txtObjectIDKhoTraCuu.Get("hidden_value") + ""));
        }
        protected void gridDanhSachTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            SearchTaiSan4ThemChuyenKho(Utils.NumberUtil.ParseToInt(this.txtObjectIDKhoTraCuu.Get("hidden_value") + ""));
        }

        private void SearchTaiSan4ThemChuyenKho(int IDKhoTraCuu)
        {
            try
            {

                //Session["ssIDKhoTraCuu"] = IDKhoTraCuu;
                //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
                //this.gridDanhSachTaiSan.DataBind();
                //if (IDKhoTraCuu > 0)
                //{
                //    this.gridDanhSachTaiSan.DataSource = DataProcess.ProcessChuyenKho.getInstance().LayDanhSachTaiSanChuyenKhoFromCache(IDKhoTraCuu, Utils.UserUtil.GetSessionUserId()); ;
                //    this.gridDanhSachTaiSan.DataBind();
                //}
                //else
                //{
                //    this.gridDanhSachTaiSan.DataSource = null;
                //    this.gridDanhSachTaiSan.DataBind();
                //}
                DataTable dt = null;
                if (IDKhoTraCuu > 0)
                {
                    dt = DataProcess.ProcessChuyenKho.getInstance().LayDanhSachTaiSanChuyenKhoFromCache(IDKhoTraCuu, Utils.UserUtil.GetSessionUserId()); ;
                }
                else
                {

                }
                Session["ssSearchTaiSan4ThemChuyenKho"] = dt;
                gridDanhSachTaiSan.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        protected void gridDanhSachTaiSan_DataBinding(object sender, EventArgs e)
        {
            gridDanhSachTaiSan.DataSource = Session["ssSearchTaiSan4ThemChuyenKho"];
        }

        protected void cbPhongBanNhan_OnCallback(object sender, CallbackEventArgsBase e)
        {
            cbPhongBanNhan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucPhongBanTheoBranchId(e.Parameter.ToString(), Utils.UserUtil.GetSessionUserId());
            cbPhongBanNhan.DataBind();
        }
        protected void cbNhanVienChuyen_OnCallback(object sender, CallbackEventArgsBase e)
        {
            cbNhanVienChuyen.DataSource = DataProcess.ProcessCfgUser.GetStaffInDepartment(Utils.NumberUtil.ParseToInt(e.Parameter), Utils.UserUtil.GetSessionUserId());
            cbNhanVienChuyen.DataBind();
        }
        protected void cbTruongDonViChuyen_OnCallback(object sender, CallbackEventArgsBase e)
        {
            DataTable dtDSNhanvien = DataProcess.ProcessCfgUser.GetStaffInDepartment(Utils.NumberUtil.ParseToInt(e.Parameter), Utils.UserUtil.GetSessionUserId());
            cbTruongDonViChuyen.DataSource = dtDSNhanvien;
            cbTruongDonViChuyen.DataBind();
            if (dtDSNhanvien != null && dtDSNhanvien.Rows.Count > 0)
            {
                cbTruongDonViChuyen.Value = dtDSNhanvien.Rows[0]["ManagerIdDepartment"];
            }
        }
        protected void cbNhanVienNhan_OnCallback(object sender, CallbackEventArgsBase e)
        {
            cbNhanVienNhan.DataSource = DataProcess.ProcessCfgUser.GetStaffInDepartment(Utils.NumberUtil.ParseToInt(e.Parameter), Utils.UserUtil.GetSessionUserId());
            cbNhanVienNhan.DataBind();
        }
        protected void cbTruongDonViNhan_OnCallback(object sender, CallbackEventArgsBase e)
        {

            DataTable dtDSNhanvien = DataProcess.ProcessCfgUser.GetStaffInDepartment(Utils.NumberUtil.ParseToInt(e.Parameter), Utils.UserUtil.GetSessionUserId());
            cbTruongDonViNhan.DataSource = dtDSNhanvien;
            cbTruongDonViNhan.DataBind();
            if (dtDSNhanvien != null && dtDSNhanvien.Rows.Count > 0)
            {
                cbTruongDonViNhan.Value = dtDSNhanvien.Rows[0]["ManagerIdDepartment"];
            }

        }

        protected void cbKhoTraCuu_OnCallback(object sender, CallbackEventArgsBase e)
        {
            cbKhoTraCuu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucKhoTheoBranchId(e.Parameter.ToString(), Utils.UserUtil.GetSessionUserId());
            cbKhoTraCuu.DataBind();
        }


        public int GetBranchID()
        {
            return Utils.NumberUtil.ParseToInt(Session[Config.SysConfig.SESSION_BRANCHID]);
        }
        public string GetUserLogin()
        {
            return Utils.UserUtil.GetSessionUserId();
        }
        public int GetDepartmentOfUserLogin()
        {
            return Utils.NumberUtil.ParseToInt(Utils.UserUtil.GetSessionDeptIdOfUserLogin());
        }
        public string GetManagerOfUserLogin()
        {
            return Utils.UserUtil.GetSessionManagerIdOfUserLogin();
        }
        /*

        // danh cho import ts
        protected void btnUploadImport_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string Extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                string FolderPath = "";// ConfigurationManager.AppSettings["FolderPath"];
                string FilePath = Server.MapPath(FolderPath + "/FilesImport/" + FileName + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss"));
                FileUpload1.SaveAs(FilePath);
                Import_To_Grid(FilePath, Extension);
            }
            else
            {
                lbThongBaoImport.Text = "Vui lòng chọn tập tin để import";
            }
        }

        private void Import_To_Grid(string FilePath, string Extension)
        {

            DataTable dt = new DataTable();
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
                // kiem tra ton tai data da import thì ko cho thuc hien, phai xoa het truoc khi import
                DataTable dtImported = ProcessChuyenKho.getInstance().GetDMTaiSanImportBanGiao(Utils.UserUtil.GetSessionUserId());
                if (dtImported != null && dtImported.Rows.Count > 0)
                {
                    lbThongBaoImport.Text = "Đã tồn tại dữ liệu import. Bạn không được phép tiếp tục import.Vui lòng xóa dữ liệu hiện tại nếu muốn import khác\n(Tra cứu để xem dữ liệu hiện tại)";
                    return;
                }

                conStr = String.Format(conStr, FilePath);
                connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;
                //Get the name of First Sheet  
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = "Sheet1$";// dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                connExcel.Close();
                //Read Data from First Sheet  
                connExcel.Open();
                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                //Bind Data to GridView  
            }
            catch (Exception ex)
            {
                //logger.Error("Lỗi xãy ra khi import thông tin cổ đông. Mã cổ đông cuối cùng đọc được: " + lastShareId, ex);
                // MessageBox.Show(this, "Có lỗi khi mở tập tin " + filePath + ", vui lòng thử lại. Mã cổ đông cuối cùng trước khi xãy ra lổi: " + lastShareId, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connExcel != null) connExcel.Close();
            }

            // autosave table

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.TableName = "QLBanGiaoCT_Import";
                DataTable dtSave = new DataTable("QLBanGiaoCT_Import");
                dtSave.Columns.Add("ID", typeof(int));
                dtSave.Columns.Add("MaTaiSan", typeof(string));
                dtSave.Columns.Add("SoLuongNhan", typeof(int));
                dtSave.Columns.Add("ViTriNhanTaiSan", typeof(string));
                dtSave.Columns.Add("MaKhoXuat", typeof(string));
                dtSave.Columns.Add("MaTrangThaiXuatKho", typeof(string));
                dtSave.Columns.Add("GhiChu", typeof(string));
                dtSave.Columns.Add("TrangThai", typeof(string));
                dtSave.Columns.Add("CreatedBy", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    if ((r["MaTrangThaiXuatKho"] + "").ToUpper() == Utils.StringUtil.eTrangThaiTaiSan.DangSuDung.ToString().ToUpper())
                    {
                        lbThongBaoImport.Text = "Trạng thái 'Đang sử dụng' không được phép bàn giao";
                        return;
                    }
                    else
                    {
                        DataRow radd = dtSave.NewRow();
                        radd["ID"] = -1;
                        radd["MaTaiSan"] = r["MaTaiSan"];
                        radd["SoLuongNhan"] = r["SoLuongNhan"];
                        radd["ViTriNhanTaiSan"] = r["ViTriNhanTaiSan"];
                        radd["MaKhoXuat"] = r["MaKhoXuat"];
                        radd["MaTrangThaiXuatKho"] = r["MaTrangThaiXuatKho"];
                        radd["GhiChu"] = r["GhiChu"];
                        radd["TrangThai"] = "NEW";
                        radd["CreatedBy"] = Utils.UserUtil.GetSessionUserId();
                        dtSave.Rows.Add(radd);
                    }
                }

                bool save = DatabaseManager.AutoSaveDataTable(DatabaseManager.CNN_STRING_HELPDESK, dtSave, "QLChuyenKhoCT_Import");
                if (save)
                {
                    LoadDataSourceImportTaiSanNhapKho();
                    lbThongBaoImport.Text = "import thành công";
                    //txtObjectResult["hide_Value"] = "ImportThanhCong";
                    FileUpload1.PostedFile.InputStream.Dispose();
                    FileUpload1.Dispose();
                }
                else
                {
                    lbThongBaoImport.Text = "import thất bại";
                }

            }
        }

        private void LoadDataSourceImportTaiSanNhapKho()
        {
            //Bind Data to GridView    
            //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();

            DataTable dtimport = ProcessChuyenKho.getInstance().GetDMTaiSanImportBanGiao(Utils.UserUtil.GetSessionUserId());
            Session["ssLoadDataSourceImportTaiSanBanGiao"] = dtimport;
            gridImportTaiSan.DataBind();

        }

        protected void gridImportTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceImportTaiSanNhapKho();
        }

        protected void gridImportTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSourceImportTaiSanNhapKho();
        }

        protected void gridImportTaiSan_DataBinding(object sender, EventArgs e)
        {

            gridImportTaiSan.DataSource = Session["ssLoadDataSourceImportTaiSanBanGiao"];

        }
        */
        //--ket thuc import-----------------------------------
    }
}