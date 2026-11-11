using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{
    internal static class GiaoDien
    {
        public static readonly Color MauChinh = Color.Navy;
        public static readonly Font FontThuong = new Font("Times New Roman", 12F);
        public static readonly Font FontDam = new Font("Times New Roman", 12F, FontStyle.Bold);

        public static void ThongBao(string noiDung)
        {
            MessageBox.Show(noiDung, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Loi(string noiDung)
        {
            MessageBox.Show(noiDung, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Loi(Exception ex)
        {
            var sql = ex as System.Data.SqlClient.SqlException;
            Loi(sql != null ? BLL.XuLyLoi.DichLoiSql(sql) : ex.Message);
        }

        public static bool XacNhan(string noiDung)
        {
            return MessageBox.Show(noiDung, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static void GanCombo(ComboBox cb, DataTable nguon, string cotGiaTri, string cotHienThi,
                                    bool coTatCa, string chuTatCa = "-- Tất cả --")
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("_GiaTri", typeof(string));
            dt.Columns.Add("_HienThi", typeof(string));
            if (coTatCa) dt.Rows.Add("", chuTatCa);
            if (nguon != null)
                foreach (DataRow r in nguon.Rows)
                    dt.Rows.Add(Convert.ToString(r[cotGiaTri]), Convert.ToString(r[cotHienThi]));

            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.DataSource = null;
            cb.DisplayMember = "_HienThi";
            cb.ValueMember = "_GiaTri";
            cb.DataSource = dt;
            if (dt.Rows.Count > 0) cb.SelectedIndex = 0;
        }

        public static void GanComboSo(ComboBox cb, IEnumerable<int> cacSo, string tienTo, bool coTatCa)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GiaTri", typeof(string));
            dt.Columns.Add("HienThi", typeof(string));
            if (coTatCa) dt.Rows.Add("", "-- Tất cả --");
            foreach (int so in cacSo) dt.Rows.Add(so.ToString(), tienTo + so);
            GanCombo(cb, dt, "GiaTri", "HienThi", false);
        }
        public static string GiaTri(ComboBox cb)
        {
            if (cb.SelectedIndex < 0 || cb.SelectedValue == null) return null;
            string v = cb.SelectedValue.ToString();
            return string.IsNullOrWhiteSpace(v) ? null : v;
        }

        public static int? GiaTriSo(ComboBox cb)
        {
            string v = GiaTri(cb);
            int so;
            return int.TryParse(v, out so) ? so : (int?)null;
        }

        public static void ChonGiaTri(ComboBox cb, object giaTri)
        {
            if (giaTri == null) { if (cb.Items.Count > 0) cb.SelectedIndex = 0; return; }
            cb.SelectedValue = giaTri.ToString();
        }

        public static void DinhDangLuoi(DataGridView dgv, bool chiDoc = true)
        {
            dgv.ReadOnly = chiDoc;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = MauChinh;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 11F, FontStyle.Bold);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.DefaultCellStyle.Font = new Font("Times New Roman", 11F);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255);
            dgv.RowTemplate.Height = 30;
        }
        public static void HienCot(DataGridView dgv, params string[] cot)
        {
            foreach (DataGridViewColumn c in dgv.Columns) c.Visible = false;
            int thuTu = 0;
            foreach (string s in cot)
            {
                string[] p = s.Split('=');
                if (!dgv.Columns.Contains(p[0])) continue;
                DataGridViewColumn c = dgv.Columns[p[0]];
                c.Visible = true;
                c.HeaderText = p.Length > 1 ? p[1] : p[0];
                c.DisplayIndex = thuTu++;
                if (c.ValueType == typeof(DateTime)) c.DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }

        public static string GiaTriDongChon(DataGridView dgv, string cot)
        {
            if (dgv.CurrentRow == null || !dgv.Columns.Contains(cot)) return null;
            object v = dgv.CurrentRow.Cells[cot].Value;
            return v == null || v == DBNull.Value ? null : v.ToString();
        }

        // ---------------- Tạo control bằng code ----------------
        public static Label TaoNhan(Control cha, string chu, int x, int y, int rong = 0)
        {
            var lb = new Label { Text = chu, Location = new Point(x, y), Font = FontThuong };
            if (rong > 0) { lb.AutoSize = false; lb.Size = new Size(rong, 24); } else lb.AutoSize = true;
            cha.Controls.Add(lb);
            return lb;
        }

        public static ComboBox TaoCombo(Control cha, int x, int y, int rong)
        {
            var cb = new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(rong, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = FontThuong
            };
            cha.Controls.Add(cb);
            return cb;
        }

        public static TextBox TaoO(Control cha, int x, int y, int rong)
        {
            var tb = new TextBox { Location = new Point(x, y), Size = new Size(rong, 30), Font = FontThuong };
            cha.Controls.Add(tb);
            return tb;
        }

        public static Button TaoNut(Control cha, string chu, int x, int y, int rong, int cao, Color mauNen)
        {
            var b = new Button
            {
                Text = chu,
                Location = new Point(x, y),
                Size = new Size(rong, cao),
                BackColor = mauNen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = FontDam,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            b.FlatAppearance.BorderSize = 0;
            cha.Controls.Add(b);
            return b;
        }
    }
}
