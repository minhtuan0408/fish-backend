using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fish_Backend.Models
{
    [Table("users")]
    public class Users
    {
        public int? id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string? username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string? password { get; set; }
        public int money { get; set; } = 0;
        public DateTime? created_at { get; set; }
        public string? email { get; set; }

    }
}
