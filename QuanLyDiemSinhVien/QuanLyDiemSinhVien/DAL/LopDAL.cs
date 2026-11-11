using QuanLyDiemSinhVien.DTO;
using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class LopDAL : IRepository<Lop>
    {
        private static Lop TaoDoiTuong(DataRow r)
        {
            return new Lop
            {
                MaLop = r["MaLop"].ToString(),
                TenLop = r["TenLop"].ToString(),
                MaKhoa = r["MaKhoa"].ToString(),
                KhoaHoc = r["KhoaHoc"] == DBNull.Value ? "" : r["KhoaHoc"].ToString(),
                SiSo = Convert.ToInt32(r["SiSo"])
            };
        }

        public List<Lop> GetAll()
        {
            var ds = new List<Lop>();
            foreach (DataRow r in DBConnection.LayBang("SELECT * FROM Lop ORDER BY MaLop").Rows)
                ds.Add(TaoDoiTuong(r));
            return ds;
        }

        public Lop GetById(string id)
        {
            DataTable dt = DBConnection.LayBang("SELECT * FROM Lop WHERE MaLop = @MaLop", new SqlParameter("@MaLop", id));
            return dt.Rows.Count == 0 ? null : TaoDoiTuong(dt.Rows[0]);
        }

        public int LaySiSo(string maLop)
        {
            object kq = DBConnection.LayGiaTri("SELECT SiSo FROM Lop WHERE MaLop = @MaLop", new SqlParameter("@MaLop", maLop));
            return kq == null ? 0 : Convert.ToInt32(kq);
        }

        private static SqlParameter[] ThamSo(Lop l)
        {
            return new[]
            {
                DBConnection.ThamSo("@MaLop", l.MaLop),
                DBConnection.ThamSo("@TenLop", l.TenLop),
                DBConnection.ThamSo("@MaKhoa", l.MaKhoa),
                DBConnection.ThamSo("@KhoaHoc", l.KhoaHoc)
            };
        }

        public bool Add(Lop entity) { return DBConnection.ThucThiProc("sp_Lop_Them", ThamSo(entity)); }
        public bool Update(Lop entity) { return DBConnection.ThucThiProc("sp_Lop_Sua", ThamSo(entity)); }
        public bool Delete(string id) { return DBConnection.ThucThiProc("sp_Lop_Xoa", new SqlParameter("@MaLop", id)); }

        public DataTable LayDanhSach(string maKhoa)
        {
            return DBConnection.LayBangProc("sp_Lop_LayTheoKhoa", DBConnection.ThamSo("@MaKhoa", maKhoa));
        }
    }
}
