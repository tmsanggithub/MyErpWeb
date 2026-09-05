using WebRunDragon.Config;
using WebRunDragon.DataProcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebRunDragon.Utils
{
    public static class OtherUtils
    {
        private static DataTable _dtIssueType = null;
        private static DataTable _dtPriority = null;
        private static DataTable _dtProgressPercent = null;
        private static DataTable _dtStaffOfITDepartment = null;
        private static DataTable _dtIssueProcessIssueStatus = null;
        private static DataTable _dtListConfirmType = null;
        private static DataTable _dtStatementFormType = null;
        private static DataTable _dtDepartmentFromStatementFormType = null;

        //public static DataTable GetStaffInDepartmentByCode(string departmetId)
        //{
        //    return ProcessUser.GetStaffInDepartmentByCode(departmetId);
        //}

        //public static DataTable GetAllStaff()
        //{
        //    return ProcessUser.GetAllStaff();
        //}
        public static DataTable GetListProgressPercent()
        {
            if (_dtProgressPercent == null)
            {
                _dtProgressPercent = MakeListProgressPercent();
            }
            return _dtProgressPercent;
        }
        private static DataTable MakeListProgressPercent()
        {
            DataTable retDt = new DataTable();
            retDt.Columns.Add("Id", typeof(int));
            retDt.Columns.Add("Name", typeof(string));

            DataRow radd10 = retDt.NewRow();
            radd10["Id"] = 100;
            radd10["Name"] = "100 %";
            retDt.Rows.Add(radd10);

            DataRow radd9 = retDt.NewRow();
            radd9["Id"] = 90;
            radd9["Name"] = "90 %";
            retDt.Rows.Add(radd9);

            DataRow radd8 = retDt.NewRow();
            radd8["Id"] = 80;
            radd8["Name"] = "80 %";
            retDt.Rows.Add(radd8);

            DataRow radd7 = retDt.NewRow();
            radd7["Id"] = 70;
            radd7["Name"] = "70 %";
            retDt.Rows.Add(radd7);

            DataRow radd6 = retDt.NewRow();
            radd6["Id"] = 60;
            radd6["Name"] = "60%";
            retDt.Rows.Add(radd6);

            DataRow radd5 = retDt.NewRow();
            radd5["Id"] = 50;
            radd5["Name"] = "50%";
            retDt.Rows.Add(radd5);

            DataRow radd4 = retDt.NewRow();
            radd4["Id"] = 40;
            radd4["Name"] = "40 %";
            retDt.Rows.Add(radd4);

            DataRow radd3 = retDt.NewRow();
            radd3["Id"] = 30;
            radd3["Name"] = "30 %";
            retDt.Rows.Add(radd3);

            DataRow radd2 = retDt.NewRow();
            radd2["Id"] = 20;
            radd2["Name"] = "20 %";
            retDt.Rows.Add(radd2);

            DataRow radd1 = retDt.NewRow();
            radd1["Id"] = 10;
            radd1["Name"] = "10 %";
            retDt.Rows.Add(radd1);


            return retDt;
        }

        public static string ToJsonString(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}