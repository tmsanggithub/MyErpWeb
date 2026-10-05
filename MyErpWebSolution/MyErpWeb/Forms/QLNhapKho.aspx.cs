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
    public partial class QLNhapKho : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLNhapKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
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
                // LoadDataSource();
                deNgayNhap.Date = DateTime.Now;
                cbMaChiNhanh.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgBranches + "", Utils.UserUtil.GetSessionUserId());
                cbMaChiNhanh.DataBind();
                cbNhanVienPhieu.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.cfgUsersHCQT + "", Utils.UserUtil.GetSessionUserId());
                cbNhanVienPhieu.DataBind();
                // cbKhoNhap.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMKho + "", Utils.UserUtil.GetSessionUserId());
                cbKhoNhap.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMucKhoTheoBranchId(Utils.UserUtil.GetSessionBranchIdOfUserLogin(), Utils.UserUtil.GetSessionUserId());
                cbKhoNhap.DataBind();
                cbTaiSanNhapKho.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTaiSan + "", Utils.UserUtil.GetSessionUserId());
                cbTaiSanNhapKho.DataBind();
                //cbTrangThaiTaiSan.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTrangThaiTaiSan + "", Utils.UserUtil.GetSessionUserId());
                //cbTrangThaiTaiSan.DataBind();
                cbNhanVienPhieu.Value = Utils.UserUtil.GetSessionUserId();
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
        protected void gridQLNhapKho_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSource();
        }
        protected void gridQLNhapKho_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDataSource();
        }
        private void LoadDataSource()
        {
            Session["ssKey4Search"] = " ";
            Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
            this.gridQLNhapKho.DataBind();

            //  gridQLNhapKho.DataSource = DataProcess.ProcessNhapKho.getInstance().TraCuuNhapKho(txtKeyword.Text + "", Utils.UserUtil.GetSessionUserId() + "");
            //   gridQLNhapKho.DataBind();

        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
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
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLNhapKho).Name.ToUpper()))
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
                //DataTable dtbDetails = DataProcess.ProcessNhapKho.getInstance().TraCuuNhapKhoChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                //this.gridDetails.DataSource = dtbDetails;
                //gridDetails.DataBind();


                Session["ssIDNhapKho"] = idMaster;
                Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
                this.gridDetails.DataBind();


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

                this.gridLichSuKyDuyet.DataSource = DataProcess.ProcessNhapKho.getInstance().TraCuuLichSuKyDuyet(int.Parse(e.Parameters), Utils.UserUtil.GetSessionUserId());
                this.gridLichSuKyDuyet.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        protected void gridDanhSachTaiSan_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            SearchTaiSan4ThemNhapKho(int.Parse(e.Parameters));
        }
        protected void gridDanhSachTaiSan_PageIndexChanged(object sender, EventArgs e)
        {
            SearchTaiSan4ThemNhapKho(int.Parse(this.txtObjectId.Get("hidden_value") + ""));
        }

        private void SearchTaiSan4ThemNhapKho(int idMasterNhapKho)
        {
            try
            {
                //Session["ssIDNhapKho"] = idMasterNhapKho;
                //Session["ssUserLogin"] = Utils.UserUtil.GetSessionUserId();
                //this.gridDanhSachTaiSan.DataBind();

                // this.gridDanhSachTaiSan.DataSource = DataProcess.ProcessNhapKho.getInstance().LayDanhSachTaiSan4ThemNhapKho(idMasterNhapKho, Utils.UserUtil.GetSessionUserId()); ;
                DataTable dt = null;
                if (idMasterNhapKho > 0)
                {
                    //   dt = DataProcess.ProcessDanhMuc.getInstance().DMTaiSan4ComboFromCache(Utils.UserUtil.GetSessionUserId());
                    dt = DataProcess.ProcessNhapKho.getInstance().LayDanhSachTaiSan4ThemNhapKho(idMasterNhapKho, Utils.UserUtil.GetSessionUserId()); ;
                }
                Session["ssSearchTaiSan4ThemNhapKho"] = dt;
                this.gridDanhSachTaiSan.DataBind();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
        protected void gridDanhSachTaiSan_DataBinding(object sender, EventArgs e)
        {
            gridDanhSachTaiSan.DataSource = Session["ssSearchTaiSan4ThemNhapKho"];
        }


        public int GetBranchID()
        {
            return Utils.NumberUtil.ParseToInt(Session[Config.SysConfig.SESSION_BRANCHID]);
        }
        public string GetUserLogin()
        {
            return Utils.UserUtil.GetSessionUserId();
        }



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
                DataTable dtImported = ProcessNhapKho.getInstance().GetDMTaiSanImportNhapKho(Utils.UserUtil.GetSessionUserId());
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
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
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
                DataTable dtMaTaiSanHopLe = ProcessNhapKho.getInstance().DSMaTaiSanHopLe();
                dt.TableName = "QLNhapKhoCT_Import";
                DataTable dtSave = new DataTable("QLNhapKhoCT_Import");
                dtSave.Columns.Add("ID", typeof(int));
                dtSave.Columns.Add("MaTaiSan", typeof(string));
                dtSave.Columns.Add("MaTrangThaiTaiSan", typeof(string));
                dtSave.Columns.Add("SoLuong", typeof(int));
                dtSave.Columns.Add("DonGia", typeof(decimal));
                dtSave.Columns.Add("GhiChu", typeof(string));
                dtSave.Columns.Add("TrangThai", typeof(string));
                dtSave.Columns.Add("CreatedBy", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    DataRow[] maTSHopLe = dtMaTaiSanHopLe.Select("MaTaiSan='" + r["MaTaiSan"] + "'", "MaTaiSan");
                    if ((r["MaTrangThaiTaiSan"] + "").ToUpper() == Utils.StringUtil.eTrangThaiTaiSan.DangSuDung.ToString().ToUpper())
                    {
                        lbThongBaoImport.Text = "Trạng thái 'Đang sử dụng' không được phép nhập kho";
                        return;
                    }
                    else if (r["MaTrangThaiTaiSan"].ToString() != "MoiMua"
                      && r["MaTrangThaiTaiSan"].ToString() != "DaQuaSuDung"
                        && r["MaTrangThaiTaiSan"].ToString() != "KhongSuDung"
                         )
                    {
                        lbThongBaoImport.Text = "Mã Trạng thái không hợp lệ. các mã trạng thái cho phép là: MoiMua, DaQuaSuDung,  KhongSuDung";
                        return;
                    }
                    else if (maTSHopLe == null || maTSHopLe.Length == 0)
                    {
                        lbThongBaoImport.Text = "Mã tài sản " + r["MaTaiSan"] + " không hợp lệ";
                        return;
                    }
                    else
                    {
                        DataRow radd = dtSave.NewRow();
                        radd["ID"] = -1;
                        radd["MaTaiSan"] = r["MaTaiSan"];
                        radd["MaTrangThaiTaiSan"] = r["MaTrangThaiTaiSan"];
                        radd["SoLuong"] = r["SoLuong"];
                        radd["DonGia"] = r["DonGia"];
                        radd["GhiChu"] = r["GhiChu"];
                        radd["TrangThai"] = "NEW";
                        radd["CreatedBy"] = Utils.UserUtil.GetSessionUserId();
                        dtSave.Rows.Add(radd);
                    }
                }

                bool save = DatabaseManager.AutoSaveDataTable(DatabaseManager.CNN_STRING_HELPDESK, dtSave, "QLNhapKhoCT_Import");
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

            DataTable dtimport = ProcessNhapKho.getInstance().GetDMTaiSanImportNhapKho(Utils.UserUtil.GetSessionUserId());
            Session["ssLoadDataSourceImportTaiSanNhapKho"] = dtimport;
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

            gridImportTaiSan.DataSource = Session["ssLoadDataSourceImportTaiSanNhapKho"];

        }

        //protected void btnSaveTSImport_Click(object sender, EventArgs e)
        //{
        //    lbThongBaoImport.Text = "";
        //    DataTable dt = ProcessNhapKho.getInstance().GetDMTaiSanImportNhapKho(Utils.UserUtil.GetSessionUserId());
        //    if (dt != null)
        //    {
        //        ////-------------------------
        //        DataTable dtSave = new DataTable("QLNhapKhoCT_Import");
        //        dtSave.Columns.Add("ID", typeof(int));
        //        dtSave.Columns.Add("MaTaiSan", typeof(string));
        //        dtSave.Columns.Add("ChuoiQRCode", typeof(string));
        //        ////-------------------------             
        //        foreach (DataRow r in dt.Rows)
        //        {
        //            string chuoiQR = ProcessQR.MakeQRString(r["MaTaiSan"] + "", r["TenTaiSan"] + "", r["ThongTinKhac"] + "", Utils.NumberUtil.ParseToDouble(r["GiaTriTaiSan"]));
        //            DataRow radd = dtSave.NewRow();
        //            radd["ID"] = r["ID"];
        //            radd["MaTaiSan"] = r["MaTaiSan"];
        //            radd["ChuoiQRCode"] = chuoiQR;
        //            dtSave.Rows.Add(radd);

        //        }
        //        ProcessQR.getInstance().DeleteAllTaiSanImportQR(Utils.UserUtil.GetSessionUserId());
        //        bool save = DatabaseManager.AutoSaveDataTable(DatabaseManager.CNN_STRING_HELPDESK, dtSave, "Import_TaoQRCode");
        //        if (save)
        //        {
        //            lbThongBaoImport.Text = "Lưu tài sản import thành công";
        //            gridImportTaiSan.DataBind();
        //        }
        //        else
        //        {
        //            lbThongBaoImport.Text = "Lưu tài sản import thất bại";
        //        }

        //        // LoadDataSourceImportTaiSanNhapKho();

        //    }
        //    else
        //    {
        //        lbThongBaoImport.Text = "Không có dữ liệu để thực hiện";
        //    }
        //}

    }
}