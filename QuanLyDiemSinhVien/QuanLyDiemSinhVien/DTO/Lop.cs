using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DTO
{
    public class Lop : Interfaces.IKiemTraHopLe
    {
        public string MaLop { get; set; }
        public string TenLop { get; set; }
        public string MaKhoa { get; set; }
        public string KhoaHoc { get; set; }
        public int SiSo { get; set; }

        public bool KiemTra(out string loi)
        {
            if (string.IsNullOrWhiteSpace(MaLop))
            {
                loi = "Mã lớp không được để trống";
                return false;
            }
            if (MaLop.Length > 10)
            {
                loi = "Mã lớp tối đa 10 ký tự";
                return false;
            }
            if (string.IsNullOrWhiteSpace(TenLop))
            {
                loi = "Tên lớp không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(MaKhoa))
            {
                loi = "Mã khoa không được để trống";
                return false;
            }
            if (string.IsNullOrWhiteSpace(KhoaHoc))
            {
                loi = "Khóa học không được để trống";
                return false;
            }
            if (SiSo <= 0)
            {
                loi = "Sĩ số phải là số dương";
                return false;
            }
            loi = null;
            return true;
        }
    }
}
