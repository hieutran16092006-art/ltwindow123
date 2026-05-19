using Newtonsoft.Json; // Thư viện dùng để chuyển đổi qua lại giữa chuỗi JSON và đối tượng C# (Serialization/Deserialization)
using System; // Thư viện cơ bản của hệ thống .NET (chứa các kiểu dữ liệu gốc, Exception,...)
using System.Collections.Generic; // Thư viện hỗ trợ sử dụng các tập hợp dữ liệu kiểu Generic (ví dụ: List<T>)
using System.Linq; // Thư viện hỗ trợ các câu lệnh truy vấn dữ liệu nhanh (như hàm .All(), .Where(),...)
using System.Net.Http; // Thư viện dùng để gửi các yêu cầu HTTP (GET, POST, PUT, DELETE) lên Server API
using System.Text; // Thư viện hỗ trợ mã hóa chuỗi văn bản (ví dụ: Encoding.UTF8)
using System.Threading.Tasks; // Thư viện hỗ trợ lập trình bất đồng bộ (Async/Await)
using System.Windows.Forms; // Thư viện chính để xây dựng giao diện Windows Forms

namespace QuanLyCafe
{
    // Khai báo lớp fNhaCungCap kế thừa từ lớp Form của hệ thống
    public partial class fNhaCungCap : Form
    {
        // Khai báo đường dẫn API endpoint kết nối tới danh sách nhà cung cấp trên MockAPI
        string apiURL = "https://6a0025352b7ab34960301a22.mockapi.io/nhacungcap";
        
        // Khởi tạo đối tượng HttpClient để thực hiện các cuộc gọi API (gửi/nhận dữ liệu mạng)
        HttpClient client = new HttpClient();

        // Hàm khởi tạo (Constructor) của Form, chạy ngay khi đối tượng Form được tạo ra
        public fNhaCungCap()
        {
            InitializeComponent(); // Hàm tự động của WinForms để khởi tạo các UI Control (nút bấm, ô nhập,...)

            // Đăng ký sự kiện CellClick của lưới hiển thị dữ liệu dgvnhacungcap bằng code tay
            this.dgvnhacungcap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvnhacungcap_CellClick);
            
            // Đăng ký sự kiện Load của Form (chạy ngay khi form chuẩn bị hiển thị lên màn hình)
            this.Load += new System.EventHandler(this.fNhaCungCap_Load);
        }

        // Phương thức xử lý sự kiện khi Form bắt đầu được Load lên
        private void fNhaCungCap_Load(object sender, EventArgs e)
        {
            LoadData(); // Gọi hàm LoadData để lấy dữ liệu từ API nạp lên bảng dữ liệu luôn
        }

        // Hàm chịu trách nhiệm tải dữ liệu từ API và đổ vào bảng DataGridView công khai
        private void LoadData()
        {
            try // Khối lệnh thử nghiệm, nếu xảy ra lỗi mạng hoặc lỗi code sẽ nhảy vào khối catch
            {
                // Gửi request GET tới API đồng bộ (.Result) và nhận về chuỗi kết quả dạng JSON
                string res = client.GetStringAsync(apiURL).Result;
                
                // Giải mã (Deserialize) chuỗi JSON nhận được thành một Danh sách các đối tượng NhaCungCapDTO
                List<NhaCungCapDTO> ds = JsonConvert.DeserializeObject<List<NhaCungCapDTO>>(res);

                // Xóa bỏ nguồn dữ liệu cũ đang liên kết với DataGridView để chuẩn bị nạp mới
                dgvnhacungcap.DataSource = null;
                
                // Nếu danh sách dữ liệu lấy về không bị rỗng (null)
                if (ds != null)
                {
                    // Cấu hình tự động kéo dãn các cột để vừa khít với kích thước DataGridView
                    dgvnhacungcap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    
                    // Gán danh sách đối tượng vừa lấy từ API làm nguồn dữ liệu cho DataGridView hiển thị
                    dgvnhacungcap.DataSource = ds;

                    // Nếu trong bảng có tồn tại cột "id" (id từ MockAPI), ẩn nó đi để người dùng không thấy
                    if (dgvnhacungcap.Columns["id"] != null) dgvnhacungcap.Columns["id"].Visible = false;

                    // Đổi tên tiêu đề hiển thị bằng Tiếng Việt cho các cột tương ứng trên giao diện
                    if (dgvnhacungcap.Columns["maNCC"] != null) dgvnhacungcap.Columns["maNCC"].HeaderText = "Mã NCC";
                    if (dgvnhacungcap.Columns["tenNCC"] != null) dgvnhacungcap.Columns["tenNCC"].HeaderText = "Tên Nhà Cung Cấp";
                    if (dgvnhacungcap.Columns["sdt"] != null) dgvnhacungcap.Columns["sdt"].HeaderText = "Số Điện Thoại";
                    if (dgvnhacungcap.Columns["diaChi"] != null) dgvnhacungcap.Columns["diaChi"].HeaderText = "Địa Chỉ";
                    if (dgvnhacungcap.Columns["trangThai"] != null) dgvnhacungcap.Columns["trangThai"].HeaderText = "Trạng Thái";
                }
            }
            catch (Exception ex) // Bắt các lỗi xảy ra trong khối try (như mất mạng, API lỗi,...)
            {
                // Hiển thị hộp thoại thông báo lỗi chi tiết cho người dùng biết
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Thông báo lỗi");
            }
        }

        // Sự kiện click nút "Thêm" nhà cung cấp mới
        private void btnThem_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra tính hợp lệ của Tên nhà cung cấp (không được để trống hoặc toàn dấu cách)
            if (string.IsNullOrWhiteSpace(txtTenNCC.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp!", "Thông báo"); // Hiện thông báo nhắc nhở
                txtTenNCC.Focus(); // Đưa con trỏ chuột tập trung vào ô nhập tên
                return; // Dừng hàm ngay lập tức, không thực hiện các lệnh phía dưới nữa
            }

            // 2. Lấy số điện thoại từ ô nhập liệu và cắt bỏ các khoảng trắng thừa ở 2 đầu
            string sdt = txtSDT.Text.Trim();
            
            // Kiểm tra xem SĐT có bị bỏ trống hay không
            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo");
                txtSDT.Focus();
                return;
            }

            // Kiểm tra quy tắc: Số điện thoại bắt buộc phải bắt đầu bằng ký tự '0'
            if (!sdt.StartsWith("0"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Phải bắt đầu bằng số 0.", "Thông báo");
                txtSDT.Focus();
                return;
            }

            // Sử dụng LINQ .All để kiểm tra xem toàn bộ các ký tự trong chuỗi sdt có phải là số (0-9) hay không
            if (!sdt.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa các ký tự số từ 0-9!", "Thông báo");
                txtSDT.Focus();
                return;
            }

            // 3. Kiểm tra tính hợp lệ của Địa chỉ (không được bỏ trống)
            if (string.IsNullOrWhiteSpace(textDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo");
                textDiaChi.Focus();
                return;
            }

            // 4. Kiểm tra tính hợp lệ của Trạng thái chọn từ ComboBox (không được bỏ trống)
            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Vui lòng chọn trạng thái!", "Thông báo");
                comboBox1.Focus();
                return;
            }

            try // Vào khối xử lý gửi dữ liệu lên server nếu tất cả validate ở trên đã vượt qua
            {
                // Tự động tạo mã nhà cung cấp duy nhất dựa trên thời gian hiện tại theo định dạng: NCC + NămThángNgàyGiờPhútGiây
                string maTuDong = "NCC" + DateTime.Now.ToString("yyyyMMddHHmmss");

                // Khởi tạo một đối tượng dữ liệu chuyển đổi mới
                NhaCungCapDTO ncc = new NhaCungCapDTO();
                ncc.maNCC = maTuDong; // Gán mã tự động sinh
                ncc.tenNCC = txtTenNCC.Text.Trim(); // Gán tên đã cắt khoảng trắng thừa
                ncc.sdt = sdt; // Gán số điện thoại hợp lệ
                ncc.diaChi = textDiaChi.Text.Trim(); // Gán địa chỉ đã cắt khoảng trắng thừa
                ncc.trangThai = comboBox1.Text; // Gán trạng thái từ ComboBox

                // Mã hóa (Serialize) đối tượng C# 'ncc' thành một chuỗi định dạng JSON
                string json = JsonConvert.SerializeObject(ncc);
                
                // Đóng gói chuỗi JSON thành nội dung gửi đi qua HTTP, sử dụng bảng mã UTF-8 và định nghĩa định dạng là application/json
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST chứa dữ liệu nhà cung cấp lên API và đợi kết quả trả về (.Result)
                HttpResponseMessage response = client.PostAsync(apiURL, content).Result;

                // Nếu Server phản hồi mã thành công (Status Code nhóm 2xx)
                if (response.IsSuccessStatusCode)
                {
                    // Thông báo thêm thành công và kèm theo mã NCC vừa sinh ra
                    MessageBox.Show("Thêm thành công! Mã NCC vừa tạo là: " + maTuDong, "Thông báo");
                    
                    LoadData(); // Gọi lại hàm LoadData để cập nhật lưới hiển thị danh sách mới nhất

                    // Xóa trắng toàn bộ các ô dữ liệu trên form để sẵn sàng cho lần nhập tiếp theo
                    txtTenNCC.Text = "";
                    txtSDT.Text = "";
                    textDiaChi.Text = "";
                    comboBox1.SelectedIndex = -1; // Đưa ComboBox về trạng thái chưa chọn mục nào
                }
                else // Nếu server báo lỗi (ví dụ: lỗi hệ thống, sai định dạng server yêu cầu,...)
                {
                    MessageBox.Show("Thêm thất bại!", "Thông báo");
                }
            }
            catch (Exception ex) // Bắt lỗi ngoại lệ phát sinh trong quá trình thêm
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi");
            }
        }

        // Sự kiện click nút "Xóa" nhà cung cấp
        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn dòng nào trên DataGridView chưa
            if (dgvnhacungcap.CurrentRow == null)
            {
                MessageBox.Show("Hãy chọn 1 nhà cung cấp để xóa!", "Thông báo");
                return; // Dừng hàm nếu chưa chọn dòng nào
            }

            // Hiển thị hộp thoại xác nhận Yes/No hỏi người dùng có chắc chắn muốn xóa không
            DialogResult chon = MessageBox.Show("Bạn có chắc chắn muốn xóa nhà cung cấp này?", "Xác nhận", MessageBoxButtons.YesNo);
            if (chon == DialogResult.No) // Nếu người dùng nhấn "No" (Không xóa)
            {
                return; // Thoát hàm, hủy lệnh xóa
            }

            try
            {
                // Lấy ra giá trị ID (khóa chính trên API) của dòng hiện tại đang được chọn trên DataGridView
                string id = dgvnhacungcap.CurrentRow.Cells["id"].Value.ToString();
                
                // Gửi yêu cầu DELETE kèm ID tới URL API (Ví dụ: .../nhacungcap/5) để thực hiện xóa trên server
                HttpResponseMessage response = client.DeleteAsync(apiURL + "/" + id).Result;

                // Nếu Server phản hồi xóa thành công
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thông báo");
                    LoadData(); // Tải lại danh sách mới sau khi xóa thành công
                }
                else // Nếu server từ chối hoặc lỗi không xóa được
                {
                    MessageBox.Show("Xóa thất bại!", "Thông báo");
                }
            }
            catch (Exception ex) // Bắt lỗi nếu quá trình kết nối tới API xảy ra sự cố
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi");
            }
        }

        // Sự kiện click nút "Quay lại" (hoặc nút Đóng)
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng Form hiện tại lại
        }

        // Sự kiện xảy ra khi người dùng click chuột vào một ô (Cell) bất kỳ trên DataGridView
        private void dgvnhacungcap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Đảm bảo hàng được click là hàng dữ liệu hợp lệ (RowIndex >= 0, loại trừ hàng tiêu đề cột có chỉ số -1)
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu từ cột "tenNCC" của dòng được chọn, kiểm tra null, nếu null gán chuỗi rỗng "", rồi đẩy lên ô nhập liệu txtTenNCC
                txtTenNCC.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["tenNCC"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["tenNCC"].Value.ToString() : "";
                
                // Lấy dữ liệu từ cột "sdt" của dòng được chọn đưa lên ô nhập liệu txtSDT
                txtSDT.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["sdt"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["sdt"].Value.ToString() : "";
                
                // Lấy dữ liệu từ cột "diaChi" của dòng được chọn đưa lên ô nhập liệu textDiaChi
                textDiaChi.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["diaChi"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["diaChi"].Value.ToString() : "";
                
                // Lấy dữ liệu từ cột "trangThai" của dòng được chọn gán vào phần văn bản hiển thị của ComboBox1
                comboBox1.Text = dgvnhacungcap.Rows[e.RowIndex].Cells["trangThai"].Value != null ? dgvnhacungcap.Rows[e.RowIndex].Cells["trangThai"].Value.ToString() : "";
            }
        }

        // Sự kiện chạy khi chỉ số lựa chọn (item được chọn) trong ComboBox thay đổi (Hiện đang để trống)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }

    // Lớp đối tượng vận chuyển dữ liệu (Data Transfer Object) ánh xạ tương thích cấu trúc dữ liệu JSON của API
    public class NhaCungCapDTO
    {
        public string id { get; set; } // Thuộc tính lưu ID tự tăng được cấp bởi MockAPI (Dùng để xác định khi Xóa/Sửa)
        public string maNCC { get; set; } // Thuộc tính lưu mã nhà cung cấp tự động sinh dạng chuỗi
        public string tenNCC { get; set; } // Thuộc tính lưu tên nhà cung cấp
        public string sdt { get; set; } // Thuộc tính lưu số điện thoại
        public string diaChi { get; set; } // Thuộc tính lưu địa chỉ
        public string trangThai { get; set; } // Thuộc tính lưu trạng thái hoạt động
    }
}
