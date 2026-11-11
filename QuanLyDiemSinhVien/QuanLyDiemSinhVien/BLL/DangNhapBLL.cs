using QuanLyDiemSinhVien.DAL;
using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class DangNhapBLL : IXacThuc
    {
        private readonly DangNhapDAL dal = new DangNhapDAL();

        public bool DangNhap(string taiKhoan, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(taiKhoan) || string.IsNullOrEmpty(matKhau))
                return false;

            string storedHash = dal.LayMatKhau(taiKhoan);
            if (string.IsNullOrEmpty(storedHash))
                return false;

            bool ok = MatKhauHash.VerifyPassword(matKhau, storedHash);
            if (ok) PhienDangNhap.TenDangNhap = taiKhoan;
            return ok;
        }

        public string LayCauHoiBaoMat(string taiKhoan)
        {
            if (string.IsNullOrWhiteSpace(taiKhoan)) return null;
            return dal.LayCauHoiBaoMat(taiKhoan.Trim());
        }

        public bool DatLaiMatKhau(string taiKhoan, string dapAn, string matKhauMoi, string xacNhan, out string loi)
        {
            if (string.IsNullOrWhiteSpace(taiKhoan)) { loi = "Chưa nhập tên đăng nhập"; return false; }
            if (string.IsNullOrWhiteSpace(dapAn)) { loi = "Chưa nhập câu trả lời bảo mật"; return false; }
            if (string.IsNullOrEmpty(matKhauMoi) || matKhauMoi.Length < 6) { loi = "Mật khẩu mới phải có ít nhất 6 ký tự"; return false; }
            if (matKhauMoi != xacNhan) { loi = "Xác nhận mật khẩu không khớp"; return false; }

            string hashDapAn = dal.LayDapAnBaoMat(taiKhoan.Trim());
            if (string.IsNullOrEmpty(hashDapAn)) { loi = "Tài khoản không tồn tại hoặc chưa đặt câu hỏi bảo mật"; return false; }

            bool dung = MatKhauHash.VerifyPassword(dapAn.Trim(), hashDapAn)
                     || MatKhauHash.VerifyPassword(dapAn.Trim().ToUpper(), hashDapAn)
                     || MatKhauHash.VerifyPassword(dapAn.Trim().ToLower(), hashDapAn);
            if (!dung) { loi = "Câu trả lời bảo mật không đúng"; return false; }

            bool ok = dal.DoiMatKhau(taiKhoan.Trim(), MatKhauHash.HashPassword(matKhauMoi));
            loi = ok ? null : "Không đổi được mật khẩu";
            return ok;
        }
    }
    internal static class PhienDangNhap
    {
        public static string TenDangNhap { get; set; }
    }
}
