using System.Text.Json.Serialization;

namespace TrainingBE.Model
{
    public class Product { 
    
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryID { get; set; }
        public Category? Category { get; set; }
        public List<Discount>? Discount { get; set; }
    }
}
