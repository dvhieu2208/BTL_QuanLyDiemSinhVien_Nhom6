using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyDiemSinhVien
{
    public partial class fQuanLyDiem : Form
    {
        private readonly DiemBLL bll = new DiemBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private readonly ThoiKhoaBieuBLL tkb = new ThoiKhoaBieuBLL();
        private readonly BaoCaoBLL baoCao = new BaoCaoBLL();
        private bool dangNap;
        private bool coThayDoi;

        private string luoiLop, luoiMon;
        private int luoiHocKy, luoiLanThi;
        public fQuanLyDiem()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void btnXemBangDiem_Click(object sender, EventArgs e)
        {

        }

        private void fQuanLyDiem_Load(object sender, EventArgs e)
        {

        }
        private void CauHinhGiaoDien()
        {
            if (btnTimKiem != null)
            {
                btnTimKiem.Click += (s, e) => TaiLuoi();
            }

            if (btnIn != null)
            {
                btnIn.Click += (s, e) => InDanhSach();
            }

            if (dgvDanhSach != null)
            {
                GiaoDien.DinhDangLuoi(dgvDanhSach, false);
                dgvDanhSach.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvDanhSach.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dgvDanhSach.CellValidating += dataGridView1_CellValidating;
                dgvDanhSach.CellEndEdit += dataGridView1_CellEndEdit;
                dgvDanhSach.DataError += (s, e) => { e.ThrowException = false; };
            }

            if (cboLop != null) cboLop.SelectedIndexChanged += (s, e) => { if (!dangNap) DoiLop(); };
            if (cboHocKy != null) cboHocKy.SelectedIndexChanged += (s, e) => { if (!dangNap) NapMon(); };
            if (cboMonHoc != null) cboMonHoc.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiLuoi(); };
            if (cboLanThi != null) cboLanThi.SelectedIndexChanged += (s, e) => { if (!dangNap) TaiLuoi(); };
        }

        private void NapLop()
        {
            try
            {
                dangNap = true;
                if (cboLop != null) GiaoDien.GanCombo(cboLop, danhMuc.LayLopTheoKhoa(null), "MaLop", "MaLop", false);
                if (cboLanThi != null) GiaoDien.GanComboSo(cboLanThi, new[] { 1, 2, 3 }, "Lần ", false);
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

        private void DoiLop()
        {
            try
            {
                dangNap = true;
                string maLop = cboLop != null ? GiaoDien.GiaTri(cboLop) : null;
                DataTable hk = maLop == null ? null : tkb.LayHocKyCuaLop(maLop);
                var ds = new List<int>();
                if (hk != null) foreach (DataRow r in hk.Rows) ds.Add(Convert.ToInt32(r["HocKy"]));
                if (cboHocKy != null) GiaoDien.GanComboSo(cboHocKy, ds, "Học kỳ ", false);
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

        private void NapMon()
        {
            try
            {
                dangNap = true;
                string maLop = cboLop != null ? GiaoDien.GiaTri(cboLop) : null;
                int? hocKy = cboHocKy != null ? GiaoDien.GiaTriSo(cboHocKy) : (int?)null;
                DataTable mon = (maLop != null && hocKy.HasValue) ? tkb.LayMonTheoLopHocKy(maLop, hocKy.Value) : null;
                if (cboMonHoc != null) GiaoDien.GanCombo(cboMonHoc  , mon, "MaMon", "TenMon", false);
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

        private void TaiLuoi()
        {
            if (!HoiLuuNeuCan()) return;
            if (dgvDanhSach == null) return;

            string maLop = cboLop != null ? GiaoDien.GiaTri(cboLop) : null;
            string maMon = cboMonHoc != null ? GiaoDien.GiaTri(cboMonHoc) : null;
            int? hocKy = cboHocKy != null ? GiaoDien.GiaTriSo(cboHocKy) : (int?)null;
            int? lanThi = cboLanThi != null ? GiaoDien.GiaTriSo(cboLanThi) : (int?)null;

            if (maLop == null || maMon == null || !hocKy.HasValue || !lanThi.HasValue)
            {
                dgvDanhSach.DataSource = null;
                if (lblThongTin != null)
                {
                    lblThongTin.Text = maLop == null ? "" : "Lớp " + maLop + " chưa có môn học nào trong thời khóa biểu học kỳ này.";
                }
                luoiLop = null;
                return;
            }

            try
            {
                DataTable goc = bll.LayDanhSachNhap(maLop, hocKy.Value, maMon, lanThi.Value);

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

                dgvDanhSach.DataSource = dt;
                GiaoDien.HienCot(dgvDanhSach, "STT=STT", "MaSV=Mã SV", "TenSV=Họ và tên",
                    "NgaySinh=Ngày sinh", "Diem=Điểm (nhập)", "XepLoai=Xếp loại");
                foreach (DataGridViewColumn c in dgvDanhSach.Columns) c.ReadOnly = c.Name != "Diem";

                if (dgvDanhSach.Columns.Contains("Diem"))
                {
                    DataGridViewColumn cotDiem = dgvDanhSach.Columns["Diem"];
                    cotDiem.DefaultCellStyle.BackColor = Color.LightYellow;
                    cotDiem.DefaultCellStyle.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
                    cotDiem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvDanhSach.Columns.Contains("STT"))
                {
                    dgvDanhSach.Columns["STT"].FillWeight = 30;
                }

                luoiLop = maLop;
                luoiMon = maMon;
                luoiHocKy = hocKy.Value;
                luoiLanThi = lanThi.Value;
                coThayDoi = false;
                CapNhatThongTin();
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void CapNhatThongTin()
        {
            if (lblThongTin == null) return;
            var dt = dgvDanhSach.DataSource as DataTable;
            if (dt == null) { lblThongTin.Text = ""; return; }
            int coDiem = 0;
            foreach (DataRow r in dt.Rows) if (!string.IsNullOrWhiteSpace(Convert.ToString(r["Diem"]))) coDiem++;
            lblThongTin.Text = string.Format(
                "Lớp {0} - Sĩ số {1} - Đã có điểm {2}/{1}. Gõ điểm trực tiếp vào cột \"Điểm\".{3}",
                luoiLop, dt.Rows.Count, coDiem, coThayDoi ? "  (*) Có thay đổi chưa lưu" : "");
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvDanhSach.Columns[e.ColumnIndex].Name != "Diem") return;
            decimal? d;
            if (!DiemBLL.DocDiem(e.FormattedValue, out d))
            {
                dgvDanhSach.Rows[e.RowIndex].ErrorText = "Điểm phải là số từ 0 đến 10";
                GiaoDien.Loi("Điểm không hợp lệ. Nhập số từ 0 đến 10 (VD: 7.5) hoặc để trống.");
                e.Cancel = true;
            }
            else
            {
                dgvDanhSach.Rows[e.RowIndex].ErrorText = "";
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDanhSach.Columns[e.ColumnIndex].Name != "Diem") return;
            DataGridViewRow dong = dgvDanhSach.Rows[e.RowIndex];
            decimal? d;
            DiemBLL.DocDiem(dong.Cells["Diem"].Value, out d);
            dong.Cells["Diem"].Value = d.HasValue ? d.Value.ToString("0.##") : "";
            dong.Cells["XepLoai"].Value = d.HasValue ? DTO.Diem.XepLoaiTheoDiem(d.Value) : "";
            if (Convert.ToString(dong.Cells["Diem"].Value) != Convert.ToString(dong.Cells["DiemGoc"].Value))
            {
                coThayDoi = true;
                dong.DefaultCellStyle.ForeColor = Color.Blue;
            }
            CapNhatThongTin();
        }

        private bool LuuDiem(bool thongBao)
        {
            if (dgvDanhSach == null) return true;
            dgvDanhSach.EndEdit();
            var dt = dgvDanhSach.DataSource as DataTable;
            if (dt == null || luoiLop == null) { if (thongBao) GiaoDien.ThongBao("Chưa có danh sách để lưu."); return true; }

            var dsLuu = new List<DTO.Diem>();
            var dsXoa = new List<string>();
            foreach (DataRow r in dt.Rows)
            {
                string moi = Convert.ToString(r["Diem"]);
                string cu = Convert.ToString(r["DiemGoc"]);
                if (moi == cu) continue;
                decimal? d;
                DiemBLL.DocDiem(moi, out d);
                if (d.HasValue)
                    dsLuu.Add(new DTO.Diem
                    {
                        MaSV = r["MaSV"].ToString(),
                        MaLop = luoiLop,
                        MaMon = luoiMon,
                        HocKy = luoiHocKy,
                        LanThi = luoiLanThi,
                        DiemSo = d.Value
                    });
                else
                    dsXoa.Add(r["MaSV"].ToString());
            }

            if (dsLuu.Count == 0 && dsXoa.Count == 0)
            {
                if (thongBao) GiaoDien.ThongBao("Không có thay đổi nào cần lưu.");
                coThayDoi = false;
                return true;
            }

            var loi = new List<string>();
            int soLuu = bll.LuuDanhSach(dsLuu, dsXoa, luoiLop, luoiMon, luoiLanThi, loi);
            coThayDoi = false;
            if (loi.Count > 0)
                GiaoDien.Loi("Đã lưu " + soLuu + " điểm. Một số dòng bị lỗi:\n" + string.Join("\n", loi));
            else if (thongBao)
                GiaoDien.ThongBao("Đã lưu " + soLuu + " điểm" + (dsXoa.Count > 0 ? ", xóa " + dsXoa.Count + " điểm." : "."));

            if (thongBao) TaiLuoi();
            return loi.Count == 0;
        }

        private bool HoiLuuNeuCan()
        {
            if (!coThayDoi) return true;
            DialogResult kq = MessageBox.Show("Có điểm vừa nhập chưa được lưu. Lưu lại không?",
                "Điểm chưa lưu", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (kq == DialogResult.Cancel) return false;
            if (kq == DialogResult.Yes) LuuDiem(false);
            coThayDoi = false;
            return true;
        }

        /// <summary>Yêu cầu 7: in danh sách điểm theo lớp, môn, lần thi.</summary>
        private void InDanhSach()
        {
            if (luoiLop == null) { GiaoDien.ThongBao("Hãy chọn lớp, học kỳ, môn và lần thi."); return; }
            if (!HoiLuuNeuCan()) return;
            try
            {
                DataTable dt = baoCao.DiemTheoLopMon(luoiLop, luoiMon, luoiLanThi);
                fBaoCao.HienThi(this, LoaiBaoCao.DiemTheoLopMon, dt,
                    string.Format("Danh sách điểm lớp {0} - môn {1} - lần thi {2}", luoiLop, cboMon != null ? cboMon.Text : "", luoiLanThi));
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }
    }
}
