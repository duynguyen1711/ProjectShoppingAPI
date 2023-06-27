using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TrainingBE.Model;
using System.Linq;
using System.Xml.Linq;

namespace TrainingBE.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ProductController : ControllerBase
    {
        public static DateTime today = new(2023, 6, 14);
        public static Category electronicsCategory = new Category("Electronics");
        public static Category clothingCategory = new Category("Clothing");
        public static Category vegetableCategory = new Category("Vegetables");
        // Discount
        public static Discount electronicsDiscount = new Discount { Percentage = 10, StartDate = new DateTime(2023, 6, 1), EndDate = new DateTime(2023, 6, 15) };
        public static Discount clothingDiscount = new Discount { Percentage = 20, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 27) };
        public static Discount vegetableDiscount = new Discount { Percentage = 20, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 28) };
        public static Discount saleDiscount = new Discount { Percentage = 5, StartDate = new DateTime(2023, 6, 28), EndDate = new DateTime(2023, 6, 30) };
        //Product
        public static List<Product> productList = new List<Product>
        {
                new Product { Id =1,Name = "Laptop", Price = 1000,Category =  electronicsCategory, Discount = new List<Discount> { electronicsDiscount, saleDiscount } },
                new Product { Id =2,Name = "Shirt", Price = 50, Category = clothingCategory, Discount = new List<Discount> { clothingDiscount,saleDiscount } },
                new Product { Id =3,Name = "Phone", Price = 800, Category = electronicsCategory, Discount = new List<Discount> {electronicsDiscount,saleDiscount  } },
                new Product { Id =4,Name = "Bucket", Price = 70, Category = clothingCategory, Discount = new List<Discount> {clothingDiscount, saleDiscount, } },
                new Product { Id =5,Name = "Baseball", Price = 70, Category = clothingCategory, Discount = new List<Discount> {clothingDiscount } },
                new Product { Id =6,Name = "Tomato", Price = 10, Category = vegetableCategory, Discount = new List<Discount> { vegetableDiscount,saleDiscount } },
                new Product { Id =7,Name = "Banana", Price = 15, Category = vegetableCategory, Discount = new List<Discount> { }},
        };
        public static bool CheckDay(Discount discount, DateTime today)
        {
            return discount.EndDate >= today && discount.StartDate <= today;
        }
        //  tính giá sản phẩm lúc giảm
        public static double CalculateDiscountedPrice(Product product)
        {
            double discountedPrice = product.Price;


            foreach (Discount discount in product.Discount)
            {

                if (CheckDay(discount, today))
                {
                    discountedPrice = discountedPrice - (discountedPrice * (discount.Percentage) / 100);

                }

            }
            return discountedPrice;
        }

        // lấy sản phẩm sau khi áp mã giảm
        public static List<ProductInfo> GetProductListWithDiscount()
        {
            List<ProductInfo> productListWithDiscount = new List<ProductInfo>();
            foreach (var product in productList)
            {

                double discountedPrice = CalculateDiscountedPrice(product);
                if (discountedPrice >= 0)
                {


                    productListWithDiscount.Add(new ProductInfo
                    {
                        Id = product.Id,
                        Name = product.Name,
                        OriginalPrice = product.Price,
                        PriceWithDiscount = discountedPrice,
                        Category = product.Category,
                        PercentageDiscount = product.Discount.Where(d => CheckDay(d, today)).Select(p => p.Percentage).ToList(),
                        Discount = product.Discount.Where(d => CheckDay(d, today)).ToList(),
                    });
                }
            }
            return productListWithDiscount;
        }

        // add product
        [HttpPost]
        [Route("Product/AddProduct")]
        public IActionResult AddProduct([FromBody] ProductWithoutID productAdd)
        {
            var productListSorted =productList.OrderBy(p => p.Id).ToList();
            Product product = new Product
            {
                Id = productListSorted.Max(p => p.Id) + 1,
                Name = productAdd.Name,
                Price = productAdd.Price,
                Category = productAdd.Category,
                Discount = productAdd.Discount.Where(d => CheckDay(d, today)).ToList(),
            };
            productList.Add(product);
            return Ok(productList);
        }
        // List product
        [HttpGet]
        [Route("/ListProduct")]
        public IActionResult GetListProductWithDiscount()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            return Ok(productListWithDiscount);
        }
        //Update product
        [HttpPut]
        [Route("Product/UdapteProduct")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductWithoutID updatedProductInfo)
        {
           int index = productList.FindIndex(p=>p.Id == id);
           if (index  != -1)
            {
                Product productToUpdate = productList[index];
                productToUpdate.Name = updatedProductInfo.Name;
                productToUpdate.Price = updatedProductInfo.Price;
                productToUpdate.Category = updatedProductInfo.Category;
                productToUpdate.Discount = updatedProductInfo.Discount;
                return Ok(new
                {
                    Message = "Product updated successfully.",
                    Product = productToUpdate
                });
            }
            return NotFound("Product not found.");

        }
        //delete
        [HttpDelete]
        [Route("Product/DeleteProduct")]
        public IActionResult DeleteProduct (int id)
        {
            int index = productList.FindIndex(p => p.Id == id);
            if (index != -1)
            {
                productList.RemoveAt(index);
                return Ok(new
                {
                    Message = "Deleted successfully.",
                    Product = GetProductListWithDiscount()
                });
            }

            return NotFound("Product not found.");
        }
        //detail product
        [HttpGet]
        [Route("Product/{id}")]
        public IActionResult GetProductByID(int id)
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            var productFound = productListWithDiscount.FirstOrDefault(p => p.Id == id);

            if (productFound != null)
            {
                return Ok(new
                {
                    Message = "Product Found",
                    Product = productFound,
                });
            }
            return NotFound("Product not found.");
        }
    } 
}
