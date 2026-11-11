using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace QuanLyDiemSinhVien.GUI
{

    internal static class GiaoDien
    {

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
            foreach (int so in cacSo) dt.Rows.Add(so.ToString(), tienTo + so);
            GanCombo(cb, dt, "GiaTri", "HienThi", coTatCa);
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
            if (giaTri == null || giaTri == DBNull.Value) { if (cb.Items.Count > 0) cb.SelectedIndex = 0; return; }
            cb.SelectedValue = giaTri.ToString();
        }

        public static string GiaTriDongChon(DataGridView dgv, string cotDuLieu)
        {
            if (dgv.CurrentRow == null) return null;
            var dong = dgv.CurrentRow.DataBoundItem as DataRowView;
            if (dong == null || !dong.Row.Table.Columns.Contains(cotDuLieu)) return null;
            object v = dong[cotDuLieu];
            return v == null || v == DBNull.Value ? null : v.ToString();
        }
    }
}
