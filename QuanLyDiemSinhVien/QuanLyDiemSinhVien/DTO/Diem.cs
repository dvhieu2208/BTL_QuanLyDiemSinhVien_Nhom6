using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DTO
{
    public class Diem : IXepLoai, IKiemTraHopLe
    {
        public string MaSV { get; set; }
        public string MaLop { get; set; }
        public string MaMon { get; set; }
        public int HocKy { get; set; }
        public int LanThi { get; set; }
        public decimal DiemSo { get; set; }

        public string XepLoai()
        {
            return XepLoaiTheoDiem(DiemSo);
        }
        public static string XepLoaiTheoDiem(decimal d)
        {
            if (d >= 9) return "Xuất sắc";
            if (d >= 8) return "Giỏi";
            if (d >= 7) return "Khá";
            if (d >= 5) return "Trung bình";
            if (d >= 4) return "Yếu";
            return "Kém";
        }

        public bool KiemTra(out string loi)
        {
            if (string.IsNullOrWhiteSpace(MaSV) || string.IsNullOrWhiteSpace(MaLop) || string.IsNullOrWhiteSpace(MaMon))
            { loi = "Thiếu mã sinh viên / lớp / môn"; return false; }
            if (DiemSo < 0 || DiemSo > 10) { loi = "Điểm phải từ 0 đến 10"; return false; }
            if (HocKy < 1) { loi = "Học kỳ không hợp lệ"; return false; }
            if (LanThi < 1 || LanThi > 3) { loi = "Lần thi phải từ 1 đến 3"; return false; }
            loi = null; return true;
        }
    }
}
