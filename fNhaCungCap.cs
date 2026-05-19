using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe
{
    public partial class fNhaCungCap : Form
    {
        // Khai báo API URL và HttpClient chạy đồng bộ tuyến tính
        string apiURL = "https://6a0025352b7ab34960301a22.mockapi.io/nhacungcap";
        HttpClient client = new HttpClient();

        public fNhaCungCap()
        {
            InitializeComponent();

            // Đăng ký sự kiện click cell lưới dữ liệu bằng tay
            this.dgvnhacungcap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvnhacungcap_CellClick);
            this.Load += new System.EventHandler(this.fNhaCungCap_Load);
        }

        private void fNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData(); // Tải dữ liệu khi mở Form
        }

        // Hàm tải dữ liệu từ API và đổ vào DataGridView
        private void LoadData()
        {
            try
            {
                string res = client.GetStringAsync(apiURL).Result;
                List<NhaCungCapDTO> ds = JsonConvert.DeserializeObject<List<NhaCungCapDTO>>(res);

                dgvnhacungcap.DataSource = null;
                if (ds != null)
                    dgvnhacungcap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                {
                    dgvnhacungcap.DataSource = ds;

                    if (dgvnhacungcap.Columns["id"] != null) dgvnhacungcap.Columns["id"].Visible = false;

                    if (dgvnhacungcap.Columns["maNCC"] != null) dgvnhacungcap.Columns["maNCC"].HeaderText = "Mã NCC";
                    if (dgvnhacungcap.Columns["tenNCC"] != null) dgvnhacungcap.Columns["tenNCC"].HeaderText = "Tên Nhà Cung Cấp";
                    if (dgvnhacungcap.Columns["sdt"] != null) dgvnhacungcap.Columns["sdt"].HeaderText = "Số Điện Thoại";
                    if (dgvnhacungcap.Columns["diaChi"] != null) dgvnhacungcap.Columns["diaChi"].HeaderText = "Địa Chỉ";
                    if (dgvnhacungcap.Columns["trangThai"] != null) dgvnhacungcap.Columns["trangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Thông báo lỗi");
            }
        }

        // Nút Thêm nhà cung cấp (Tích hợp TỰ ĐỘNG TẠO MÃ và VALIDATE SĐT)
        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra tên NCC
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo");
                txtTenNCC.Focus();
                return;
            }

            // 2. Kiểm tra số điện thoại (Phải bắt đầu bằng số 0 và chỉ chứa số)
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo");
                txtSDT.Focus();
                return;
            }

            // Validate: Bắt buộc bắt đầu bằng số 0
            if (!sdt.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Phải bắt đầu bằng số 0.", "Thông báo");
                txtSDT.Focus();
                return;
            }

            // Validate phụ: Đảm bảo người dùng không nhập chữ (ví dụ: 098abc123)
            if (!sdt.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa các ký tự số từ 0-9!", "Thông báo");
                txtSDT.Focus();
                return;
            }

            // 3. Kiểm tra địa chỉ
            if (string.IsNullOrWhiteSpace(textDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo");
                textDiaChi.Focus();
                return;
            }

            // 4. Kiểm tra trạng thái
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Thông báo");
                comboBox1.Focus();
                return;
            }

            try
            {
                string maTuDong = "NCC" + DateTime.Now.ToString("yyyyMMddHHmmss");

                NhaCungCapDTO ncc = new NhaCungCapDTO();
                ncc.maNCC = maTuDong;
                ncc.tenNCC = txtTenNCC.Text.Trim();
                ncc.sdt = sdt; // Sử dụng chuỗi sdt đã trim sạch sẽ
                ncc.diaChi = textDiaChi.Text.Trim();
                ncc.trangThai = comboBox1.Text;

                string json = JsonConvert.SerializeObject(ncc);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(apiURL, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Thêm thành công! Mã NCC vừa tạo là: " + maTuDong, "Thông báo");
                    LoadData();

                    // Xóa trắng các ô thông tin nhập liệu
                    txtTenNCC.Text = "";
                    txtSDT.Text = "";
                    textDiaChi.Text = "";
                    comboBox1.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Thêm thất bại!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi");
            }
        }

        // Nút Xóa nhà cung cấp
        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvnhacungcap.CurrentRow == null)
            {
                MessageBox.Show("Hãy chọn 1 nhà cung cấp để xóa!", "Thông báo");
                return;
            }

            DialogResult chon = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (chon == DialogResult.No)
            {
                return;
            }

            try
            {
                string id = dgvnhacungcap.CurrentRow.Cells["id"].Value.ToString();
                HttpResponseMessage response = client.DeleteAsync(apiURL + "/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa thất bại!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvnhacungcap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtTenNCC.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["tenNCC"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["tenNCC"].Value.ToString() : "";
                txtSDT.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["sdt"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["sdt"].Value.ToString() : "";
                textDiaChi.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["diaChi"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["diaChi"].Value.ToString() : "";
                comboBox1.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["trangThai"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["trangThai"].Value.ToString() : "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }

    public class NhaCungCapDTO
    {
        public string id { get; set; }
        public string maNCC { get; set; }
        public string tenNCC { get; set; }
        public string sdt { get; set; }
        public string diaChi { get; set; }
        public string trangThai { get; set; }
    }
}
