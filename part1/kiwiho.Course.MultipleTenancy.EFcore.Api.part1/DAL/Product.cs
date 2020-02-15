using System.ComponentModel.DataAnnotations;

namespace kiwiho.Course.MultipleTenancy.EFcore.Api.DAL
{
    public class Product
    {
        [Key]
        public int Id { get; set; } 

        [StringLength(50), Required]
        public string Name { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        public double? Price { get; set; }
    }
}