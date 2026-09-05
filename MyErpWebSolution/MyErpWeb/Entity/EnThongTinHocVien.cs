using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAsset.Entity
{
    public class EnThongTinHocVien
    {
        private int _iD;
        private string _maChiNhanh;
        private string _maHocVien;
        private string _hoVaTenHocVien;
        private int _nu0Nam1;
        private DateTime _ngaySinh;
        private string _diaChi;
        private string _email;
        private string _hoTenPhuHuynh;
        private string _ngheNghiep;
        private string _dienThoai;
        private string _ghiChu;
        private int _isDeleted;
        private string _createdBy;
        private string _updatedBy;
        private string _deletedBy;
        private DateTime _createdTime;
        private DateTime _updatedTime;
        private DateTime _deletedTime;

        public int ID { get { return _iD; } set { _iD = value; } }
        public string MaChiNhanh { get { return _maChiNhanh; } set { _maChiNhanh = value; } }
        public string MaHocVien { get { return _maHocVien; } set { _maHocVien = value; } }
        public string HoVaTenHocVien { get { return _hoVaTenHocVien; } set { _hoVaTenHocVien = value; } }
        public int Nu0Nam1 { get { return _nu0Nam1; } set { _nu0Nam1 = value; } }
        public DateTime NgaySinh { get { return _ngaySinh; } set { _ngaySinh = value; } }
        public string DiaChi { get { return _diaChi; } set { _diaChi = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public string HoTenPhuHuynh { get { return _hoTenPhuHuynh; } set { _hoTenPhuHuynh = value; } }
        public string NgheNghiep { get { return _ngheNghiep; } set { _ngheNghiep = value; } }
        public string DienThoai { get { return _dienThoai; } set { _dienThoai = value; } }
        public string GhiChu { get { return _ghiChu; } set { _ghiChu = value; } }
        public int IsDeleted { get { return _isDeleted; } set { _isDeleted = value; } }
        public string CreatedBy { get { return _createdBy; } set { _createdBy = value; } }
        public string UpdatedBy { get { return _updatedBy; } set { _updatedBy = value; } }
        public string DeletedBy { get { return _deletedBy; } set { _deletedBy = value; } }
        public DateTime CreatedTime { get { return _createdTime; } set { _createdTime = value; } }
        public DateTime UpdatedTime { get { return _updatedTime; } set { _updatedTime = value; } }
        public DateTime DeletedTime { get { return _deletedTime; } set { _deletedTime = value; } }
    }
}