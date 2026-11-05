using QuanLyDiemSinhVien.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyDiemSinhVien
{
    public partial class fTrangChu : Form
    {
        private TrangChuBLL bll = new TrangChuBLL();
        public fTrangChu()
        {
            InitializeComponent();
            LoadThongKe();
            LoadBieuDo();
        }
        private void LoadThongKe()
        {
            try
            {
                label9.Text = bll.GetTongSinhVien().ToString();
                label10.Text = "Sinh viên hiện có";

                label11.Text = bll.GetTongLop().ToString();
                label13.Text = "Lớp hiện có";

                label15.Text = bll.GetTongMonHoc().ToString();
                label16.Text = "Môn học hiện có";

                decimal tyLe = bll.GetTyLeDat();

                label18.Text = tyLe.ToString("0.00") + "%";
                label19.Text = "Tỷ lệ đạt theo môn";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu thống kê!\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void LoadBieuDo()
        {
            try
            {
                DataTable dt = bll.GetThongKeKetQua();

                chart1.Series.Clear();
                chart1.Titles.Clear();

                chart1.Titles.Add("Thống kê kết quả học tập");

                Series series = new Series("Kết quả");

                series.ChartType = SeriesChartType.Doughnut;
                series.IsValueShownAsLabel = true;

                foreach (DataRow row in dt.Rows)
                {
                    string ten = row["KetQua"].ToString();

                    int soLuong = Convert.ToInt32(
                        row["SoLuong"]
                    );

                    series.Points.AddXY(ten, soLuong);
                }

                chart1.Series.Add(series);
                chart1.Legends[0].Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải biểu đồ: " + ex.Message
                );
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            using (fSinhVien form = new fSinhVien())
            {
                form.ShowDialog(this);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fQuanLyDiem fQuanLyDiem = new fQuanLyDiem();
            fQuanLyDiem.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fXemBangDiem fxembangdiem = new fXemBangDiem();
            fxembangdiem.ShowDialog();
        }

        private void fTrangChu_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            fThongKe fthongke = new fThongKe();
            fthongke.ShowDialog();
        }
    }
}
