using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyDiemSinhVien.DAL;

namespace QuanLyDiemSinhVien.BLL
{
    internal class DangNhapBLL
    {
        private DangNhapDAL dal = new DangNhapDAL();

        public bool DangNhap(string taiKhoan, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(taiKhoan) ||
                string.IsNullOrEmpty(matKhau))
            {
                return false;
            }

            // Lấy mật khẩu đã băm từ database
            string storedHash = dal.LayMatKhau(taiKhoan);

            if (string.IsNullOrEmpty(storedHash))
                return false;

            // Xác thực bằng PBKDF2
            return MatKhauHash.VerifyPassword(
                matKhau,
                storedHash
            );
        }
    }
}
