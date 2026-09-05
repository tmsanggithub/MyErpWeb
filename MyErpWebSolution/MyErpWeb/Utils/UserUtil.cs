using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebRunDragon.DataProcess;

namespace WebRunDragon.Utils
{
    public class UserUtil
    {
        public static Entity.EnUserInfo GetSessionStaff()
        {
            object xxx = HttpContext.Current.Session[Config.SysConfig.SESSION_USERINFO];
            return xxx != null ? (Entity.EnUserInfo)xxx : null;
        }

        public static string GetSessionUserFullName()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? GetSessionUserId() : staff.StaffName;
        }

        public static string GetSessionUserId()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? "" : staff.UserName;
        }

        public static DataTable GetSessionUserStaffInDept()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? null : staff.DtbStaffsInDept;
        }
        public static DataTable GetSessionUserStaffIsDirectManagers()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? null : staff.DtbStaffsIsDirectManagers;
        }
        public static DataTable GetSessionUserStaff()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? null : staff.DtbStaffsIsDirectManagers;
        }
        //public static DataTable GetAllStaff()
        //{
        //   return ProcessUser.GetAllStaff();
        //}
        public static string GetSessionDeptCodeOfUserLogin()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? "" : staff.DeptCode;
        }
        public static string GetSessionDeptIdOfUserLogin()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? "" : staff.DeptId;
        }
        public static string GetSessionBranchIdOfUserLogin()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? "" : staff.BranchId;
        }
        public static string GetSessionManagerIdOfUserLogin()
        {
            Entity.EnUserInfo staff = GetSessionStaff();
            return staff == null ? "" : staff.ManagerId;
        }
    }
}