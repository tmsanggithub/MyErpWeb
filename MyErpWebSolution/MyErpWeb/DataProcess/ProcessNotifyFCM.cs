using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WebRunDragon.Utils;

namespace WebRunDragon.DataProcess
{
    public class ProcessNotifyFCM
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessNotifyFCM));
        private string className = typeof(ProcessNotifyFCM).Name;
        private static ProcessNotifyFCM instance = null;
        public static ProcessNotifyFCM GetInstance()
        {
            if (instance == null)
            {
                instance = new ProcessNotifyFCM();
            }
            return instance;
        }

        public void SendNotifyByFunction(string lang, string functionCode, string pushId)
        {
            try
            {
                //   string title = ResourceUtil.GetString("FCM_TITLE_SDRAGON", lang);
                //   string notifyContent = ResourceUtil.GetString("FCM_MESSAGE_" + functionCode.ToUpper() + "", lang);
                //   SendNotification(title, notifyContent, pushId);

            }
            catch (Exception er)
            {
                logger.Error(className + "SendNotifyByFunction()Exception.mesage:" + er.Message + "\nTrace:" + er.StackTrace);
            }
        }


        private void SendNotification(string title, string notifycation, string toRegId)
        {
            try
            {

                string fcmKey = AppSettingUtil.FCMKey;
                string senderId = AppSettingUtil.FCM_SenderCode;
                if (string.IsNullOrEmpty(fcmKey) || string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(toRegId))
                {
                    return;
                }
                //fcmKey = "AAAAE5JlKZU:APA91bHYWEEEEa-nOP-J6ZJlp6rtTQEGJ96ZDyO1vIgO0-E-N_7uddXr20SW_lk7BRBzOODq7EE6U8cAZBtSuby5i_wGojUp6JW8O-QUz7C0f_itMK4WF74RTmFLA4jePkG7cIFb7BfK";
                //senderId = "YenSaoNhaTrang";
                //toRegId = "cYnXp1l6T0aRHEbV9kkP7g:APA91bH1pz3NMHnE-9DiW3OcWTD5y8sH3b0S0aAnHYKJs4lugSSIhohvGSGujkPzSN8Xwoiv_ZJmW4y73cH9XKlhn3Igwm-e7jprGV852wiylP4D0d_W8xquXiIXKZR269gYjPI2kPyq";
                dynamic data = new
                {
                    to = toRegId, // Uncoment this if you want to test for single device
                                  // registration_ids = toRegIds, // this is for multiple user 
                    notification = new
                    {
                        title = title,// "YSNT Thông báo",     // Notification title
                        body = notifycation,// "Co don hang cho DVKH duyet",                       
                    }
                };
                var json = JsonConvert.SerializeObject(data);
                logger.Debug(className + " SendNotification(" + senderId + ")" + "\nfcmKey:" + fcmKey + "\ntoRegIds:" + toRegId);
                Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);
                string SERVER_API_KEY = fcmKey;
                string SENDER_ID = senderId;
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();
                logger.Debug(className + " SendNotification(" + senderId + ")" + "\nsResponseFromServer:" + sResponseFromServer?.ToJsonString() + "\ntResponse:" + tResponse?.ToJsonString());
                tReader.Close();
                dataStream.Close();
                tResponse.Close();


            }
            catch (Exception er)
            {
                logger.Error(className + " SendNotification()Exception.mesage:" + er.Message + "\nTrace:" + er.StackTrace);
            }
        }

        private List<string> GetRegIdDVKHfromCache()
        {
            List<string> ret = null;
            try
            {
                //BusinessMemCache cache = new BusinessMemCache();
                //string key = "GetRegIdDVKHfromCache";
                //var objCache = cache.GetMemCachedItem(key);
                //if (objCache != null)
                //{
                //    ret = (List<string>)objCache;
                //    return ret;
                //}
                DataTable DanhSachRegId = null;// DanhSachRegIdFromCache();
                if (DanhSachRegId != null)
                {
                    ret = new List<string>();
                    foreach (DataRow r in DanhSachRegId.Rows)
                    {
                        if (r["MaNhomChucNangThongBao"] + "" == "DVKH" && r["GeneralPushRegId"] + "" != "")
                        {
                            ret.Add(r["GeneralPushRegId"] + "");
                        }
                    }
                }
                //if (ret != null)
                //{
                //    cache.AddToMemCache(key, ret, MyCachePriority.Default);
                //}
            }
            catch (Exception er)
            {
                logger.Error(className + ".GetRegIdDVKHfromCache()Exception.mesage:" + er.Message + "\nTrace:" + er.StackTrace);
            }
            return ret;
        }

        //private DataTable DanhSachRegIdFromCache()
        //{
        //    DataTable ret = null;
        //    try
        //    {

        //        ret = Process.ProcessNotifyFCM.GetInstance().DanhSachRegId();

        //    }
        //    catch (Exception er)
        //    {
        //        LogUtils.WriteErr(className, "DanhSachRegIdFromCache()Exception.mesage:" + er.Message + "\nTrace:" + er.StackTrace, er.Message);
        //    }
        //    return ret;
        //}

        //internal DataTable DanhSachRegId()
        //{

        //    try
        //    {
        //        //SqlParameter para01 = new SqlParameter("DeviceId",  "");
        //        //SqlParameter[] paras = new SqlParameter[1] { para01 };             
        //        return SQLConnectionManager.SQLGetDataTable((int)Constants.eDataBaseId.DBID_YSNT, "SP_CDNhanVienNhanThongBa_GetListPushId", "dtReturn", CommandType.StoredProcedure, null);
        //    }
        //    catch (Exception er)
        //    {
        //        LogUtils.WriteErr(className, "DanhSachSanPhamTopSale()Exception.mesage:" + er.Message + "\nTrace:" + er.StackTrace, er.Message);
        //        return null;
        //    }
        //}


    }
}