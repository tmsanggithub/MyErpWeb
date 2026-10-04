using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using WebRunDragon.Forms;

namespace WebRunDragon.Actions
{
    /// <summary>
    /// Summary description for QLHangHoaAction
    /// </summary>
    public class QLHangHoaAction : IHttpHandler, IRequiresSessionState
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(QLHangHoaAction));
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

                id = Utils.NumberUtil.ParseToInt(context.Request.Form["ID"]);

                string mode = context.Request.Form["mode"].Trim().ToUpper();
                if (mode == "EDIT")
                {
                    string subMode = context.Request.Form["subMode"].Trim().ToUpper();

                    if (subMode == "EditOnly".ToUpper() && Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper())
                        || subMode == "ReadOnly".ToUpper() && Utils.ActionUtil.CanRead(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessHangHoa.getInstance().GetHangHoaByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "B?n không có quy?n th?c hi?n ch?c n?ng này";
                }
                else if (mode == "AddOrUpdate".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper()) && id == 0))
                    {
                        int idInsertUpdate = 0;

                        success = AddOrUpdateHangHoa(id
                            , context.Request.Form["MaHangHoa"] + ""
                            , context.Request.Form["MaVach"] + ""
                            , context.Request.Form["TenHangHoa"] + ""
                            , context.Request.Form["ImageUrl"] + ""
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["IDNhomHangHoa"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["IDThuongHieu"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["DinhMucTonKhoThapNhat"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["DinhMucTonCaoThapNhat"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["TenViTri"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["TrongLuong"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViTrongLuong"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["KichThuotRong"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["KichThuotDai"])
                            , Utils.NumberUtil.ParseToInt(context.Request.Form["IDDonViKichThuot"])
                            , context.Request.Form["MoTaChiTietHangHoa"] + ""
                            , sessionUserId, out message, out idInsertUpdate);
                        if (success) { message = "L?u thành công"; }
                        // truong hop them moi thi tra ve l?i ID m?i thêm vào database
                        if (id <= 0) id = idInsertUpdate;
                    }
                    else
                        message = "B?n không có quy?n thêm ho?c ch?nh s?a ch?c n?ng này";

                }
                else if (mode == "DELETE")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLHangHoa).Name.ToUpper()))
                    {
                        success = DeleteHangHoa(id, sessionUserId, out message);
                        if (success) { message = "Xóa thành công"; }
                    }
                    else
                        message = "B?n không có quy?n xóa ch?c n?ng này";
                }
                else if (mode == "AddOrUpDateDetail".ToUpper())
                {

                    if ((Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper()) && id > 0) ||
                        (Utils.ActionUtil.CanCreate(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper()) && id == 0))
                    {
                        if (context.Request.Form["TenDonViTinh"] + "" == "")
                        {
                            message = "Ch?a nh?p Tên ??n v? tính";
                            success = false;
                        }
                        else
                        {
                            int idInsertUpdate = 0;
                            int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);

                            success = AddOrUpdateHangHoaDonViTinh(id, idMaster
                                , context.Request.Form["TenDonViTinh"] + ""
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaTriQuiDoi"])
                                , Utils.NumberUtil.ParseToDecimal(context.Request.Form["GiaBan"])
                                , Utils.NumberUtil.ParseToBool(context.Request.Form["IsDonViCoBan"]) == true ? 1 : 0
                                , sessionUserId, out message, out idInsertUpdate);
                            if (success) { message = "L?u chi ti?t thành công"; }

                            // truong hop them moi thi tra ve l?i ID m?i thêm vào database
                            if (id <= 0) id = idInsertUpdate;
                        }
                    }
                    else
                        message = "B?n không có quy?n thêm ho?c ch?nh s?a ch?c n?ng này";

                }
                else if (mode == "DELETEDETAIL")
                {
                    if (Utils.ActionUtil.CanDelete(sessionUserId, typeof(QLHangHoa).Name.ToUpper()))
                    {
                        int idMaster = Utils.NumberUtil.ParseToInt(context.Request.Form["IDMaster"]);
                        success = DeleteHangHoaDonViTinh(id, idMaster, sessionUserId, out message);
                        if (success) { message = "Xóa chi ti?t thành công"; }
                    }
                    else
                        message = "B?n không có quy?n xóa ch?c n?ng này";
                }
                else if (mode == "EDITDETAIL")
                {
                    if (Utils.ActionUtil.CanEdit(Utils.UserUtil.GetSessionUserId(), typeof(QLHangHoa).Name.ToUpper()))
                    {
                        JObject entity = DataProcess.ProcessHangHoa.getInstance().GetHangHoaDonViTinhByID(id);
                        success = entity != null;
                        retObject["entity"] = entity;
                    }
                    else
                        message = "B?n không có quy?n th?c hi?n ch?c n?ng này";
                }
                else
                    message = "Invalid mode !";

            }
            catch (Exception ex)
            {
                message = ex.Message;
                logger.Error("ProcessRequest Error");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            finally
            {
                retObject["success"] = success;
                retObject["retCode"] = retCode;
                retObject["message"] = success ? (message == "" ? "Th?c hi?n thành công" : message) : (message == "" ? "Th?c hi?n th?t b?i" : message);
                retObject["id"] = id;
            }
            context.Response.Write(retObject.ToString());
        }

        private bool AddOrUpdateHangHoa(int id, string maHangHoa, string maVach, string tenHangHoa, string imageUrl,
            int idNhomHangHoa, int idThuongHieu, int dinhMucTonKhoThapNhat, int dinhMucTonCaoThapNhat, int tenViTri,
            int trongLuong, int idDonViTrongLuong, int kichThuotRong, int kichThuotDai, int idDonViKichThuot,
            string moTaChiTietHangHoa, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessHangHoa.getInstance().AddOrUpdate(id, maHangHoa, maVach, tenHangHoa, imageUrl,
                idNhomHangHoa, idThuongHieu, dinhMucTonKhoThapNhat, dinhMucTonCaoThapNhat, tenViTri,
                trongLuong, idDonViTrongLuong, kichThuotRong, kichThuotDai, idDonViKichThuot,
                moTaChiTietHangHoa, userLogin, out message, out idInsertNew);
        }
        private bool DeleteHangHoa(int id, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessHangHoa.getInstance().Delete(id, userLogin, out message);
        }
        private bool AddOrUpdateHangHoaDonViTinh(int id, int idHangHoa, string tenDonViTinh, decimal giaTriQuiDoi,
            decimal giaBan, int isDonViCoBan, string userLogin, out string message, out int idInsertNew)
        {
            return DataProcess.ProcessHangHoa.getInstance().AddOrUpdateHangHoaDonViTinh(id, idHangHoa, tenDonViTinh, giaTriQuiDoi, giaBan, isDonViCoBan, userLogin, out message, out idInsertNew);
        }
        private bool DeleteHangHoaDonViTinh(int idCT, int idMaster, string userLogin, out string message)
        {
            message = "";
            return DataProcess.ProcessHangHoa.getInstance().DeleteHangHoaDonViTinh(idCT, idMaster, userLogin, out message);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
