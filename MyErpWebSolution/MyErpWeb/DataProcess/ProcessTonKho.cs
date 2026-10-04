using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessTonKho
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessTonKho));
        static string className = typeof(ProcessTonKho).Name;
        private static ProcessTonKho _instance;
        public static ProcessTonKho getInstance()
        {
            if (_instance == null)
                _instance = new ProcessTonKho();
            return _instance;
        }
        public DataTable TraCuuTonKhoNhanVienFromCache(string key4Search, string userLogin, int idPhongBan)
        {
            try
            {
                BusinessMemCache cache = new BusinessMemCache();
                string key = "TraCuuTonKhoNhanVienFromCache_" + userLogin + "_" + key4Search + "_" + idPhongBan;
                var objCache = cache.GetMemCachedItem(key);
                if (objCache != null)
                {
                    return (DataTable)objCache;
                }
                DataTable dtRet = TraCuuTonKhoNhanVienByIdPhongBan(key4Search, userLogin, idPhongBan);
                if (dtRet != null && dtRet.Rows.Count > 0)
                {
                    cache.AddToMemCacheAutoRemove(key, dtRet, MyCachePriority.Default, 5 * 60);
                }
                return dtRet;
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuTonKhoNhanVien() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }

        private DataTable TraCuuTonKhoNhanVien(string key4Search, string userLogin)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);

                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_TonKhoNhanVien_Search", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuTonKhoNhanVien() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
        private DataTable TraCuuTonKhoNhanVienByIdPhongBan(string key4Search, string userLogin, int idPhongBan)
        {
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Key4Search", key4Search);
                param.Add("UserLogin", userLogin);
                param.Add("IdPhongBan", idPhongBan);
                return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_TonKhoNhanVien_Search_by_idphongban", CommandType.StoredProcedure, param);
            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".TraCuuTonKhoNhanVienByIdPhongBan() Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                return null;
            }
        }
    }
}