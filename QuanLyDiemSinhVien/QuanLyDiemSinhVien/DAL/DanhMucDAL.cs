using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class DanhMucDAL
    {
        public DataTable LayKhoa()
        {
            return DBConnection.LayBang("SELECT MaKhoa, TenKhoa FROM Khoa ORDER BY TenKhoa");
        }

        public DataTable LayQue()
        {
            return DBConnection.LayBang("SELECT MaQue, TenQue FROM Que ORDER BY TenQue");
        }

        public DataTable LayDanToc()
        {
            return DBConnection.LayBang("SELECT MaDanToc, TenDanToc FROM DanToc ORDER BY TenDanToc");
        }

        public DataTable LayHeDaoTao()
        {
            return DBConnection.LayBang("SELECT MaHeDT, TenHeDT FROM HeDaoTao ORDER BY TenHeDT");
        }

        public DataTable LayChucVu()
        {
            return DBConnection.LayBang("SELECT MaChucVu, TenChucVu FROM ChucVu ORDER BY MaChucVu");
        }

        public DataTable LayPhongHoc()
        {
            return DBConnection.LayBang("SELECT MaPhong, TenPhong FROM PhongHoc ORDER BY MaPhong");
        }

        public DataTable LayMonHoc()
        {
            return DBConnection.LayBang("SELECT MaMon, TenMon, DVHT FROM MonHoc ORDER BY TenMon");
        }

        public DataTable LayHocKy()
        {
            return DBConnection.LayBang("SELECT DISTINCT HocKy FROM ThoiKhoaBieu ORDER BY HocKy");
        }

        // ---- Thủ tục lưu trữ ----
        /// <summary>Yêu cầu 2: chỉ lấy các lớp thuộc khoa được chọn.</summary>
        public DataTable LayLopTheoKhoa(string maKhoa)
        {
            return DBConnection.LayBangProc("sp_Lop_LayTheoKhoa", DBConnection.ThamSo("@MaKhoa", maKhoa));
        }

        /// <summary>Yêu cầu 4: chỉ lấy chuyên ngành thuộc khoa được chọn.</summary>
        public DataTable LayChuyenNganhTheoKhoa(string maKhoa)
        {
            return DBConnection.LayBangProc("sp_ChuyenNganh_LayTheoKhoa", DBConnection.ThamSo("@MaKhoa", maKhoa));
        }
    }
}
