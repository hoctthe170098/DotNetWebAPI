using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI1.Data
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName  { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
    }
}
