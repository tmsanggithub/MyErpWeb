using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRunDragon.Utils
{
    public class NumberUtil
    {
        public static string FormatNumber(decimal number)
        {
            return number == 0 ? "0" : String.Format("{0:#,##0.###}", number);
        }
        public static decimal ParseToDecimal(object value)
        {
            decimal temp = 0;
            decimal.TryParse(value + "", out temp);
            return temp;
        }

        public static double ParseToDouble(object value)
        {
            double temp = 0;
            double.TryParse(value + "", out temp);
            return temp;
        }

        public static int ParseToInt(object value)
        {
            int temp = 0;
            int.TryParse(value + "", out temp);
            return temp;
        }

        public static DateTime ParseToDate(object value)
        {
            DateTime temp = Convert.ToDateTime("1900-01-01");
            temp = Convert.ToDateTime(value);
            if (temp.Year == 1)
                temp = Convert.ToDateTime("1900-01-01");
            return temp;
        }

        public static Boolean ParseToBool(object value)
        {
            bool temp = false;
            bool.TryParse(value + "", out temp);
            return temp;
        }


        public static string Read(string sNumber)
        {
            string result = "";
            decimal d = 0;
            decimal.TryParse(sNumber, out d);
            d = Math.Round(d);
            sNumber = d + "";
            int size = sNumber.Length;
            if (size > 0)
            {
                if (size <= 3)
                {
                    int number = Convert.ToInt16(sNumber);
                    result = readThree(number, 1, false);
                }
                else if (size <= 6)
                {
                    string sv1 = sNumber.Substring(size - 3, 3);
                    string sv2 = sNumber.Substring(0, size - 3);
                    int nv1 = Convert.ToInt16(sv1);
                    int nv2 = Convert.ToInt16(sv2);
                    bool odd = true;
                    if (nv2 == 0) { odd = false; }
                    result = readThree(nv2, 2, false) + readThree(nv1, 1, odd);
                }
                else if (size <= 9)
                {
                    string sv1 = sNumber.Substring(size - 3, 3);
                    string sv2 = sNumber.Substring(size - 6, 3);
                    string sv3 = sNumber.Substring(0, size - 6);
                    int nv1 = Convert.ToInt16(sv1);
                    int nv2 = Convert.ToInt16(sv2);
                    int nv3 = Convert.ToInt16(sv3);
                    bool odd1 = true;
                    bool odd2 = true;
                    if (nv3 == 0)
                    {
                        odd2 = false;
                        if (nv2 == 0)
                        {
                            odd1 = false;
                        }
                    }
                    result = readThree(nv3, 3, false) + readThree(nv2, 2, odd2) + readThree(nv1, 1, odd1);
                }
                else if (size <= 12)
                {
                    string sv1 = sNumber.Substring(size - 3, 3);
                    string sv2 = sNumber.Substring(size - 6, 3);
                    string sv3 = sNumber.Substring(size - 9, 3);
                    string sv4 = sNumber.Substring(0, size - 9);
                    int nv1 = Convert.ToInt16(sv1);
                    int nv2 = Convert.ToInt16(sv2);
                    int nv3 = Convert.ToInt16(sv3);
                    int nv4 = Convert.ToInt16(sv4);
                    bool odd1 = true;
                    bool odd2 = true;
                    bool odd3 = true;
                    if (nv4 == 0)
                    {
                        odd3 = false;
                        if (nv3 == 0)
                        {
                            odd2 = false;
                            if (nv2 == 0)
                            {
                                odd1 = false;
                            }
                        }
                    }
                    result = readThree(nv4, 4, false) + readThree(nv3, 3, odd3) + readThree(nv2, 2, odd2) + readThree(nv1, 1, odd1);
                }
                else if (size <= 15)
                {
                    string sv1 = sNumber.Substring(size - 3, 3);
                    string sv2 = sNumber.Substring(size - 6, 3);
                    string sv3 = sNumber.Substring(size - 9, 3);
                    string sv4 = sNumber.Substring(size - 12, 3);
                    string sv5 = sNumber.Substring(0, size - 12);
                    int nv1 = Convert.ToInt16(sv1);
                    int nv2 = Convert.ToInt16(sv2);
                    int nv3 = Convert.ToInt16(sv3);
                    int nv4 = Convert.ToInt16(sv4);
                    int nv5 = Convert.ToInt16(sv5);
                    bool odd1 = true;
                    bool odd2 = true;
                    bool odd3 = true;
                    bool odd4 = true;
                    if (nv5 == 0)
                    {
                        odd4 = false;
                        if (nv4 == 0)
                        {
                            odd3 = false;
                            if (nv3 == 0)
                            {
                                odd2 = false;
                                if (nv2 == 0)
                                {
                                    odd1 = false;
                                }
                            }
                        }
                    }
                    if (nv4 == 0)
                    {
                        result = readThree(nv5, 5, false) + " tỷ " + readThree(nv4, 4, odd4) + readThree(nv3, 3, odd3) + readThree(nv2, 2, odd2) + readThree(nv1, 1, odd1);
                    }
                    else
                    {
                        result = readThree(nv5, 5, false) + readThree(nv4, 4, odd4) + readThree(nv3, 3, odd3) + readThree(nv2, 2, odd2) + readThree(nv1, 1, odd1);
                    }
                }
            }
            if (result + "" != "")
                return result.Substring(0, 1).ToUpper() + result.Substring(1) + " đồng.";
            else
                return "";
        }
        //Đọc ba số	
        private static string readThree(int number, int flag, bool odd)
        {
            string result = "";
            int v1 = number / 100;
            int v2 = number % 100;
            string str0 = "";
            string str1 = "";
            string str2 = "";
            string str3 = "";
            string str4 = "";
            if (odd == true)
            {
                if (v2 < 10)
                {
                    str0 = " lẻ ";
                }
                else
                {
                    str0 = " không trăm ";
                }
            }
            switch (flag)
            {
                case 1:
                    str1 = " trăm";
                    str2 = " trăm lẻ " + readTwo(v2);
                    str3 = " trăm " + readTwo(v2);
                    break;
                case 2:
                    str1 = " trăm ngàn ";
                    str2 = " trăm lẻ " + readTwo(v2) + " ngàn ";
                    str3 = " trăm " + readTwo(v2) + " ngàn ";
                    str4 = " ngàn ";
                    break;
                case 3:
                    str1 = " trăm triệu ";
                    str2 = " trăm lẻ " + readTwo(v2) + " triệu ";
                    str3 = " trăm " + readTwo(v2) + " triệu ";
                    str4 = " triệu ";
                    break;
                case 4:
                    str1 = " trăm tỷ ";
                    str2 = " trăm lẻ " + readTwo(v2) + " tỷ ";
                    str3 = " trăm " + readTwo(v2) + " tỷ ";
                    str4 = " tỷ ";
                    break;
                case 5:
                    str1 = " trăm ngàn ";
                    str2 = " trăm lẻ " + readTwo(v2) + " ngàn ";
                    str3 = " trăm " + readTwo(v2) + " ngàn ";
                    str4 = " ngàn ";
                    break;
            }
            switch (v1)
            {
                case 0:
                    if (v2 != 0)
                    {
                        result = str0 + readTwo(number) + str4;
                    }
                    else
                    {
                        result = "";
                    }
                    break;
                default:
                    switch (v2)
                    {
                        case 0:
                            result = readTwo(v1) + str1;//trăm
                            break;
                        default:
                            if (v2 < 10)
                            {
                                result = readTwo(v1) + str2;//trăm lẻ	
                            }
                            else
                            {
                                result = readTwo(v1) + str3;//trăm 		
                            }
                            break;
                    }
                    break;
            }
            return result;
        }
        //Đọc hai số
        private static string readTwo(int number)
        {
            string result = "";

            int v1 = number / 10;//so hang chuc
            int v2 = number % 10;//so hang don vi
            switch (v1)
            {
                //neu nho hon 10 thi dung ham doc 1 so
                case 0:
                    result = readOne(number);
                    break;
                //neu 10-19
                case 1:
                    switch (v2)
                    {
                        case 0:
                            result = "mười";//mười
                            break;
                        case 5://truong hop dac biet(khong the muoi nam)
                            result = "mười lăm";//mười lăm
                            break;
                        default://cac truong hop khac thi muoi+doc 1 so
                            result = "mười " + readOne(v2);//mười
                            break;
                    }
                    break;
                //neu 20-99
                default:
                    switch (v2)
                    {
                        case 0://mod=0->chia het->20,30,40,60,70....,->doc1so + muoi
                            result = readOne(v1) + " mươi";//mươi
                            break;
                        case 1://mod=1(vd:31,41)->doc1so + muoimot
                            result = readOne(v1) + " mươi mốt";//mươi mốt
                            break;
                        case 5://truong hop dac biet(lam thay vi nam)
                            result = readOne(v1) + " mươi lăm";//mươi lăm
                            break;
                        default://cac truong hop con lai(vd:37)->doc1so + muoi +doc1so
                            result = readOne(v1) + " mươi " + readOne(v2);//mươi
                            break;
                    }
                    break;
            }
            return result;
        }
        //Đọc một số
        private static string readOne(int number)
        {
            string sn = "";

            switch (number)
            {
                case 0:
                    sn = "không";//không
                    break;
                case 1:
                    sn = "một";
                    break;
                case 2:
                    sn = "hai";
                    break;
                case 3:
                    sn = "ba";
                    break;
                case 4:
                    sn = "bốn";
                    break;
                case 5:
                    sn = "năm";
                    break;
                case 6:
                    sn = "sáu";
                    break;
                case 7:
                    sn = "bảy";
                    break;
                case 8:
                    sn = "tám";
                    break;
                case 9:
                    sn = "chín";
                    break;
            }
            return sn;
        }
        public string GetRamdon()
        {
            Random rd = new Random(3);
            return rd.ToString();
        }
    }
}