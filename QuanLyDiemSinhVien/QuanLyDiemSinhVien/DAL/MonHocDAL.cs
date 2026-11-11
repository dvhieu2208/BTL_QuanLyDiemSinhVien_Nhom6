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
    internal class MonHocDAL : IRepository<MonHoc>, ITimKiem<MonHoc>
    {
        private static MonHoc TaoDoiTuong(DataRow r)
        {
            return new MonHoc
            {
                MaMon = r["MaMon"].ToString(),
                TenMon = r["TenMon"].ToString(),
                DVHT = r["DVHT"] == DBNull.Value ? 0 : Convert.ToInt32(r["DVHT"])
            };
        }

        private static List<MonHoc> ChuyenDoi(DataTable dt)
        {
            var ds = new List<MonHoc>();
            foreach (DataRow r in dt.Rows) ds.Add(TaoDoiTuong(r));
            return ds;
        }

        public List<MonHoc> GetAll()
        {
            return ChuyenDoi(DBConnection.LayBang("SELECT MaMon, TenMon, DVHT FROM MonHoc ORDER BY MaMon"));
        }

        public MonHoc GetById(string id)
        {
            DataTable dt = DBConnection.LayBang("SELECT MaMon, TenMon, DVHT FROM MonHoc WHERE MaMon = @MaMon",
                new SqlParameter("@MaMon", id));
            return dt.Rows.Count == 0 ? null : TaoDoiTuong(dt.Rows[0]);
        }

        private static SqlParameter[] ThamSo(MonHoc m)
        {
            return new[]
            {
                DBConnection.ThamSo("@MaMon", m.MaMon),
                DBConnection.ThamSo("@TenMon", m.TenMon),
                DBConnection.ThamSo("@DVHT", m.DVHT)
            };
        }

        public bool Add(MonHoc entity) { return DBConnection.ThucThiProc("sp_MonHoc_Them", ThamSo(entity)); }
        public bool Update(MonHoc entity) { return DBConnection.ThucThiProc("sp_MonHoc_Sua", ThamSo(entity)); }
        public bool Delete(string id) { return DBConnection.ThucThiProc("sp_MonHoc_Xoa", new SqlParameter("@MaMon", id)); }

        public List<MonHoc> TimKiem(string tuKhoa)
        {
            return ChuyenDoi(DBConnection.LayBang(
                "SELECT MaMon, TenMon, DVHT FROM MonHoc WHERE MaMon LIKE @k OR TenMon LIKE @k ORDER BY MaMon",
                new SqlParameter("@k", "%" + (tuKhoa ?? "") + "%")));
        }

        /// <summary>Danh sách môn kèm số lớp đang học môn đó (truy vấn trực tiếp).</summary>
        public DataTable LayDanhSach(string tuKhoa, int? dvht)
        {
            return DBConnection.LayBang(@"
                SELECT m.MaMon, m.TenMon, m.DVHT,
                       (SELECT COUNT(*) FROM ThoiKhoaBieu t WHERE t.MaMon = m.MaMon) AS SoLop
                FROM MonHoc m
                WHERE (m.MaMon LIKE @k OR m.TenMon LIKE @k)
                  AND (@dv IS NULL OR m.DVHT = @dv)
                ORDER BY m.MaMon",
                new SqlParameter("@k", "%" + (tuKhoa ?? "") + "%"),
                DBConnection.ThamSo("@dv", dvht));
        }
    }
}
