using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DTO
{
    public class Khoa : Interfaces.IKiemTraHopLe
    {
        public string MaKhoa { get; set; }
        public string TenKhoa { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string Website { get; set; }
        public bool KiemTra(out string loi)
        {
            if (string.IsNullOrWhiteSpace(MaKhoa))
            {
                loi = "Mã khoa không được để trống";
                return false;
            }
            if (MaKhoa.Length > 10)
            {
                loi = "Mã khoa tối đa 10 ký tự";
                return false;
            }
            if (string.IsNullOrWhiteSpace(TenKhoa))
            {
                loi = "Tên khoa không được để trống";
                return false;
            }
            loi = null;
            return true;
        }
    }
}
