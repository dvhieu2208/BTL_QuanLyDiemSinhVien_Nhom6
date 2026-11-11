using System;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.DTO;

namespace QuanLyDiemSinhVien.GUI
{
    /// <summary>
    /// Nhập / sửa thông tin sinh viên.
    /// - Yêu cầu 2: chọn Khoa -> cboLop chỉ hiện các lớp của khoa.
    /// - Yêu cầu 3: chọn Lớp -> dgvDanhSach hiện danh sách SV của lớp + sĩ số (lbDanhSach);
    ///   sau khi lưu, sĩ số được trigger trong CSDL cập nhật và hiển thị lại.
    /// - Yêu cầu 4: chọn Khoa -> cboChuyenNganh chỉ hiện của khoa.
    /// </summary>
    public partial class fSinhVienChiTiet : Form
    {
        private readonly SinhVienBLL bll = new SinhVienBLL();
        private readonly LopBLL lopBll = new LopBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private readonly string maSVSua;      // null = thêm mới
        private readonly string khoaMacDinh;
        private readonly string lopMacDinh;
        private bool dangNap;

        public fSinhVienChiTiet() : this(null, null, null) { }

        public fSinhVienChiTiet(string maSV, string maKhoa, string maLop)
        {
            InitializeComponent();
            maSVSua = maSV;
            khoaMacDinh = maKhoa;
            lopMacDinh = maLop;
            dgvDanhSach.AutoGenerateColumns = false;
            if (!LaThemMoi)
            {
                Text = "Sửa thông tin sinh viên";
                btnThem.Text = "Lưu";
            }

            cboKhoa.SelectedIndexChanged += (s, e) => { if (!dangNap) DoiKhoa(null, null); };
            cboLop.SelectedIndexChanged += (s, e) => { if (!dangNap) NapDanhSachLop(); };
            btnThem.Click += (s, e) => Luu();
            btnLamMoi.Click += (s, e) => LamMoi();
            btnDong.Click += (s, e) => Close();
            Load += fSinhVienChiTiet_Load;
        }

        private bool LaThemMoi { get { return maSVSua == null; } }

        private void fSinhVienChiTiet_Load(object sender, EventArgs e)
        {
            try
            {
                dangNap = true;
                if (cboGioiTinh.Items.Count == 0) cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
                cboGioiTinh.SelectedIndex = 0;
                GiaoDien.GanCombo(cboKhoa, danhMuc.LayKhoa(), "MaKhoa", "TenKhoa", false);
                GiaoDien.GanCombo(cboQueQuan, danhMuc.LayQue(), "MaQue", "TenQue", false);
                GiaoDien.GanCombo(cboDanToc, danhMuc.LayDanToc(), "MaDanToc", "TenDanToc", false);
                GiaoDien.GanCombo(cboHeDaoTao, danhMuc.LayHeDaoTao(), "MaHeDT", "TenHeDT", false);
                GiaoDien.GanCombo(cboChucVu, danhMuc.LayChucVu(), "MaChucVu", "TenChucVu", false);
                GiaoDien.ChonGiaTri(cboChucVu, "SV");
                dangNap = false;

                if (LaThemMoi)
                {
                    if (khoaMacDinh != null) GiaoDien.ChonGiaTri(cboKhoa, khoaMacDinh);
                    DoiKhoa(lopMacDinh, null);
                }
                else
                {
                    HienThiSinhVien(bll.LayTheoMa(maSVSua));
                }
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

        private void HienThiSinhVien(SinhVien sv)
        {
            if (sv == null) { GiaoDien.Loi("Không tìm thấy sinh viên."); return; }
            txtMaSV.Text = sv.Ma;
            txtMaSV.ReadOnly = true;
            txtHoTen.Text = sv.HoTen;
            if (sv.NgaySinh.HasValue) dtpNgaySinh.Value = sv.NgaySinh.Value;
            cboGioiTinh.SelectedItem = sv.GioiTinh == "Nữ" ? "Nữ" : "Nam";
            dangNap = true;
            GiaoDien.ChonGiaTri(cboKhoa, sv.MaKhoa);
            if (sv.MaQue != null) GiaoDien.ChonGiaTri(cboQueQuan, sv.MaQue);
            if (sv.MaDanToc != null) GiaoDien.ChonGiaTri(cboDanToc, sv.MaDanToc);
            if (sv.MaHeDT != null) GiaoDien.ChonGiaTri(cboHeDaoTao, sv.MaHeDT);
            if (sv.MaChucVu != null) GiaoDien.ChonGiaTri(cboChucVu, sv.MaChucVu);
            dangNap = false;
            DoiKhoa(sv.MaLop, sv.MaChuyenNganh);
        }

        /// <summary>Yêu cầu 2 + 4: lọc Lớp và Chuyên ngành theo Khoa.</summary>
        private void DoiKhoa(string chonLop, string chonChuyenNganh)
        {
            try
            {
                dangNap = true;
                string maKhoa = GiaoDien.GiaTri(cboKhoa);
                GiaoDien.GanCombo(cboLop, danhMuc.LayLopTheoKhoa(maKhoa), "MaLop", "TenLop", false);
                GiaoDien.GanCombo(cboChuyenNganh, danhMuc.LayChuyenNganhTheoKhoa(maKhoa), "MaChuyenNganh", "TenChuyenNganh", false);
                if (chonLop != null) GiaoDien.ChonGiaTri(cboLop, chonLop);
                if (chonChuyenNganh != null) GiaoDien.ChonGiaTri(cboChuyenNganh, chonChuyenNganh);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
            NapDanhSachLop();
        }

        /// <summary>Yêu cầu 3: hiện danh sách SV của lớp đang chọn và sĩ số.</summary>
        private void NapDanhSachLop()
        {
            string maLop = GiaoDien.GiaTri(cboLop);
            if (maLop == null)
            {
                dgvDanhSach.DataSource = null;
                lbDanhSach.Text = "Khoa này chưa có lớp nào";
                return;
            }
            try
            {
                dgvDanhSach.DataSource = bll.LayTheoLop(maLop);
                lbDanhSach.Text = string.Format("Danh sách sinh viên lớp {0} - Sĩ số: {1}", maLop, lopBll.LaySiSo(maLop));
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private SinhVien DocForm()
        {
            return new SinhVien
            {
                Ma = txtMaSV.Text.Trim().ToUpper(),
                HoTen = txtHoTen.Text.Trim(),
                NgaySinh = dtpNgaySinh.Value.Date,
                GioiTinh = Convert.ToString(cboGioiTinh.SelectedItem),
                MaKhoa = GiaoDien.GiaTri(cboKhoa),
                MaLop = GiaoDien.GiaTri(cboLop),
                MaChuyenNganh = GiaoDien.GiaTri(cboChuyenNganh),
                MaQue = GiaoDien.GiaTri(cboQueQuan),
                MaDanToc = GiaoDien.GiaTri(cboDanToc),
                MaHeDT = GiaoDien.GiaTri(cboHeDaoTao),
                MaChucVu = GiaoDien.GiaTri(cboChucVu)
            };
        }

        private void Luu()
        {
            SinhVien sv = DocForm();
            string loi;
            bool ok = LaThemMoi ? bll.Them(sv, out loi) : bll.Sua(sv, out loi);
            if (!ok) { GiaoDien.Loi(loi); return; }

            NapDanhSachLop();   // hiện lại danh sách + sĩ số mới
            GiaoDien.ThongBao((LaThemMoi ? "Đã thêm sinh viên " : "Đã cập nhật sinh viên ") + sv.Ma +
                              ".\nSĩ số lớp " + sv.MaLop + " hiện là " + lopBll.LaySiSo(sv.MaLop) + ".");
            if (LaThemMoi)
            {
                txtMaSV.Clear();
                txtHoTen.Clear();
                txtMaSV.Focus();
            }
        }

        private void LamMoi()
        {
            if (LaThemMoi)
            {
                txtMaSV.Clear();
                txtHoTen.Clear();
                dtpNgaySinh.Value = new DateTime(2005, 1, 1);
                txtMaSV.Focus();
            }
            else
            {
                HienThiSinhVien(bll.LayTheoMa(maSVSua));
            }
        }
    }
}
