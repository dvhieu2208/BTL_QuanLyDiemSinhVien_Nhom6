using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.DAL
{
    public class DBConnection
    {
        private static string strcon =
            @"Data Source=localhost\SQLEXPRESS;Initial Catalog = QuanLyDiemSinhVienDaiHoc; User ID = sa;Pwd = 123456";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(strcon);
        }
    }

}
