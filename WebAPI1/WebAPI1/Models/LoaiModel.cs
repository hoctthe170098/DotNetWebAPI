using System.ComponentModel.DataAnnotations;

namespace WebAPI1.Models
{
    public class LoaiModel
    {
        [Required]
        [MaxLength(50)]
        public string TenLoai { get; set; }
    }
}
