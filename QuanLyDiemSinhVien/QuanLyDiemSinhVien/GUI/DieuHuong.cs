using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    /// <summary>
    /// Gắn sự kiện cho thanh menu bên trái (dùng chung cho mọi form)
    /// và điều hướng giữa các chức năng.
    /// </summary>
    internal static class DieuHuong
    {
        public static Type FormTiepTheo { get; set; }
        public static bool DangXuat { get; set; }

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
            Button nutDangXuat = TatCa(form).OfType<Button>().FirstOrDefault(b => b.Text.Trim() == "Đăng xuất");
            Control menu = nutDangXuat != null && nutDangXuat.Parent != null ? nutDangXuat.Parent : form;

            foreach (Button nut in TatCa(menu).OfType<Button>().ToList())
            {
                bool laDangXuat;
                Type dich = TimDich(nut.Text, out laDangXuat);
                if (dich == null && !laDangXuat) continue;

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
                FormTiepTheo = null;   // form từ chối đóng (VD: còn điểm chưa lưu)
        }
    }
}
