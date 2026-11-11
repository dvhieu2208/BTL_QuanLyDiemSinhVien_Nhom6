using QuanLyDiemSinhVien.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class ThoiKhoaBieuDAL
    {
        private static SqlParameter[] ThamSo(LichHoc t)
        {
            return new[]
            {
                DBConnection.ThamSo("@MaLop", t.MaLop),
                DBConnection.ThamSo("@MaMon", t.MaMon),
                DBConnection.ThamSo("@HocKy", t.HocKy),
                DBConnection.ThamSo("@ThuHoc", t.ThuHoc),
                DBConnection.ThamSo("@CaHoc", t.CaHoc),
                DBConnection.ThamSo("@MaPhong", t.MaPhong)
            };
        }

        public DataTable LayDanhSach(int? hocKy, string maLop, string maMon, string tuKhoa, string maKhoa = null)
        {
            return DBConnection.LayBangProc("sp_TKB_LayDanhSach",
                DBConnection.ThamSo("@HocKy", hocKy),
                DBConnection.ThamSo("@MaLop", maLop),
                DBConnection.ThamSo("@MaMon", maMon),
                DBConnection.ThamSo("@MaKhoa", maKhoa),
                DBConnection.ThamSo("@TuKhoa", tuKhoa));
        }

        public bool Them(LichHoc t) { return DBConnection.ThucThiProc("sp_TKB_Them", ThamSo(t)); }
        public bool Sua(LichHoc t) { return DBConnection.ThucThiProc("sp_TKB_Sua", ThamSo(t)); }

        public bool Xoa(string maLop, string maMon)
        {
            return DBConnection.ThucThiProc("sp_TKB_Xoa",
                new SqlParameter("@MaLop", maLop), new SqlParameter("@MaMon", maMon));
        }

        /// <summary>Yêu cầu 5: các môn của lớp trong học kỳ theo thời khóa biểu.</summary>
        public DataTable LayMonTheoLopHocKy(string maLop, int hocKy)
        {
            return DBConnection.LayBangProc("sp_TKB_LayMonTheoLopHocKy",
                DBConnection.ThamSo("@MaLop", maLop), DBConnection.ThamSo("@HocKy", hocKy));
        }

        /// <summary>Các học kỳ mà lớp có lịch học (truy vấn trực tiếp).</summary>
        public DataTable LayHocKyCuaLop(string maLop)
        {
            return DBConnection.LayBang("SELECT DISTINCT HocKy FROM ThoiKhoaBieu WHERE MaLop = @MaLop ORDER BY HocKy",
                new SqlParameter("@MaLop", maLop));
        }
    }
}
