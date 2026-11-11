using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class BaoCaoDAL
    {
        public DataTable DiemTheoLopMon(string maLop, string maMon, int lanThi)
        {
            return DBConnection.LayBangProc("sp_BaoCao_DiemTheoLopMon",
                DBConnection.ThamSo("@MaLop", maLop),
                DBConnection.ThamSo("@MaMon", maMon),
                DBConnection.ThamSo("@LanThi", lanThi));
        }

        public DataTable BangDiemSinhVien(string maSV, int? hocKy)
        {
            return DBConnection.LayBangProc("sp_BaoCao_BangDiemSV",
                DBConnection.ThamSo("@MaSV", maSV),
                DBConnection.ThamSo("@HocKy", hocKy));
        }

        public DataTable ThoiKhoaBieu(string maLop, int hocKy)
        {
            return DBConnection.LayBangProc("sp_BaoCao_ThoiKhoaBieu",
                DBConnection.ThamSo("@MaLop", maLop),
                DBConnection.ThamSo("@HocKy", hocKy));
        }

        public DataTable ThongKeSVTheoKhoa()
        {
            return DBConnection.LayBangProc("sp_ThongKe_SVTheoKhoa");
        }

        public DataTable ThongKeXepLoai(string maLop)
        {
            return DBConnection.LayBangProc("sp_ThongKe_XepLoai", DBConnection.ThamSo("@MaLop", maLop));
        }

        public DataTable ThongKeDiemTBTheoMon(string maLop)
        {
            return DBConnection.LayBangProc("sp_ThongKe_DiemTBTheoMon", DBConnection.ThamSo("@MaLop", maLop));
        }
    }
}
