using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    public partial class fLopHocPhanChiTiet : Form
    {
        private readonly ThoiKhoaBieuBLL bll = new ThoiKhoaBieuBLL();
        private readonly DanhMucBLL danhMuc = new DanhMucBLL();
        private readonly string maLopSua, maMonSua, lopMacDinh;
        private readonly int? hocKyMacDinh;
        public fLopHocPhanChiTiet()
        {
            InitializeComponent();
        }
        public fLopHocPhanChiTiet(string maLop, string maMon, string lopMacDinh, int? hocKyMacDinh) : this()
        {
            maLopSua = maLop;
            maMonSua = maMon;
            this.lopMacDinh = lopMacDinh;
            this.hocKyMacDinh = hocKyMacDinh;

            Text = maLop == null ? "Thêm lớp học phần" : "Sửa lớp học phần";
            btnHuy.Click += (s, e) => Close();
            Load += (s, e) => NapDuLieu();
        }
        private void NapDuLieu()
        {
            try
            {
                GiaoDien.GanCombo(cboLop, danhMuc.LayLopTheoKhoa(null), "MaLop", "TenLop", false);
                GiaoDien.GanCombo(cboMonHoc, danhMuc.LayMonHoc(), "MaMon", "TenMon", false);
                GiaoDien.GanCombo(cboPhongHoc, danhMuc.LayPhongHoc(), "MaPhong", "TenPhong", false);

                var thu = new DataTable();
                thu.Columns.Add("Ma"); thu.Columns.Add("Ten");
                for (int i = 2; i <= 7; i++) thu.Rows.Add(i.ToString(), "Thứ " + i);
                thu.Rows.Add("8", "Chủ nhật");
                GiaoDien.GanCombo(cboThu, thu, "Ma", "Ten", false);

                var ca = new DataTable();
                ca.Columns.Add("Ma"); ca.Columns.Add("Ten");
                ca.Rows.Add("1", "Ca 1 - Tiết 1-3 (7h00)");
                ca.Rows.Add("2", "Ca 2 - Tiết 4-6 (9h35)");
                ca.Rows.Add("3", "Ca 3 - Tiết 7-9 (13h00)");
                ca.Rows.Add("4", "Ca 4 - Tiết 10-12 (15h35)");
                GiaoDien.GanCombo(cboCaHoc, ca, "Ma", "Ten", false);

                if (maLopSua == null)
                {
                    if (lopMacDinh != null) GiaoDien.ChonGiaTri(cboLop, lopMacDinh);
                    if (hocKyMacDinh.HasValue) numHocKy.Value = hocKyMacDinh.Value;
                    return;
                }

                DataTable dt = bll.LayDanhSach(null, maLopSua, maMonSua, null);
                if (dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];
                GiaoDien.ChonGiaTri(cboLop, maLopSua);
                GiaoDien.ChonGiaTri(cboMonHoc, maMonSua);
                cboLop.Enabled = false;
                cboMonHoc.Enabled = false;
                numHocKy.Value = Convert.ToInt32(r["HocKy"]);
                GiaoDien.ChonGiaTri(cboThu, r["ThuHoc"]);
                GiaoDien.ChonGiaTri(cboCaHoc, r["CaHoc"]);
                GiaoDien.ChonGiaTri(cboPhongHoc, r["MaPhong"]);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }
        private void Luu()
        {
            var t = new LichHoc
            {
                MaLop = GiaoDien.GiaTri(cboLop),
                MaMon = GiaoDien.GiaTri(cboMonHoc),
                HocKy = (int)numHocKy.Value,
                ThuHoc = GiaoDien.GiaTriSo(cboThu) ?? 0,
                CaHoc = GiaoDien.GiaTriSo(cboCaHoc) ?? 0,
                MaPhong = GiaoDien.GiaTri(cboPhongHoc)
            };
            string loi;
            bool ok = maLopSua == null ? bll.Them(t, out loi) : bll.Sua(t, out loi);
            if (!ok) { GiaoDien.Loi(loi); return; }
            DialogResult = DialogResult.OK;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
