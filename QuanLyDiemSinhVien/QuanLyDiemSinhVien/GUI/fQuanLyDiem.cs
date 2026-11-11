using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;

namespace QuanLyDiemSinhVien
{
    /// <summary>
    /// Yêu cầu 5: chọn Lớp + Học kỳ -> cboMonHoc chỉ hiện các môn theo thời khóa biểu của lớp;
    /// chọn Môn + Lần thi -> dgvDanhSach hiện toàn bộ SV của lớp, nhập điểm trực tiếp vào cột "Điểm".
    /// Điểm được LƯU NGAY khi gõ xong một ô (Enter hoặc chuyển ô); xóa trắng ô = xóa điểm.
    /// btnTimKiem: tải lại danh sách. btnIn - Yêu cầu 7: in danh sách điểm theo lớp, môn, lần thi.
    /// </summary>
    public partial class fQuanLyDiem : Form
    {
        private readonly DiemBLL bll = new DiemBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private readonly ThoiKhoaBieuBLL tkb = new ThoiKhoaBieuBLL();
        private readonly BaoCaoBLL baoCao = new BaoCaoBLL();
        private readonly PhanTrang phanTrang;
        private bool dangNap;

        // Ngữ cảnh của danh sách đang hiển thị trên lưới
        private string luoiLop, luoiMon;
        private int luoiHocKy, luoiLanThi;

        public fQuanLyDiem()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            phanTrang = new PhanTrang(dgvDanhSach, button12, button18,
                PhanTrang.Nut(btn1, btn2, btn3, btn4, btn5), null, "sinh viên", 10);

            cboLop.SelectedIndexChanged += (s, e) => { if (!dangNap) DoiLop(); };
            cboHocKy.SelectedIndexChanged += (s, e) => { if (!dangNap) NapMon(); };
            cboMonHoc.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiLuoi(); };
            cboLanThi.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiLuoi(); };
            btnTimKiem.Click += (s, e) => TaiLuoi();
            btnIn.Click += (s, e) => InDanhSach();
            dgvDanhSach.CellValidating += dgvDanhSach_CellValidating;
            dgvDanhSach.CellEndEdit += dgvDanhSach_CellEndEdit;
            dgvDanhSach.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void fQuanLyDiem_Load(object sender, EventArgs e)
        {
            NapLop();
        }

        // ================== Nạp combo theo dây chuyền ==================
        private void NapLop()
        {
            try
            {
                dangNap = true;
                GiaoDien.GanCombo(cboLop, danhMuc.LayLopTheoKhoa(null), "MaLop", "MaLop", false);
                GiaoDien.GanComboSo(cboLanThi, new[] { 1, 2, 3 }, "Lần ", false);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
            DoiLop();
        }

        /// <summary>Đổi lớp: chỉ hiện các học kỳ mà lớp có lịch học.</summary>
        private void DoiLop()
        {
            try
            {
                dangNap = true;
                string maLop = GiaoDien.GiaTri(cboLop);
                var ds = new List<int>();
                if (maLop != null)
                    foreach (DataRow r in tkb.LayHocKyCuaLop(maLop).Rows) ds.Add(Convert.ToInt32(r["HocKy"]));
                GiaoDien.GanComboSo(cboHocKy, ds, "Học kỳ ", false);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
            NapMon();
        }

        /// <summary>Chỉ hiện các môn có trong thời khóa biểu của lớp ở học kỳ đã chọn.</summary>
        private void NapMon()
        {
            try
            {
                dangNap = true;
                string maLop = GiaoDien.GiaTri(cboLop);
                int? hocKy = GiaoDien.GiaTriSo(cboHocKy);
                DataTable mon = (maLop != null && hocKy.HasValue) ? tkb.LayMonTheoLopHocKy(maLop, hocKy.Value) : null;
                GiaoDien.GanCombo(cboMonHoc, mon, "MaMon", "TenMon", false);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
            finally
            {
                dangNap = false;
            }
            TaiLuoi();
        }

        // ================== Lưới nhập điểm ==================
        private void TaiLuoi()
        {
            string maLop = GiaoDien.GiaTri(cboLop);
            string maMon = GiaoDien.GiaTri(cboMonHoc);
            int? hocKy = GiaoDien.GiaTriSo(cboHocKy);
            int? lanThi = GiaoDien.GiaTriSo(cboLanThi);
            if (maLop == null || maMon == null || !hocKy.HasValue || !lanThi.HasValue)
            {
                phanTrang.GanDuLieu(null);
                dgvDanhSach.DataSource = null;
                luoiLop = null;
                lblThongTin.Text = maLop == null ? "" : "Lớp " + maLop + " chưa có môn học nào trong thời khóa biểu học kỳ này.";
                return;
            }

            try
            {
                DataTable goc = bll.LayDanhSachNhap(maLop, hocKy.Value, maMon, lanThi.Value);

                // Cột điểm để dạng chuỗi để gõ được cả "8,5" lẫn "8.5"; DiemGoc = điểm đã lưu trong CSDL
                var dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("MaSV", typeof(string));
                dt.Columns.Add("TenSV", typeof(string));
                dt.Columns.Add("NgaySinh", typeof(DateTime));
                dt.Columns.Add("Diem", typeof(string));
                dt.Columns.Add("XepLoai", typeof(string));
                dt.Columns.Add("DiemGoc", typeof(string));
                int stt = 1;
                foreach (DataRow r in goc.Rows)
                {
                    string diem = r["Diem"] == DBNull.Value ? "" : Convert.ToDecimal(r["Diem"]).ToString("0.##");
                    dt.Rows.Add(stt++, r["MaSV"], r["TenSV"], r["NgaySinh"], diem, r["XepLoai"], diem);
                }

                luoiLop = maLop;
                luoiMon = maMon;
                luoiHocKy = hocKy.Value;
                luoiLanThi = lanThi.Value;
                phanTrang.GanDuLieu(dt);
                CapNhatThongTin(null);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void CapNhatThongTin(string thongBaoThem)
        {
            DataTable dt = phanTrang.DuLieu;
            if (dt == null || luoiLop == null) { lblThongTin.Text = ""; return; }
            int coDiem = 0;
            foreach (DataRow r in dt.Rows) if (!string.IsNullOrWhiteSpace(Convert.ToString(r["DiemGoc"]))) coDiem++;
            lblThongTin.Text = string.Format("Lớp {0} - Sĩ số {1} - Đã có điểm {2}/{1}. Gõ điểm vào cột \"Điểm\", điểm được lưu ngay.{3}",
                luoiLop, dt.Rows.Count, coDiem, thongBaoThem == null ? "" : "  " + thongBaoThem);
        }

        private bool LaCotDiem(int cot)
        {
            return cot >= 0 && dgvDanhSach.Columns[cot].DataPropertyName == "Diem";
        }

        private void dgvDanhSach_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!LaCotDiem(e.ColumnIndex)) return;
            decimal? d;
            if (!DiemBLL.DocDiem(e.FormattedValue, out d))
            {
                GiaoDien.Loi("Điểm không hợp lệ. Nhập số từ 0 đến 10 (VD: 7.5) hoặc để trống.");
                e.Cancel = true;
            }
        }

        /// <summary>Gõ xong một ô điểm -> lưu (hoặc xóa nếu để trống) vào CSDL ngay.</summary>
        private void dgvDanhSach_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!LaCotDiem(e.ColumnIndex) || luoiLop == null) return;
            var dong = dgvDanhSach.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (dong == null) return;

            decimal? d;
            DiemBLL.DocDiem(dong["Diem"], out d);
            string moi = d.HasValue ? d.Value.ToString("0.##") : "";
            dong["Diem"] = moi;
            dong.EndEdit();
            if (moi == Convert.ToString(dong["DiemGoc"])) return;   // không thay đổi

            string maSV = dong["MaSV"].ToString();
            var dsLuu = new List<DTO.Diem>();
            var dsXoa = new List<string>();
            if (d.HasValue)
                dsLuu.Add(new DTO.Diem
                {
                    MaSV = maSV, MaLop = luoiLop, MaMon = luoiMon,
                    HocKy = luoiHocKy, LanThi = luoiLanThi, DiemSo = d.Value
                });
            else
                dsXoa.Add(maSV);

            var loi = new List<string>();
            bll.LuuDanhSach(dsLuu, dsXoa, luoiLop, luoiMon, luoiLanThi, loi);
            if (loi.Count > 0)
            {
                // Trả lại điểm cũ nếu lưu lỗi
                dong["Diem"] = dong["DiemGoc"];
                dong.EndEdit();
                GiaoDien.Loi(string.Join("\n", loi));
                return;
            }

            dong["DiemGoc"] = moi;
            dong["XepLoai"] = d.HasValue ? DTO.Diem.XepLoaiTheoDiem(d.Value) : "";
            dong.EndEdit();
            CapNhatThongTin(d.HasValue ? "Đã lưu điểm " + maSV + "." : "Đã xóa điểm " + maSV + ".");
        }

        /// <summary>Yêu cầu 7: in danh sách điểm theo lớp, môn, lần thi.</summary>
        private void InDanhSach()
        {
            if (luoiLop == null) { GiaoDien.ThongBao("Hãy chọn lớp, học kỳ, môn và lần thi."); return; }
            dgvDanhSach.EndEdit();
            try
            {
                DataTable dt = baoCao.DiemTheoLopMon(luoiLop, luoiMon, luoiLanThi);
                fBaoCao.HienThi(this, LoaiBaoCao.DiemTheoLopMon, dt,
                    string.Format("Danh sách điểm lớp {0} - môn {1} - lần thi {2}", luoiLop, cboMonHoc.Text, luoiLanThi));
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        // ---- Các hàm sự kiện do Designer tạo sẵn ----
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label13_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void btnXemBangDiem_Click(object sender, EventArgs e) { }
    }
}
