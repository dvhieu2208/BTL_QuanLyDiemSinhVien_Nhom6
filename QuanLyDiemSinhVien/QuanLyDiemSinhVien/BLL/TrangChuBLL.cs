using QuanLyDiemSinhVien.DAL;
using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuanLyDiemSinhVien.BLL
{
    internal class TrangChuBLL : IThongKe
    {
        private readonly TrangChuDAL dal = new TrangChuDAL();

        public int TongSinhVien() { return dal.GetTongSinhVien(); }
        public int TongLop() { return dal.GetTongLop(); }
        public int TongMonHoc() { return dal.GetTongMonHoc(); }

        public int GetTongSinhVien() { return TongSinhVien(); }
        public int GetTongLop() { return TongLop(); }
        public int GetTongMonHoc() { return TongMonHoc(); }
        public decimal GetTyLeDat() { return dal.GetTyLeDat(); }
        public DataTable GetThongKeKetQua() { return dal.GetThongKeKetQua(); }
    }
}
