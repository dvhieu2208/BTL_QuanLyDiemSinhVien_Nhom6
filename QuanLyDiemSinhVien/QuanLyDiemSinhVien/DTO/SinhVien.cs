using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyDiemSinhVien.Interfaces;

namespace QuanLyDiemSinhVien.DTO
{
    public class SinhVien : Nguoi,IKiemTraHopLe
    {
        public string MaKhoa { get; set; }
        public string MaLop { get; set; }
        public string MaQue { get; set; }
        public string MaDanToc { get; set; }
        public string MaChuyenNganh { get; set; }
        public string MaHeDT { get; set; }
        public string MaChucVu { get; set; }

        public bool KiemTra(out string loi)
        {
            if (string.IsNullOrWhiteSpace(Ma)) { loi = "Mã sinh viên không được trống"; return false; }
            if (string.IsNullOrWhiteSpace(HoTen)) { loi = "Tên sinh viên không được trống"; return false; }
            loi = null;
            return true;
        }
    }
}
