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

            bool kq = bll.DangNhap(taiKhoan, matKhau);
            if (kq)
            {
                fTrangChu trangChu = new fTrangChu();
                this.Hide();
                trangChu.ShowDialog();
                this.Close();
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
            fQuenMatKhau quenMatKhau = new fQuenMatKhau();
            quenMatKhau.ShowDialog();
            this.Hide();
        }
    }
}
