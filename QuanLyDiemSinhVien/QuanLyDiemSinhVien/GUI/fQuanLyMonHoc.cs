using System;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;

namespace QuanLyDiemSinhVien
{

    public partial class fQuanLyMonHoc : Form
    {
        private readonly MonHocBLL bll = new MonHocBLL();
        private readonly PhanTrang phanTrang;
        private bool dangNap;

        public fQuanLyMonHoc()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            phanTrang = new PhanTrang(dgvDanhSach, button13, button19,
                PhanTrang.Nut(btn1, btn2, btn3, btn4, btn5), null, "môn học", 12);

            textBox1.TextChanged += (s, e) => TaiDanhSach();
            cboDVHT.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            btnThem.Click += (s, e) => MoChiTiet(null);
            btnSua.Click += (s, e) => Sua();
            btnXoa.Click += (s, e) => Xoa();
            dgvDanhSach.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) Sua(); };
            Load += (s, e) =>
            {
                dangNap = true;
                GiaoDien.GanComboSo(cboDVHT, new[] { 1, 2, 3, 4, 5 }, "", true);
                dangNap = false;
                TaiDanhSach();
            };
        }

        private void TaiDanhSach()
        {
            try
            {
                phanTrang.GanDuLieu(bll.LayDanhSach(textBox1.Text.Trim(), GiaoDien.GiaTriSo(cboDVHT)));
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void MoChiTiet(string maMon)
        {
            using (var f = new fMonHocChiTiet(maMon))
            {
                if (f.ShowDialog(this) == DialogResult.OK) TaiDanhSach();
            }
        }

        private void Sua()
        {
            string ma = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaMon");
            if (ma == null) { GiaoDien.ThongBao("Hãy chọn môn học cần sửa."); return; }
            MoChiTiet(ma);
        }

        private void Xoa()
        {
            string ma = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaMon");
            if (ma == null) { GiaoDien.ThongBao("Hãy chọn môn học cần xóa."); return; }
            if (!GiaoDien.XacNhan("Xóa môn học " + ma + "?\n(Môn đã có điểm sẽ không xóa được)")) return;
            string loi;
            if (bll.Xoa(ma, out loi)) { GiaoDien.ThongBao("Đã xóa môn học."); TaiDanhSach(); }
            else GiaoDien.Loi(loi);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
    }
}
