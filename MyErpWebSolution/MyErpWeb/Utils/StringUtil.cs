using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace WebRunDragon.Utils
{
    public class StringUtil
    {
        private static string PASSWORD = "AbcMNPxyZ&*^2013";

        public static bool IsNullOrBlank(string xxx)
        {
            if (string.IsNullOrEmpty(xxx)) return true;
            return (xxx.Trim().ToUpper() == "NULL");
        }

        public static string EncryptString(string inputText)
        {
            return EncryptString(inputText, PASSWORD);
        }

        public static string EncryptString(string inputText, string encryptedPwd)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(inputText);
            byte[] Salt = Encoding.ASCII.GetBytes(encryptedPwd.Length.ToString());

            //This class uses an extension of the PBKDF1 algorithm defined in the PKCS#5 v2.0 
            //standard to derive bytes suitable for use as key material from a password. 
            //The standard is documented in IETF RRC 2898.

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(encryptedPwd, Salt);
            //Creates a symmetric encryptor object. 
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] CipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            return EncryptedData;

        }

        public static string DecryptString(string inputText)
        {
            return DecryptString(inputText, PASSWORD);
        }

        public static string DecryptString(string inputText, string encryptedPwd)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            byte[] EncryptedData = Convert.FromBase64String(inputText);
            byte[] Salt = Encoding.ASCII.GetBytes(encryptedPwd.Length.ToString());
            //Making of the key for decryption
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(encryptedPwd, Salt);
            //Creates a symmetric Rijndael decryptor object.
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(EncryptedData);
            //Defines the cryptographics stream for decryption.THe stream contains decrpted data
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
            byte[] PlainText = new byte[EncryptedData.Length];
            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            //Converting to string
            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
            return DecryptedData;

        }

        //public static void SetPassword4InternalEncryption(string pwd)
        //{
        //PASSWORD = pwd;
        //}
        private static readonly string[] VietnameseSigns = new string[]
       {

            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
       };



        public static string RemoveSign4VietnameseString(string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            //for (int i = 1; i < VietnameseSigns.Length; i++)
            //{
            //    for (int j = 0; j < VietnameseSigns[i].Length; j++)
            //        str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            //}
            //return str;

            return UnicodeKhongDau(str);

        }

        public static string UnicodeKhongDau(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+", RegexOptions.None, TimeSpan.FromMilliseconds(100)); string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        
        public static JArray ConvertDataTable2Array(DataTable dtbInfo)
        {
            if (dtbInfo == null) return null;
            if (dtbInfo != null && dtbInfo.Rows.Count <= 0) return new JArray();

            JArray array = new Newtonsoft.Json.Linq.JArray();
            foreach (DataRow row in dtbInfo.Rows)
            {
                Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                foreach (DataColumn col in dtbInfo.Columns)
                {
                    if (col.DataType == typeof(int))
                        jObject[col.ColumnName] = Utils.NumberUtil.ParseToInt(row[col.ColumnName] + "");
                    if (col.DataType == typeof(decimal) || col.DataType == typeof(double))
                        jObject[col.ColumnName] = Utils.NumberUtil.ParseToDecimal(row[col.ColumnName] + "");
                    else
                        jObject[col.ColumnName] = row[col.ColumnName] + "";
                }
                array.Add(jObject);
            }

            return array;
        }
        public enum eTrangThaiTaiSan
        {
            MoiMua,
            DaQuaSuDung,
            DangSuDung,
            KhongSuDung
        }
        public enum eTrangThaiyeuCauHoTro
        {
            NEW,// Mới tạo, tđv nhận/người tham khảo phân công nhân sự 
            WREQSUP,// Chờ duyệt gởi   , người tạo gửi yc   
            RET,// Trả lại,  tđv ko duyệt gửi yc
            WREISUP,// chờ duyệt nhận, tđv duyệt gửi yc   
                    //  RET,// Trả lại,  tđv nhận ko đồng ý nhận yc 
            REISUPAPP,//   Chờ phân công,  tđv nhận đồng ý nhận yc 
            WDO,// Chờ thực hiện, tđv nhận/người tham khảo phân công nhân sự  
            DONE,//	Đã thực hiện"nút chuyển trạng thái(chỉ ns chính mới có)"
            RENEW,//   Mở lại yêu cầu
            CLOSE,//   Yêu cầu ko còn hiệu lực

        }

        public enum enumFCMFunction
        {
            BANGIAO_CHODUYET_YC, //SP_QLBanGiaoTaiSan_SendApproval---------------------------->done chua test
            BANGIAO_CHODUYET_NHAN_TS, //SP_QLBanGiaoTaiSan_ApprovalBanGiao-------------------->done chua test
            THUHOI_CHODUYET_YC, //SP_QLThuHoiTaiSan_SendApproval------------------------------>done chua test
            THUHOI_CHODUYET_TRA_TAISAN,//SP_QLThuHoiTaiSan_ApprovalThuHoi--------------------->done chua test
            THUHOI_CHODUYET_NHAPKHO_TS_TRALAI, //SP_QLThuHoiTaiSan_ApprovalPass--------------->done chua test
            THANHTOAN_CHODUYET_YC, //SP_QLThanhToan_SendApproval: ---------------------------->done
            DMTAISAN_CHODUYET_YC, //SP_DMTaiSan_SendApproval----------------------------------->done chua test
            DIEUCHINHTAISANKHO_CHODUYET_YC, //SP_QLDieuChinhKho_SendApproval------------------->done chua test
            DIEUCHINHTAISANNV_CHODUYET_YC, //SP_QLDieuChinhKhoNhanVien_SendApproval------------>done chua test
            NHAPKHO_CHODUYET_YC, //SP_QLNhapKho_SendApproval----------------------------------->done chua test
            QLYEUCAU_CHODUYET_GUI_YC, //SP_QLYeuCau_SendApproval------------------------------->done chua test
            QLYEUCAU_CHODUYET_NHAN_YC, //SP_QLYeuCau_RequestedManagerApproval------------------>done chua test
        }
    }
}