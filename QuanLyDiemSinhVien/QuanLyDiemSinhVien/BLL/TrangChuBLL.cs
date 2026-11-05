using QuanLyDiemSinhVien.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace QuanLyDiemSinhVien.BLL
{
    internal class TrangChuBLL
    {
        private TrangChuDAL dal = new TrangChuDAL();

        public int GetTongSinhVien()
        {
            return dal.GetTongSinhVien();
        }

        public int GetTongLop()
        {
            return dal.GetTongLop();
        }

        public int GetTongMonHoc()
        {
            return dal.GetTongMonHoc();
        }

        public decimal GetTyLeDat()
        {
            return dal.GetTyLeDat();
        }

        public DataTable GetThongKeKetQua()
        {
            return dal.GetThongKeKetQua();
        }
    }
}
