namespace QuanLyDiemSinhVien.GUI
{
    partial class fBaoCao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblThongBao = new System.Windows.Forms.Label();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.dgvDuLieu = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuLieu)).BeginInit();
            this.SuspendLayout();
            // 
            // lblThongBao
            // 
            this.lblThongBao.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblThongBao.ForeColor = System.Drawing.Color.DarkRed;
            this.lblThongBao.Location = new System.Drawing.Point(0, 0);
            this.lblThongBao.Padding = new System.Windows.Forms.Padding(10, 8, 10, 0);
            this.lblThongBao.Size = new System.Drawing.Size(1200, 60);
            this.lblThongBao.Text = "Chưa có file báo cáo .rpt";
            this.lblThongBao.Visible = false;
            this.lblThongBao.Name = "lblThongBao";
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 60);
            this.crystalReportViewer1.Size = new System.Drawing.Size(1200, 700);
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            // 
            // dgvDuLieu
            // 
            this.dgvDuLieu.AllowUserToAddRows = false;
            this.dgvDuLieu.AllowUserToDeleteRows = false;
            this.dgvDuLieu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDuLieu.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvDuLieu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDuLieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDuLieu.Location = new System.Drawing.Point(0, 60);
            this.dgvDuLieu.ReadOnly = true;
            this.dgvDuLieu.RowHeadersWidth = 51;
            this.dgvDuLieu.Size = new System.Drawing.Size(1200, 700);
            this.dgvDuLieu.Visible = false;
            this.dgvDuLieu.Name = "dgvDuLieu";
            // 
            // fBaoCao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 760);
            this.Controls.Add(this.dgvDuLieu);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.lblThongBao);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Name = "fBaoCao";
            this.Text = "Báo cáo";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuLieu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblThongBao;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.DataGridView dgvDuLieu;
    }
}
