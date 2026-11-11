using System;
using System.Data;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.DTO;

namespace QuanLyDiemSinhVien.GUI
{
    /// <summary>
    /// Quản lý lớp. button6 = Thêm, button8 = Sửa, button9 = Xóa, button10 = Mới.
    /// Yêu cầu 1: lớp mới luôn có Sĩ số = 0; sĩ số chỉ thay đổi khi thêm/xóa/chuyển sinh viên.
    /// txtKhoa: nhập MÃ khoa (VD: CNTT, KT) - có gợi ý khi gõ.
    /// </summary>
    public partial class fQuanLyLop : Form
    {
        private readonly LopBLL bll = new LopBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private bool dangNap;

        public fQuanLyLop()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            cboKhoa.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            txtTimKiem.TextChanged += (s, e) => TaiDanhSach();
            dgvDanhSach.SelectionChanged += (s, e) => HienDongChon();
            button6.Click += (s, e) => Them();
            button8.Click += (s, e) => Sua();
            button9.Click += (s, e) => Xoa();
            button10.Click += (s, e) => LamMoi();
            Load += (s, e) => NapDuLieu();
        }

        private void NapDuLieu()
        {
            try
            {
                dangNap = true;
                DataTable khoa = danhMuc.LayKhoa();
                GiaoDien.GanCombo(cboKhoa, khoa, "MaKhoa", "TenKhoa", true);

                // Gợi ý mã khoa khi gõ vào txtKhoa
                var goiY = new AutoCompleteStringCollection();
                foreach (DataRow r in khoa.Rows) goiY.Add(r["MaKhoa"].ToString());
                txtKhoa.AutoCompleteCustomSource = goiY;
                txtKhoa.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtKhoa.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
            LamMoi();
        }

        private void TaiDanhSach()
        {
            try
            {
                DataTable dt = bll.LayDanhSach(GiaoDien.GiaTri(cboKhoa));
                string tim = txtTimKiem.Text.Trim().Replace("'", "''");
                if (tim.Length > 0)
                    dt.DefaultView.RowFilter = string.Format("MaLop LIKE '%{0}%' OR TenLop LIKE '%{0}%'", tim);
                dgvDanhSach.DataSource = dt;
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void HienDongChon()
        {
            string ma = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaLop");
            if (ma == null) return;
            txtMaLop.Text = ma;
            txtMaLop.ReadOnly = true;
            txtTenLop.Text = GiaoDien.GiaTriDongChon(dgvDanhSach, "TenLop");
            txtKhoa.Text = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaKhoa");
            txtKhoaHoc.Text = GiaoDien.GiaTriDongChon(dgvDanhSach, "KhoaHoc");
            lbSiSo.Text = GiaoDien.GiaTriDongChon(dgvDanhSach, "SiSo");
        }

        private Lop DocForm()
        {
            return new Lop
            {
                MaLop = txtMaLop.Text.Trim().ToUpper(),
                TenLop = txtTenLop.Text.Trim(),
                MaKhoa = txtKhoa.Text.Trim().ToUpper(),
                KhoaHoc = txtKhoaHoc.Text.Trim(),
                SiSo = 0
            };
        }

        private void LamMoi()
        {
            dgvDanhSach.ClearSelection();
            txtMaLop.ReadOnly = false;
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtKhoa.Clear();
            txtKhoaHoc.Clear();
            lbSiSo.Text = "0";
            txtMaLop.Focus();
        }

        private void Them()
        {
            if (txtMaLop.ReadOnly) { LamMoi(); GiaoDien.ThongBao("Nhập thông tin lớp mới rồi bấm Thêm."); return; }
            string loi;
            if (bll.Them(DocForm(), out loi))
            {
                GiaoDien.ThongBao("Đã thêm lớp (sĩ số ban đầu = 0).");
                TaiDanhSach();
                LamMoi();
            }
            else GiaoDien.Loi(loi);
        }

        private void Sua()
        {
            if (!txtMaLop.ReadOnly) { GiaoDien.ThongBao("Hãy chọn lớp cần sửa trong danh sách."); return; }
            string loi;
            if (bll.Sua(DocForm(), out loi)) { GiaoDien.ThongBao("Đã cập nhật lớp."); TaiDanhSach(); }
            else GiaoDien.Loi(loi);
        }

        private void Xoa()
        {
            string ma = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaLop");
            if (ma == null) { GiaoDien.ThongBao("Hãy chọn lớp cần xóa."); return; }
            if (!GiaoDien.XacNhan("Xóa lớp " + ma + "?\n(Chỉ xóa được lớp chưa có sinh viên)")) return;
            string loi;
            if (bll.Xoa(ma, out loi)) { GiaoDien.ThongBao("Đã xóa lớp."); TaiDanhSach(); LamMoi(); }
            else GiaoDien.Loi(loi);
        }

        // ---- Các hàm sự kiện do Designer tạo sẵn ----
        private void label8_Click(object sender, EventArgs e) { }
        private void button12_Click(object sender, EventArgs e) { }
    }
}
