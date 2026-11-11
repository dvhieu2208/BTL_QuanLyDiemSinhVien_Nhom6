using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    /// <summary>Các loại báo cáo của chương trình.</summary>
    public enum LoaiBaoCao
    {
        DiemTheoLopMon,     // Yêu cầu 7 - Reports\rptDiemLopMon.rpt    - bảng DiemLopMon
        BangDiemSinhVien,   // Yêu cầu 8 - Reports\rptBangDiemSV.rpt    - bảng BangDiemSV
        ThoiKhoaBieu        // Yêu cầu 9 - Reports\rptThoiKhoaBieu.rpt  - bảng ThoiKhoaBieu
    }

    /// <summary>
    /// Form hiển thị báo cáo (chạy giống bản đầu tiên):
    /// - Nếu có file Reports\*.rpt  -> mở bằng Crystal Report.
    /// - Nếu chưa có file .rpt       -> hiện dữ liệu báo cáo ở dgvDuLieu, KHÔNG khởi động Crystal,
    ///   nên máy chưa cài Crystal Reports Runtime vẫn chạy bình thường.
    /// Dữ liệu lấy từ thủ tục lưu trữ (sp_BaoCao_...).
    /// </summary>
    public partial class fBaoCao : Form
    {
        private IDisposable report;   // ReportDocument của Crystal (nếu có dùng)

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
            using (var f = new fBaoCao())
            {
                f.NapBaoCao(loai, duLieu, tieuDe);
                f.ShowDialog(chuSoHuu);
            }
        }

        public fBaoCao()
        {
            InitializeComponent();
            FormClosed += (s, e) =>
            {
                if (report != null) report.Dispose();
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
                    MoCrystal(duongDan, dt);
                    return;
                }
                catch (Exception ex)
                {
                    var chiTiet = new System.Text.StringBuilder();
                    for (Exception e = ex; e != null; e = e.InnerException) chiTiet.AppendLine(e.Message);
                    GiaoDien.Loi("Không mở được Crystal Report, chuyển sang hiển thị dạng bảng.\n\n" + chiTiet);
                    if (dt.DataSet != null) dt.DataSet.Tables.Remove(dt);
                }
            }

            // Chưa có file .rpt (hoặc Crystal lỗi): hiện dữ liệu báo cáo ở lưới
            lblThongBao.Text = tieuDe.ToUpper() + "   (ngày in: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ")";
            dgvDuLieu.DataSource = dt;
        }

        /// <summary>
        /// Chỉ gọi khi có file .rpt. Tách riêng (NoInlining) để thư viện Crystal chỉ được nạp
        /// khi thật sự cần, tránh lỗi trên máy chưa cài Crystal Reports Runtime.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void MoCrystal(string duongDan, DataTable dt)
        {
            var ds = new DataSet("DsBaoCao");
            ds.Tables.Add(dt);

            var rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            rpt.Load(duongDan);
            rpt.SetDataSource(ds);
            report = rpt;

            var viewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer
            {
                Dock = DockStyle.Fill,
                ReportSource = rpt,
                ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None
            };
            Controls.Add(viewer);
            viewer.BringToFront();
        }
    }
}
