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
    internal class LopBLL : QuanLyCoBanBLL<Lop>
    {
        private readonly LopDAL dal;

        public LopBLL() : this(new LopDAL()) { }

        private LopBLL(LopDAL d) : base(d)
        {
            dal = d;
        }

        protected override bool KiemTraNghiepVu(Lop lop, bool themMoi, out string loi)
        {
            if (themMoi)
            {
                if (dal.GetById(lop.MaLop) != null) { loi = "Mã lớp đã tồn tại."; return false; }
                lop.SiSo = 0;
            }
            loi = null;
            return true;
        }

        public DataTable LayDanhSach(string maKhoa) { return dal.LayDanhSach(maKhoa); }
        public int LaySiSo(string maLop) { return dal.LaySiSo(maLop); }
    }
}
