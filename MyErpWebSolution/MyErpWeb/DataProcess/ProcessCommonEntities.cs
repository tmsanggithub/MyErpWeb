using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessCommonEntities
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessCommonEntities));
        static string className = typeof(ProcessCommonEntities).Name;
        private static ProcessCommonEntities _instance;
        public static ProcessCommonEntities getInstance()
        {
            if (_instance == null)
                _instance = new ProcessCommonEntities();
            return _instance;
        }
        public JObject GetEntityByID(int ID, string tableName)
        {
            return ProcessDanhMuc.getInstance().GetDanhMucByID(ID, tableName);
        }
        public bool DeleteEntityByIDCheckConstrainKey(int id, string tableName, string userLoginId, out string message)
        {
            return ProcessDanhMuc.getInstance().DeleteDanhMucByIDCheckConstrainKey(id, tableName, userLoginId, out message);
        }
        public bool DeleteEntityByIDNotCheckConstrainKey(int id, string tableName, string userLoginId, out string message)
        {
            return ProcessDanhMuc.getInstance().DeleteDanhMucByIDNotCheckConstrainKey(id, tableName, userLoginId, out message);
        }
    }
}