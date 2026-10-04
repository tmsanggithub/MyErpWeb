using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebRunDragon.Forms
{
    public partial class QLChuyenKhoDuyet : System.Web.UI.Page
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLChuyenKho));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // clear text==================================================

                lbChiNhanh.Text = "";
                lbDonViGiao.Text = "";

                hddCanDuyetBanGiao["hidden_value"] = 0;
                hddCanDuyetNhan["hidden_value"] = 0;
                hddCanNhapKhoTSTraLai["hidden_value"] = 0;

                btnDuyet.Enabled = false;
                btnKhongDuyet.Enabled = false;
                btnDuyetNhanTaiSan.Enabled = false;
                btnKhongDuyetNhanTaiSan.Enabled = false;
                btnNhapKhoTSTraLai.Enabled = false;
                //=============================================================

                // lần đầu gọi trang thì lấy Id của issue cần gọi
                int issueId = 0;
                Int32.TryParse(Request.Params["id"], out issueId);
                hddId["hidden_value"] = issueId;
                if (issueId > 0)
                {
                    if (Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                        || Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                        || Utils.ActionUtil.CanSpecialAction(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                        )
                    {
                        // nếu có quyền thì                     

                        DataSet dtsIssue = DataProcess.ProcessChuyenKho.getInstance().TraCuuThongTinCanDuyet(issueId, Utils.UserUtil.GetSessionUserId());

                        if (dtsIssue != null && dtsIssue.Tables[0].Rows.Count > 0)
                        {
                            // lấy được thông tin chi tiết 1 issue thì:
                            // 1.thông tin issue  

                            lbChiNhanh.Text = dtsIssue.Tables[0].Rows[0]["TenChiNhanh"] + "";
                            lbDonViNhan.Text = dtsIssue.Tables[0].Rows[0]["DonViNhan"] + "";
                            lbDonViGiao.Text = dtsIssue.Tables[0].Rows[0]["DonViChuyen"] + "";
                            lbGhiChu.Text = dtsIssue.Tables[0].Rows[0]["GhiChu"] + "";
                            lbNgayGiaoDich.Text = dtsIssue.Tables[0].Rows[0]["NgayChuyen"] + "";
                            lbNguoiGiao.Text = dtsIssue.Tables[0].Rows[0]["NguoiChuyen"] + "";
                            lbNguoiNhan.Text = dtsIssue.Tables[0].Rows[0]["NguoiNhan"] + "";
                            lbSoPhieuBanGiao.Text = dtsIssue.Tables[0].Rows[0]["SoPhieu"] + "";
                            lbTruongDonViGiao.Text= dtsIssue.Tables[0].Rows[0]["TruongDonViChuyen"] + "";
                            lbTruongDonViNhan.Text= dtsIssue.Tables[0].Rows[0]["TruongDonViNhan"] + "";


                            // 2.thông các lưới
                            //this.gridAttachments.DataSource = dtsIssue.Tables[1];
                            DataTable dtbDetails = DataProcess.ProcessChuyenKho.getInstance().TraCuuChuyenKhoChiTiet(issueId, Utils.UserUtil.GetSessionUserId());
                            this.gridDetails.DataSource = dtbDetails;
                            gridDetails.DataBind();

                            //this.gridTasks.DataSource = dtsIssue.Tables[2];
                            this.gridLichSuKyDuyet.DataBind();


                            // Xử lý các nút chức năng  Ẩn hiện ở client, ko xử lý ở server
                            if (Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                                || Utils.ActionUtil.CanSpecialAction(Utils.UserUtil.GetSessionUserId(), typeof(Forms.QLChuyenKho).Name.ToUpper())
                                )
                            {
                                hddCanDuyetBanGiao["hidden_value"] = (int)dtsIssue.Tables[0].Rows[0]["CanDuyetChuyenKho"];
                                hddCanDuyetNhan["hidden_value"] = (int)dtsIssue.Tables[0].Rows[0]["CanDuyetNhan"];
                                hddCanNhapKhoTSTraLai["hidden_value"] = (int)dtsIssue.Tables[0].Rows[0]["CanNhapKhoTSTraLai"];

                                btnDuyet.Enabled = true;
                                btnKhongDuyet.Enabled = true;
                                btnDuyetNhanTaiSan.Enabled = true;
                                btnKhongDuyetNhanTaiSan.Enabled = true;
                                btnNhapKhoTSTraLai.Enabled = true;
                            }
                        }


                    }
                }
                else
                {
                    // chuyển đến trang thông báo ko có quyền truy cập
                    //Server.TransferRequest("IssuePermissionDenied.htm");
                }

            }
            else
            {
                // chuyển đến trang thông báo không có issue này
                //Server.TransferRequest("IssueNotFound.htm");
            }
        }

        //protected void gridDetails_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    SearchDetails(int.Parse(e.Parameters));
        //}
        //protected void gridDetails_PageIndexChanged(object sender, EventArgs e)
        //{
        //    SearchDetails(int.Parse(this.hddId.Get("hidden_value") + ""));
        //}
       
        //private void SearchDetails(int idMaster)
        //{
        //    try
        //    {
        //        DataTable dtbDetails = DataProcess.ProcessChuyenKho.getInstance().TraCuuChuyenKhoChiTiet(idMaster, Utils.UserUtil.GetSessionUserId());
        //        this.gridDetails.DataSource = dtbDetails;
        //        gridDetails.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message, ex);
        //    }
        //}
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
    }
}