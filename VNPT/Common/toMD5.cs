using System.Security.Cryptography;
using System.Text;

namespace VNPT.Common
{
    public class toMD5
    {
        public static string GetMd5Hash(string input)
        {
            try
            {
                using var md5 = MD5.Create();//tạo hàm thuật toán
                var bytes = Encoding.UTF8.GetBytes(input);//chuyển chữ, ký tự sang nhị phân/số 
                var hashBytes = md5.ComputeHash(bytes);//encode mật khẩu
                // chuyển sang chuỗi hexa
                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
