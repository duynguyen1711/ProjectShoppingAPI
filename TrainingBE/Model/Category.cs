namespace TrainingBE.Model
{
    public class Category
    {
        
        public string Name { get; set; }
        public List<ProductInfo> Products { get; set; }

        public Category()
        {
           
        }
        public Category(string name)
        {
            Name = name;
        }

    }
}
