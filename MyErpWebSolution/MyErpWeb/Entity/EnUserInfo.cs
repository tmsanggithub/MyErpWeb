using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.Entity
{
    public class EnUserInfo
    {
        private int id;
        private string staffNo; // mã nhân viên
        private string staffName;
        private string userName;
        private string email;

        private string deptCode;
        private string deptName;
        private string branchName;
        private string deptId;
        private string branchId;



        private int isLocked;
        private int failedLoginCounter;
        private DateTime lastLoginFailedTime;
        private DateTime lastLoginSuccessTime;
        private string lastLoginFailedFrom;
        private string lastLoginSuccessFrom;

        private Dictionary<string, Entity.EnAccessRightInfo> listAccessRight;
        private DataTable dtbStaffsInDept;
        private DataTable dtbStaffsIsDirectManagers;

        private string managerId;


        public virtual int ID { get { return id; } set { id = value; } }
        public virtual string StaffNo { get { return staffNo; } set { staffNo = value; } }
        public virtual string StaffName { get { return staffName; } set { staffName = value; } }
        public virtual string UserName { get { return userName; } set { userName= value; } }
        public virtual string Email { get { return email; } set { email = value; } }

        public virtual string DeptCode { get { return deptCode; } set { deptCode = value; } }
        public string DeptName { get { return deptName; } set { deptName = value; } }

        public virtual string BranchName { get { return branchName; } set { branchName = value; } }

        public virtual string DeptId { get { return deptId; } set { deptId = value; } }

        public virtual string BranchId { get { return branchId; } set { branchId = value; } }

        public virtual int IsLocked { get { return isLocked; } set { isLocked = value; } }
        public virtual int FailedLoginCounter { get { return failedLoginCounter; } set { failedLoginCounter = value; } }
        public virtual DateTime LastLoginFailedTime { get { return lastLoginFailedTime; } set { lastLoginFailedTime = value; } }
        public virtual DateTime LastLoginSuccessTime { get { return lastLoginSuccessTime; } set { lastLoginSuccessTime = value; } }
        public virtual string LastLoginFailedFrom { get { return lastLoginFailedFrom; } set { lastLoginFailedFrom = value; } }
        public virtual string LastLoginSuccessFrom { get { return lastLoginSuccessFrom; } set { lastLoginSuccessFrom = value; } }

        public virtual DataTable DtbStaffsInDept { get { return dtbStaffsInDept; } set { dtbStaffsInDept = value; } }

        public virtual DataTable DtbStaffsIsDirectManagers { get { return dtbStaffsIsDirectManagers; } set { dtbStaffsIsDirectManagers = value; } }

        public virtual Dictionary<string, Entity.EnAccessRightInfo> ListAccessRight { get { return listAccessRight; } set { listAccessRight = value; } }

        public virtual Dictionary<string, string> GetMenuGroups()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Entity.EnAccessRightInfo> kvp in listAccessRight)
            {
                if (!list.ContainsKey(kvp.Value.ModuleId) && !Utils.StringUtil.IsNullOrBlank(kvp.Value.ModuleId))
                    list.Add(kvp.Value.ModuleId, kvp.Value.ModuleName);
            }
            return list;
        }

        public virtual string ManagerId { get { return managerId; } set { managerId = value; } }
    }
}