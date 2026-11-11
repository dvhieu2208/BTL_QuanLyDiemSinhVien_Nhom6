using System;
using System.Data;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;

namespace QuanLyDiemSinhVien
{

    public partial class fQuanLyLopHocPhan : Form
    {
        private readonly ThoiKhoaBieuBLL bll = new ThoiKhoaBieuBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private readonly BaoCaoBLL baoCao = new BaoCaoBLL();
        private readonly PhanTrang phanTrang;
        private bool dangNap;

        public fQuanLyLopHocPhan()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            phanTrang = new PhanTrang(dgvDanhSach, button13, button18,
                PhanTrang.Nut(btn1, btn2, btn3, btn4, btn5), null, "lớp học phần", 12);

            cboHocKy.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            cboKhoa.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            cboMonHoc.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiDanhSach(); };
            textBox1.TextChanged += (s, e) => TaiDanhSach();
            btnThem.Click += (s, e) => MoChiTiet(null, null);
            btnSua.Click += (s, e) => Sua();
            btnXoa.Click += (s, e) => Xoa();
            btnInTKB.Click += (s, e) => InThoiKhoaBieu();
            dgvDanhSach.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) Sua(); };
            Load += (s, e) => { NapBoLoc(); TaiDanhSach(); };
        }

        private void NapBoLoc()
        {
            try
            {
                dangNap = true;
                GiaoDien.GanComboSo(cboHocKy, new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, "Học kỳ ", true);
                GiaoDien.GanCombo(cboKhoa, danhMuc.LayKhoa(), "MaKhoa", "TenKhoa", true);
                GiaoDien.GanCombo(cboMonHoc, danhMuc.LayMonHoc(), "MaMon", "TenMon", true);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
        }

        private void TaiDanhSach()
        {
            try
            {
                phanTrang.GanDuLieu(bll.LayDanhSach(GiaoDien.GiaTriSo(cboHocKy), null,
                    GiaoDien.GiaTri(cboMonHoc), textBox1.Text.Trim(), GiaoDien.GiaTri(cboKhoa)));
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void MoChiTiet(string maLop, string maMon)
        {
            using (var f = new fLopHocPhanChiTiet(maLop, maMon, null, GiaoDien.GiaTriSo(cboHocKy)))
            {
                if (f.ShowDialog(this) == DialogResult.OK) TaiDanhSach();
            }
        }

        private void Sua()
        {
            string lop = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaLop");
            string mon = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaMon");
            if (lop == null) { GiaoDien.ThongBao("Hãy chọn lớp học phần cần sửa."); return; }
            MoChiTiet(lop, mon);
        }

        private void Xoa()
        {
            string lop = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaLop");
            string mon = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaMon");
            if (lop == null) { GiaoDien.ThongBao("Hãy chọn lớp học phần cần xóa."); return; }
            if (!GiaoDien.XacNhan("Xóa môn " + mon + " khỏi thời khóa biểu lớp " + lop + "?")) return;
            string loi;
            if (bll.Xoa(lop, mon, out loi)) { GiaoDien.ThongBao("Đã xóa."); TaiDanhSach(); }
            else GiaoDien.Loi(loi);
        }

        /// <summary>Yêu cầu 9: in thời khóa biểu của lớp (dòng đang chọn) theo học kỳ.</summary>
        private void InThoiKhoaBieu()
        {
            string maLop = GiaoDien.GiaTriDongChon(dgvDanhSach, "MaLop");
            int hocKy;
            if (maLop == null || !int.TryParse(GiaoDien.GiaTriDongChon(dgvDanhSach, "HocKy"), out hocKy))
            {
                GiaoDien.ThongBao("Hãy chọn một dòng của lớp cần in thời khóa biểu.\n(In theo lớp và học kỳ của dòng đó)");
                return;
            }
            try
            {
                DataTable dt = baoCao.ThoiKhoaBieu(maLop, hocKy);
                fBaoCao.HienThi(this, LoaiBaoCao.ThoiKhoaBieu, dt, "Thời khóa biểu lớp " + maLop + " - Học kỳ " + hocKy);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        // ---- Hàm sự kiện do Designer tạo sẵn ----
        private void button8_Click(object sender, EventArgs e) { }
    }
}
