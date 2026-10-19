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
        public bool KiemTraDangNhap(string taiKhoan, string matKhau)
        {
            bool isValid = false;
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM TaiKhoan WHERE TenDangNhap = @TaiKhoan AND MatKhau = @MatKhau";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count > 0)
                        {
                            isValid = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                }
            }
            return isValid;
        }
    }
}
