using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QuanLyDiemSinhVien.BLL;
using QuanLyDiemSinhVien.GUI;

namespace QuanLyDiemSinhVien
{

    public partial class fThongKe : Form
    {
        private readonly BaoCaoBLL bll = new BaoCaoBLL();

        private const string TK_XEPLOAI = "Thống kê kết quả học tập";
        private const string TK_DIEMTB = "Điểm trung bình theo môn";

        public fThongKe()
        {
            InitializeComponent();
            DieuHuong.GanMenu(this);
            dgvDanhSach.AutoGenerateColumns = false;
            cboLoaiBaoCao.SelectedIndexChanged += (s, e) => ThongKe();
        }

        private void fThongKe_Load(object sender, EventArgs e)
        {
            try
            {
                if (cboLoaiBaoCao.Items.Count == 0)
                    cboLoaiBaoCao.Items.AddRange(new object[] { TK_XEPLOAI, TK_DIEMTB });
                dgvDanhSach.DataSource = bll.ThongKeSVTheoKhoa();
                cboLoaiBaoCao.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void ThongKe()
        {
            try
            {
                if (Convert.ToString(cboLoaiBaoCao.SelectedItem) == TK_DIEMTB)
                {
                    label7.Text = "Điểm trung bình (lần thi 1) theo môn";
                    VeDiemTrungBinh(bll.ThongKeDiemTBTheoMon(null));
                }
                else
                {
                    label7.Text = "Kết quả học tập (lần thi 1)";
                    VeXepLoai(bll.ThongKeXepLoai(null));
                }
            }
            catch (Exception ex)
            {
                GiaoDien.Loi(ex);
            }
        }

        private void VeXepLoai(DataTable dt)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            var vung = new ChartArea("MainArea");
            vung.AxisX.Interval = 1;
            chart1.ChartAreas.Add(vung);

            var s = new Series("Số lượt thi") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true };
            foreach (DataRow r in dt.Rows)
                s.Points.AddXY(r["KetQua"].ToString(), Convert.ToInt32(r["SoLuong"]));
            chart1.Series.Add(s);
        }

        private void VeDiemTrungBinh(DataTable dt)
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            var vung = new ChartArea("MainArea");
            vung.AxisX.Interval = 1;
            vung.AxisY.Maximum = 10;
            chart1.ChartAreas.Add(vung);

            var s = new Series("Điểm TB") { ChartType = SeriesChartType.Bar, IsValueShownAsLabel = true, Color = Color.RoyalBlue };
            foreach (DataRow r in dt.Rows)
                s.Points.AddXY(r["TenMon"].ToString(), Convert.ToDouble(r["DiemTB"]));
            chart1.Series.Add(s);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ThongKe();
        }

        private void button2_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void chart2_Click(object sender, EventArgs e) { }
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
    }
}
