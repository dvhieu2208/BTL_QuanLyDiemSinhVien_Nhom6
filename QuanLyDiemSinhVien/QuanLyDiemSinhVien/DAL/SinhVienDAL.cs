using QuanLyDiemSinhVien.DTO;
using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.DAL
{
    internal class SinhVienDAL : IRepository<SinhVien>, ITimKiem<SinhVien>
    {
        private const string SQL_CHON = @"SELECT MaSV, TenSV, MaKhoa, MaLop, NgaySinh, GioiTinh, MaQue,
                                                 MaDanToc, MaChuyenNganh, MaHeDT, MaChucVu FROM SinhVien";

        private static string Chuoi(DataRow r, string cot)
        {
            return r[cot] == DBNull.Value ? null : r[cot].ToString();
        }

        private static SinhVien TaoDoiTuong(DataRow r)
        {
            return new SinhVien
            {
                Ma = Chuoi(r, "MaSV"),
                HoTen = Chuoi(r, "TenSV"),
                MaKhoa = Chuoi(r, "MaKhoa"),
                MaLop = Chuoi(r, "MaLop"),
                NgaySinh = r["NgaySinh"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["NgaySinh"]),
                GioiTinh = Chuoi(r, "GioiTinh"),
                MaQue = Chuoi(r, "MaQue"),
                MaDanToc = Chuoi(r, "MaDanToc"),
                MaChuyenNganh = Chuoi(r, "MaChuyenNganh"),
                MaHeDT = Chuoi(r, "MaHeDT"),
                MaChucVu = Chuoi(r, "MaChucVu")
            };
        }

        private static SqlParameter[] ThamSo(SinhVien sv)
        {
            return new[]
            {
                DBConnection.ThamSo("@MaSV", sv.Ma),
                DBConnection.ThamSo("@TenSV", sv.HoTen),
                DBConnection.ThamSo("@MaKhoa", sv.MaKhoa),
                DBConnection.ThamSo("@MaLop", sv.MaLop),
                DBConnection.ThamSo("@NgaySinh", sv.NgaySinh),
                DBConnection.ThamSo("@GioiTinh", sv.GioiTinh),
                DBConnection.ThamSo("@MaQue", sv.MaQue),
                DBConnection.ThamSo("@MaDanToc", sv.MaDanToc),
                DBConnection.ThamSo("@MaChuyenNganh", sv.MaChuyenNganh),
                DBConnection.ThamSo("@MaHeDT", sv.MaHeDT),
                DBConnection.ThamSo("@MaChucVu", sv.MaChucVu)
            };
        }

        // ===== IRepository (truy vấn trực tiếp cho SELECT, thủ tục cho thêm/sửa/xóa) =====
        public List<SinhVien> GetAll()
        {
            var ds = new List<SinhVien>();
            foreach (DataRow r in DBConnection.LayBang(SQL_CHON + " ORDER BY MaSV").Rows)
                ds.Add(TaoDoiTuong(r));
            return ds;
        }

        public SinhVien GetById(string id)
        {
            DataTable dt = DBConnection.LayBang(SQL_CHON + " WHERE MaSV = @MaSV", new SqlParameter("@MaSV", id));
            return dt.Rows.Count == 0 ? null : TaoDoiTuong(dt.Rows[0]);
        }

        public bool Add(SinhVien entity)
        {
            return DBConnection.ThucThiProc("sp_SinhVien_Them", ThamSo(entity));
        }

        public bool Update(SinhVien entity)
        {
            return DBConnection.ThucThiProc("sp_SinhVien_Sua", ThamSo(entity));
        }

        public bool Delete(string id)
        {
            return DBConnection.ThucThiProc("sp_SinhVien_Xoa", new SqlParameter("@MaSV", id));
        }

        // ===== ITimKiem: tìm theo mã hoặc tên (truy vấn trực tiếp) =====
        public List<SinhVien> TimKiem(string tuKhoa)
        {
            var ds = new List<SinhVien>();
            DataTable dt = DBConnection.LayBang(SQL_CHON + " WHERE MaSV LIKE @k OR TenSV LIKE @k ORDER BY MaSV",
                new SqlParameter("@k", "%" + (tuKhoa ?? "") + "%"));
            foreach (DataRow r in dt.Rows) ds.Add(TaoDoiTuong(r));
            return ds;
        }

        // ===== Thủ tục: lọc / tìm kiếm nâng cao (yêu cầu 3, 6) =====
        public DataTable TimKiemNangCao(string maKhoa, string maLop, string maChuyenNganh, string maQue, string tuKhoa)
        {
            return DBConnection.LayBangProc("sp_SinhVien_TimKiem",
                DBConnection.ThamSo("@MaKhoa", maKhoa),
                DBConnection.ThamSo("@MaLop", maLop),
                DBConnection.ThamSo("@MaChuyenNganh", maChuyenNganh),
                DBConnection.ThamSo("@MaQue", maQue),
                DBConnection.ThamSo("@TuKhoa", tuKhoa));
        }

        public DataTable LayThongTin(string maSV)
        {
            return DBConnection.LayBangProc("sp_SinhVien_LayThongTin", new SqlParameter("@MaSV", maSV));
        }
    }
}
