using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrainingBE.Model
{
    public class Product {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
        public List<Product_Discount>? Product_Discounts { get; set; }
        public ICollection<Review>? Reviews { get; set; }

    }
}
