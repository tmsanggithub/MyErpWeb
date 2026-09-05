using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessCache
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessCache));
        static string className = typeof(ProcessCache).Name;
        private static ProcessCache _instance;
        public static ProcessCache getInstance()
        {
            if (_instance == null)
                _instance = new ProcessCache();
            return _instance;
        }
        public void clearCacheAndSessionDMTaiSan()
        {
            BusinessMemCache cache = new BusinessMemCache();
            var key = "DMTaiSan";
            var objCache = cache.GetMemCachedItem(key);
            if (objCache != null)
            {
                cache.RemoveMyCachedItem(key);
            }
            HttpContext.Current.Session.Remove("ssDMTaiSan");
        }
    }
}