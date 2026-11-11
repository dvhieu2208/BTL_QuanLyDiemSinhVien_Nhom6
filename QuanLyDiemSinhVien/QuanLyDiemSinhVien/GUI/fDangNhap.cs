using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;


namespace QuanLyDiemSinhVien
{
    public partial class fDangNhap : Form
    {
        private DangNhapBLL bll = new DangNhapBLL();
        public fDangNhap()
        {
            InitializeComponent();
            this.AcceptButton = btnDangNhap;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool kq;
            try
            {
                kq = bll.DangNhap(taiKhoan, matKhau);
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
                return;
            }

            if (kq)
            {
                DieuHuong.DangXuat = false;
                this.Hide();
                using (fTrangChu trangChu = new fTrangChu())
                {
                    trangChu.ShowDialog();
                }

                if (DieuHuong.DangXuat)
                {
                    DieuHuong.DangXuat = false;
                    txtMatKhau.Clear();
                    this.Show();
                    txtMatKhau.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Clear();
                txtMatKhau.Focus();
            }
        }

        private void fDangNhap_Load(object sender, EventArgs e)
        {

        }

        bool isVisible = false;
        private void button1_Click(object sender, EventArgs e)
        {
            isVisible = !isVisible;
            if (isVisible)
            {
                txtMatKhau.PasswordChar = '\0';
            }
            else
            {
                txtMatKhau.PasswordChar = '•';
            }
        }

        private void lkQuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (fQuenMatKhau quenMatKhau = new fQuenMatKhau())
            {
                quenMatKhau.TenDangNhapBanDau = txtTenDangNhap.Text.Trim();
                quenMatKhau.ShowDialog(this);
            }
            txtMatKhau.Clear();
            txtMatKhau.Focus();
        }
    }
}
