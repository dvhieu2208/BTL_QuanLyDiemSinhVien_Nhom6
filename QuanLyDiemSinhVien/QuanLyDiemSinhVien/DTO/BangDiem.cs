using QuanLyDiemSinhVien.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyDiemSinhVien.DTO
{
    public class KetQuaMon
    {
        public int HocKy { get; set; }
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public int DVHT { get; set; }
        public int LanThi { get; set; }
        public decimal DiemSo { get; set; }
    }

    public class BangDiem : IXepLoai
    {
        public SinhVien SinhVien { get; set; }
        public string TenLop { get; set; }
        public string TenKhoa { get; set; }

        private readonly List<KetQuaMon> _ketQua = new List<KetQuaMon>();
        public List<KetQuaMon> KetQua { get { return _ketQua; } }

        private IEnumerable<KetQuaMon> KetQuaCuoi()
        {
            return _ketQua.GroupBy(k => k.MaMon)
                          .Select(g => g.OrderByDescending(k => k.LanThi).First());
        }

        public int TongTinChi
        {
            get { return KetQuaCuoi().Where(k => k.DiemSo >= 4).Sum(k => k.DVHT); }
        }

        public decimal DiemTrungBinh
        {
            get
            {
                var ds = KetQuaCuoi().ToList();
                int tongDV = ds.Sum(k => k.DVHT);
                if (tongDV == 0) return 0;
                return System.Math.Round(ds.Sum(k => k.DiemSo * k.DVHT) / tongDV, 2);
            }
        }

        public string XepLoai()
        {
            if (_ketQua.Count == 0) return "Chưa có điểm";
            return Diem.XepLoaiTheoDiem(DiemTrungBinh);
        }
    }
}
