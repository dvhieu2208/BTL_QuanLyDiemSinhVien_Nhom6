using QuanLyDiemSinhVien.DAL;
using QuanLyDiemSinhVien.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.BLL
{
    internal class BaoCaoBLL
    {
        private readonly BaoCaoDAL dal = new BaoCaoDAL();

        public DataTable DiemTheoLopMon(string maLop, string maMon, int lanThi) { return dal.DiemTheoLopMon(maLop, maMon, lanThi); }
        public DataTable BangDiemSinhVien(string maSV, int? hocKy) { return dal.BangDiemSinhVien(maSV, hocKy); }
        public DataTable ThoiKhoaBieu(string maLop, int hocKy) { return dal.ThoiKhoaBieu(maLop, hocKy); }
        public DataTable ThongKeSVTheoKhoa() { return dal.ThongKeSVTheoKhoa(); }
        public DataTable ThongKeXepLoai(string maLop) { return dal.ThongKeXepLoai(maLop); }
        public DataTable ThongKeDiemTBTheoMon(string maLop) { return dal.ThongKeDiemTBTheoMon(maLop); }
        public BangDiem TaoBangDiem(string maSV, int? hocKy)
        {
            var svBll = new SinhVienBLL();
            SinhVien sv = svBll.LayTheoMa(maSV);
            if (sv == null) return null;

            var bd = new BangDiem { SinhVien = sv };
            DataTable tt = svBll.LayThongTin(maSV);
            if (tt.Rows.Count > 0)
            {
                bd.TenLop = tt.Rows[0]["TenLop"].ToString();
                bd.TenKhoa = tt.Rows[0]["TenKhoa"].ToString();
            }

            foreach (DataRow r in dal.BangDiemSinhVien(maSV, hocKy).Rows)
            {
                if (r["Diem"] == DBNull.Value) continue;
                bd.KetQua.Add(new KetQuaMon
                {
                    HocKy = Convert.ToInt32(r["HocKy"]),
                    MaMon = r["MaMon"].ToString(),
                    TenMon = r["TenMon"].ToString(),
                    DVHT = Convert.ToInt32(r["DVHT"]),
                    LanThi = Convert.ToInt32(r["LanThi"]),
                    DiemSo = Convert.ToDecimal(r["Diem"])
                });
            }
            return bd;
        }
    }
}
