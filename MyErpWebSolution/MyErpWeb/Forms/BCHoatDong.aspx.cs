using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using WebRunDragon.Config;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Forms
{
    public partial class BCHoatDong : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(BCHoatDong));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Lưu lại giá trị user đang chọn TRƯỚC KHI DataBind() reset nó
                Session["cbKhachHangValue"] = cbKhachHang.Value;
            }



            // Luôn bind DataSource để UI hiển thị items (kể cả PostBack)
            cbKhachHang.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.ql_race + "", Utils.UserUtil.GetSessionUserId());
            cbKhachHang.DataBind();

            if (IsPostBack && Session["cbKhachHangValue"] != null)
            {
                // Khôi phục lại giá trị đã chọn sau khi DataBind()
                cbKhachHang.Value = Session["cbKhachHangValue"];
            }

            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                {
                    BindControl();
                    // Nếu có id truyền vào (load theo id) thì load header + chi tiết
                    int idLoad = Utils.NumberUtil.ParseToInt(Request.QueryString["id"] + "");
                    if (idLoad > 0)
                    {
                        // defer LoadInvoice call until after Page_Load completes (call here)
                        LoadInvoice(idLoad);
                    }
                }
                else
                {
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                }
            }
        }

        private void BindControl()
        {
            try
            {
                //  deNgayDieuChinhTu.Date = DateTime.Now.AddDays(-30);
                //  deNgayDieuChinhDen.Date = DateTime.Now;

                //InitData();
                LoadDataSourceSum();

            }
            catch (Exception ex)
            {
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
        }
        protected void btnSearchSum_Click(object sender, EventArgs e)
        {
            LoadDataSourceSum();
        }
        private void LoadDataSourceSum()
        {
            int idGiaiChay = Utils.NumberUtil.ParseToInt(cbKhachHang.Value + "");
            var dt = ProcessDanhMuc.getInstance().TraCuuDanhMucTestImg(Utils.UserUtil.GetSessionUserId() + "");
            Session["ssBCHoatDong"] = dt;
            gridHoatDong.DataBind();
        }
        protected void gridHoatDong_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceSum();
        }
        protected void gridHoatDong_PageIndexChanged(object sender, EventArgs e)
        {
            // Chỉ bind lại từ Session, không query DB lại
            gridHoatDong.DataBind();
        }
        protected void gridHoatDong_DataBinding(object sender, EventArgs e)
        {
            gridHoatDong.DataSource = Session["ssBCHoatDong"];
            // Session["ssDMTaiSan"] = null;
        }

        protected void ASPxGridViewRight_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                string param = e.Parameters + "";
                if (string.IsNullOrEmpty(param)) return;
                string[] parts = param.Split(new char[] { '|' }, 5);
                if (parts.Length == 0) return;

                string cmd = parts[0];
                if (cmd == "ADD")
                {
                    string id = parts.Length > 1 ? parts[1] : "";
                    string ma = parts.Length > 2 ? parts[2] : "";
                    string ten = parts.Length > 3 ? parts[3] : "";
                    string price = parts.Length > 4 ? parts[4] : "0";

                    DataTable dt = Session["ssInvoiceDetails"] as DataTable;
                    if (dt == null)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("id", typeof(int));
                        dt.Columns.Add("id_hoa_don_ban", typeof(int));
                        dt.Columns.Add("id_hang_hoa", typeof(string));
                        dt.Columns.Add("ma_hang_hoa", typeof(string));
                        dt.Columns.Add("ten_hang_hoa", typeof(string));
                        dt.Columns.Add("so_luong", typeof(decimal));
                        dt.Columns.Add("don_gia", typeof(decimal));
                        dt.Columns.Add("thanh_tien", typeof(decimal));
                        dt.Columns.Add("ghi_chu", typeof(string));
                        dt.Columns.Add("createdby", typeof(string));
                        dt.Columns.Add("createdtime", typeof(DateTime));
                    }

                    DataRow newRow = dt.NewRow();
                    newRow["id"] = 0;
                    newRow["id_hoa_don_ban"] = 0;
                    newRow["id_hang_hoa"] = id;
                    newRow["ma_hang_hoa"] = ma;
                    newRow["ten_hang_hoa"] = ten;
                    newRow["so_luong"] = 1;
                    decimal dg = 0;
                    decimal.TryParse(price + "", out dg);
                    newRow["don_gia"] = dg;
                    newRow["thanh_tien"] = dg * 1;
                    newRow["ghi_chu"] = "";
                    newRow["createdby"] = Utils.UserUtil.GetSessionUserId() + "";
                    newRow["createdtime"] = DateTime.Now;

                    dt.Rows.Add(newRow);

                    Session["ssInvoiceDetails"] = dt;

                    ASPxGridViewRight.DataSource = dt;
                    ASPxGridViewRight.DataBind();
                }
                else if (cmd == "DEL")
                {
                    int idx = -1;
                    int.TryParse(parts.Length > 1 ? parts[1] : "-1", out idx);
                    DataTable dt = Session["ssInvoiceDetails"] as DataTable;
                    if (dt != null && idx >= 0 && idx < dt.Rows.Count)
                    {
                        dt.Rows.RemoveAt(idx);
                        Session["ssInvoiceDetails"] = dt;
                        ASPxGridViewRight.DataSource = dt;
                        ASPxGridViewRight.DataBind();
                    }
                }
                else if (cmd == "UPD")
                {
                    int idx = -1;
                    int.TryParse(parts.Length > 1 ? parts[1] : "-1", out idx);
                    decimal so = 0;
                    decimal dg = 0;
                    decimal.TryParse(parts.Length > 2 ? parts[2] : "0", out so);
                    decimal.TryParse(parts.Length > 3 ? parts[3] : "0", out dg);
                    DataTable dt = Session["ssInvoiceDetails"] as DataTable;
                    if (dt != null && idx >= 0 && idx < dt.Rows.Count)
                    {
                        dt.Rows[idx]["so_luong"] = so;
                        dt.Rows[idx]["don_gia"] = dg;
                        dt.Rows[idx]["thanh_tien"] = so * dg;
                        Session["ssInvoiceDetails"] = dt;
                        ASPxGridViewRight.DataSource = dt;
                        ASPxGridViewRight.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("ASPxGridViewRight_CustomCallback error: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        protected void ASPxGridViewRight_PageIndexChanged(object sender, EventArgs e)
        {
            // Bind from session so paging works without re-querying DB
            ASPxGridViewRight.DataSource = Session["ssInvoiceDetails"];
            ASPxGridViewRight.DataBind();
        }

        protected void ASPxGridViewRight_DataBinding(object sender, EventArgs e)
        {
            ASPxGridViewRight.DataSource = Session["ssInvoiceDetails"];
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            LoadDataSourceSum();
            //gridThanhToan.Columns["Xem"].Visible = false;

            gridExporter.DataBind();
            gridExporter.WriteXlsxToResponse();
        }
        public string GetRight(object Xem0Them1Sua2Xoa3Duyet5)
        {
            string ret = "";

            switch (Xem0Them1Sua2Xoa3Duyet5 + "")
            {
                case Config.SysConfig.ValueRead:
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.BCHoatDong).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }

        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            SaveInvoice(false);
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            SaveInvoice(true);
        }

        private void SaveInvoice(bool isPay)
        {
            // Collect header data
            DateTime ngayBan = DateTime.Now;
            int idKhachHang = Utils.NumberUtil.ParseToInt(cbKhachHang.Value + "");
            string ghiChu = memoNotes.Text + "";
            string user = Utils.UserUtil.GetSessionUserId() + "";

            SqlConnection cnn = null;
            SqlTransaction tran = null;
            try
            {
                cnn = DatabaseManager.OpenSqlConnection(DatabaseManager.CNN_STRING_HELPDESK);
                tran = cnn.BeginTransaction();

                string insertHeader = "INSERT INTO ql_hoa_don_ban (ngay_ban, id_khach_hang, ghi_chu, isdeleted, createdby, createdtime) OUTPUT INSERTED.id VALUES (@ngay_ban, @id_khachhang, @ghi_chu, 0, @createdby, GETDATE())";
                SqlParameter[] parasHeader = new SqlParameter[] {
                    new SqlParameter("@ngay_ban", ngayBan),
                    new SqlParameter("@id_khach_hang", idKhachHang),
                    new SqlParameter("@ghi_chu", ghiChu),
                    new SqlParameter("@createdby", user)
                };

                object objId = DatabaseManager.SQLExecScalar(insertHeader, CommandType.Text, ref tran, parasHeader);
                int idHoaDon = Utils.NumberUtil.ParseToInt(objId + "");

                // iterate detail grid rows and insert details
                int rowCount = ASPxGridViewRight.VisibleRowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    object idHang = ASPxGridViewRight.GetRowValues(i, "id_strava");
                    object soLuongObj = ASPxGridViewRight.GetRowValues(i, "quantity");
                    object donGiaObj = ASPxGridViewRight.GetRowValues(i, "price");

                    int idHangHoa = Utils.NumberUtil.ParseToInt(idHang + "");
                    decimal soLuong = Utils.NumberUtil.ParseToDecimal(soLuongObj);
                    decimal donGia = Utils.NumberUtil.ParseToDecimal(donGiaObj);
                    decimal thanhTien = soLuong * donGia;

                    string insertDetail = "INSERT INTO ql_hoa_don_ban_ct (id_hoadonban, id_hanghoa, so_luong, don_gia, thanh_tien, ghi_chu, isdeleted, createdby, createdtime) VALUES (@id_hoadonban, @id_hanghoa, @so_luong, @don_gia, @thanh_tien, @ghi_chu, 0, @createdby, GETDATE())";
                    SqlParameter[] parasDet = new SqlParameter[] {
                        new SqlParameter("@id_hoa_don_ban", idHoaDon),
                        new SqlParameter("@id_hanghoa", idHangHoa),
                        new SqlParameter("@so_luong", soLuong),
                        new SqlParameter("@don_gia", donGia),
                        new SqlParameter("@thanh_tien", thanhTien),
                        new SqlParameter("@ghi_chu", ""),
                        new SqlParameter("@createdby", user)
                    };

                    DatabaseManager.SQLExecuteNonQuery(insertDetail, CommandType.Text, ref tran, parasDet);
                }

                tran.Commit();
                // feedback
                memoNotes.Text = "";
                // reload data if needed
                LoadDataSourceSum();
            }
            catch (Exception ex)
            {
                if (tran != null) tran.Rollback();
                logger.Error("SaveInvoice exception: " + ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                if (cnn != null) DatabaseManager.CloseDbConnection(cnn);
            }
        }

        private void LoadInvoice(int id)
        {
            try
            {
                var param = new Dictionary<string, object>() { { "@id", id } };
                string sqlHeader = "SELECT id, ngay_ban, id_khach_hang, ghi_chu, isdeleted, createdby, createdtime FROM ql_hoa_don_ban WHERE id = @id";
                DataTable dtHeader = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sqlHeader, CommandType.Text, param);
                if (dtHeader != null && dtHeader.Rows.Count > 0)
                {
                    DataRow row = dtHeader.Rows[0];
                    // set header fields
                    try
                    {
                        cbKhachHang.Value = row["id_khach_hang"];
                    }
                    catch
                    {
                    }
                    try
                    {
                        memoNotes.Text = row["ghi_chu"] + "";
                    }
                    catch
                    {
                    }
                    try
                    {
                        txtObjectId.Set("hidden_value", row["id"] + "");
                    }
                    catch
                    {
                    }
                }

                string sqlDetail = @"SELECT ct.id
                                    ,ct.id_hoa_don_ban
                                    ,ct.id_hang_hoa
                                    ,ct.so_luong
                                    ,ct.don_gia
                                    ,ct.thanh_tien
                                    ,ct.ghi_chu
	                                ,hh.ma_hang_hoa, hh.ten_hang_hoa
                                FROM [dbo].[ql_hoa_don_ban_ct] ct
                                left join [dm_hang_hoa] hh on hh.id=ct.id_hang_hoa
                                where id_hoa_don_ban = @id";
                DataTable dtDetail = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, sqlDetail, CommandType.Text, param);
                if (dtDetail != null)
                {
                    // Ensure column names match grid expectations (add aliases if missing)
                    // Grid expects fields like id, id_hoa_don_ban, id_hang_hoa, so_luong, don_gia, thanh_tien
                    ASPxGridViewRight.DataSource = dtDetail;
                    ASPxGridViewRight.DataBind();
                    Session["ssInvoiceDetails"] = dtDetail;
                }
            }
            catch (Exception ex)
            {
                logger.Error("LoadInvoice exception: " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
