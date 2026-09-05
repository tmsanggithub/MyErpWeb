
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Utils
{
    public class ResourceUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static BusinessMemCache memcache = new BusinessMemCache();
        private const string RESOURCE_VN = "ResourceTable_vn";
        private const string RESOURCE_EN = "ResourceTable_en";
        private const string TITLE_SUFFIX = ".Title";

        /// <summary>
        /// GetString by language
        /// </summary>
        public static string GetString(string str, string lang)
        {

            string strOutput = string.Empty;
            try
            {

                // Check language first
                if (String.IsNullOrEmpty(lang))
                    lang = Constants.DEFAULT_LANGUAGE;

                if (lang.Equals(Constants.en_US))
                {
                    Hashtable enString = (Hashtable)memcache.GetMemCachedItem(RESOURCE_EN);
                    strOutput = enString[str].ToString();
                }
                else
                {
                    Hashtable vnString = (Hashtable)memcache.GetMemCachedItem(RESOURCE_VN);
                    strOutput = vnString[str].ToString();
                }


            }
            catch (Exception e)
            {
                //Log.Error("Message Code: " + str + ",language: " + lang);
                //Log.Error(e.StackTrace.ToString(), e);
            }
            return strOutput;
        }

        /// <summary>
        /// GetString default
        /// </summary>
        public static string GetString(string str)
        {
            string strOutput = "";
            try
            {
                Hashtable vnString = (Hashtable)memcache.GetMemCachedItem(RESOURCE_VN);
                strOutput = vnString[str].ToString();
            }
            catch (Exception e)
            {
                //Log.Error("Message Code: " + str + ",resource: " + RESOURCE_VN);
                //Log.Error(e.StackTrace.ToString(), e);
            }
            return strOutput;
        }

        /// <summary>
        /// GetString by language
        /// </summary>
        public static string GetStringTitle(string str, string lang)
        {

            string strOutput = string.Empty;
            try
            {
                string key = str + TITLE_SUFFIX;
                // Check language first
                if (String.IsNullOrEmpty(lang))
                    lang = Constants.DEFAULT_LANGUAGE;

                if (lang.Equals(Constants.en_US))
                {
                    Hashtable vnString = (Hashtable)memcache.GetMemCachedItem(RESOURCE_EN);
                    strOutput = vnString[key].ToString();
                }
                else
                {
                    Hashtable enString = (Hashtable)memcache.GetMemCachedItem(RESOURCE_VN);
                    strOutput = enString[key].ToString();
                }
            }
            catch (Exception e)
            {
                //Log.Error("Message Code: " + str + ",language: " + lang);
                //Log.Error(e.StackTrace.ToString(), e);
            }
            return strOutput;
        }

        /// <summary>
        /// GetString default
        /// </summary>
        public static string GetStringTitle(string str)
        {
            string strOutput = "";
            try
            {
                string key = str + TITLE_SUFFIX;
                Hashtable vnString = (Hashtable)memcache.GetMemCachedItem(RESOURCE_VN);
                strOutput = vnString[str].ToString();
            }
            catch (Exception e)
            {
                //Log.Error("Message Code: " + str + ",resource: " + RESOURCE_VN);
                //Log.Error(e.StackTrace.ToString(), e);
            }
            return strOutput;
        }


        /// <summary>
        /// put resource to memory cache
        /// </summary>
        public static void PutResourceToMemory()
        {
            try
            {
                Hashtable checkViResource = (Hashtable)memcache.GetMemCachedItem(RESOURCE_VN);
                // Neu chua co thi add vao cache
                if (checkViResource == null)
                {
                    //get message from eportal
                    // var viEportalMessage = EbankRepository.GetSystemMessage(Constants.vi_VN);
                    // var enEportalMessage = EbankRepository.GetSystemMessage(Constants.en_US);

                    //get resource file
                    Hashtable viResource = ResourceLibraryClass.GetAllResource(Constants.vi_VN);
                    Hashtable enResource = ResourceLibraryClass.GetAllResource(Constants.en_US);

                    //var viList = from item in viEportalMessage
                    //             select new Tuple<String, String, String>(item.Code, item.Message, item.Language);
                    //var enList = from item in enEportalMessage
                    //             select new Tuple<String, String, String>(item.Code, item.Message, item.Language);

                    //foreach (var t in viList)
                    //{
                    //    if (!viResource.ContainsKey(t.Item1))
                    //    {
                    //        viResource.Add(t.Item1, t.Item2);
                    //    }
                    //}
                    //foreach (var t in enList)
                    //{
                    //    if (!enResource.ContainsKey(t.Item1))
                    //    {
                    //        enResource.Add(t.Item1, t.Item2);
                    //    }
                    //}
                    //put to memory
                    memcache.AddToMemCacheNotRemovable(RESOURCE_EN, enResource);
                    memcache.AddToMemCacheNotRemovable(RESOURCE_VN, viResource);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace.ToString(), e);
            }

        }

    }
}