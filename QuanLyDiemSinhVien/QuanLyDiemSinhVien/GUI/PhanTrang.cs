using System;
using System.Data;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{

    internal class PhanTrang
    {
        private const string COT_DONG = "__Dong";
        private readonly DataGridView dgv;
        private readonly Button nutTruoc;
        private readonly Button nutSau;
        private readonly Button[] nutSo;
        private readonly Label nhanThongTin;
        private readonly string tenDoiTuong;
        private DataTable nguon;

        public int KichThuocTrang { get; set; }
        public int TrangHienTai { get; private set; }

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
                    int trang;
                    if (int.TryParse(nut.Text, out trang)) DenTrang(trang);
                };
            }
        }

        public DataTable DuLieu { get { return nguon; } }

        public int SoTrang
        {
            get
            {
                if (nguon == null || nguon.Rows.Count == 0) return 1;
                return (nguon.Rows.Count + KichThuocTrang - 1) / KichThuocTrang;
            }
        }

        public void GanDuLieu(DataTable dt)
        {
            nguon = dt;
            if (nguon != null)
            {
                if (!nguon.Columns.Contains(COT_DONG)) nguon.Columns.Add(COT_DONG, typeof(int));
                for (int i = 0; i < nguon.Rows.Count; i++) nguon.Rows[i][COT_DONG] = i + 1;
                nguon.AcceptChanges();
            }
            dgv.DataSource = nguon;
            TrangHienTai = 1;
            DenTrang(1);
        }

        public void DenTrang(int trang)
        {
            if (nguon == null) return;
            dgv.EndEdit();
            trang = Math.Max(1, Math.Min(trang, SoTrang));
            TrangHienTai = trang;

            int batDau = (trang - 1) * KichThuocTrang;
            int ketThuc = Math.Min(batDau + KichThuocTrang, nguon.Rows.Count);
            nguon.DefaultView.RowFilter = string.Format("{0} > {1} AND {0} <= {2}", COT_DONG, batDau, ketThuc);

            int tong = SoTrang;
            int dau = Math.Max(1, Math.Min(TrangHienTai - nutSo.Length / 2, tong - nutSo.Length + 1));
            for (int i = 0; i < nutSo.Length; i++)
            {
                nutSo[i].Text = (dau + i).ToString();
                nutSo[i].Enabled = dau + i <= tong && dau + i != TrangHienTai;
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
