using System;
using System.Data;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;

namespace QuanLyDiemSinhVien
{

    public partial class fSinhVien : Form
    {
        private readonly SinhVienBLL bll = new SinhVienBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private readonly LopBLL lopBll = new LopBLL();
        private readonly PhanTrang phanTrang;
        private bool dangNap;

        public fSinhVien()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            phanTrang = new PhanTrang(dgvDanhSach, button13, button19,
                PhanTrang.Nut(btn1, btn2, btn3, btn4, btn5), null, "sinh viên", 12);

            cboKhoa.SelectedIndexChanged += (s, e) => { if (!dangNap) DoiKhoa(); };
            cboLop.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            cboTinhTrang.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            txtTimKiem.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDanhSach(); }
            };
            btnTimKiem.Click += (s, e) => TaiDanhSach();
            btnThem.Click += (s, e) => MoChiTiet(null);
            btnSua.Click += (s, e) => SuaSinhVien();
            btnXoa.Click += (s, e) => XoaSinhVien();
            dgvDanhSach.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) SuaSinhVien(); };
            Load += (s, e) => NapBoLoc();
        }

        private void NapBoLoc()
        {
            try
            {
                dangNap = true;
                GiaoDien.GanCombo(cboKhoa, danhMuc.LayKhoa(), "MaKhoa", "TenKhoa", true);
                NapLopVaChuyenNganh();
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
            TaiDanhSach();
        }

        private void NapLopVaChuyenNganh()
        {
            string maKhoa = GiaoDien.GiaTri(cboKhoa);
            GiaoDien.GanCombo(cboLop, danhMuc.LayLopTheoKhoa(maKhoa), "MaLop", "MaLop", true);
            GiaoDien.GanCombo(cboTinhTrang, danhMuc.LayChuyenNganhTheoKhoa(maKhoa), "MaChuyenNganh", "TenChuyenNganh", true);
        }

        private void DoiKhoa()
        {
            try
            {
                dangNap = true;
                NapLopVaChuyenNganh();
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
            TaiDanhSach();
        }

        private void TaiDanhSach()
        {
            try
            {
                DataTable dt = bll.TimKiem(GiaoDien.GiaTri(cboKhoa), GiaoDien.GiaTri(cboLop),
                    GiaoDien.GiaTri(cboTinhTrang), null, txtTimKiem.Text.Trim());
                phanTrang.GanDuLieu(dt);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void MoChiTiet(string maSV)
        {
            using (var f = new fSinhVienChiTiet(maSV, GiaoDien.GiaTri(cboKhoa), GiaoDien.GiaTri(cboLop)))
            {
                f.ShowDialog(this);
            }
            int trang = phanTrang.TrangHienTai;
            TaiDanhSach();
            phanTrang.DenTrang(trang);
        }

        private void SuaSinhVien()
        {
            string ma = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaSV");
            if (ma == null) { GiaoDien.ThongBao("Hãy chọn sinh viên cần sửa."); return; }
            MoChiTiet(ma);
        }

        private void XoaSinhVien()
        {
            string ma = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaSV");
            if (ma == null) { GiaoDien.ThongBao("Hãy chọn sinh viên cần xóa."); return; }
            string ten = GiaoDien.GiaTriDongChon(dgvDanhSach, "TenSV");
            string lop = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaLop");
            if (!GiaoDien.XacNhan("Xóa sinh viên " + ma + " - " + ten + "?\nToàn bộ điểm của sinh viên cũng sẽ bị xóa.")) return;

            string loi;
            if (bll.Xoa(ma, out loi))
            {
                GiaoDien.ThongBao("Đã xóa sinh viên.\nSĩ số lớp " + lop + " hiện là " + lopBll.LaySiSo(lop) + ".");
                TaiDanhSach();
            }
            else GiaoDien.Loi(loi);
        }

        // ---- Các hàm sự kiện do Designer tạo sẵn ----
        private void label8_Click(object sender, EventArgs e) { }
        private void button19_Click(object sender, EventArgs e) { }
        private void button13_Click(object sender, EventArgs e) { }
        private void button15_Click(object sender, EventArgs e) { }
        private void button16_Click(object sender, EventArgs e) { }
        private void button14_Click(object sender, EventArgs e) { }
        private void button17_Click(object sender, EventArgs e) { }
        private void button18_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
    }
}
