using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DAL
{
    internal class DangNhapDAL
    {
        public string LayMatKhau(string taiKhoan)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT MatKhau
                    FROM TaiKhoan
                    WHERE TenDangNhap = @TaiKhoan";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TaiKhoan",
                        System.Data.SqlDbType.NVarChar, 100).Value = taiKhoan;

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return null;

                    return result.ToString();
                }
            }
        }
    }
}
