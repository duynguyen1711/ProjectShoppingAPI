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




        //  đếm tong so san pham
        [HttpGet]
        [Route("Product/CountProduct")]
        public IActionResult CountProduct()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            int total = productListWithDiscount.Count();
            return Ok(new
            {
                Message = "Total Product",
                Total = total
            });
        }
        // đếm sản phẩm nhiều danh mục
        [HttpGet]
        [Route("Product/CountProductWithCategories")]
        public IActionResult CountProductByCategoríes([FromQuery] List<string> categoryNames)
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();

            var categoryProducts = productListWithDiscount
            .Where(product => categoryNames.Contains(product.Category.Name, StringComparer.OrdinalIgnoreCase))
            .GroupBy(product => product.Category)
            .Select(group => new
            {
                Category = new
                {
                    Name = group.Key.Name,
                    Products = group.ToList()
                },
                Total = group.Count()
            })
            .ToList();

            int total = categoryProducts.Sum(c => c.Total);

            var result = new
            {
                CategoryProducts = categoryProducts,
                Message = "Total Products",
                Total = total
            };

            return Ok(result);
        }
        //đếm sản phẩm trong mức giá 
        [HttpGet]
        [Route("Product/CountProductByMultiplePriceRanges")]
        public IActionResult CountProductByMultiplePriceRanges([FromQuery] List<string> priceRanges)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();

            // Tạo dictionary để lưu trữ kết quả theo từng khoảng giá
            Dictionary<string, List<ProductInfo>> productByPriceRange = new Dictionary<string, List<ProductInfo>>();

            // Lặp qua từng khoảng giá và tìm các sản phẩm tương ứng
            foreach (var range in priceRanges)
            {
                // Phân tách khoảng giá thành MinPrice và MaxPrice
                var rangeValues = range.Split('-');
                if (rangeValues.Length != 2)
                {
                    // Nếu không đúng định dạng khoảng giá, bỏ qua và chuyển sang khoảng giá tiếp theo
                    continue;
                }

                if (double.TryParse(rangeValues[0], out double minPrice) && double.TryParse(rangeValues[1], out double maxPrice))
                {
                    // Tạo tên khoảng giá
                    string rangeName = $"{minPrice}-{maxPrice}";

                    // Lấy danh sách sản phẩm trong khoảng giá
                    List<ProductInfo> productsInRange = productListWithPriceDiscount
                        .Where(p => p.PriceWithDiscount >= minPrice && p.PriceWithDiscount <= maxPrice)
                        .ToList();

                    // Thêm vào dictionary
                    productByPriceRange[rangeName] = productsInRange;
                }
            }

            return Ok(new
            {
                Message = "Total Products by Price Ranges",
                ProductByPriceRange = productByPriceRange
            });
        }
        //đếm sản phẩm theo % giảm
        [HttpGet]
        [Route("Product/CountProductByDiscountPeriod")]
        public IActionResult CountProductByDiscountPeriod()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();

            List<ProductInfo> productsWithValidDiscount = productListWithDiscount.Where(p => p.Discount.Any(d => CheckDay(d, today)))
            .ToList();

            int total = productsWithValidDiscount.Count();
            if (total < 0)
            {
                return NotFound("No products are currently on discount.");
            }
            var result = new
            {
                Message = "Total Product",
                Total = total,
                Product = productsWithValidDiscount.Select(product => new ProductInfo
                {
                    Id = product.Id,
                    Name = product.Name,
                    OriginalPrice = product.OriginalPrice,
                    PriceWithDiscount = product.PriceWithDiscount,
                    Category = product.Category,
                    PercentageDiscount = product.Discount.Where(d => CheckDay(d, today))
                        .Select(d => d.Percentage)
                        .ToList(),
                    Discount = product.Discount.Where(d =>CheckDay(d,today)).ToList<Discount>()
                })
            };
            return Ok(result);

        }

        //đếm sản phẩm đã giảm giá và chưa giảm
        [HttpGet]
        [Route("Product/CountProductDiscounted")]
        public IActionResult CountProductDiscountedPrice()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productDiscounted = productListWithDiscount.Where(p => p.PriceWithDiscount < p.OriginalPrice && p.PriceWithDiscount > 0 &&p.OriginalPrice > 0).ToList();
            List<ProductInfo> productNotDiscounted = productListWithDiscount.Except(productDiscounted).ToList();



            int totalProductDiscounted = productDiscounted.Count();
            int totalProduct = productListWithDiscount.Count();
            return Ok(new
            {
                Message1 = "Total product discounted",
                ProductDiscounted = productDiscounted.Select(product => new ProductInfo
                {
                    Id = product.Id,
                    Name = product.Name,
                    OriginalPrice = product.OriginalPrice,
                    PriceWithDiscount = product.PriceWithDiscount,
                    Category = product.Category,
                    PercentageDiscount = product.Discount.Where(d => CheckDay(d, today))
                        .Select(d => d.Percentage)
                        .ToList(),
                    Discount = product.Discount.Where(d => CheckDay(d, today)).ToList<Discount>()
                }),
                TotalProductDiscounted = totalProductDiscounted,
                Message2 = "Products not yet discounted",
                ProductNotDiscounted = productNotDiscounted.Select(product => new ProductInfo
                {
                    Id = product.Id,
                    Name = product.Name,
                    OriginalPrice = product.OriginalPrice,
                    PriceWithDiscount = product.PriceWithDiscount,
                    Category = product.Category,
                }),
                TotalProductsNotDiscounted = totalProduct - totalProductDiscounted,
            });
        }
    }
}
