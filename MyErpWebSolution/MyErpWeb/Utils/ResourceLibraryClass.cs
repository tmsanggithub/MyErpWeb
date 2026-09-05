using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Resources;

namespace WebRunDragon.Utils
{
    public class ResourceLibraryClass
    {
        public static String RESOURCE_FOLDER = "App_GlobalResources";
        public static String RESOURCE_EN = "en-US.resx";
        public static String RESOURCE_VI = "vi-VN.resx";
        private static string RESOURCE_TALBE_VN = "ResourceTable_vn";
        private static string RESOURCE_TABLE_EN = "ResourceTable_en";

        /// <summary>
        /// get resoure string from resx file
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static String GetString(String resourceName, String lang)
        {
            String filePath = System.Web.HttpContext.Current.Server.MapPath("~")
                            + System.IO.Path.DirectorySeparatorChar
                            + RESOURCE_FOLDER
                            + System.IO.Path.DirectorySeparatorChar
                            + (lang.Equals(Constants.en_US) ? RESOURCE_EN : RESOURCE_VI);

            ResXResourceReader rs = new System.Resources.ResXResourceReader(filePath);

            IEnumerable<DictionaryEntry> enumerator = rs.OfType<DictionaryEntry>();
            var entry = enumerator.Where(obj => obj.Key.ToString().Equals(resourceName))
                                        .Select(obj => obj.Value).FirstOrDefault();
            return (entry == null ? null : entry.ToString());
        }

        /// <summary>
        /// Lay ra tat ca cac resource theo ngon ngu
        /// </summary>
        /// <param name="language">[en-US][vi-VN]</param>
        /// <returns></returns>
        public static Hashtable GetAllResource(String language)
        {
            Hashtable allResource = new Hashtable();
            String filePath = System.Web.HttpContext.Current.Server.MapPath("~")
                            + System.IO.Path.DirectorySeparatorChar
                            + RESOURCE_FOLDER
                            + System.IO.Path.DirectorySeparatorChar
                            + (language.Equals(Constants.en_US) ? RESOURCE_EN : RESOURCE_VI);

            ResXResourceReader rs = new System.Resources.ResXResourceReader(filePath);

            IEnumerable<DictionaryEntry> enumerator = rs.OfType<DictionaryEntry>();
            foreach (DictionaryEntry entry in enumerator)
            {
                if (!allResource.ContainsKey(entry.Key.ToString()))
                {
                    allResource.Add(entry.Key, entry.Value);
                }
            }
            return allResource;
        }

        /// <summary>
        /// update resource to resx file
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourcevalue"></param>
        /// <param name="lang"></param>
        public static void UpdateResource(String resourceName, String resourcevalue, String lang)
        {
            String filePath = System.Web.HttpContext.Current.Server.MapPath("~")
                            + System.IO.Path.DirectorySeparatorChar
                            + RESOURCE_FOLDER
                            + System.IO.Path.DirectorySeparatorChar
                            + (lang.Equals(Constants.en_US) ? RESOURCE_EN : RESOURCE_VI);

            ResXResourceReader reader = new System.Resources.ResXResourceReader(filePath);
            ResXResourceWriter writer = new System.Resources.ResXResourceWriter(filePath);
            Boolean isExisted = false;
            foreach (DictionaryEntry entry in reader)
            {
                if (entry.Key.ToString().Equals(resourceName))
                {
                    writer.AddResource(resourceName, resourcevalue);
                    isExisted = true;
                }
                else
                {
                    writer.AddResource(entry.Key.ToString(), entry.Value);
                }
            }
            if (!isExisted)
            {
                writer.AddResource(resourceName, resourcevalue);
            }
            writer.Generate();
            writer.Close();
        }

        /// <summary>
        /// Cap nhat lai file resource
        /// </summary>
        /// <param name="listSysMessage"></param>
        public static void UpdateResource(List<Tuple<String, String, String>> listSysMessage)
        {
            String folderPath = System.Web.HttpContext.Current.Server.MapPath("~")
                            + System.IO.Path.DirectorySeparatorChar
                            + RESOURCE_FOLDER
                            + System.IO.Path.DirectorySeparatorChar;

            ResXResourceReader viReader = new System.Resources.ResXResourceReader(folderPath + RESOURCE_VI);
            ResXResourceReader enReader = new System.Resources.ResXResourceReader(folderPath + RESOURCE_EN);
            ResXResourceWriter viWriter = new System.Resources.ResXResourceWriter(folderPath + RESOURCE_VI);
            ResXResourceWriter enWriter = new System.Resources.ResXResourceWriter(folderPath + RESOURCE_EN);

            Hashtable viTbl = new Hashtable();
            Hashtable enTbl = new Hashtable();
            foreach (var tuple in listSysMessage)
            {
                if (tuple.Item3.Equals(Constants.vi_VN))
                {
                    if (viTbl[tuple.Item1] == null)
                    {
                        viTbl.Add(tuple.Item1, tuple.Item2);
                    }
                }
                else
                {
                    if (enTbl[tuple.Item1] == null)
                    {
                        enTbl.Add(tuple.Item1, tuple.Item2);
                    }
                }
            }

            foreach (DictionaryEntry entry in viReader)
            {
                if (viTbl[entry.Key] == null)
                {
                    viTbl.Add(entry.Key, entry.Value);
                }
            }


            foreach (DictionaryEntry entry in enReader)
            {
                if (enTbl[entry.Key] == null)
                {
                    enTbl.Add(entry.Key, entry.Value);
                }
            }

            foreach (var key in viTbl.Keys)
            {
                viWriter.AddResource(key.ToString(), viTbl[key].ToString());
            }

            foreach (var key in enTbl.Keys)
            {
                enWriter.AddResource(key.ToString(), enTbl[key].ToString());
            }


            viWriter.Generate();
            viWriter.Close();

            enWriter.Generate();
            enWriter.Close();
        }

        public static String GetStringFromMemory(String key, String language = "")
        {
            ObjectCache cache = MemoryCache.Default;
            String CacheKeyName = RESOURCE_TALBE_VN;
            if (language.Equals(Constants.en_US))
                CacheKeyName = RESOURCE_TABLE_EN;
            if (cache[CacheKeyName] != null && cache[CacheKeyName] is Hashtable)
            {
                Hashtable resourceTable = (Hashtable)cache[CacheKeyName];
                if (resourceTable != null)
                {
                    if (resourceTable.ContainsKey(key))
                    {
                        return resourceTable[key].ToString();
                    }
                }
            }
            return "";
        }

    }
}