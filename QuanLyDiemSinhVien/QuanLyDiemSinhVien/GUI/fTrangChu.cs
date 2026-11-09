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
                label11.Text = bll.GetTongLop().ToString();
                label15.Text = bll.GetTongMonHoc().ToString();
                decimal tyLe = bll.GetTyLeDat();
                label18.Text = tyLe.ToString("0.00") + "%";
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
                chart1.ChartAreas.Clear(); // Thêm dòng này để làm sạch khu vực biểu đồ cũ

                // Tạo ChartArea cho biểu đồ cột
                ChartArea chartArea = new ChartArea("MainArea");
                chartArea.AxisX.MajorGrid.Enabled = false; // Tắt lưới dọc cho gọn (tùy chọn)
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.Interval = 1; // Hiển thị đầy đủ tất cả tên các cột trên trục X
                chart1.ChartAreas.Add(chartArea);


                Series series = new Series("Xếp loại");

                // Đổi kiểu biểu đồ từ Doughnut sang Column (Cột)
                series.ChartType = SeriesChartType.Column;
                series.IsValueShownAsLabel = true;

                foreach (DataRow row in dt.Rows)
                {
                    string ten = row["KetQua"].ToString(); // Ví dụ: Xuất sắc, Giỏi, Khá, Trung bình, Yếu, Học lại
                    int soLuong = Convert.ToInt32(row["SoLuong"]);

                    series.Points.AddXY(ten, soLuong);
                }

                // Tùy chọn màu sắc cho từng cột sinh động hơn (nếu cơ sở dữ liệu trả về đúng tên các mức)
                foreach (DataPoint pt in series.Points)
                {
                    string label = pt.AxisLabel;
                    if (label.Contains("Xuất sắc")) pt.Color = Color.FromArgb(46, 204, 113);      // Xanh lá
                    else if (label.Contains("Giỏi")) pt.Color = Color.FromArgb(52, 152, 219);     // Xanh dương
                    else if (label.Contains("Khá")) pt.Color = Color.FromArgb(241, 196, 15);      // Vàng
                    else if (label.Contains("Trung bình")) pt.Color = Color.FromArgb(230, 126, 34); // Cam
                    else if (label.Contains("Yếu")) pt.Color = Color.FromArgb(231, 76, 60);       // Đỏ
                    else if (label.Contains("Học lại")) pt.Color = Color.FromArgb(149, 165, 166); // Xám
                }

                chart1.Series.Add(series);

                // Kiểm tra và bật Legend nếu tồn tại
                if (chart1.Legends.Count > 0)
                {
                    chart1.Legends[0].Enabled = true;
                }
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
