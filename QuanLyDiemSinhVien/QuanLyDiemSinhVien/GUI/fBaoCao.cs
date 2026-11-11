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
    public enum LoaiBaoCao
    {
        DiemTheoLopMon,  
        BangDiemSinhVien, 
        ThoiKhoaBieu       
    }
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

        public fBaoCao()
        {
            InitializeComponent();
        }
        
    }
}
