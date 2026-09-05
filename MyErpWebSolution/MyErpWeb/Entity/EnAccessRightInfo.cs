using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRunDragon.Entity
{
    public class EnAccessRightInfo
    {
        private int functionId;
        private string code;
        private string functionName;
        private string functionUrl;
        private int showMenuLeft;
        private int sortOrder;
        private string moduleId;
        private string moduleName;

        private int canRead;
        private int canCreate;
        private int canEdit;
        private int canDelete;
        private int canApprove;
        private int canPrint;
        private int canSpecial;

        public int FunctionId { get { return functionId; } set { functionId = value; } }
        public string Code { get { return code; } set { code = value; } }
        public string FunctionName { get { return functionName; } set { functionName = value; } }
        public string FunctionUrl { get { return functionUrl; } set { functionUrl = value; } }
        public int ShowMenuLeft { get { return showMenuLeft; } set { showMenuLeft = value; } }
        public int SortOrder { get { return sortOrder; } set { sortOrder = value; } }
        public string ModuleId { get { return moduleId; } set { moduleId = value; } }
        public string ModuleName { get { return moduleName; } set { moduleName = value; } }

        public int CanRead { get { return canRead; } set { canRead = value; } }
        public int CanCreate { get { return canCreate; } set { canCreate = value; } }
        public int CanEdit { get { return canEdit; } set { canEdit = value; } }
        public int CanDelete { get { return canDelete; } set { canDelete = value; } }
        public int CanApprove { get { return canApprove; } set { canApprove = value; } }
        public int CanPrint { get { return canPrint; } set { canPrint = value; } }
        public int CanSpecial { get { return canSpecial; } set { canSpecial = value; } }
    }
}