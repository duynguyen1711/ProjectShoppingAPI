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
        public static Category electronicsCategory = new Category("Electronics");
        public static Category clothingCategory = new Category("Clothing");
        public static Category vegetableCategory = new Category("Vegetables");
        // Discount
        public static Discount electronicsDiscount = new Discount { Percentage = 10, StartDate = new DateTime(2023, 6, 1), EndDate = new DateTime(2023, 6, 15) };
        public static Discount clothingDiscount = new Discount { Percentage = 20, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 20) };
        public static Discount vegetableDiscount = new Discount { Percentage = 20, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 26) };
        public static Discount saleDiscount = new Discount { Percentage = 5, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 26) };
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
        //  tính giá sản phẩm lúc giảm
        public static double CalculateDiscountedPrice(Product product)
        {
            double discountedPrice = product.Price;

            foreach (Discount discount in product.Discount)
            {

                if (DateTime.Now >= discount.StartDate && DateTime.Now <= discount.EndDate)
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
                        Discount = product.Discount,
                    });
                }
            }
            return productListWithDiscount;
        }
        [HttpGet]
        [Route("test")]
        public IActionResult GetAll()
        {
            return Ok(productList);
        }
        // lay danh sach san pham gia da ap dung ma giam
        [HttpGet]
        [Route("Product")]
        public IActionResult GetAllProductWithDiscount()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            return Ok(productListWithDiscount);
        }
        // sap xep theo gia 
        [HttpGet]
        [Route("Product/Sort/Price")]
        public IActionResult GetAllProductSortByPrice()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productListSortByPriceIncrease = productListWithDiscount.OrderBy((p) => p.PriceWithDiscount).ToList();
            List<ProductInfo> productListSortByPriceDecrease = productListWithDiscount.OrderByDescending((p) => p.PriceWithDiscount).ToList();
            return new OkObjectResult(new
            {
                productListSortByPriceIncrease,
                productListSortByPriceDecrease
            });


        }
        // sap xep theo ten 
        [HttpGet]
        [Route("Product/Sort/Name")]
        public IActionResult GetAllProductSortByName()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productListSortByNameIncrease = productListWithDiscount.OrderBy((p) => p.Name.ToLower()).ToList();
            List<ProductInfo> productListSortByNameDecrease = productListWithDiscount.OrderByDescending((p) => p.Name.ToLower()).ToList();
            return new OkObjectResult(new
            {
                productListSortByNameIncrease,
                productListSortByNameDecrease
            });

        }
        // sap xep theo % giam
        [HttpGet]
        [Route("Product/Sort/Percentage")]
        public IActionResult GetAllProductSortByPercentage()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productSortByPercentageIncrease = productListWithDiscount
                                    .OrderBy(p => p.Discount.Count > 0 ? p.Discount.Where(d => d.EndDate >= DateTime.Now && d.StartDate <= DateTime.Now)
                                        .Select(d => d.Percentage)
                                        .DefaultIfEmpty(0)
                                        .Max() : 0)
                                    .ThenBy(p => p.Name)
                                    .ToList();

            List<ProductInfo> productSortByPercentageDecrease = productListWithDiscount
                                    .OrderByDescending(p => p.Discount.Count > 0 ? p.Discount.Where(d => d.EndDate >= DateTime.Now && d.StartDate <= DateTime.Now)
                                        .Select(d => d.Percentage)
                                        .DefaultIfEmpty(0)
                                        .Max() : 0)
                                    .ThenByDescending(p => p.Name)
                                    .ToList();


            return new OkObjectResult(new
            {
                productSortByPercentageIncrease,
                productSortByPercentageDecrease

            });

        }
        // tim danh sach san pham theo 1 danh muc
        [HttpGet]
        [Route("Product/FindByCategoryName/{categoryName}")]
        public IActionResult GetProductWithCategory(string categoryName)
        {
            Category category = new Category(categoryName);
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<ProductInfo> productListWithCategory = productList
                 .Where(product => product.Category.Name.ToLower() == categoryName.ToLower())
                 .Select(product => new ProductInfo
                 {
                     Id = product.Id,
                     Name = product.Name,
                     PriceWithDiscount = CalculateDiscountedPrice(product)
                 })
                .ToList();

            if (productListWithCategory.Count > 0)
            {
                return Ok(productListWithCategory);
            }
            else
            {
                return NotFound($"No products found in the category '{categoryName}'.");
            }
        }
        // tim san pham theo 1 ten 
        [HttpGet]
        [Route("Product/FindByProductName/{productName}")]
        public IActionResult FindProductByName(string productName)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<ProductInfo> productFound = productListWithPriceDiscount.Where(p => p.Name.ToLower().Contains(productName.ToLower())).ToList();
            if (productFound.Count == 0)
            {
                return BadRequest("Not found");
            }
            return Ok(productFound);

        }



        // TÌM THEO KHOẢNG GIÁ 
        [HttpGet]
        [Route("Product/FindByPrice/{minPrice} && {maxPrice}")]
        public IActionResult FindProductByPrice(double minPrice, double maxPrice)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<ProductInfo> productFound = productListWithPriceDiscount.Where(p => p.PriceWithDiscount > minPrice && p.PriceWithDiscount <= maxPrice).ToList();
            if (productFound.Count <= 0)
            {
                return NotFound($"Not found product have price between {minPrice} to {maxPrice}");
            }
            return Ok(productFound);
        }
        [HttpGet]
        [Route("Product/FindByID/{productId}")]
        // lay san pham theo id
        public IActionResult FindProductById(int productId)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            ProductInfo productFound = productListWithPriceDiscount.SingleOrDefault(p => p.Id == productId);
            if (productFound == null)
            {
                return NotFound($"Don't find product have id {productId}");
            }
            return Ok(productFound);
        }
        // tim san pham nhieu categories
        [HttpGet]
        [Route("Product/FindByCategories")]
        public IActionResult GetProductsWithCategories([FromQuery] List<string> categoryNames)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();

            var categoriesWithProducts = productListWithPriceDiscount
                .Where(product => categoryNames.Contains(product.Category.Name, StringComparer.OrdinalIgnoreCase))
                .GroupBy(product => product.Category)
                .Select(group => new
                {
                    CategoryName = group.Key.Name,
                    Products = group.ToList()
                })
                .ToList();

            if (categoriesWithProducts.Count > 0)
            {
                return Ok(categoriesWithProducts);
            }
            else
            {
                return NotFound("No products found in the specified categories.");
            }
        }





    }
}
