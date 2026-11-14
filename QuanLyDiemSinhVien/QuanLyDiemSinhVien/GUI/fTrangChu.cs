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
using System.Windows.Forms.DataVisualization.Charting;

namespace QuanLyDiemSinhVien
{
    public partial class fTrangChu : Form
    {
        private TrangChuBLL bll = new TrangChuBLL();
        public fTrangChu()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);  
            LoadThongKe();
            LoadBieuDo();
        }

        public void MoChucNang(Type loaiForm)
        {
            if (loaiForm == null || loaiForm == typeof(fTrangChu)) return;

            Type tiepTheo = loaiForm;
            while (tiepTheo != null)
            {
                DieuHuong.FormTiepTheo = null;
                using (Form f = (Form)Activator.CreateInstance(tiepTheo))
                {
                    f.ShowDialog(this);
                }
                if (DieuHuong.DangXuat)
                {
                    Close();
                    return;
                }
                tiepTheo = DieuHuong.FormTiepTheo;
            }
            LoadThongKe();
            LoadBieuDo();
        }
        private void LoadThongKe()
        {
            try
            {
                lbTongSinhVien.Text = bll.GetTongSinhVien().ToString();
                lbTongLopHoc.Text = bll.GetTongLop().ToString();
                lbTongMonHoc.Text = bll.GetTongMonHoc().ToString();
                decimal tyLe = bll.GetTyLeDat();
                lbTyLeDat.Text = tyLe.ToString("0.00") + "%";
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

                chBieuDo.Series.Clear();
                chBieuDo.Titles.Clear();
                chBieuDo.ChartAreas.Clear(); // Thêm dòng này để làm sạch khu vực biểu đồ cũ

                // Tạo ChartArea cho biểu đồ cột
                ChartArea chartArea = new ChartArea("MainArea");
                chartArea.AxisX.MajorGrid.Enabled = false; // Tắt lưới dọc cho gọn (tùy chọn)
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisX.Interval = 1; // Hiển thị đầy đủ tất cả tên các cột trên trục X
                chBieuDo.ChartAreas.Add(chartArea);


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

                chBieuDo.Series.Add(series);

                // Kiểm tra và bật Legend nếu tồn tại
                if (chBieuDo.Legends.Count > 0)
                {
                    chBieuDo.Legends[0].Enabled = true;
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

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
   
        }

        private void fTrangChu_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
