using QuanLyDiemSinhVien.DAL;
using QuanLyDiemSinhVien.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class SinhVienBLL : QuanLyCoBanBLL<SinhVien>
    {
        private readonly SinhVienDAL dal;

        public SinhVienBLL() : this(new SinhVienDAL()) { }

        private SinhVienBLL(SinhVienDAL d) : base(d)
        {
            dal = d;
        }

        protected override bool KiemTraNghiepVu(SinhVien sv, bool themMoi, out string loi)
        {
            if (themMoi && dal.GetById(sv.Ma) != null)
            {
                loi = "Mã sinh viên " + sv.Ma + " đã tồn tại.";
                return false;
            }
            loi = null;
            return true;
        }

        public DataTable TimKiem(string maKhoa, string maLop, string maChuyenNganh, string maQue, string tuKhoa)
        {
            return dal.TimKiemNangCao(maKhoa, maLop, maChuyenNganh, maQue, tuKhoa);
        }

        public DataTable LayTheoLop(string maLop)
        {
            return dal.TimKiemNangCao(null, maLop, null, null, null);
        }

        public DataTable LayThongTin(string maSV)
        {
            return dal.LayThongTin(maSV);
        }
    }
}
