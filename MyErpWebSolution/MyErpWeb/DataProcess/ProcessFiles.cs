using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using WebRunDragon.Entity;

namespace WebRunDragon.DataProcess
{
    public class ProcessFiles
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessFiles));
        static string className = typeof(ProcessFiles).Name;
        private static ProcessFiles _instance;
        public static ProcessFiles getInstance()
        {
            if (_instance == null)
                _instance = new ProcessFiles();
            return _instance;
        }

        public void GetFolderSaveFile(string fileName, string objName, out string folderSaveyyyyMM, out string folderSaveDataBaseFull, out string folderSaveDataBaseExt, out string fileNameSaved)
        {
            int lastIndexOf = fileName.LastIndexOf(".");
            fileNameSaved=fileName.Substring(0, lastIndexOf) + "." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + "." + fileName.Substring(lastIndexOf + 1);

            folderSaveyyyyMM = Config.SysConfig.ATTACHMENTS_FOLDER_CO_ODIA_NOT_YYYYMM + "\\" + objName + "\\" + DateTime.Now.ToString("yyyyMM");
            folderSaveDataBaseFull = folderSaveyyyyMM + "\\" + fileNameSaved;

            string folderExternal = Config.SysConfig.ATTACHMENTS_FOLDER_ONLY_NOT_ODIA_NOT_YYYYMM + "\\" + objName + "\\" + DateTime.Now.ToString("yyyyMM");
            folderSaveDataBaseExt = folderExternal + "\\" + fileName.Substring(0, lastIndexOf) + "." + DateTime.Now.ToString("yyyyMMdd.HHmmss") + "." + fileName.Substring(lastIndexOf + 1);

        }


    }
}