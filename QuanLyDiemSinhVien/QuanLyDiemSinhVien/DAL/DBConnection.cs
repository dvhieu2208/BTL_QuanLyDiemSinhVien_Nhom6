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
    public static class DBConnection
    {
        private static readonly string strcon =
            ConfigurationManager.ConnectionStrings["QLDiemSV"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(strcon);
        }

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

        public static DataTable LayBang(string sql, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(ps);
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt); 
                }
                return dt;
            }
        }

        public static object LayGiaTri(string sql, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddRange(ps);
                conn.Open();
                object kq = cmd.ExecuteScalar();
                return kq == DBNull.Value ? null : kq;
            }
        }

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

        public static DataTable LayBangProc(string tenProc, params SqlParameter[] ps)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(tenProc, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(ps);
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public static SqlParameter ThamSo(string ten, object giaTri)
        {
            object v = giaTri;
            if (v == null) v = DBNull.Value;
            else if (v is string && string.IsNullOrWhiteSpace((string)v)) v = DBNull.Value;
            return new SqlParameter(ten, v);
        }
    }

}
