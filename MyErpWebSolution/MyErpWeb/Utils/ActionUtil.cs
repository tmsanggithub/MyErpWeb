using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRunDragon.Utils
{
    public class ActionUtil
    {
        public static bool CanRead(string userId, string actionName)
        {
            if (actionName == "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanRead > 0;
            
        }

        public static bool CanCreate(string userId, string actionName)
        {
            if (actionName == "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanCreate > 0;
            
        }

        public static bool CanEdit(string userId, string actionName)
        {
            if (actionName == "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            if (actionName == "ql_activities".ToUpper()) { actionName = "DMHoatDong".ToUpper(); }
            if (actionName == "app_user_registed".ToUpper()) { actionName = "DMVanDongVien".ToUpper(); }


            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanEdit > 0;
            
        }

        public static bool CanDelete(string userId, string actionName)
        {
            if (actionName== "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanDelete > 0;
            
        }

        public static bool CanSpecialAction(string userId, string actionName)
        {
            if (actionName == "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanSpecial > 0;
        }

        public static bool CanPrint(string userId, string actionName)
        {
            if (actionName == "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanPrint > 0;
            
        }

        public static bool CanApprove(string userId, string actionName)
        {
            if (actionName == "dm_run_group".ToUpper()) { actionName = "DMNhomChay".ToUpper(); }
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            if (xxx == null) return false;
            Entity.EnUserInfo staff = (Entity.EnUserInfo)xxx;

            if (staff.ListAccessRight == null) return false;
            if (!staff.ListAccessRight.ContainsKey(actionName)) return false;
            Entity.EnAccessRightInfo access = staff.ListAccessRight[actionName];
            return access.CanApprove > 0;
            
        }

        public static string GetUrl4FirstLoginSuccess(string lastUrl, Dictionary<string, Entity.EnAccessRightInfo> listAccessRight)
        {
            string theFirstLeftMenu = "";
            foreach (KeyValuePair<string, Entity.EnAccessRightInfo> entry in listAccessRight)
            {
                if (entry.Value.ShowMenuLeft > 0 && entry.Value.CanRead > 0)
                {
                    string tmpUrl = lastUrl.Substring(lastUrl.IndexOf("/") + 1);
                    if (entry.Value.FunctionUrl.Substring(entry.Value.FunctionUrl.IndexOf("/") + 1) == tmpUrl && !Utils.StringUtil.IsNullOrBlank(lastUrl))
                        return lastUrl;

                    if (theFirstLeftMenu == "")
                        theFirstLeftMenu = entry.Value.FunctionUrl;
                }
            }
            return theFirstLeftMenu;
        }

        public static bool ObjStatusCanUpdate(string objStatus)
        {
            return (objStatus == Config.SysConfig.OBJ_STATUS_NEW || objStatus == Config.SysConfig.OBJ_STATUS_RETURNED);
        }

        public static bool ObjStatusCanSendApprove(string objStatus)
        {
            return (objStatus == Config.SysConfig.OBJ_STATUS_NEW || objStatus == Config.SysConfig.OBJ_STATUS_RETURNED);
        }

    }
}