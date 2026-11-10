using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.DAL
{
    public class DBConnection
    {
        private static readonly string strcon = ConfigurationManager.ConnectionStrings["QLDiemSV"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(strcon);
        }
        // INSERT / UPDATE / DELETE
        public static bool ThucThi(string sql, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(ps);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // SELECT trả về nhiều dòng
        public static DataTable LayBang(string sql, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(ps);
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);   // tự mở/đóng kết nối
                return dt;
            }
        }

        // Dùng cho stored procedure
        public static bool ThucThiProc(string tenProc, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(tenProc, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(ps);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }

}
