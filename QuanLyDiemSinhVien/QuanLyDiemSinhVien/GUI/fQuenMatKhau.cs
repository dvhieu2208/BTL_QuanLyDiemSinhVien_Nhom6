using System;
using System.Windows.Forms;
using QuanLyDiemSinhVien.BLL;

namespace QuanLyDiemSinhVien.GUI
{

    public partial class fQuenMatKhau : Form
    {
        private readonly DangNhapBLL bll = new DangNhapBLL();

        public string TenDangNhapBanDau { get; set; }

        public fQuenMatKhau()
        {
            InitializeComponent();
            AcceptButton = btnXacNhan;
            txtTenDangNhap.Leave += (s, e) => HienCauHoi();
            btnXacNhan.Click += btnXacNhan_Click;
            btnMatKhau.Click += (s, e) => txtMatKhau.PasswordChar = txtMatKhau.PasswordChar == '\0' ? '•' : '\0';
            btnXNMatKhau.Click += (s, e) => txtXNMatKhau.PasswordChar = txtXNMatKhau.PasswordChar == '\0' ? '•' : '\0';
            Load += (s, e) =>
            {
                if (string.IsNullOrEmpty(TenDangNhapBanDau)) return;
                txtTenDangNhap.Text = TenDangNhapBanDau;
                HienCauHoi();
            };
        }

        private void HienCauHoi()
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text)) return;
            try
            {
                string cauHoi = bll.LayCauHoiBaoMat(txtTenDangNhap.Text);
                lbCauHoiBaoMat.Text = string.IsNullOrEmpty(cauHoi)
                    ? "Câu hỏi bảo mật: (không tìm thấy tài khoản)"
                    : "Câu hỏi bảo mật: " + cauHoi;
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                string loi;
                if (bll.DatLaiMatKhau(txtTenDangNhap.Text, textBox1.Text, txtMatKhau.Text, txtXNMatKhau.Text, out loi))
                {
                    GiaoDien.ThongBao("Đổi mật khẩu thành công! Hãy đăng nhập bằng mật khẩu mới.");
                    Close();
                }
                else
                {
                    GiaoDien.Loi(loi);
                }
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
    }
}
