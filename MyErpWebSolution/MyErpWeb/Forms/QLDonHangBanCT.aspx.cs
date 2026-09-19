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
using DevExpress.Web;

namespace WebRunDragon.Forms
{
    public partial class QLDonHangBanCT : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLDonHangBanCT));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Lưu lại giá trị user đang chọn TRƯỚC KHI DataBind() reset nó
                Session["cbKhachHangValue"] = cbKhachHang.Value;
            }

            // Luôn bind DataSource để UI hiển thị items (kể cả PostBack)
            cbKhachHang.DataSource = DataProcess.ProcessDanhMuc.getInstance().LoadDanhMuc4ComboFromCache(ProcessDanhMuc.eTenDanhMuc.dm_khach_hang + "", Utils.UserUtil.GetSessionUserId());
            cbKhachHang.DataBind();


            if (!IsPostBack)
            {
                if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                    || Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                    || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                    || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                {
                    BindControl();
                    // NOTE: Do not load invoice server-side here. Client-side JS will request header and details when opening edit.
                }
                else
                {
                    Response.Redirect(Config.SysConfig.URL_ERROR_FORBIDDEN);
                }
            }
        }

        protected void cbKhachHang_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            try
            {
                string param = (e.Parameter ?? "").ToString();
                var dtRaw = DataProcess.ProcessDanhMuc.getInstance()
                    .LoadDanhMuc4ComboNoCache(ProcessDanhMuc.eTenDanhMuc.dm_khach_hang + "", Utils.UserUtil.GetSessionUserId());

                if (string.IsNullOrEmpty(param))
                {
                    cbKhachHang.DataSource = dtRaw;
                    cbKhachHang.DataBindItems();
                    return;
                }

                string paramNorm = WebRunDragon.Utils.StringUtil.UnicodeKhongDau(param).ToLower();
                DataTable dtFiltered = dtRaw.Clone();

                foreach (DataRow r in dtRaw.Rows)
                {
                    string ten = r.Table.Columns.Contains("TenKhachHang") ? (r["TenKhachHang"] + "") : "";
                    string phone = r.Table.Columns.Contains("SoDienThoai") ? (r["SoDienThoai"] + "") : "";

                    string tenNorm = WebRunDragon.Utils.StringUtil.UnicodeKhongDau(ten).ToLower();

                    bool match = false;
                    if (!string.IsNullOrEmpty(tenNorm) && tenNorm.IndexOf(paramNorm) >= 0) match = true;
                    if (!match && !string.IsNullOrEmpty(phone) && phone.IndexOf(param) >= 0) match = true;

                    if (match) dtFiltered.ImportRow(r);
                }

                cbKhachHang.DataSource = dtFiltered;
                cbKhachHang.DataBindItems();
            }
            catch (System.Exception ex)
            {
                logger.Error("cbKhachHang_Callback error: " + ex.Message + "\n" + ex.StackTrace);
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
            // int idGiaiChay = Utils.NumberUtil.ParseToInt(cbKhachHang.Value + "");
            var dt = ProcessDanhMuc.getInstance().TraCuuDanhMucTestImg(Utils.UserUtil.GetSessionUserId() + "");
            Session["ssQlDonHangBanCTHangHoa"] = dt;
            gridHangHoa.DataBind();
        }
        protected void gridHangHoa_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            LoadDataSourceSum();
        }
        protected void gridHangHoa_PageIndexChanged(object sender, EventArgs e)
        {
            // Chỉ bind lại từ Session, không query DB lại
            gridHangHoa.DataBind();
        }
        protected void gridHangHoa_DataBinding(object sender, EventArgs e)
        {
            gridHangHoa.DataSource = Session["ssQlDonHangBanCTHangHoa"];

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
                //if (cmd == "REFRESH")
                //{
                //    DataTable dtRef = Session["ssInvoiceDetails"] as DataTable;
                //    ASPxGridViewRight.DataSource = dtRef;
                //    ASPxGridViewRight.DataBind();
                //    return;
                //}
                if (cmd == "LOAD")
                {
                    int idMaster = 0;
                    int.TryParse(parts.Length > 1 ? parts[1] : "0", out idMaster);
                    if (idMaster > 0)
                    {
                        DataTable dtDetail = ProcessDonHangBan.getInstance().TraCuuDonHangBanChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
                        Session["ssInvoiceDetails"] = dtDetail;
                        ASPxGridViewRight.DataSource = dtDetail;
                        ASPxGridViewRight.DataBind();
                    }
                    return;
                }
                //if (cmd == "ADD")
                //{
                //    string id = parts.Length > 1 ? parts[1] : "";
                //    string ma = parts.Length > 2 ? parts[2] : "";
                //    string ten = parts.Length > 3 ? parts[3] : "";
                //    string price = parts.Length > 4 ? parts[4] : "0";

                //    DataTable dt = Session["ssInvoiceDetails"] as DataTable;
                //    if (dt == null)
                //    {
                //        dt = new DataTable();
                //        dt.Columns.Add("id", typeof(int));
                //        dt.Columns.Add("id_hoa_don_ban", typeof(int));
                //        dt.Columns.Add("id_hang_hoa", typeof(string));
                //        dt.Columns.Add("ma_hang_hoa", typeof(string));
                //        dt.Columns.Add("ten_hang_hoa", typeof(string));
                //        dt.Columns.Add("so_luong", typeof(decimal));
                //        dt.Columns.Add("don_gia", typeof(decimal));
                //        dt.Columns.Add("thanh_tien", typeof(decimal));
                //        dt.Columns.Add("ghi_chu", typeof(string));
                //        dt.Columns.Add("createdby", typeof(string));
                //        dt.Columns.Add("createdtime", typeof(DateTime));
                //    }

                //    DataRow newRow = dt.NewRow();
                //    newRow["id"] = 0;
                //    newRow["id_hoa_don_ban"] = 0;
                //    newRow["id_hang_hoa"] = id;
                //    newRow["ma_hang_hoa"] = ma;
                //    newRow["ten_hang_hoa"] = ten;
                //    newRow["so_luong"] = 1;
                //    decimal dg = 0;
                //    decimal.TryParse(price + "", out dg);
                //    newRow["don_gia"] = dg;
                //    newRow["thanh_tien"] = dg * 1;
                //    newRow["ghi_chu"] = "";
                //    newRow["createdby"] = Utils.UserUtil.GetSessionUserId() + "";
                //    newRow["createdtime"] = DateTime.Now;

                //    dt.Rows.Add(newRow);

                //    Session["ssInvoiceDetails"] = dt;

                //    ASPxGridViewRight.DataSource = dt;
                //    ASPxGridViewRight.DataBind();
                //}
                //else if (cmd == "DEL")
                //{
                //    int idx = -1;
                //    int.TryParse(parts.Length > 1 ? parts[1] : "-1", out idx);
                //    DataTable dt = Session["ssInvoiceDetails"] as DataTable;
                //    if (dt != null && idx >= 0 && idx < dt.Rows.Count)
                //    {
                //        dt.Rows.RemoveAt(idx);
                //        Session["ssInvoiceDetails"] = dt;
                //        ASPxGridViewRight.DataSource = dt;
                //        ASPxGridViewRight.DataBind();
                //    }
                //}
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
                        try
                        {
                            // Update persisted detail in database when it has an id
                            int detailId = Utils.NumberUtil.ParseToInt(dt.Rows[idx]["id"] + "");
                            int idMaster = Utils.NumberUtil.ParseToInt(dt.Rows[idx]["id_hoa_don_ban"] + "");

                            decimal thanhTien = so * dg;

                            if (detailId > 0 && idMaster > 0)
                            {
                                // Persist change to DB
                                string sql = "UPDATE ql_hoa_don_ban_ct SET so_luong = @so_luong, don_gia = @don_gia, thanh_tien = @thanh_tien, updatedby = @modifiedby, updatedtime = GETDATE() WHERE id = @id";
                                SqlParameter[] pars = new SqlParameter[] {
                                    new SqlParameter("@so_luong", so),
                                    new SqlParameter("@don_gia", dg),
                                    new SqlParameter("@thanh_tien", thanhTien),
                                    new SqlParameter("@modifiedby", Utils.UserUtil.GetSessionUserId() + ""),
                                    new SqlParameter("@id", detailId)
                                };

                                SqlConnection cnnUpd = null;
                                SqlTransaction tranUpd = null;
                                try
                                {
                                    cnnUpd = DatabaseManager.OpenSqlConnection(DatabaseManager.CNN_STRING_HELPDESK);
                                    tranUpd = cnnUpd.BeginTransaction();
                                    DatabaseManager.SQLExecuteNonQuery(sql, CommandType.Text, ref tranUpd, pars);
                                    tranUpd.Commit();
                                }
                                catch (Exception ex)
                                {
                                    try { if (tranUpd != null) tranUpd.Rollback(); } catch { }
                                    logger.Error("Error updating detail in DB: " + ex.Message + "\n" + ex.StackTrace);
                                }
                                finally
                                {
                                    try { if (cnnUpd != null) DatabaseManager.CloseDbConnection(cnnUpd); } catch { }
                                }
                            }

                            // Update session copy and rebind so UI reflects changes immediately
                            dt.Rows[idx]["so_luong"] = so;
                            dt.Rows[idx]["don_gia"] = dg;
                            dt.Rows[idx]["thanh_tien"] = thanhTien;
                            Session["ssInvoiceDetails"] = dt;
                            ASPxGridViewRight.DataSource = dt;
                            ASPxGridViewRight.DataBind();
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Error processing UPD callback: " + ex.Message + "\n" + ex.StackTrace);
                        }
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
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueCreate:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueEdit:
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueDelete:
                    if (Utils.ActionUtil.CanDelete(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueApproval:
                    if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
                case SysConfig.ValueSave:
                    if (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper())
                        || Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLDonHangBan).Name.ToUpper()))
                        ret = SysConfig.displayButton;
                    else
                        ret = SysConfig.noDisplayButton;
                    break;
            }
            return ret;
        }

        protected void btnSaveTemp_Click(object sender, EventArgs e)
        {
            SaveInvoice(false, "NEW");
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            SaveInvoice(true, "SENDAPPROVAL");
        }

        private void SaveInvoice(bool isPay, string trangThai)
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

                string insertHeader = "INSERT INTO ql_hoa_don_ban (ngay_ban, id_khach_hang, ghi_chu,trang_thai, isdeleted, createdby, createdtime) OUTPUT INSERTED.id VALUES (@ngay_ban, @id_khach_hang, @ghi_chu,@trang_thai, 0, @createdby, GETDATE())";
                SqlParameter[] parasHeader = new SqlParameter[] {
                    new SqlParameter("@ngay_ban", ngayBan),
                    new SqlParameter("@id_khach_hang", idKhachHang),
                    new SqlParameter("@ghi_chu", ghiChu),
                    new SqlParameter("@trang_thai", trangThai),
                    new SqlParameter("@createdby", user)
                };

                object objId = DatabaseManager.SQLExecScalar(insertHeader, CommandType.Text, ref tran, parasHeader);
                int idHoaDon = Utils.NumberUtil.ParseToInt(objId + "");

                // iterate details from session-stored DataTable to avoid relying on grid field names
                DataTable dt = Session["ssInvoiceDetails"] as DataTable;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int idHangHoa = Utils.NumberUtil.ParseToInt(dr["id_hang_hoa"] + "");
                        decimal soLuong = Utils.NumberUtil.ParseToDecimal(dr["so_luong"]);
                        decimal donGia = Utils.NumberUtil.ParseToDecimal(dr["don_gia"]);
                        decimal thanhTien = soLuong * donGia;

                        string insertDetail = "INSERT INTO ql_hoa_don_ban_ct (id_hoa_don_ban, id_hang_hoa, so_luong, don_gia, thanh_tien, ghi_chu, isdeleted, createdby, createdtime) VALUES (@id_hoa_don_ban, @id_hang_hoa, @so_luong, @don_gia, @thanh_tien, @ghi_chu, 0, @createdby, GETDATE())";
                        SqlParameter[] parasDet = new SqlParameter[] {
                            new SqlParameter("@id_hoa_don_ban", idHoaDon),
                            new SqlParameter("@id_hang_hoa", idHangHoa),
                            new SqlParameter("@so_luong", soLuong),
                            new SqlParameter("@don_gia", donGia),
                            new SqlParameter("@thanh_tien", thanhTien),
                            new SqlParameter("@ghi_chu", dr["ghi_chu"] + ""),
                            new SqlParameter("@createdby", user)
                        };

                        DatabaseManager.SQLExecuteNonQuery(insertDetail, CommandType.Text, ref tran, parasDet);
                    }
                }

                tran.Commit();
                // feedback
                memoNotes.Text = "";
                // clear session details and reload UI
                // Session["ssInvoiceDetails"] = null;
                ASPxGridViewRight.DataSource = null;
                ASPxGridViewRight.DataBind();
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


    }
}
