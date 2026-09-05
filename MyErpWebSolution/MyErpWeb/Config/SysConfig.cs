using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebRunDragon.Config
{
    public class SysConfig
    {
        public static string DBTYPE_DB2 = "DB2";
        public static string DBTYPE_SQLSERVER = "SQLSERVER";

        public static string OBJ_STATUS_NEW = "NEW";
        public static string OBJ_STATUS_RETURNED = "RETURNED";

        public static string SESSION_USERINFO = "SESSION_USERINFO";
        public static string SESSION_BRANCHID = "SESSION_BRANCHID";

        public static string SESSION_LASTURL_BEFORE_AUTHEN = "SESSION_LASTURL_BEFORE_AUTHEN";

        public static string URL_ERROR_FORBIDDEN = "~/Forms/_Error_Forbidden.aspx";

        public static string WS_PWD_EXEC_AUTHEN_SERVICE = "Auth3n@2016";

        public static int DEFAULT_RETCODE = 99;

        public static bool ITHELPDESK_CNNSTRING_IS_ENCRYPTED = false;

        public static int DAYSPAN_FROMDATE = -7;

        public static DataTable DT_APPLICATIONS = null;//"SESSION_APPLICATIONS";
        public static DataTable DT_APPUSERGROUP = null;// "SESSION_APPUSERGROUP";
        public static DataTable DT_LISTITSTAFF = null;//"SESSION_LISTITSTAFF";
        public static DataTable DT_LISTOBJSTATUS = null;//"SESSION_OBJSTATUS";

        public static DataTable DT_DEPARTMENT = null;//"SESSION_LISTDEPARTMENT";
        public static DataTable DT_TEMPLATECREATUSER = null;//"SESSION_LISTTEMPLATECREATUSER";

        public static string USERDEPARTMENTCODE = "";
        // public static string USERBRANCHCODE = "";
        public static string DECRYPTED_PWD = "";

        public static string ITDepartmentCode = "CNTT";
        public static string DVTCDepartmentCode = "DVTC";
        public static string HCQTDepartmentCode = "HCQT";

        public static string ATTACHMENTS_FOLDER_CO_ODIA_NOT_YYYYMM = "";//    \AttachmentsFolder   =  Server.Mapath(  "~/" + ConfigurationManager.AppSettings["AttachmentsFolder"];)
        public static string ATTACHMENTS_RECYCLEBIN_FOLDER_CO_ODIA_NOT_YYYYMM = ""; //      \AttachmentsRecycleBinFolder 
        public static string ATTACHMENTS_FOLDER_ONLY_NOT_ODIA_NOT_YYYYMM = "";//    \AttachmentsFolder

        public static bool IS_BYPASS_PASSWORD = false;

        public const string ValueRead = "0";
        public const string ValueCreate = "1";
        public const string ValueEdit = "2";
        public const string ValueDelete = "3";
        public const string ValueApproval = "5";
        public const string ValueDeleteDetail = "60";
        public const string ValueSave = "10";

        public const string displayButton = "inline";
        public const string noDisplayButton = "none";

        public const string EnableApproval = "50";
        public const bool EnableTrue = true;
        public const bool EnableFalse = false;

    }
}