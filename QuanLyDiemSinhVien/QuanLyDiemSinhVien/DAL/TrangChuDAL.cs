using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace QuanLyDiemSinhVien.DAL
{
    internal class TrangChuDAL
    {
        private static int Dem(string sql)
        {
            object kq = DBConnection.LayGiaTri(sql);
            return kq == null ? 0 : Convert.ToInt32(kq);
        }

        public int GetTongSinhVien() { return Dem("SELECT COUNT(*) FROM SinhVien"); }
        public int GetTongLop() { return Dem("SELECT COUNT(*) FROM Lop"); }
        public int GetTongMonHoc() { return Dem("SELECT COUNT(*) FROM MonHoc"); }

        public decimal GetTyLeDat()
        {
            object kq = DBConnection.LayGiaTri(@"
                SELECT CASE WHEN COUNT(*) = 0 THEN 0
                            ELSE SUM(CASE WHEN Diem >= 4 THEN 1.0 ELSE 0 END) * 100.0 / COUNT(*) END
                FROM Diem WHERE LanThi = 1");
            return kq == null ? 0 : Convert.ToDecimal(kq);
        }

        public DataTable GetThongKeKetQua()
        {
            return DBConnection.LayBangProc("sp_ThongKe_XepLoai", DBConnection.ThamSo("@MaLop", null));
        }
    }
}
