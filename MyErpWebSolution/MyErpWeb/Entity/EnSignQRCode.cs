using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRunDragon.Entity
{
    public class QRSignContentBase
    {
        public DateTime SignedDate { get; set; }
        public string Checksum { get; set; }
        public string RefId { get; set; }
    }

    public class QRSignDocument : QRSignContentBase
    {
        public string LosId { get; set; }
    }

    public class SimpleQRSignContent
    {
        public DateTime SignedDate { get; set; }
        public string UserName { get; set; }
        public string StaffCode { get; set; }
    }

    public class QRSignRQ
    {

        public string SourceFile { get; set; }

        public QRSignContentBase SignContent { get; set; }

        public List<QRLocation> Locations { get; set; }
    }

    public class QRSignRS
    {
        public string FilePath { get; set; }
    }

    public class QRLocation
    {

        public int X { get; set; }

        public int Y { get; set; }

        public QRPosType Type { get; set; }
        public int? Height { get; set; }
        public string Pages { get; set; }
    }

    public enum QRPosType
    {
        Horizontal,
        FullSize
    }
    public class SharedStorage
    {
        public string AuthentUser { get; set; }
        public string AuthentPass { get; set; }
        public string AuthenDomain { get; set; }
        public string Path { get; set; }
    }


    public class ESFileSign
    {
        public decimal id { get; set; }
        public string ref_id { get; set; }
        public string rq_signed_data { get; set; }
        public string file_org_checksum { get; set; }
        public string file_sign_checksum { get; set; }
        public string path_org { get; set; }
        public string path_sign { get; set; }
        public DateTime signed_at { get; set; }
        public string signed_by { get; set; }
        public string status { get; set; }
        public string updated_by { get; set; }
        public string created_by { get; set; }
        public DateTime? updated_date { get; set; }
        public DateTime created_date { get; set; }
    }

}