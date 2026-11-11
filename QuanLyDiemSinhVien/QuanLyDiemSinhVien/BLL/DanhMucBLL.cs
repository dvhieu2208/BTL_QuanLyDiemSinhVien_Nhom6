using QuanLyDiemSinhVien.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class DanhMucBLL
    {
        private readonly DanhMucDAL dal = new DanhMucDAL();

        public DataTable LayKhoa() { return dal.LayKhoa(); }
        public DataTable LayQue() { return dal.LayQue(); }
        public DataTable LayDanToc() { return dal.LayDanToc(); }
        public DataTable LayHeDaoTao() { return dal.LayHeDaoTao(); }
        public DataTable LayChucVu() { return dal.LayChucVu(); }
        public DataTable LayPhongHoc() { return dal.LayPhongHoc(); }
        public DataTable LayMonHoc() { return dal.LayMonHoc(); }
        public DataTable LayHocKy() { return dal.LayHocKy(); }
        public DataTable LayLopTheoKhoa(string maKhoa) { return dal.LayLopTheoKhoa(maKhoa); }
        public DataTable LayChuyenNganhTheoKhoa(string maKhoa) { return dal.LayChuyenNganhTheoKhoa(maKhoa); }
    }
}
