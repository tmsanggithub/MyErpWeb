using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class InBaoCao : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(InBaoCao));
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{

            string mode = Request.Params["Mode"];
            if (mode == "InThanhToan")
            {
                int id = Utils.NumberUtil.ParseToInt(Request.Params["IDQLThanhToan"]);
                CreateReportPhieuThanhToan(id);
            }
            else if (mode == "InTamUng")
            {
                int id = Utils.NumberUtil.ParseToInt(Request.Params["IDQLTamUng"]);
                CreateReportPhieuTamUng(id);
            }
           

            //if (Session["CreateReport"] != null)
            //{
            //    CreateReport();
            //}
        }
        private void CreateReportPhieuThanhToan(int idThanhToan)
        {
            try
            {
                /*
                DataTable dtSource = DataProcess.ProcessThanhToan.getInstance().InPhieuThanhToan(idThanhToan, Utils.UserUtil.GetSessionUserId());
                string MaChiNhanh = (dtSource != null && dtSource.Rows.Count > 0) ? dtSource.Rows[0]["MaChiNhanh"] + "" : "";
                if (MaChiNhanh == "HS")
                {
                    Reports.rptThanhToan rpt = new Reports.rptThanhToan();
                    rpt.DataSource = dtSource;
                    rpt.lbGiaTriThanhToanBangChu.Text = Utils.NumberUtil.Read(dtSource.Rows[0]["GiaTriThanhToan"] + "");
                    if (Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TongThanhToan"] + "") >= 0)
                        rpt.lbTongSoTienPhaiThanhToan.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TongThanhToan"] + "").ToString("#,###") + " vnđ.";
                    if (Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TienDaUngTruoc"] + "") >= 0)
                        rpt.lbDaUngTruoc.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TienDaUngTruoc"] + "").ToString("#,###") + " vnđ.";
                    if (Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["GiaTriThanhToan"] + "") >= 0)
                        rpt.lbTienDeNghiThanhToan.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["GiaTriThanhToan"] + "").ToString("#,###") + " vnđ.";
                    rpt.lbNguoiTao.Text = dtSource.Rows[0]["NguoiTao"] + "" + "";
                    rpt.lbTruongDonVi.Text = dtSource.Rows[0]["NguoiDuyet"] + "" + "";
                    rpt.lbNgayDeNghi.Text = Utils.NumberUtil.ParseToDate(dtSource.Rows[0]["NgayDeNghi"]).ToString("dd/MM/yyyy");

                    // thong tin cho hoi so
                    rpt.lbDonViDeNghi.Text = string.Format(rpt.lbDonViDeNghi.Text, dtSource.Rows[0]["PhongBanDeNghi"] + ""); //string.Format(rpt.lbDonViDeNghi.Text, "Phòng Hành chính quản trị");
                                                                                                                             //   rpt.lbTruongPhongHCQT.Text =  "Trưởng phòng HCQT";
                                                                                                                             //  rpt.lbDuyetDeNghi.Text = "Giám đốc điều hành Khối hỗ trợ";
                    rpt.lbFixGiamDocHoiSo.Text = dtSource.Rows[0]["GDDuyetThanhToan"] + "";//"NGUYỄN NGỌC VÂN PHƯƠNG";
                    //------------------------------------------                                                                  

                    ASPxDocumentViewer1.Report = rpt;
                    ASPxDocumentViewer1.Report.Margins.Top = 500;
                    ASPxDocumentViewer1.Report.Margins.Bottom = 800;
                }
                else
                {
                    Reports.rptThanhToanCN rpt = new Reports.rptThanhToanCN();
                    rpt.DataSource = dtSource;
                    rpt.lbGiaTriThanhToanBangChu.Text = Utils.NumberUtil.Read(dtSource.Rows[0]["GiaTriThanhToan"] + "");
                    if (Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TongThanhToan"] + "") >= 0)
                        rpt.lbTongSoTienPhaiThanhToan.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TongThanhToan"] + "").ToString("#,###") + " vnđ.";
                    if (Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TienDaUngTruoc"] + "") >= 0)
                        rpt.lbDaUngTruoc.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["TienDaUngTruoc"] + "").ToString("#,###") + " vnđ.";
                    if (Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["GiaTriThanhToan"] + "") >= 0)
                        rpt.lbTienDeNghiThanhToan.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Rows[0]["GiaTriThanhToan"] + "").ToString("#,###") + " vnđ.";
                    rpt.lbNguoiTao.Text = dtSource.Rows[0]["NguoiTao"] + "" + "";
                    rpt.lbTruongDonVi.Text = dtSource.Rows[0]["NguoiDuyet"] + "" + "";
                    rpt.lbNgayDeNghi.Text = Utils.NumberUtil.ParseToDate(dtSource.Rows[0]["NgayDeNghi"]).ToString("dd/MM/yyyy");

                    // thong tin cho hoi so
                    rpt.lbDonViDeNghi.Text = string.Format(rpt.lbDonViDeNghi.Text, dtSource.Rows[0]["PhongBanDeNghi"] + ""); // string.Format(rpt.lbDonViDeNghi.Text, "Phòng Hành chính quản trị");                                                                            

                    ASPxDocumentViewer1.Report = rpt;
                    ASPxDocumentViewer1.Report.Margins.Top = 500;
                    ASPxDocumentViewer1.Report.Margins.Bottom = 800;

                }
              */
            }
            catch (Exception er)
            {
                logger.Info("print. Message Eror=" + er.Message + "");
            }
        }
        private void CreateReportPhieuTamUng(int idTamUng)
        {
            try
            {
              
            }
            catch (Exception er)
            {
                logger.Info("print. Message Eror=" + er.Message + "");
            }
        }
      
        //private void CreateReportThanhLyTaiSan(int idKiemKeKhoNV)
        //{
        //    try
        //    {
        //        Reports.rptThanhLy rpt = new Reports.rptThanhLy();
        //        DataTable dtSource = DataProcess.ProcessThanhLy.getInstance().InThanhLyTaiSan(idKiemKeKhoNV, Utils.UserUtil.GetSessionUserId());

        //        dtSource.TableName = "dtRpt";
        //        rpt.DataSource = dtSource;
        //        rpt.DataMember = "dtRpt";

        //        if (dtSource != null && dtSource.Rows.Count > 0)
        //        {
        //            rpt.lbTongTienThuHoi.Text = Utils.NumberUtil.ParseToDecimal(dtSource.Compute("Sum(GiaTriThuHoi)", string.Empty)).ToString("N0");
        //        }
        //        ASPxDocumentViewer1.Report = rpt;
        //        ASPxDocumentViewer1.Report.Margins.Top = 500;
        //        ASPxDocumentViewer1.Report.Margins.Bottom = 800;
        //    }
        //    catch (Exception er)
        //    {
        //        logger.Info("print. Message Eror=" + er.Message + "");
        //    }
        //}
    }
}