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
    internal class MonHocBLL : QuanLyCoBanBLL<MonHoc>
    {
        private readonly MonHocDAL dal;

        public MonHocBLL() : this(new MonHocDAL()) { }

        private MonHocBLL(MonHocDAL d) : base(d)
        {
            dal = d;
        }

        protected override bool KiemTraNghiepVu(MonHoc m, bool themMoi, out string loi)
        {
            if (themMoi && dal.GetById(m.MaMon) != null) { loi = "Mã môn đã tồn tại."; return false; }
            if (m.DVHT > 10) { loi = "Số đơn vị học trình tối đa là 10."; return false; }
            loi = null;
            return true;
        }

        public DataTable LayDanhSach(string tuKhoa, int? dvht) { return dal.LayDanhSach(tuKhoa, dvht); }
    }
}
