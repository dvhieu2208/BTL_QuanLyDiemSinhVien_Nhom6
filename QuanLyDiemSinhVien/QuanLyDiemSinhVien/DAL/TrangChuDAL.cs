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

        public int GetTongSinhVien()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM SinhVien";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetTongLop()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Lop";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int GetTongMonHoc()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM MonHoc";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public decimal GetTyLeDat()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT
                    CASE
                        WHEN COUNT(*) = 0 THEN 0
                        ELSE
                            SUM(CASE WHEN Diem >= 4 THEN 1.0
                                     ELSE 0 END) * 100.0 / COUNT(*)
                    END
                    FROM Diem";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }
        }

        public DataTable GetThongKeKetQua()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
            SELECT 
                CASE 
                    WHEN Diem >= 9.4 THEN N'Xuất sắc (9.4-10)'
                    WHEN Diem >= 8.5 THEN N'Giỏi (8.5-9.3)'
                    WHEN Diem >= 7.0 THEN N'Khá (7.0-8.4)'
                    WHEN Diem >= 5.5 THEN N'Trung bình (5.5-6.9)'
                    WHEN Diem >= 4.0 THEN N'Yếu (4.0-5.4)'
                    ELSE N'Học lại (<4.0)'
                END AS KetQua,
                COUNT(*) AS SoLuong
            FROM Diem
            GROUP BY 
                CASE 
                    WHEN Diem >= 9.4 THEN N'Xuất sắc (9.4-10)'
                    WHEN Diem >= 8.5 THEN N'Giỏi (8.5-9.3)'
                    WHEN Diem >= 7.0 THEN N'Khá (7.0-8.4)'
                    WHEN Diem >= 5.5 THEN N'Trung bình (5.5-6.9)'
                    WHEN Diem >= 4.0 THEN N'Yếu (4.0-5.4)'
                    ELSE N'Học lại (<4.0)'
                END
            ORDER BY 
                CASE 
                    WHEN MIN(Diem) >= 9.4 THEN 1
                    WHEN MIN(Diem) >= 8.5 THEN 2
                    WHEN MIN(Diem) >= 7.0 THEN 3
                    WHEN MIN(Diem) >= 5.5 THEN 4
                    WHEN MIN(Diem) >= 4.0 THEN 5
                    ELSE 6
                END";

                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
