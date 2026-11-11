using System;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.DTO;

namespace QuanLyDiemSinhVien.GUI
{

    public partial class fMonHocChiTiet : Form
    {
        private readonly MonHocBLL bll = new MonHocBLL();
        private readonly string maSua;   

        public fMonHocChiTiet() : this(null) { }

        public fMonHocChiTiet(string maMon)
        {
            InitializeComponent();
            maSua = maMon;
            Text = maMon == null ? "Thêm môn học" : "Sửa môn học";
            btnHuy.Click += (s, e) => Close();
            Load += (s, e) => HienThi();
        }

        private void HienThi()
        {
            if (maSua == null) return;
            try
            {
                MonHoc m = bll.LayTheoMa(maSua);
                if (m == null) return;
                txtMaMon.Text = m.MaMon;
                txtMaMon.ReadOnly = true;
                txtTenMonHoc.Text = m.TenMon;
                numDVHT.Value = Math.Max(numDVHT.Minimum, Math.Min(numDVHT.Maximum, m.DVHT));
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var m = new MonHoc
            {
                MaMon = txtMaMon.Text.Trim().ToUpper(),
                TenMon = txtTenMonHoc.Text.Trim(),
                DVHT = (int)numDVHT.Value
            };
            string loi;
            bool ok = maSua == null ? bll.Them(m, out loi) : bll.Sua(m, out loi);
            if (!ok) { GiaoDien.Loi(loi); return; }
            DialogResult = DialogResult.OK;
        }
    }
}
