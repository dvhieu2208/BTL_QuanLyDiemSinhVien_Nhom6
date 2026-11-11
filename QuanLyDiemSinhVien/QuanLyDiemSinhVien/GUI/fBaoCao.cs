using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    /// <summary>Các loại báo cáo Crystal Report của chương trình.</summary>
    public enum LoaiBaoCao
    {
        DiemTheoLopMon,     // Yêu cầu 7 - Reports\rptDiemLopMon.rpt    - bảng DiemLopMon
        BangDiemSinhVien,   // Yêu cầu 8 - Reports\rptBangDiemSV.rpt    - bảng BangDiemSV
        ThoiKhoaBieu        // Yêu cầu 9 - Reports\rptThoiKhoaBieu.rpt  - bảng ThoiKhoaBieu
    }

    /// <summary>
    /// Form hiển thị báo cáo bằng Crystal Report (crystalReportViewer1).
    /// Dữ liệu lấy từ thủ tục lưu trữ, đưa vào DataSet "DsBaoCao" (cấu trúc trong Reports\DsBaoCao.xsd)
    /// rồi gán cho file .rpt. Nếu chưa có file .rpt thì hiện tạm dữ liệu ở dgvDuLieu.
    /// </summary>
    public partial class fBaoCao : Form
    {
        private CrystalDecisions.CrystalReports.Engine.ReportDocument report;

        public static string TenFile(LoaiBaoCao loai)
        {
            switch (loai)
            {
                case LoaiBaoCao.DiemTheoLopMon: return "rptDiemLopMon.rpt";
                case LoaiBaoCao.BangDiemSinhVien: return "rptBangDiemSV.rpt";
                default: return "rptThoiKhoaBieu.rpt";
            }
        }

        public static string TenBang(LoaiBaoCao loai)
        {
            switch (loai)
            {
                case LoaiBaoCao.DiemTheoLopMon: return "DiemLopMon";
                case LoaiBaoCao.BangDiemSinhVien: return "BangDiemSV";
                default: return "ThoiKhoaBieu";
            }
        }

        public static void HienThi(IWin32Window chuSoHuu, LoaiBaoCao loai, DataTable duLieu, string tieuDe)
        {
            fBaoCao f;
            try
            {
                f = new fBaoCao();   // tạo CrystalReportViewer -> cần Crystal Reports Runtime
            }
            catch (Exception ex)
            {
                BaoLoiCrystal(ex);
                return;
            }
            using (f)
            {
                f.NapBaoCao(loai, duLieu, tieuDe);
                f.ShowDialog(chuSoHuu);
            }
        }

        /// <summary>Hiện nguyên nhân gốc khi không khởi tạo được Crystal Report.</summary>
        private static void BaoLoiCrystal(Exception ex)
        {
            var chiTiet = new System.Text.StringBuilder();
            for (Exception e = ex; e != null; e = e.InnerException)
                chiTiet.AppendLine("- " + e.GetType().Name + ": " + e.Message);

            string cheDo = Environment.Is64BitProcess ? "64-bit" : "32-bit";
            MessageBox.Show(
                "Không khởi tạo được Crystal Report.\n\n" +
                "Chương trình đang chạy " + cheDo + ", máy cần cài Crystal Reports Runtime " + cheDo + ".\n" +
                "Cách sửa: cài CRRuntime_" + (Environment.Is64BitProcess ? "64" : "32") + "bit_13_0_xx.msi, " +
                "hoặc đổi Platform target của project cho khớp với bản runtime đã cài.\n\n" +
                "Chi tiết lỗi:\n" + chiTiet,
                "Lỗi Crystal Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public fBaoCao()
        {
            InitializeComponent();
            FormClosed += (s, e) =>
            {
                if (report != null)
                {
                    report.Close();
                    report.Dispose();
                }
            };
        }

        private void NapBaoCao(LoaiBaoCao loai, DataTable duLieu, string tieuDe)
        {
            Text = tieuDe;

            // Thêm cột tiêu đề + ngày in để dùng trên report
            DataTable dt = duLieu.Copy();
            dt.TableName = TenBang(loai);
            if (!dt.Columns.Contains("TieuDe")) dt.Columns.Add("TieuDe", typeof(string));
            if (!dt.Columns.Contains("NgayIn")) dt.Columns.Add("NgayIn", typeof(DateTime));
            foreach (DataRow r in dt.Rows)
            {
                r["TieuDe"] = tieuDe;
                r["NgayIn"] = DateTime.Now;
            }

            string duongDan = Path.Combine(Application.StartupPath, "Reports", TenFile(loai));
            if (File.Exists(duongDan))
            {
                try
                {
                    var ds = new DataSet("DsBaoCao");
                    ds.Tables.Add(dt);
                    report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    report.Load(duongDan);
                    report.SetDataSource(ds);
                    crystalReportViewer1.ReportSource = report;
                    return;
                }
                catch (Exception ex)
                {
                    if (report != null) { report.Dispose(); report = null; }
                    if (dt.DataSet != null) dt.DataSet.Tables.Remove(dt);
                    HienBangTam(dt, "Không mở được báo cáo Crystal: " + ex.Message);
                    return;
                }
            }

            HienBangTam(dt, "Chưa có file báo cáo " + duongDan +
                            "\nHãy tạo file .rpt theo hướng dẫn trong README (mục Crystal Report). Dưới đây là dữ liệu của báo cáo:");
        }

        /// <summary>Khi chưa có file .rpt: hiện dữ liệu ở lưới thay cho Crystal Report.</summary>
        private void HienBangTam(DataTable dt, string thongBao)
        {
            crystalReportViewer1.Visible = false;
            lblThongBao.Text = thongBao;
            lblThongBao.Visible = true;
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.Visible = true;
        }
    }
}
