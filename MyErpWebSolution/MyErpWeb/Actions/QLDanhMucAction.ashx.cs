using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using WebRunDragon.DataProcess;
using WebRunDragon.Forms;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for QLDanhMucAction
    /// </summary>
    public class QLDanhMucAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLDanhMucAction));
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            bool success = false;
            int retCode = -1;
            string message = "";
            int id = 0;
            JObject retObject = new JObject();
            try
            {
                string sessionUserId = Utils.UserUtil.GetSessionUserId();
                id = Utils.NumberUtil.ParseToInt(context.Request.Form["id"]);
                string mode = context.Request.Form["mode"].Trim().ToUpper();
                string tableName = context.Request.Form["tableName"].Trim().ToUpper();

                if (mode == "EDIT")
                {
                    string subMode = context.Request.Form["subMode"].Trim().ToUpper();

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), tableName.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), tableName.ToUpper())
                        || subMode == "ApprovalOnly".ToUpper() && Utils.ActionUtil.CanApprove(Utils.UserUtil.GetSessionUserId(), tableName.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessDanhMuc.getInstance().GetDanhMucByID(id, tableName);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "Bạn không có quyền thực hiện chức năng này";
                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, tableName.ToUpper()))
                    {
                        if (tableName == "DMTaiSan".ToUpper())
                        {
                            success = DeleteTaiSan(id, sessionUserId, out message);

                        }
                        else
                        {
                            if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DmHoatDong.ToString().ToUpper())
                            {
                                tableName = "QL_ACTIVITIES";
                                var entity = DataProcess.ProcessDanhMuc.getInstance().GetDanhMucByID(id, tableName);
                                if (entity == null)
                                {
                                    success = false;
                                    message = "Không tìm thấy hoạt động cần xóa";
                                }
                                else if ((entity["created_by"] + "").ToLower() != "admin_import")
                                {
                                    success = false;
                                    message = "Chỉ cho phép xóa hoạt động có created_by = admin_import";
                                }
                                else
                                {
                                    success = DataProcess.ProcessDanhMuc.getInstance().DeleteDanhMucByIDCheckConstrainKey(id, tableName, sessionUserId, out message);
                                }
                            }
                            else
                            {
                                success = DataProcess.ProcessDanhMuc.getInstance().DeleteDanhMucByIDCheckConstrainKey(id, tableName, sessionUserId, out message);
                            }
                        }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), tableName.ToUpper()))// ||
                                                                                                          //  (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), tableName.ToUpper()) && id == 0)
                        )
                    {
                        int idInsertNew = 0;
                        if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMDonViTinh.ToString().ToUpper() + "")
                        {
                            success = AddOrUdateDanhMucDonViTinh(id, context.Request.Form["MaDonViTinh"], context.Request.Form["TenDonViTinh"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.dm_run_group.ToString().ToUpper() + "")
                        {
                            success = AddOrUdateDanhMucNhomChay(id, context.Request.Form["MaNhomChay"], context.Request.Form["TenNhomChay"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);
                            if (success)
                            {
                                BusinessMemCache cache = new BusinessMemCache();
                                var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_DMKho_";
                                cache.RemoveMyCachedItem(keyCache);
                            }

                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.app_user_registed.ToString().ToUpper() + "")
                        {
                            success = UdateDanhMucVanDongVien(id, context.Request.Form["MaVanDongVien"], context.Request.Form["TenHienThi"], context.Request.Form["TenDayDu"], context.Request.Form["GioiTinh"], context.Request.Form["UserType"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);

                        }

                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMLoaiBaoHiem.ToString().ToUpper() + "")
                        {
                            success = AddOrUdateDanhMucLoaiBaoHiem(id, context.Request.Form["MaLoaiBaoHiem"], context.Request.Form["TenLoaiBaoHiem"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMNhaCungCapNhom.ToString().ToUpper() + "")
                        {
                            success = AddOrUdateDanhMucNhomNhaCungCap(id, context.Request.Form["MaNhomNhaCungCap"], context.Request.Form["TenNhomNhaCungCap"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMSanPham.ToString().ToUpper() + "")
                        {
                            string loaiSP = context.Request.Form["LoaiSanPham"];
                            int idNhomSP = Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhomSanPham"]);

                            success = AddOrUdateDanhMucSanPham(id, loaiSP, idNhomSP, context.Request.Form["MaSanPham"], context.Request.Form["TenSanPham"],
                              Utils.NumberUtil.ParseToDecimal(context.Request.Form["DonGia"]), Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViTinh"]), context.Request.Form["GhiChu"], context.Request.Form["GhiChuHighLight"], sessionUserId, out message, out idInsertNew);
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.ql_activities.ToString().ToUpper() + "")
                        {
                            int hopLe = Utils.NumberUtil.ParseToBool(context.Request.Form["HopLe"]) ? 1 : 0;
                            decimal quangDuonhHopLe = Utils.NumberUtil.ParseToDecimal(context.Request.Form["QuangDuongHopLe"]);
                            string ghiChuThayDoi = context.Request.Form["GhiChuThayDoi"] + "";
                            if (id > 0)
                            {
                                success = UpdateHoatDong(id, hopLe, quangDuonhHopLe, ghiChuThayDoi, sessionUserId, out message, out idInsertNew);
                            }
                            else
                            {
                                int idRace = Utils.NumberUtil.ParseToInt(context.Request.Form["IDRace"]);
                                string idStrava = context.Request.Form["IDStrava"] + "";
                                int idRunner = Utils.NumberUtil.ParseToInt(context.Request.Form["IDRunner"]);
                                DateTime ngayHoatDong = Utils.NumberUtil.ParseToDate(context.Request.Form["NgayHoatDong"]);
                                decimal tongQuangDuong = Utils.NumberUtil.ParseToDecimal(context.Request.Form["TongQuangDuong"]);
                                string idActivities = context.Request.Form["IDActivities"] + "";

                                success = AddHoatDongThuCong(idRace, idStrava, idRunner, ngayHoatDong, tongQuangDuong, quangDuonhHopLe, idActivities, hopLe, out message, out idInsertNew);
                            }
                        }
                        else if (tableName == "DMCONFIG")
                        {
                            success = UdateDanhMucConfig(id, context.Request.Form["MaConfig"], context.Request.Form["GiaTriConfig"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);
                        }
                        // truong hop them moi thi tra ve lại ID mới thêm vào database
                        if (id <= 0) id = idInsertNew;
                        if (success)
                        {
                            DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
                            // context.Session.Remove("ssDMTaiSan");
                        }
                    }
                    else
                        message = "Bạn không có quyền thêm hoặc chỉnh sửa chức năng này";

                }

                else
                    message = "Invalid mode !";

            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error("ProcessRequest Error");
                //     logger.Error("Exception message: " + context.Request.Form["MaNhaCungCap"] + "";// ex.Message + ".Stack trace: context.Request.Form["MaNhaCungCap"] + "";// ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                retObject["success"] = success;
                retObject["retCode"] = retCode;
                retObject["message"] = message;
                retObject["id"] = id;
            }
            context.Response.Write(retObject.ToString());
        }
        public bool AddOrUdateDanhMucDonViTinh(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã đơn vị tính";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên đơn vị tính";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucDonViTinh(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }

        public bool AddHoatDongThuCong(int idRace, string idStrava, int idRunner, DateTime ngayHoatDong, decimal tongQuangDuong, decimal quangDuongHopLe, string idActivities, int valid, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;

            if (idRace <= 0)
            {
                message = "Chưa chọn giải chạy";
                return false;
            }

            if (idRunner <= 0 || idStrava + "" == "")
            {
                message = "Chưa chọn người dùng";
                return false;
            }

            if (idActivities + "" == "")
            {
                message = "Chưa nhập ID hoạt động";
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().AddHoatDongThuCong(idRace, idStrava, idRunner, ngayHoatDong, tongQuangDuong, quangDuongHopLe, idActivities, valid, out message, out idInsertUpdate);
            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucNhomChay(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucNhomChay(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool UdateDanhMucVanDongVien(int id, string ma, string tenHienThi, string tenDayDu, string gioiTinh, string userType, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (id <= 0)
            {
                message = "Không tìm thấy dữ liệu ID";
                return false;
            }
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã";
                return false;
            }
            else if (tenHienThi + "" == "")
            {
                message = "Chưa nhập Tên hiển thị";
                return false;
            }
            else if (tenDayDu + "" == "")
            {
                message = "Chưa nhập Tên đầy đủ";
                return false;
            }
            else if (gioiTinh + "" == "")
            {
                message = "Chưa nhập Giới tính";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().UpdateDanhMucVanDongVien(id, ma, tenHienThi, tenDayDu, gioiTinh, userType, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucKho(int id, string ma, string ten, int idChiNhanh, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã Kho";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên Kho";
                return false;
            }
            else if (idChiNhanh <= 0)
            {
                message = "Chưa nhập chi nhánh";
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucKho(id, ma, ten, idChiNhanh, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhNhaSanXuat(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã nhà sản xuất";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên nhà sản xuất";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucNhaSanXuat(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhNhaCungCap(int id, string ma, string ten, string diaChi, string dienThoaiCty, string website, string fax, int idNhomNhaCungCap, string ghiChu, string dienThoaiNguoiDaiDien, string emailNguoiDaiDien, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã nhà cung cấp";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên nhà cung cấp";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucNhaCungCap(id, ma, ten, diaChi, dienThoaiCty, website, fax, idNhomNhaCungCap, ghiChu, dienThoaiNguoiDaiDien, emailNguoiDaiDien, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucLoaiTaiSan(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã loại tài sản";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên loại tài sản";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucLoaiTaiSan(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucNhomTaiSan(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã nhóm tài sản";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên nhóm tài sản";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucNhomTaiSan(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool UpdateHoatDong(int id, int hopLe, decimal quangDuongHopLe, string ghiChuThayDoi, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;

            ret = DataProcess.ProcessDanhMuc.getInstance().UpdateHoatDong(id, hopLe, quangDuongHopLe, ghiChuThayDoi, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucSanPham(int id, string loaiSP, int idnhomSanPham, string ma, string ten, decimal donGia, int idDonViTinh, string ghiChu, string ghiChuHighLight, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã sản phẩm";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên sản phẩm";
                return false;
            }


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucSanPham(id, loaiSP, idnhomSanPham, ma, ten, donGia, idDonViTinh, ghiChu, ghiChuHighLight, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucQLDangKyGuiThuChiPhi(int id, int IDTinhThanhTu,
                int IDTinhThanhDen,
                int TrongLuongTu,
                int TrongLuongDen,
                string HinhThucGui,
                decimal ChiPhiGuiThu,
                decimal PhuPhiVungSau,
                decimal PhuPhiXangDau,
                decimal PhuPhiKhac,
                decimal PhiVAT,
                decimal TongChiPhi,
                string GhiChu,
                string userLoginId, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;

            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucQLDangKyGuiThuChiPhi(id, IDTinhThanhTu,
                 IDTinhThanhDen,
                 TrongLuongTu,
                 TrongLuongDen,
                 HinhThucGui,
                 ChiPhiGuiThu,
                 PhuPhiVungSau,
                 PhuPhiXangDau,
                 PhuPhiKhac,
                 PhiVAT,
                 TongChiPhi,
                 GhiChu,
                userLoginId, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }

        #region tài sản
        public bool AddOrUpdateDanhMucTaiSan(int id, string maTaiSan, string TenTaiSan, int IDDonViTinh, int IDNhomTaiSan,
            int IDLoaiTaiSan, string SoSerialNumber, decimal giaTriTaiSan, string CauHinh, int IDNhaSanXuat, DateTime NamSanXuat, string SoPhieuBaoHanh, int SoThangBaoHanh
            , DateTime ngayHetHan, string TyLeHaoMon, DateTime NamDuaVaoSuDung, int IDNhaCungCap
            , string ChungNhanCOCQ, string GhiChu, int IDLoaiBaoHiem, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (maTaiSan + "" == "")
            {
                message = "Chưa nhập Mã tài sản";
                return false;
            }
            else if (TenTaiSan + "" == "")
            {
                message = "Chưa nhập Tên tài sản";
                return false;
            }
            else if (IDDonViTinh <= 0)
            {
                message = "Chưa nhập Đơn vị tính";
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucTaiSan(id, maTaiSan, TenTaiSan, IDDonViTinh, IDNhomTaiSan,
             IDLoaiTaiSan, SoSerialNumber, giaTriTaiSan, CauHinh, IDNhaSanXuat, NamSanXuat, SoPhieuBaoHanh, SoThangBaoHanh
            , ngayHetHan, TyLeHaoMon, NamDuaVaoSuDung, IDNhaCungCap, ChungNhanCOCQ, GhiChu, IDLoaiBaoHiem, userLogin, out message, out idInsertUpdate);

            if (ret)
            {
                message = "Lưu thành công";
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
        }
        private bool DeleteTaiSan(int id, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessDanhMuc.getInstance().DeleteTaiSan(id, userLogin, out message);
            if (ret)
            {
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
        }
        private bool SendApprovalTaiSan(int id, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessDanhMuc.getInstance().SendApprovalTaiSan(id, userLogin, out message);
            if (ret)
            {
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
        }
        private bool ApprovalTaiSan(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessDanhMuc.getInstance().ApprovalTaiSan(id, ghiChu, userLogin, out message);
            if (ret)
            {
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
        }
        private bool RejectTaiSan(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessDanhMuc.getInstance().RejectTaiSan(id, ghiChu, userLogin, out message);
            if (ret)
            {
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
        }
        private bool CapNhatGiaTriTaiSan(int id, decimal giaTriTaiSan, int idLoaiBaoHiem, int idNhomTaiSan, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessDanhMuc.getInstance().CapNhatGiaTriTaiSan(id, giaTriTaiSan, idLoaiBaoHiem, idNhomTaiSan, userLogin, out message);
            if (ret)
            {
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
        }
        public bool AddOrUdateDanhMucLoaiBaoHiem(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên";
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucLoaiBaoHiem(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool AddOrUdateDanhMucNhomNhaCungCap(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên";
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateDanhMucNhomNhaCungCap(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool UdateDanhMucConfig(int id, string ma, string ten, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (ma + "" == "")
            {
                message = "Chưa nhập Mã";
                return false;
            }
            else if (ten + "" == "")
            {
                message = "Chưa nhập Tên";
                return false;
            }
            if (!ValidateConfigRule(ma, ten, out message))
            {
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().UpdaeDanhMucConfig(id, ma, ten, ghiChu, userLogin, out message, out idInsertUpdate);

            if (ret) { message = "Lưu thành công"; }
            return ret;
        }
        public bool ValidateConfigRule(string key, string value, out string errorMessage)
        {
            errorMessage = null;

            if (string.IsNullOrWhiteSpace(value))
            {
                errorMessage = $"{key} không được để trống.";
                return false;
            }
            Decimal decimalValue = 0;
            Int32 pauseTime = 0;
            bool boolValue = false;
            TimeSpan temTimeSpan;
            DateTime dateTemp;
            switch (key)
            {
                // Decimal >= 0
                case "soKmToiThieu":
                case "soKmToiDaTrongNgay":
                case "tocDoTrungBinhTu":
                case "tocDoTrungBinhDen":
                case "splitsPaceTu":
                case "splitsPaceDen":

                    if (!Decimal.TryParse(
                        value,
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out decimalValue))
                    {
                        errorMessage = $"{key} phải là số. Ví dụ: 3.00";
                        return false;
                    }

                    if (decimalValue < 0 && key != "tocDoTrungBinhTu")
                    {
                        errorMessage = $"{key} phải lớn hơn hoặc bằng 0.";
                        return false;
                    }

                    return true;


                // Boolean
                case "checkSplitsPace":
                case "checkWorkTime":
                case "raceOffline":
                case "checkConfigTime":

                    if (!bool.TryParse(value, out boolValue))
                    {
                        errorMessage = $"{key} phải có giá trị true hoặc false.";
                        return false;
                    }

                    return true;


                // Time HH:mm
                case "workTimeAmFrom":
                case "workTimeAmTo":
                case "workTimePmFrom":
                case "workTimePmTo":
                case "configTimeAmFrom":
                case "configTimeAmTo":

                    if (!TimeSpan.TryParseExact(
                        value,
                        @"hh\:mm",
                        CultureInfo.InvariantCulture,
                        out temTimeSpan))
                    {
                        errorMessage = $"{key} phải đúng định dạng HH:mm. Ví dụ: 08:00";
                        return false;
                    }

                    return true;


                // Integer >= 0
                case "pauseTime":

                    if (!Int32.TryParse(value, out pauseTime))
                    {
                        errorMessage = $"{key} phải là số nguyên. Ví dụ: 14400";
                        return false;
                    }

                    if (pauseTime < 0)
                    {
                        errorMessage = $"{key} phải lớn hơn hoặc bằng 0.";
                        return false;
                    }

                    return true;


                // Date yyyy-MM-dd
                case "raceDateFrom":
                case "raceDateTo":

                    if (!DateTime.TryParseExact(
                        value,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out dateTemp))
                    {
                        errorMessage = $"{key} phải đúng định dạng yyyy-MM-dd. Ví dụ: 2026-05-25";
                        return false;
                    }

                    return true;
                case "LIST_HOLIDAY_DATE_ALL_RACE":

                    string[] dates = value.Split(';');

                    if (dates.Length == 0)
                    {
                        errorMessage = key + " không có dữ liệu ngày.";
                        return false;
                    }

                    foreach (string item in dates)
                    {
                        DateTime dateValue;

                        if (!DateTime.TryParseExact(
                            item.Trim(),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out dateValue))
                        {
                            errorMessage = key + " có ngày không đúng định dạng yyyy-MM-dd. Giá trị lỗi: " + item;
                            return false;
                        }
                    }

                    return true;
                default:
                    errorMessage = "";
                    return true;
            }
        }


        #endregion tài sản

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }






        private bool DeleteThayDoiTaiSan(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDanhMuc.getInstance().DeleteThayDoiTaiSan(id, userLogin, out message);

        }
        private bool SendApprovalThayDoiTaiSan(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDanhMuc.getInstance().SendApprovalThayDoiTaiSan(id, userLogin, out message);

        }
        public bool AddOrUpdateThayDoiTaiSan(int id, int idTaiSan, string noiDungThayDoi, string GhiChuThayDoi, DateTime ngayThayDoi, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;
            if (noiDungThayDoi + "" == "")
            {
                message = "Chưa nhập nội dung thay đổi";
                return false;
            }

            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateThayDoiTaiSan(id, idTaiSan, noiDungThayDoi, GhiChuThayDoi, ngayThayDoi, userLogin, out message, out idInsertUpdate);

            if (ret)
            {
                message = "Lưu thành công";

            }
            return ret;
        }

        private bool ApprovalThayDoiTaiSan(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDanhMuc.getInstance().ApprovalThayDoiTaiSan(id, ghiChu, userLogin, out message);

        }
        private bool RejectThayDoiTaiSan(int id, string ghiChu, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessDanhMuc.getInstance().RejectThayDoiTaiSan(id, ghiChu, userLogin, out message);

        }
    }
}