using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    internal static class DieuHuong
    {
        /// <summary>Form cần mở tiếp theo sau khi form hiện tại đóng.</summary>
        public static Type FormTiepTheo { get; set; }
        public static bool DangXuat { get; set; }

        private static readonly Color MauNenMenu = Color.RoyalBlue;
        private static readonly Color MauDangChon = Color.Navy;

        private static Type TimDich(string chu, out bool laDangXuat)
        {
            laDangXuat = false;
            switch (chu.Trim())
            {
                case "Trang chủ": return typeof(fTrangChu);
                case "Sinh viên": return typeof(fSinhVien);
                case "Môn học": return typeof(fQuanLyMonHoc);
                case "Lớp học phần": return typeof(fQuanLyLopHocPhan);
                case "Điểm": return typeof(fQuanLyDiem);
                case "Xem bảng điểm": return typeof(fXemBangDiem);
                case "Báo cáo": return typeof(fThongKe);
                case "Cài đặt":
                case "Quản lý lớp": return typeof(fQuanLyLop);
                case "Đăng xuất": laDangXuat = true; return null;
                default: return null;
            }
        }

        private static IEnumerable<Control> TatCa(Control cha)
        {
            foreach (Control c in cha.Controls)
            {
                yield return c;
                foreach (Control con in TatCa(c)) yield return con;
            }
        }

        public static void GanMenu(Form form)
        {
            Control menu = TatCa(form).OfType<Panel>().FirstOrDefault(p => p.Dock == DockStyle.Left);
            if (menu == null) menu = form;

            foreach (Button nut in TatCa(menu).OfType<Button>().ToList())
            {
                bool laDangXuat;
                Type dich = TimDich(nut.Text, out laDangXuat);
                if (dich == null && !laDangXuat) continue;

                if (nut.Text.Trim() == "Cài đặt") nut.Text = "Quản lý lớp";
                nut.Cursor = Cursors.Hand;

                if (dich == form.GetType())
                {
                    nut.FlatStyle = FlatStyle.Flat;
                    nut.UseVisualStyleBackColor = false;
                    nut.BackColor = MauDangChon;
                }

                Type dichCuoi = dich;
                bool dangXuatCuoi = laDangXuat;
                nut.Click += (s, e) => ChuyenDen(form, dichCuoi, dangXuatCuoi);
            }

            var nhan = TatCa(menu).OfType<Label>()
                                  .Where(l => l.Text.StartsWith("label"))
                                  .OrderBy(l => l.Top).ToList();
            if (menu != form && nhan.Count >= 2)
            {
                nhan[0].Text = string.IsNullOrEmpty(BLL.PhienDangNhap.TenDangNhap) ? "Admin" : BLL.PhienDangNhap.TenDangNhap;
                nhan[1].Text = "Quản trị viên";
            }
        }

        private static bool BiHuyDong(Form f)
        {
            if (f.IsDisposed) return false;
            return f.Modal ? f.DialogResult == DialogResult.None : f.Visible;
        }

        private static void ChuyenDen(Form hienTai, Type dich, bool dangXuat)
        {
            if (dangXuat)
            {
                if (!GiaoDien.XacNhan("Bạn có chắc muốn đăng xuất?")) return;
                DangXuat = true;
                FormTiepTheo = null;
                hienTai.Close();
                if (BiHuyDong(hienTai)) DangXuat = false;
                return;
            }

            var trangChu = hienTai as fTrangChu;
            if (trangChu != null)
            {
                trangChu.MoChucNang(dich);
                return;
            }

            if (dich == hienTai.GetType()) return;
            FormTiepTheo = dich == typeof(fTrangChu) ? null : dich;
            hienTai.Close();
            if (BiHuyDong(hienTai))
                FormTiepTheo = null; 
        }
    }
}
