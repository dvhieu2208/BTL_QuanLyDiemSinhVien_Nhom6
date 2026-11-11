using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    internal class PhanTrang
    {
        private readonly DataGridView dgv;
        private readonly Button nutTruoc;
        private readonly Button nutSau;
        private readonly Button[] nutSo;
        private readonly Label nhanThongTin;
        private readonly string tenDoiTuong;
        private DataTable nguon;

        public int KichThuocTrang { get; set; }
        public int TrangHienTai { get; private set; }

        public string[] Cot { get; set; }

        public event EventHandler DaHienThi;

        public PhanTrang(DataGridView dgv, Button truoc, Button sau, Button[] so, Label thongTin,
                         string tenDoiTuong, int kichThuoc = 10)
        {
            this.dgv = dgv;
            nutTruoc = truoc;
            nutSau = sau;
            nutSo = so ?? new Button[0];
            nhanThongTin = thongTin;
            this.tenDoiTuong = tenDoiTuong;
            KichThuocTrang = kichThuoc;
            TrangHienTai = 1;

            if (nutTruoc != null) nutTruoc.Click += (s, e) => DenTrang(TrangHienTai - 1);
            if (nutSau != null) nutSau.Click += (s, e) => DenTrang(TrangHienTai + 1);
            foreach (Button b in nutSo)
            {
                Button nut = b;
                nut.Click += (s, e) =>
                {
                    int soTrangClick;
                    if (int.TryParse(nut.Text, out soTrangClick)) DenTrang(soTrangClick);
                };
            }
        }

        public int SoTrang
        {
            get
            {
                if (nguon == null || nguon.Rows.Count == 0) return 1;
                return (nguon.Rows.Count + KichThuocTrang - 1) / KichThuocTrang;
            }
        }

        public void GanDuLieu(DataTable dt, bool giuTrang = false)
        {
            nguon = dt;
            if (!giuTrang) TrangHienTai = 1;
            DenTrang(TrangHienTai);
        }

        public void DenTrang(int trang)
        {
            if (nguon == null) return;
            trang = Math.Max(1, Math.Min(trang, SoTrang));
            TrangHienTai = trang;

            DataTable trangDuLieu = nguon.Clone();
            int batDau = (trang - 1) * KichThuocTrang;
            int ketThuc = Math.Min(batDau + KichThuocTrang, nguon.Rows.Count);
            for (int i = batDau; i < ketThuc; i++) trangDuLieu.ImportRow(nguon.Rows[i]);

            dgv.DataSource = trangDuLieu;
            if (Cot != null) GiaoDien.HienCot(dgv, Cot);
            CapNhatNut(batDau, ketThuc);
            if (DaHienThi != null) DaHienThi(this, EventArgs.Empty);
        }

        private void CapNhatNut(int batDau, int ketThuc)
        {
            int tong = SoTrang;
            int dau = Math.Max(1, Math.Min(TrangHienTai - nutSo.Length / 2, tong - nutSo.Length + 1));
            for (int i = 0; i < nutSo.Length; i++)
            {
                int so = dau + i;
                nutSo[i].Text = so.ToString();
                nutSo[i].Visible = so <= tong;
                bool dangChon = so == TrangHienTai;
                nutSo[i].BackColor = dangChon ? GiaoDien.MauChinh : Color.White;
                nutSo[i].ForeColor = dangChon ? Color.White : Color.Black;
            }
            if (nutTruoc != null) nutTruoc.Enabled = TrangHienTai > 1;
            if (nutSau != null) nutSau.Enabled = TrangHienTai < tong;

            if (nhanThongTin != null)
            {
                int n = nguon.Rows.Count;
                nhanThongTin.Text = n == 0
                    ? "Không có " + tenDoiTuong + " nào"
                    : string.Format("Hiển thị {0}-{1} trong tổng số {2} {3}", batDau + 1, ketThuc, n, tenDoiTuong);
            }
        }

        public static Button[] Nut(params Button[] nut) { return nut; }
    }
}
