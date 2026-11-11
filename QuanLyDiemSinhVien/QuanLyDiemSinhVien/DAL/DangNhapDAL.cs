using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class DangNhapDAL
    {
        public string LayMatKhau(string taiKhoan)
        {
            object kq = DBConnection.LayGiaTri(
                "SELECT MatKhau FROM TaiKhoan WHERE TenDangNhap = @TaiKhoan AND TrangThai = 1",
                new SqlParameter("@TaiKhoan", SqlDbType.VarChar, 50) { Value = taiKhoan });
            return kq == null ? null : kq.ToString();
        }

        public string LayCauHoiBaoMat(string taiKhoan)
        {
            object kq = DBConnection.LayGiaTri(
                "SELECT CauHoiBaoMat FROM TaiKhoan WHERE TenDangNhap = @TaiKhoan",
                new SqlParameter("@TaiKhoan", SqlDbType.VarChar, 50) { Value = taiKhoan });
            return kq == null ? null : kq.ToString();
        }

        public string LayDapAnBaoMat(string taiKhoan)
        {
            object kq = DBConnection.LayGiaTri(
                "SELECT DapAnBaoMat FROM TaiKhoan WHERE TenDangNhap = @TaiKhoan",
                new SqlParameter("@TaiKhoan", SqlDbType.VarChar, 50) { Value = taiKhoan });
            return kq == null ? null : kq.ToString();
        }

        public bool DoiMatKhau(string taiKhoan, string matKhauDaBam)
        {
            return DBConnection.ThucThi(
                "UPDATE TaiKhoan SET MatKhau = @MatKhau WHERE TenDangNhap = @TaiKhoan",
                new SqlParameter("@MatKhau", matKhauDaBam),
                new SqlParameter("@TaiKhoan", SqlDbType.VarChar, 50) { Value = taiKhoan });
        }
    }
}
