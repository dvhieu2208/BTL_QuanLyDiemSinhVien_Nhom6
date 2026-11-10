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
            if (DiemSo >= 9) return "Xuất sắc";
            if (DiemSo >= 8) return "Giỏi";
            if (DiemSo >= 7) return "Khá";
            if (DiemSo >= 5) return "Trung bình";
            return "Yếu";
        }

        public bool KiemTra(out string loi)
        {
            if (DiemSo < 0 || DiemSo > 10) { loi = "Điểm phải từ 0 đến 10"; return false; }
            loi = null; return true;
        }
    }
}
