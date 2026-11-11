using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DTO
{
    public class LichHoc : Interfaces.IKiemTraHopLe
    {
        public string MaLop { get; set; }
        public string MaMon { get; set; }
        public int HocKy { get; set; }
        public int ThuHoc { get; set; }
        public int CaHoc { get; set; }
        public string MaPhong { get; set; }

        public bool KiemTra(out string loi)
        {
            if (string.IsNullOrWhiteSpace(MaLop)) { loi = "Chưa chọn lớp"; return false; }
            if (string.IsNullOrWhiteSpace(MaMon)) { loi = "Chưa chọn môn học"; return false; }
            if (HocKy < 1 || HocKy > 10) { loi = "Học kỳ phải từ 1 đến 10"; return false; }
            if (ThuHoc < 2 || ThuHoc > 8) { loi = "Thứ học phải từ 2 đến 8 (Chủ nhật)"; return false; }
            if (CaHoc < 1 || CaHoc > 4) { loi = "Ca học phải từ 1 đến 4"; return false; }
            if (string.IsNullOrWhiteSpace(MaPhong)) { loi = "Chưa chọn phòng học"; return false; }
            loi = null; return true;
        }
    }
 }
