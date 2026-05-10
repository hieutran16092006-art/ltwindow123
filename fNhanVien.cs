using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace QuanLyCafe
{
    public partial class fNhanVien : Form
    {
        // 1. ĐỊNH NGHĨA BIẾN TOÀN CỤC
        // apiURL: Địa chỉ "nhà kho" chứa dữ liệu nhân viên trên internet (MockAPI)
        string apiURL = "https://69fff1032b7ab349602ff7ba.mockapi.io/nhanvien";

        // client: Đối tượng dùng để thực hiện các yêu cầu (gửi/nhận dữ liệu) qua mạng
        HttpClient client = new HttpClient();

        public fNhanVien()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SỰ KIỆN LOAD FORM: Chạy ngay khi cửa sổ Nhân Viên vừa mở lên
        /// Mục đích: Tự động đổ dữ liệu từ mạng vào bảng DataGridView để người dùng xem
        /// </summary>
        private async void fNhanVien_LoadAsync(object sender, EventArgs e)
        {
            // Gọi hàm LoadData để tải dữ liệu
            await LoadData();
        }

        /// <summary>
        /// HÀM TẢI DỮ LIỆU (READ): Lấy danh sách từ API về máy
        /// </summary>
        async Task LoadData()
        {
            try
            {
                // 1. Gửi yêu cầu GET lên API để lấy chuỗi văn bản JSON
                string response = await client.GetStringAsync(apiURL);

                // 2. Chuyển chuỗi JSON đó thành Danh sách (List) các đối tượng NhanVienDTO
                List<NhanVienDTO> list = JsonConvert.DeserializeObject<List<NhanVienDTO>>(response);

                // 3. Hiển thị danh sách này lên bảng dgvNhanVien
                dgvNhanVien.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách: " + ex.Message);
            }
        }

        /// <summary>
        /// NÚT THÊM (CREATE): Đẩy một nhân viên mới lên Cloud
        /// </summary>
        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            // 1. Gom dữ liệu từ các ô nhập liệu (TextBox) vào một đối tượng nhân viên
            var nv = new NhanVienDTO
            {
                manv = txtMaNV.Text,
                hoten = txtHoTen.Text,
                gioitinh = cbGioiTinh.Text,
                sdt = txtSDT.Text,
                cccd = txtCCCD.Text
            };

            // 2. Chuyển đối tượng nhân viên thành chuỗi JSON để gửi đi
            var json = JsonConvert.SerializeObject(nv);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 3. Sử dụng lệnh POST để gửi nhân viên mới lên API
            var res = await client.PostAsync(apiURL, content);

            if (res.IsSuccessStatusCode)
            {
                MessageBox.Show("Thêm nhân viên lên Cloud thành công!");
                await LoadData(); // Sau khi thêm xong thì tải lại bảng để cập nhật giao diện
            }
        }

        /// <summary>
        /// NÚT XÓA (DELETE): Gỡ bỏ nhân viên khỏi hệ thống
        /// </summary>
        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn dòng nào trong bảng chưa
            if (dgvNhanVien.CurrentRow != null)
            {
                // 1. Lấy mã định danh "id" của nhân viên ở dòng đang chọn (id này do API tự sinh)
                string id = dgvNhanVien.CurrentRow.Cells["id"].Value.ToString();

                // 2. Sử dụng lệnh DELETE kèm theo id để xóa đúng người đó trên API
                var res = await client.DeleteAsync(apiURL + "/" + id);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đã xóa nhân viên!");
                    await LoadData(); // Tải lại bảng sau khi xóa
                }
            }
        }

        /// <summary>
        /// NÚT SỬA (UPDATE): Cập nhật thông tin nhân viên đã có
        /// </summary>
        private async void button4_ClickAsync(object sender, EventArgs e)
        {
            if (dgvNhanVien.CurrentRow != null)
            {
                // 1. Xác định id của người cần sửa
                string id = dgvNhanVien.CurrentRow.Cells["id"].Value.ToString();

                // 2. Lấy thông tin mới từ các ô nhập liệu
                var nv = new NhanVienDTO
                {
                    manv = txtMaNV.Text,
                    hoten = txtHoTen.Text,
                    gioitinh = cbGioiTinh.Text,
                    sdt = txtSDT.Text,
                    cccd = txtCCCD.Text
                };

                // 3. Chuyển thành JSON và dùng lệnh PUT để cập nhật đè lên dữ liệu cũ trên API
                var json = JsonConvert.SerializeObject(nv);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PutAsync(apiURL + "/" + id, content);

                if (res.IsSuccessStatusCode)
                {
                    MessageBox.Show("Cập nhật thông tin thành công!");
                    await LoadData(); // Làm mới bảng dữ liệu
                }
            }
        }

        // Đóng form hiện tại
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        // Các hàm phụ phát sinh do click nhầm, để trống để không lỗi
        private void label3_Click(object sender, EventArgs e) { }
        private void dgvNhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }

    /// <summary>
    /// LỚP DỮ LIỆU NHÂN VIÊN (DTO): 
    /// Mục đích: Làm khuôn mẫu để chuyển đổi dữ liệu giữa C# và JSON (API)
    /// Các tên biến (id, manv, hoten...) phải trùng khớp hoàn toàn với tên cột trên MockAPI
    /// </summary>
    public class NhanVienDTO
    {
        public string id { get; set; }        // Mã ID tự động của hệ thống
        public string manv { get; set; }      // Mã nhân viên riêng của quán
        public string hoten { get; set; }     // Tên nhân viên
        public string gioitinh { get; set; }  // Giới tính
        public string sdt { get; set; }       // Số điện thoại
        public string cccd { get; set; }      // Căn cước công dân
    }
}
