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
    public partial class fMonHocChiTiet : Form
    {
        private readonly MonHocBLL bll = new MonHocBLL();
        private readonly string maSua;
        public fMonHocChiTiet()
        {
            InitializeComponent();
        }
        public fMonHocChiTiet(string maMon) : this()
        {
            maSua = maMon;
            Text = maMon == null ? "Thêm môn học" : "Sửa môn học";
            Font = GiaoDien.FontThuong;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            if (txtMaMon != null)
            {
                txtMaMon.CharacterCasing = CharacterCasing.Upper;
                txtMaMon.MaxLength = 10;
            }

            if (numDVHT != null)
            {
                numDVHT.Minimum = 1;
                numDVHT.Maximum = 10;
                numDVHT.Value = 3;
            }

            Control btnLuuCtrl = this.Controls["btnLuu"] ?? this.Controls["button1"];
            Control btnHuyCtrl = this.Controls["btnHuy"] ?? this.Controls["button2"];

            if (btnLuuCtrl != null)
            {
                btnLuuCtrl.Click += (s, e) => Luu();
                AcceptButton = btnLuuCtrl as Button;
            }

            if (btnHuyCtrl != null)
            {
                btnHuyCtrl.Click += (s, e) => Close();
                CancelButton = btnHuyCtrl as Button;
            }

            Load += (s, e) =>
            {
                if (maSua == null) return;
                MonHoc m = bll.LayTheoMa(maSua);
                if (m == null) return;
                if (txtMaMon != null)
                {
                    txtMaMon.Text = m.MaMon;
                    txtMaMon.ReadOnly = true;
                }
                if (txtTenMonHoc != null) txtTenMonHoc.Text = m.TenMon;
                if (numDVHT != null) numDVHT.Value = Math.Max(1, Math.Min(10, m.DVHT));
            };
        }
        private void Luu()
        {
            var m = new MonHoc { MaMon = txtMaMon.Text.Trim(), TenMon = txtTenMonHoc.Text.Trim(), DVHT = (int)numDVHT.Value };
            string loi;
            bool ok = maSua == null ? bll.Them(m, out loi) : bll.Sua(m, out loi);
            if (!ok) { GiaoDien.Loi(loi); return; }
            DialogResult = DialogResult.OK;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {

        }
    }
}
