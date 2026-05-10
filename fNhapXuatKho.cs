using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fNhapXuatKho : Form
    {
        // Bạn nên tạo một Resource mới trên MockAPI tên là "kho" hoặc "nhapxuat"
        private string apiURL = "https://6a0025352b7ab34960301a22.mockapi.io/nhapxuat";
        private HttpClient client = new HttpClient();

        public fNhapXuatKho()
        {
            InitializeComponent();
        }

        private async void fNhapXuatKho_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                string res = await client.GetStringAsync(apiURL);
                var ds = JsonConvert.DeserializeObject<List<KhoDTO>>(res);
                dgvKho.DataSource = null;
                if (ds != null)
                {
                    dgvKho.DataSource = ds;
                    if (dgvKho.Columns["id"] != null) dgvKho.Columns["id"].Visible = false;
                }
            }
            catch { MessageBox.Show("Không thể tải dữ liệu kho!"); }
        }

        private async void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHang.Text) || string.IsNullOrEmpty(txtSoLuong.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã hàng và Số lượng!");
                return;
            }

            var item = new KhoDTO
            {
                maHang = txtMaHang.Text,
                tenHang = txtTenHang.Text,
                soLuong = int.Parse(txtSoLuong.Text),
                loai = cbLoai.Text, // "Nhập" hoặc "Xuất"
                ngayThucHien = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                await LoadData();
                ClearInput();
                MessageBox.Show("Đã lưu giao dịch kho!");
            }
        }

        private void ClearInput()
        {
            txtMaHang.Clear();
            txtTenHang.Clear();
            txtSoLuong.Clear();
            cbLoai.SelectedIndex = -1;
        }

        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow != null)
            {
                string id = dgvKho.CurrentRow.Cells["id"].Value.ToString();
                await client.DeleteAsync(apiURL + "/" + id);
                await LoadData();
            }
        }

        private void btnDong_Click(object sender, EventArgs e) => this.Close();

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class KhoDTO
    {
        public string id { get; set; }
        public string maHang { get; set; }
        public string tenHang { get; set; }
        public int soLuong { get; set; }
        public string loai { get; set; } // Nhập hoặc Xuất
        public string ngayThucHien { get; set; }
    }
}
