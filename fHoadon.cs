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
    public partial class fHoaDon : Form
    {
        // Link MockAPI
        private string apiURL = "https://6a001ec82b7ab349603014ae.mockapi.io/hoaDon";
        private HttpClient client = new HttpClient();

        public fHoaDon()
        {
            InitializeComponent();

            // Đăng ký sự kiện đổi món để tự điền giá
            cbMaHang.SelectedIndexChanged += (s, e) => {
                if (cbMaHang.Text == "Cafe Đen") txtDonGia.Text = "25000";
                else if (cbMaHang.Text == "Cafe Sữa") txtDonGia.Text = "30000";
                else if (cbMaHang.Text == "Bạc Xỉu") txtDonGia.Text = "35000";
                else if (cbMaHang.Text == "Trà Đào Cam Sả") txtDonGia.Text = "40000";
                else if (cbMaHang.Text == "Trà Thạch Vải") txtDonGia.Text = "45000";
            };
        }

        // Sự kiện khi Form load
        private async void fHoaDon_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        // 1. Hàm tải dữ liệu từ API và tính tổng tiền
        private async Task LoadData()
        {
            try
            {
                string res = await client.GetStringAsync(apiURL);
                var ds = JsonConvert.DeserializeObject<List<HoaDonDTO>>(res);

                dgvData.DataSource = null;
                if (ds != null)
                {
                    dgvData.DataSource = ds;
                    // Ẩn cột ID của MockAPI nếu có
                    if (dgvData.Columns["id"] != null) dgvData.Columns["id"].Visible = false;

                    // Tính tổng tiền
                    double tong = ds.Sum(x => {
                        double.TryParse(x.donGia, out double dg);
                        return dg;
                    });
                    txtTongTien.Text = tong.ToString("N0");
                }
            }
            catch
            {
                txtTongTien.Text = "0";
            }
        }

        // 2. Nút Thêm hóa đơn
        private async void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaHD.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã hóa đơn!");
                return;
            }

            var hd = new HoaDonDTO
            {
                maHoaDon = txtMaHD.Text,
                maHang = cbMaHang.Text,
                donGia = txtDonGia.Text,
                ghiChu = txtGhiChu.Text
            };

            var json = JsonConvert.SerializeObject(hd);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                await LoadData();
                // Xóa dữ liệu cũ trên ô nhập
                txtMaHD.Clear();
                txtDonGia.Clear();
                txtGhiChu.Clear();
                cbMaHang.SelectedIndex = -1;
                MessageBox.Show("Đã thêm thành công!");
            }
            else
            {
                MessageBox.Show("Lỗi kết nối API!");
            }
        }

        // 3. Nút Xóa hóa đơn
        private async void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvData.CurrentRow != null)
            {
                // Lấy ID ẩn từ MockAPI để xóa
                string id = dgvData.CurrentRow.Cells["id"].Value.ToString();
                var response = await client.DeleteAsync(apiURL + "/" + id);

                if (response.IsSuccessStatusCode)
                {
                    await LoadData();
                    MessageBox.Show("Đã xóa thành công!");
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!");
            }
        }

        // 4. Nút Tính (Làm mới dữ liệu)
        private async void btnTinh_Click(object sender, EventArgs e)
        {
            await LoadData();
        }

        // Nút Đóng Form
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Khi click vào bảng thì hiện dữ liệu lên các ô nhập
        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaHD.Text = dgvData.Rows[e.RowIndex].Cells["maHoaDon"].Value?.ToString();
                cbMaHang.Text = dgvData.Rows[e.RowIndex].Cells["maHang"].Value?.ToString();
                txtDonGia.Text = dgvData.Rows[e.RowIndex].Cells["donGia"].Value?.ToString();
            }
        }
    }

    // Class hứng dữ liệu API
    public class HoaDonDTO
    {
        public string id { get; set; }
        public string maHoaDon { get; set; }
        public string maHang { get; set; }
        public string donGia { get; set; }
        public string ghiChu { get; set; }
    }
}
