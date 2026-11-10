using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DTO
{
    public class MonHoc : Interfaces.IKiemTraHopLe
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public int DVHT { get; set; }

        public bool KiemTra(out string loi)
        {
            if (string.IsNullOrWhiteSpace(MaMon))
            {
                loi = "Mã môn không được để trống";
                return false;
            }
            if (MaMon.Length > 10)
            {
                loi = "Mã môn tối đa 10 ký tự";
                return false;
            }
            if (string.IsNullOrWhiteSpace(TenMon))
            {
                loi = "Tên môn không được để trống";
                return false;
            }
            if (DVHT <= 0)
            {
                loi = "Đơn vị học trình phải là số dương";
                return false;
            }
            loi = null;
            return true;
        }
    }
}
