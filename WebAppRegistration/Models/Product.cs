using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppRegistration.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва товару є обов'язковою.")]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ціна є обов'язковою.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Ціна повинна бути більшою за нуль.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть групу.")]
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public Group? Group { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Кількість на складі не може бути від'ємною.")]
        public int StockQuantity { get; set; } = 0;
    }
}