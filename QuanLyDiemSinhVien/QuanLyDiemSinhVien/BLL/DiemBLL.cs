using QuanLyDiemSinhVien.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class DiemBLL
    {
        private readonly DiemDAL dal = new DiemDAL();

        public DataTable LayDanhSachNhap(string maLop, int hocKy, string maMon, int lanThi)
        {
            return dal.LayDanhSachNhap(maLop, hocKy, maMon, lanThi);
        }

        public int LuuDanhSach(IEnumerable<DTO.Diem> dsDiem, IEnumerable<string> dsXoa,
                               string maLop, string maMon, int lanThi, List<string> danhSachLoi)
        {
            int dem = 0;
            foreach (DTO.Diem d in dsDiem)
            {
                string loi;
                if (!d.KiemTra(out loi)) { danhSachLoi.Add(d.MaSV + ": " + loi); continue; }
                try
                {
                    if (dal.Luu(d)) dem++;
                }
                catch (SqlException ex)
                {
                    danhSachLoi.Add(d.MaSV + ": " + ex.Message);
                }
            }
            foreach (string maSV in dsXoa)
            {
                try { dal.Xoa(maSV, maLop, maMon, lanThi); }
                catch (SqlException ex) { danhSachLoi.Add(maSV + ": " + ex.Message); }
            }
            return dem;
        }

        public static bool DocDiem(object giaTri, out decimal? diem)
        {
            diem = null;
            if (giaTri == null || giaTri == DBNull.Value) return true;
            if (giaTri is decimal) { diem = (decimal)giaTri; return diem >= 0 && diem <= 10; }
            string s = giaTri.ToString().Trim().Replace(',', '.');
            if (s.Length == 0) return true;
            decimal d;
            if (!decimal.TryParse(s, System.Globalization.NumberStyles.Number,
                                  System.Globalization.CultureInfo.InvariantCulture, out d)) return false;
            if (d < 0 || d > 10) return false;
            diem = Math.Round(d, 2);
            return true;
        }
    }
}
