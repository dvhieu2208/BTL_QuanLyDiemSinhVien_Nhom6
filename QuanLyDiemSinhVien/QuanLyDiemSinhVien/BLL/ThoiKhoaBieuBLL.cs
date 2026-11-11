using QuanLyDiemSinhVien.DAL;
using QuanLyDiemSinhVien.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class ThoiKhoaBieuBLL
    {
        private readonly ThoiKhoaBieuDAL dal = new ThoiKhoaBieuDAL();

        public DataTable LayDanhSach(int? hocKy, string maLop, string maMon, string tuKhoa)
        {
            return dal.LayDanhSach(hocKy, maLop, maMon, tuKhoa);
        }

        public DataTable LayMonTheoLopHocKy(string maLop, int hocKy) { return dal.LayMonTheoLopHocKy(maLop, hocKy); }
        public DataTable LayHocKyCuaLop(string maLop) { return dal.LayHocKyCuaLop(maLop); }

        public bool Them(LichHoc t, out string loi) { return Luu(t, true, out loi); }
        public bool Sua(LichHoc t, out string loi) { return Luu(t, false, out loi); }

        private bool Luu(LichHoc t, bool themMoi, out string loi)
        {
            if (!t.KiemTra(out loi)) return false;
            try
            {
                bool ok = themMoi ? dal.Them(t) : dal.Sua(t);
                loi = ok ? null : "Không lưu được thời khóa biểu";
                return ok;
            }
            catch (SqlException ex)
            {
                loi = XuLyLoi.DichLoiSql(ex);
                return false;
            }
        }

        public bool Xoa(string maLop, string maMon, out string loi)
        {
            try
            {
                bool ok = dal.Xoa(maLop, maMon);
                loi = ok ? null : "Không tìm thấy lớp học phần cần xóa";
                return ok;
            }
            catch (SqlException ex)
            {
                loi = XuLyLoi.DichLoiSql(ex);
                return false;
            }
        }
    }
}
