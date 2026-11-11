using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class DiemDAL
    {
        public DataTable LayDanhSachNhap(string maLop, int hocKy, string maMon, int lanThi)
        {
            return DBConnection.LayBangProc("sp_Diem_LayDanhSachNhap",
                DBConnection.ThamSo("@MaLop", maLop),
                DBConnection.ThamSo("@HocKy", hocKy),
                DBConnection.ThamSo("@MaMon", maMon),
                DBConnection.ThamSo("@LanThi", lanThi));
        }

        public bool Luu(DTO.Diem d)
        {
            return DBConnection.ThucThiProc("sp_Diem_Luu",
                DBConnection.ThamSo("@MaSV", d.MaSV),
                DBConnection.ThamSo("@MaLop", d.MaLop),
                DBConnection.ThamSo("@MaMon", d.MaMon),
                DBConnection.ThamSo("@HocKy", d.HocKy),
                DBConnection.ThamSo("@LanThi", d.LanThi),
                DBConnection.ThamSo("@Diem", d.DiemSo));
        }

        public bool Xoa(string maSV, string maLop, string maMon, int lanThi)
        {
            return DBConnection.ThucThiProc("sp_Diem_Xoa",
                new SqlParameter("@MaSV", maSV), new SqlParameter("@MaLop", maLop),
                new SqlParameter("@MaMon", maMon), DBConnection.ThamSo("@LanThi", lanThi));
        }
    }
}
