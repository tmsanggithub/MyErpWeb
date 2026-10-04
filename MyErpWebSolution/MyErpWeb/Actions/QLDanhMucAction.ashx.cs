using DevExpress.XtraReports.Web.WebDocumentViewer.Native.DataContracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using System.Web.UI;
using WebRunDragon.DataProcess;
using WebRunDragon.Forms;
using WebRunDragon.Utils;

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
                            success = DataProcess.ProcessDanhMuc.getInstance().DeleteDanhMucByIDCheckConstrainKey(id, tableName, sessionUserId, out message);


                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), tableName.ToUpper())))
                    {
                        int idInsertNew = 0;

                        if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMDonViTinh.ToString().ToUpper() + "")
                        {
                            success = AddOrUdateDanhMucDonViTinh(id, context.Request.Form["MaDonViTinh"], context.Request.Form["TenDonViTinh"], context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMKho.ToString().ToUpper() + "")
                        {
                            success = AddOrUdateDanhMucKho(id, context.Request.Form["MaKho"], context.Request.Form["TenKho"], NumberUtil.ParseToInt(context.Request.Form["IDChiNhanh"]), context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);

                        }
                        else if (tableName == "DM_KHACH_HANG")
                        {
                            // Fields expected from client: SoDienThoai, TenKhachHang, Email, DiaChi, PhuongXa, TinhThanhPho, GhiChu
                            string soDienThoai = context.Request.Form["SoDienThoai"] + "";
                            string tenKhachHang = context.Request.Form["TenKhachHang"] + "";
                            string email = context.Request.Form["Email"] + "";
                            string diaChi = context.Request.Form["DiaChi"] + "";
                            string phuongXa = context.Request.Form["PhuongXa"] + "";
                            string tinhThanhPho = context.Request.Form["TinhThanhPho"] + "";
                            string ghiChuKh = context.Request.Form["GhiChu"] + "";

                            success = AddOrUdateDanhMucKhachHang(id, soDienThoai, tenKhachHang, email, diaChi, phuongXa, tinhThanhPho, ghiChuKh, sessionUserId, out message, out idInsertNew);
                            var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_dm_khach_hang_";
                            BusinessMemCache cache = new BusinessMemCache();
                            cache.RemoveMyCachedItem(keyCache);
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMNhaSanXuat.ToString().ToUpper() + "")
                        {
                            string tenNSX = (context.Request.Form["TenNhaSanXuat"] + "").Replace("{sang_va}", "&");
                            success = AddOrUdateDanhNhaSanXuat(id, context.Request.Form["MaNhaSanXuat"], tenNSX, context.Request.Form["GhiChu"], sessionUserId, out message, out idInsertNew);

                            if (success)
                            {
                                BusinessMemCache cache = new BusinessMemCache();
                                var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_DMNhaSanXuat_";
                                cache.RemoveMyCachedItem(keyCache);
                            }
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMNhaCungCap.ToString().ToUpper() + "")
                        {
                            string tenNCC = (context.Request.Form["TenNhaCungCap"] + "").Replace("{sang_va}", "&");

                            success = AddOrUdateDanhNhaCungCap(id, context.Request.Form["MaNhaCungCap"], tenNCC, context.Request.Form["DiaChi"], context.Request.Form["DienThoaiCty"], context.Request.Form["Website"], context.Request.Form["Fax"]
                                , Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhomNhaCungCap"]), context.Request.Form["GhiChu"], context.Request.Form["DienThoaiNguoiDaiDien"], context.Request.Form["EmailNguoiDaiDien"], sessionUserId, out message, out idInsertNew);

                            if (success)
                            {
                                BusinessMemCache cache = new BusinessMemCache();
                                var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_DMNhaCungCap_";
                                cache.RemoveMyCachedItem(keyCache);
                            }
                        }
                        else if (tableName == DataProcess.ProcessDanhMuc.eTenDanhMuc.DMTaiSan.ToString().ToUpper() + "")
                        {
                            string MaTaiSan = context.Request.Form["MaTaiSan"] + "";// txtMaTaiSan.GetText();
                            string TenTaiSan = (context.Request.Form["TenTaiSan"] + "").Replace("'", " ").Replace("\"", "");// txtTenTaiSan.GetText();
                            int IDDonViTinh = Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViTinh"]);// cbDonViTinh.GetValue();
                            int IDNhomTaiSan = Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhomTaiSan"]);// cbNhomTaiSan.GetValue();
                            int IDNhaSanXuat = Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhaSanXuat"]);// cbNhaSanXuat.GetValue();
                            int IDViTri = Utils.NumberUtil.ParseToInt(context.Request.Form["IDViTri"]);// cbViTri.GetValue();

                            string SoSerialNumber = context.Request.Form["SoSerialNumber"] + "";// txtSoSerialNumber.GetText();
                            decimal GiaTriTaiSan = Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaTriTaiSan"] + "");// txtGiaTriTaiSan.GetText();

                            decimal TrongLuong = Utils.NumberUtil.ParseToDecimal(context.Request.Form["TrongLuong"] + "");// txtTrongLuong.GetValue();
                            int IdDonViTrongLuong = Utils.NumberUtil.ParseToInt(context.Request.Form["IdDonViTrongLuong"]);// cbDonViTinhTrongLuong.GetValue();
                            decimal KichThuotDai = Utils.NumberUtil.ParseToDecimal(context.Request.Form["KichThuotDai"] + "");// txtKichThuotDai.GetValue();
                            decimal KichThuotRong = Utils.NumberUtil.ParseToDecimal(context.Request.Form["KichThuotRong"] + "");// txtKichThuotRong.GetValue();
                            int IdDonViKichThuot = Utils.NumberUtil.ParseToInt(context.Request.Form["IdDonViKichThuot"]);// cbDonViTinhKichThuot.GetValue();

                            string GhiChu = (context.Request.Form["GhiChu"] + "").Replace("'", " ").Replace("\"", "");// txtGhiChu.GetValue();
                            success = AddOrUpdateDanhMucTaiSan(id, MaTaiSan, TenTaiSan, IDDonViTinh, IDNhomTaiSan,
                                        IDNhaSanXuat, SoSerialNumber, GiaTriTaiSan, IDViTri, TrongLuong, IdDonViTrongLuong,
                                        KichThuotDai, KichThuotRong, IdDonViKichThuot, GhiChu, sessionUserId, out message, out idInsertNew);

                            if (success)
                            {
                                BusinessMemCache cache = new BusinessMemCache();
                                var keyCache = "LoadDanhMuc4ComboFromCacheFromCache_DMTaiSan_";
                                cache.RemoveMyCachedItem(keyCache);
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
                            BusinessMemCache cache = new BusinessMemCache();
                            var keyCache = "LoadDanhMuc4ComboFromCache_table_" + tableName;
                            cache.RemoveMyCachedItem(keyCache);
                        }
                    }
                    else
                        message = "Bạn không có quyền thêm hoặc chỉnh sửa chức năng này";

                }
                else if (mode == "SendApprovalTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMTaiSan).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = SendApprovalTaiSan(id, sessionUserId, out message);
                        if (success)
                        {
                            message = "Gửi duyệt thành công";
                            //context.Session.Remove("ssDMTaiSan");
                            DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
                        }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "ApprovalTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = ApprovalTaiSan(id, context.Request.Form["GhiChuDuyet"] + "", sessionUserId, out message);
                        if (success)
                        {
                            message = "Duyệt thành công";
                            DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
                        }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "RejectTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = RejectTaiSan(id, context.Request.Form["GhiChuKhongDuyet"] + "", sessionUserId, out message);
                        if (success)
                        {
                            message = "Từ chối thành công";
                            context.Session.Remove("ssDMTaiSan");
                        }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "CapNhatGiaTriTS".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        int idNhomTaiSan = Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhomTaiSan"]);// cbNhomTaiSan.GetValue();                      
                        int idViTri = Utils.NumberUtil.ParseToInt(context.Request.Form["IDViTri"]);// cbIDViTri.GetValue();
                        decimal giaTriTaiSan = Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaTriTaiSan"] + "");
                        success = CapNhatGiaTriTaiSan(id, giaTriTaiSan, idViTri, idNhomTaiSan, sessionUserId, out message);
                        if (success)
                        {
                            message = "Cập nhật thông tin (giá, nhóm TS, loại BH) thành công";
                            context.Session.Remove("ssDMTaiSan");
                        }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "AddOrUpdateThayDoiTS".ToUpper())
                {

                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                        int idDonViTinh = Utils.NumberUtil.ParseToInt(context.Request.Form["IdDonViTinh"]);
                        decimal giaTriQuiDoi = Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaTriQuiDoiSoVoiDonViCoBan"]);
                        int idInsertNew = 0;
                        success = AddOrUpdateThayDoiTaiSan(id, idMaster, idDonViTinh, giaTriQuiDoi, sessionUserId, out message, out idInsertNew);
                        if (id <= 0) id = idInsertNew;
                        if (success)
                        {
                            message = "Cập nhật thay đổi tài sản thành công";
                        }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "GetThayDoiTaiSanDetail".ToUpper())
                {
                    JObject entity = DataProcess.ProcessDanhMuc.getInstance().GetDanhMucByID(id, "DMTaiSanCT");
                    success = entity != null;
                    retObject["entity"] = entity;
                }
                else if (mode == "deleteThayDoiTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMTaiSan).Name.ToUpper()) || Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = DeleteThayDoiTaiSan(id, sessionUserId, out message);
                        if (success)
                        {
                            message = "Xóa thay đổi thành công";
                        }
                        else
                        {
                            message = "Xóa thất bại";
                        }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "SendApprovalThayDoiTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMTaiSan).Name.ToUpper())
                      || Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = SendApprovalThayDoiTaiSan(id, sessionUserId, out message);
                        if (success)
                        {
                            message = "Gửi duyệt thành công";
                        }
                        else
                        {
                            message = "Gửi duyệt thất bại";
                        }
                    }
                    else
                        message = "Bạn không có quyền xóa chức năng này";
                }
                else if (mode == "ApprovalThayDoiTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = ApprovalThayDoiTaiSan(id, context.Request.Form["GhiChuDuyetThayDoi"] + "", sessionUserId, out message);
                        if (success)
                        {
                            message = "Duyệt thành công";
                            DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
                        }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "RejectThayDoiTaiSan".ToUpper())
                {
                    if (Utils.ActionUtil.CanApprove(sessionUserId, typeof(DMTaiSan).Name.ToUpper()))
                    {
                        success = RejectThayDoiTaiSan(id, context.Request.Form["GhiChuKhongDuyetThayDoi"] + "", sessionUserId, out message);
                        if (success)
                        {
                            message = "Từ chối thành công";
                            context.Session.Remove("ssDMTaiSan");
                        }
                    }
                    else
                        message = "Bạn không có quyền duyệt (hoặc từ chối) chức năng này";
                }
                else if (mode == "COPYTAISAN".ToUpper())
                {
                    if (Utils.ActionUtil.CanCreate(sessionUserId, typeof(DMTaiSan).Name.ToUpper())
                       || Utils.ActionUtil.CanEdit(sessionUserId, typeof(DMTaiSan).Name.ToUpper())
                        )
                    {
                        string mataisancopy = context.Request.Form["MaTaiSan"] + "";


                        JObject entity = DataProcess.ProcessDanhMuc.getInstance().GetDanhMucByCode("MaTaiSan", mataisancopy, tableName);
                        success = entity != null;
                        retObject["entity"] = entity;

                        if (success)
                        {

                        }
                        else
                        {
                            message = "Không tìm thấy thông tin tài sản với mã tài sản " + mataisancopy;
                        }
                    }
                    else
                        message = "Bạn không có quyền chỉnh sửa chức năng này";
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

        public bool AddOrUdateDanhMucKhachHang(int id, string soDienThoai, string tenKhachHang, string email, string diaChi, string phuongXa, string tinhThanhPho, string ghiChu, string userLogin, out string message, out int idInsertNew)
        {
            message = ""; idInsertNew = id;
            bool ret = false;
            try
            {
                ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateKhachHang(id, soDienThoai, tenKhachHang, email, diaChi, phuongXa, tinhThanhPho, ghiChu, userLogin, out message, out idInsertNew);
                if (ret) message = "Lưu thành công";
            }
            catch (Exception ex)
            {
                logger.Error("AddOrUdateDanhMucKhachHang Error: ", ex);
                message = ex.Message;
                ret = false;
            }
            return ret;
        }

        public bool AddOrUdateDanhMucKho(int id, string ma, string ten, int idChiNhanh, string ghiChu, string userLogin, out string message, out int idInsertUpdate)
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
            else if (idChiNhanh <= 0)
            {
                message = "Chưa nhập Chi nhánh";
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

        #region tài sản
        public bool AddOrUpdateDanhMucTaiSan(int id, string maTaiSan, string TenTaiSan, int IDDonViTinh, int IDNhomTaiSan,
            int IDNhaSanXuat, string SoSerialNumber, decimal giaTriTaiSan, int IDViTri, decimal TrongLuong, int IdDonViTrongLuong,
            decimal KichThuotDai, decimal KichThuotRong, int IdDonViKichThuot, string GhiChu, string userLogin, out string message, out int idInsertUpdate)
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
             IDNhaSanXuat, SoSerialNumber, giaTriTaiSan, IDViTri, TrongLuong, IdDonViTrongLuong,
             KichThuotDai, KichThuotRong, IdDonViKichThuot, GhiChu, userLogin, out message, out idInsertUpdate);

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
        private bool CapNhatGiaTriTaiSan(int id, decimal giaTriTaiSan, int idViTri, int idNhomTaiSan, string userLogin, out string message)
        {
            message = "";
            var ret = DataProcess.ProcessDanhMuc.getInstance().CapNhatGiaTriTaiSan(id, giaTriTaiSan, idViTri, idNhomTaiSan, userLogin, out message);
            if (ret)
            {
                DataProcess.ProcessCache.getInstance().clearCacheAndSessionDMTaiSan();
            }
            return ret;
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
        public bool AddOrUpdateThayDoiTaiSan(int id, int idTaiSan, int idDVT, decimal giaTriQuiDoi, string userLogin, out string message, out int idInsertUpdate)
        {
            message = ""; idInsertUpdate = 0;
            bool ret = false;


            ret = DataProcess.ProcessDanhMuc.getInstance().AddOrUpdateThayDoiTaiSan(id, idTaiSan, idDVT, giaTriQuiDoi, userLogin, out message, out idInsertUpdate);

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