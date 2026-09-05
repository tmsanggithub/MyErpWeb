using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.DataProcess
{
    public class ProcessCfgDataRightAccess
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ProcessCfgDataRightAccess));

        public static DataTable SearchDataAccessGroup(string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserId", userId);
            /*
            ALTER PROCEDURE [dbo].[SP_cfgDataAccessGroups_Search]
                @UserId nvarchar(25)
                as
             */
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroups_Search", CommandType.StoredProcedure, param);
        }

        public static int DeleteDataAccessGroup(int id, string userId)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_cfgDataAccessGroups_Delete]
                @Id int,
                @UserId nvarchar(25)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("UserId", userId);
            int retCode = int.Parse(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroups_Delete", CommandType.StoredProcedure, param) + "");
            if (retCode > 0) return 0;
            return Math.Abs(retCode);
        }

        public static int AddOrUpdateDataAccessGroup(ref int id, string code, string groupName, string description, string userId)
        {
            try
            {
                /*
                    ALTER PROCEDURE [dbo].[SP_cfgDataAccessGroups_AddOrUpdate]
                        @Id int,
                        @Code nvarchar (10),
                        @GroupName nvarchar(250),
                        @Description nvarchar(500),
                        @UserId nvarchar(25)
                        as
                 */
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("Id", id);
                param.Add("Code", code);
                param.Add("GroupName", groupName);
                param.Add("Description", description);
                param.Add("UserId", userId);

                int retCode = int.Parse(DatabaseManager.ExecuteScalarSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroups_AddOrUpdate", CommandType.StoredProcedure, param) + "");
                if (retCode > 0)
                {
                    if (id == 0) id = retCode; // cập nhật id trong trường hợp insert
                    return 0; // ko có lỗi
                }
                return Math.Abs(retCode);

            }
            catch (Exception ex)
            {
                logger.Error("AddOrUpdateDataAccessGroup Error: ");
                logger.Error("Exception message: " + ex.Message + ". Stack trace: " + ex.StackTrace);
                logger.Error(ex);
            }
            return Config.SysConfig.DEFAULT_RETCODE;
        }

        public static JObject GetDataAccessGroupById(int id, string userId)
        {
            /*
             Create PROCEDURE [dbo].[SP_cfgDataAccessGroups_GetById]
                @Id int,
                @UserId nvarchar(25)
                as
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("Id", id);
            param.Add("UserId", userId);
            JArray array = DatabaseManager.GetJsonArraySql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroups_GetById", CommandType.StoredProcedure, param);
            if (array != null && array.Count > 0)
                return (JObject)array[0];
            return null;
        }

        public static DataTable GetPrivilegeStaffByAccessGroup(int accessId, string userId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("AccessId", accessId);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroupDetails_GetStaffByAccessGroup", CommandType.StoredProcedure, param);

        }

        public static DataTable GetUnPrivilegeStaffByAccessGroup(int accessId, string filterName, string userId)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_cfgDataAccessGroupDetails_GetUnPrivilegeStaffByAccessGroup]
                @AccessId int,
                @FilterName nvarchar(25),
                @UserId nvarchar(25)
             */
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("AccessId", accessId);
            param.Add("FilterName", filterName);
            param.Add("UserId", userId);
            return DatabaseManager.GetDataTableSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroupDetails_GetUnPrivilegeStaffByAccessGroup", CommandType.StoredProcedure, param);

        }

        public static int AddStaff(string staffId, int accessId, string userId)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_cfgDataAccessGroupDetails_Add]
                @StaffId nvarchar(25),
                @AccessId int,
                @UserId nvarchar(25)
                as
             */

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("StaffId", staffId);
            param.Add("AccessId", accessId);
            param.Add("UserId", userId);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroupDetails_Add", CommandType.StoredProcedure, param);
        }

        public static int RemoveStaff(string staffId, int accessId, string userId)
        {
            /*
             ALTER PROCEDURE [dbo].[SP_cfgDataAccessGroupDetails_Delete]
                @StaffId nvarchar(25),
                @AccessId int,
                @UserId nvarchar(25)
                as
             */

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("StaffId", staffId);
            param.Add("AccessId", accessId);
            param.Add("UserId", userId);
            return DatabaseManager.ExecuteUpdateSql(DatabaseManager.CNN_STRING_HELPDESK, "SP_cfgDataAccessGroupDetails_Delete", CommandType.StoredProcedure, param);
        }

    }
}