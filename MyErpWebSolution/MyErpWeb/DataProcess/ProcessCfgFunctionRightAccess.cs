using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessCfgFunctionRightAccess
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessCfgFunctionRightAccess));

        public static DataTable SearchFunctionRightAccessGroup(string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRoles_Search", CommandType.StoredProcedure, param);
        }

        public static int DeleteFunctionRightAccessGroup(int id, string userId)
        {
            /*
                ALTER PROCEDURE [dbo].[SP_cfgRoles_Delete]
                @Id int,
                @UserId nvarchar(25)
                as

             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("UserId", userId);
            int retCode = int.Parse(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRoles_Delete", CommandType.StoredProcedure, param) + "");
            if (retCode > 0) return 0;
            return Math.Abs(retCode);
        }

        public static int AddOrUpdateFunctionRightAccessGroup(ref int id, string code, string roleName, string description, string userId)
        {
            try
            {
                /*
                    ALTER PROCEDURE [dbo].[SP_cfgRoles_AddOrUpdate]
                        @Id int,
                        @Code nvarchar (10),
                        @RoleName nvarchar(250),
                        @Description nvarchar(500),
                        @UserId nvarchar(25)
                        as
                 */
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("Code", code);
                param.Add("RoleName", roleName);
                param.Add("Description", description);
                param.Add("UserId", userId);

                int retCode = int.Parse(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRoles_AddOrUpdate", CommandType.StoredProcedure, param) + "");
                if (retCode > 0)
                {
                    if (id == 0) id = retCode; // cập nhật id trong trường hợp insert
                    return 0; // ko có lỗi
                }
                return Math.Abs(retCode);

            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdateFunctionRightAccessGroup Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            return Config.SysConfig.DEFAULT_RETCODE;
        }

        public static JObject GetFunctionRightAccessGroupById(int id, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("UserId", userId);
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRoles_GetById", CommandType.StoredProcedure, param);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }

        #region Role detail

        public static DataTable GetFunctionList4Adding(int roleId, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("RoleId", roleId);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgFunctions_GetUnPrivilegeFunctionByRole", CommandType.StoredProcedure, param);
        }

        public static DataTable GetFunctionList4Editing(int roleId, int functionId, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("RoleId", roleId);
            param.Add("FunctionId", functionId);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgFunctions_GetUnPrivilegeFunctionByRole_4Editing", CommandType.StoredProcedure, param);
        }

        public static DataTable SearchRoleDetails(int roleId, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("RoleId", roleId);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRolesGroupDetail_Search", CommandType.StoredProcedure, param);
        }

        public static JObject GetRoleDetailById(int id, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("UserId", userId);
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRolesGroupDetail_GetById", CommandType.StoredProcedure, param);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }

        public static int AddOrUpdateRoleDetail(int id, int roleId, int functionId, int canRead, int canCreate,
            int canUpdate, int canDelete, int canPrint, int canApprove, int canSpecial, string userId)
        {

            try
            {
                /*
                    CREATE procedure [dbo].[SP_cfgFunctionAndRoles_AddOrUpdate]
                        @Id int,
                        @RoleId int,
                        @FunctionId int,
                        @CanRead int,
                        @CanCreate int,
                        @CanEdit int,
                        @CanDelete int,
                        @CanPrint int,
                        @CanApprove int,
                        @CanSpecial int,
                        @UserId int
                        as
                 */
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("RoleId", roleId);
                param.Add("FunctionId", functionId);
                param.Add("CanRead", canRead);
                param.Add("CanCreate", canCreate);
                param.Add("CanEdit", canUpdate);
                param.Add("CanDelete", canDelete);
                param.Add("CanPrint", canPrint);
                param.Add("CanApprove", canApprove);
                param.Add("CanSpecial", canSpecial);
                param.Add("UserId", userId);

                int retCode = int.Parse(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_CfgRolesGroupDetail_AddOrUpdate", CommandType.StoredProcedure, param) + "");
                if (retCode > 0)
                {
                    if (id == 0) id = retCode; // cập nhật id trong trường hợp insert
                    return 0; // ko có lỗi
                }
                return Math.Abs(retCode);

            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdateRoleDetail Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            return Config.SysConfig.DEFAULT_RETCODE;


        }

        public static bool DeleteRoleDetail(int id, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("UserId", userId);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgRolesGroupDetail_Delete", CommandType.StoredProcedure, param) > 0;
        }

        #endregion
    }
}