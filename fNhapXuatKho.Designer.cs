namespace QuanLyQuanCafe
{
    partial class fNhapXuatKho
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtMaHang = new System.Windows.Forms.TextBox();
            this.txtTenHang = new System.Windows.Forms.TextBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.cbLoai = new System.Windows.Forms.ComboBox();
            this.dgvKho = new System.Windows.Forms.DataGridView();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnDong = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKho)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMaHang
            // 
            this.txtMaHang.Location = new System.Drawing.Point(100, 17);
            this.txtMaHang.Name = "txtMaHang";
            this.txtMaHang.Size = new System.Drawing.Size(150, 22);
            this.txtMaHang.TabIndex = 0;
            // 
            // txtTenHang
            // 
            this.txtTenHang.Location = new System.Drawing.Point(100, 57);
            this.txtTenHang.Name = "txtTenHang";
            this.txtTenHang.Size = new System.Drawing.Size(150, 22);
            this.txtTenHang.TabIndex = 1;
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(340, 17);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(100, 22);
            this.txtSoLuong.TabIndex = 2;
            // 
            // cbLoai
            // 
            this.cbLoai.Items.AddRange(new object[] {
            "Nhập kho",
            "Xuất kho"});
            this.cbLoai.Location = new System.Drawing.Point(340, 57);
            this.cbLoai.Name = "cbLoai";
            this.cbLoai.Size = new System.Drawing.Size(100, 24);
            this.cbLoai.TabIndex = 3;
            // 
            // dgvKho
            // 
            this.dgvKho.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKho.ColumnHeadersHeight = 29;
            this.dgvKho.Location = new System.Drawing.Point(20, 124);
            this.dgvKho.Name = "dgvKho";
            this.dgvKho.RowHeadersWidth = 51;
            this.dgvKho.Size = new System.Drawing.Size(550, 250);
            this.dgvKho.TabIndex = 4;
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(470, 15);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(100, 30);
            this.btnLuu.TabIndex = 5;
            this.btnLuu.Text = "Lưu phiếu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(470, 55);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(100, 30);
            this.btnXoa.TabIndex = 6;
            this.btnXoa.Text = "Xóa dòng";
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnDong
            // 
            this.btnDong.Location = new System.Drawing.Point(0, 0);
            this.btnDong.Name = "btnDong";
            this.btnDong.Size = new System.Drawing.Size(75, 23);
            this.btnDong.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mã hàng:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tên hàng:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(270, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Số lượng:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(270, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Loại:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(507, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 30);
            this.button1.TabIndex = 11;
            this.button1.Text = "Back";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fNhapXuatKho
            // 
            this.ClientSize = new System.Drawing.Size(602, 386);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtMaHang);
            this.Controls.Add(this.txtTenHang);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.cbLoai);
            this.Controls.Add(this.dgvKho);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Name = "fNhapXuatKho";
            this.Text = "Quản lý Nhập Xuất Kho";
            this.Load += new System.EventHandler(this.fNhapXuatKho_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKho)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtMaHang;
        private System.Windows.Forms.TextBox txtTenHang;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.ComboBox cbLoai;
        private System.Windows.Forms.DataGridView dgvKho;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnDong;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}
