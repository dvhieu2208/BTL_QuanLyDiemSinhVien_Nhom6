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
            if (string.IsNullOrWhiteSpace(taiKhoan) || string.IsNullOrWhiteSpace(matKhau))
            {
                return false;
            }

            return dal.KiemTraDangNhap(taiKhoan, matKhau);
        }
    }
}
