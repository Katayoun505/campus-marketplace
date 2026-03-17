using System.ComponentModel.DataAnnotations;

namespace ITGN350_Assignment.ViewModels
{
    public class SellProductViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}