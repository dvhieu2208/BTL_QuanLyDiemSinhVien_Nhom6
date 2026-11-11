using System;
using System.Data;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.DTO;
using QuanLyDiemSinhVien.GUI;

namespace QuanLyDiemSinhVien
{
    /// <summary>
    /// Yêu cầu 8: xem và in bảng điểm các môn theo mã sinh viên.
    /// textBox1 = mã SV, cboHocKy = lọc học kỳ, btnTimKiem = Xem, button6 = In.
    /// </summary>
    public partial class fXemBangDiem : Form
    {
        private readonly BaoCaoBLL baoCao = new BaoCaoBLL();
        private readonly SinhVienBLL svBll = new SinhVienBLL();
        private string maSVDangXem;
        private bool dangNap;

        public fXemBangDiem()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            dangNap = true;
            GiaoDien.GanComboSo(cboHocKy, new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, "Học kỳ ", true);
            dangNap = false;

            textBox1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; XemBangDiem(); }
            };
            btnTimKiem.Click += (s, e) => XemBangDiem();
            button6.Click += (s, e) => InBangDiem();
            XoaThongTin();
        }

        private void XoaThongTin()
        {
            lbHoTen.Text = "";
            lbMaSv.Text = lbLop.Text = lbKhoa.Text = "";
            lbDTB.Text = label19.Text = label20.Text = "";
            dgvDanhSach.DataSource = null;
            maSVDangXem = null;
        }

        private void fXemBangDiem_Load(object sender, EventArgs e)
        {
            try
            {
                // Gợi ý mã sinh viên khi gõ
                var goiY = new AutoCompleteStringCollection();
                foreach (SinhVien sv in svBll.LayTatCa()) goiY.Add(sv.Ma);
                textBox1.AutoCompleteCustomSource = goiY;
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void XemBangDiem()
        {
            string maSV = textBox1.Text.Trim().ToUpper();
            if (maSV.Length == 0) { GiaoDien.ThongBao("Hãy nhập mã sinh viên."); textBox1.Focus(); return; }
            try
            {
                int? hocKy = GiaoDien.GiaTriSo(cboHocKy);
                BangDiem bd = baoCao.TaoBangDiem(maSV, hocKy);
                if (bd == null)
                {
                    XoaThongTin();
                    GiaoDien.Loi("Không tìm thấy sinh viên có mã " + maSV);
                    return;
                }

                maSVDangXem = maSV;
                lbHoTen.Text = bd.SinhVien.HoTen;
                lbMaSv.Text = bd.SinhVien.Ma;
                lbLop.Text = bd.TenLop;
                lbKhoa.Text = bd.TenKhoa;
                lbDTB.Text = bd.KetQua.Count == 0 ? "-" : bd.DiemTrungBinh.ToString("0.00");
                label19.Text = bd.TongTinChi.ToString();
                label20.Text = bd.XepLoai();

                dgvDanhSach.DataSource = baoCao.BangDiemSinhVien(maSV, hocKy);
                if (bd.KetQua.Count == 0)
                    GiaoDien.ThongBao("Sinh viên chưa có điểm" + (hocKy.HasValue ? " trong học kỳ " + hocKy : "") + ".");
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        /// <summary>In bảng điểm bằng Crystal Report.</summary>
        private void InBangDiem()
        {
            if (maSVDangXem == null) XemBangDiem();
            if (maSVDangXem == null) return;
            try
            {
                DataTable dt = baoCao.BangDiemSinhVien(maSVDangXem, GiaoDien.GiaTriSo(cboHocKy));
                if (dt.Rows.Count == 0) { GiaoDien.ThongBao("Sinh viên chưa có điểm để in."); return; }
                fBaoCao.HienThi(this, LoaiBaoCao.BangDiemSinhVien, dt, "Bảng điểm sinh viên " + maSVDangXem);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        // ---- Các hàm sự kiện do Designer tạo sẵn ----
        private void cboHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!dangNap && maSVDangXem != null) XemBangDiem();
        }
        private void label13_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
    }
}
