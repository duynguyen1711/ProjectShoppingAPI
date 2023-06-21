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
        public static Discount electronicsDiscount = new Discount { Percentage = 10, StartDate = new DateTime(2023, 6, 1), EndDate = new DateTime(2023, 6, 15) };
        public static Discount clothingDiscount =  new Discount { Percentage = 20, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 26) };
        public static Discount vegetableDiscount =  new Discount { Percentage = 20, StartDate = new DateTime(2023, 6, 5), EndDate = new DateTime(2023, 6, 26) };
        public static List<Product> productList= new List<Product>
        {
                new Product { Id =1,Name = "Laptop", Price = 1000,Category =  electronicsCategory, Discount = new List<Discount> { electronicsDiscount } },
                new Product { Id =2,Name = "Shirt", Price = 50, Category = clothingCategory, Discount = new List<Discount> { clothingDiscount } },
                new Product { Id =3,Name = "Phone", Price = 800, Category = electronicsCategory, Discount = new List<Discount> {electronicsDiscount } },
                new Product { Id =4,Name = "Bucket", Price = 70, Category = clothingCategory, Discount = new List<Discount> {clothingDiscount} },
                new Product { Id =5,Name = "Baseball", Price = 70, Category = clothingCategory, Discount = new List<Discount> {clothingDiscount } },
                new Product { Id =6,Name = "Tomato", Price = 10, Category = vegetableCategory, Discount = new List<Discount> { vegetableDiscount } },
        };
       

        [HttpGet]
        [Route("test")]
        public IActionResult GetAll()
        {
            return Ok(productList);
        }

        [HttpGet]
        [Route("Product")]
        public IActionResult GetAllProductWithDiscount()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            return Ok(productListWithDiscount); 
        }
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

        [HttpGet]
        [Route("Product/Sort/Name")]
        public IActionResult GetAllProductSortByName()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List <ProductInfo> productListSortByNameIncrease = productListWithDiscount.OrderBy((p) => p.Name).ToList();
            List <ProductInfo> productListSortByNameDecrease = productListWithDiscount.OrderByDescending((p) => p.Name).ToList();
            return new OkObjectResult(new
            {
                productListSortByNameIncrease,
                productListSortByNameDecrease
            });
           
        }
        [HttpGet]
        [Route("Product/Sort/Percentage")]
        public IActionResult GetAllProductSortByPercentage()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productSortByPercentageIncrease = productListWithDiscount
                                    .OrderBy(p => p.Discount.Percentage)
                                    .ThenBy(p => p.Name)
                                    .ToList();
            List<ProductInfo> productSortByPercentageDecrease = productListWithDiscount
                                   .OrderByDescending(p => p.Discount.Percentage)
                                   .ThenBy(p => p.Name)
                                   .ToList();
            return new OkObjectResult(new
            {
                productSortByPercentageDecrease,
                productSortByPercentageIncrease
            }) ;

        }
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
        [HttpGet]
        [Route("Product/FindByProductName/{productName}")]
        public IActionResult FindProductByName(string productName)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<ProductInfo> productFound = productListWithPriceDiscount.Where(p => p.Name.ToLower() == productName.ToLower()).ToList();
            if (productFound.Count == 0)
            {
                return BadRequest("Not found");
            }

            return Ok(productFound);

        }
        public static double CalculateDiscountedPrice(Product product)
        {
            double discountedPrice = product.Price;
            
            foreach ( Discount discount in product.Discount)
            {

                if (DateTime.Now >= discount.StartDate && DateTime.Now <= discount.EndDate)
                {
                    discountedPrice = discountedPrice - (discountedPrice * (discount.Percentage)/100);

                }
                
            }
            return discountedPrice;
        }
        
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
                        Id  =product.Id,
                        Name = product.Name,
                        OriginalPrice = product.Price,
                        PriceWithDiscount = discountedPrice,
                        Category = product.Category,
                        Discount = product.Discount.Count > 0 ? product.Discount[0] : null,
                    });
                }
            }
            return productListWithDiscount;
        }

        [HttpGet]
        [Route("Product/FindByPrice/{minPrice} && {maxPrice}")]
        public IActionResult FindProductByPrice(double minPrice,double maxPrice)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<ProductInfo> productFound = productListWithPriceDiscount.Where(p => p.PriceWithDiscount >minPrice && p.PriceWithDiscount <= maxPrice).ToList();
            if (productFound.Count <= 0)
            {
                return NotFound($"Not found product have price between {minPrice} to {maxPrice}");
            }
            return Ok(productFound);
        }
        [HttpGet]
        [Route("Product/FindByID/{productId}")]
        public IActionResult FindProductById(int productId)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            ProductInfo productFound = productListWithPriceDiscount.SingleOrDefault(p => p.Id == productId);
            if (productFound ==null)
            {
                return NotFound($"Don't find product have id {productId}");
            }
            return Ok(productFound);
        }
        [HttpGet]
        [Route("Product/FindByCategories")]
        public IActionResult GetProductsWithCategories([FromQuery]List<string> categoryNames)
        {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<Category> categoriesWithProducts = new List<Category>();
            foreach (string categoryName in categoryNames)
            {
                Category category = new Category { Name = categoryName };
                List<ProductInfo> productsInCategory = productListWithPriceDiscount
                    .Where(product => product.Category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
                    .Select(product => new ProductInfo
                    {
                        Id = product.Id,
                        Name = product.Name,
                        OriginalPrice = product.OriginalPrice,
                        PriceWithDiscount = product.PriceWithDiscount,
                        Discount = product.Discount,
                    })
                    .ToList();

                if (productsInCategory.Count > 0)
                {
                    category.Products = productsInCategory;
                    categoriesWithProducts.Add(category);
                }
            }

            if (categoriesWithProducts.Count > 0)
            {
                return Ok(categoriesWithProducts);
            }
            else
            {
                return NotFound($"No products found in the specified categories.");
            }
        }
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
        [HttpGet]
        [Route("Product/CountProduct/{categoryName}")]
        public IActionResult CountProductByCategory(string categoryName)
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            int total = productListWithDiscount.Count(p=> p.Category.Name.ToLower() == categoryName.ToLower());
            return Ok(new
            {
                Message = "Total Product",
                Total = total
            });
        }
        [HttpGet]
        [Route("Product/CountProductWithCategories")]
        public IActionResult CountProductByCategoríes([FromQuery] List<string> categoryNames)
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<Category> category = new List<Category>();
            List<ProductInfo> productListWithCategories = productListWithDiscount
            .Where(product => categoryNames.Any(categoryName =>
                product.Category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)))
            .Select(p => new ProductInfo
            {
                Id = p.Id,
                Name = p.Name,
                OriginalPrice = p.OriginalPrice,
                PriceWithDiscount = p.PriceWithDiscount,
                Category =p.Category,
                Discount = p.Discount
            }
            )   
            .ToList();

            var categoryTotals = productListWithCategories
            .GroupBy(product => product.Category.Name)
            .Select(group => new
            {
                CategoryName = group.Key,
                Total = group.Count()
            })
            .ToList();

            int total = productListWithCategories.Count();
            var result = new
            {
                CategoryTotals = categoryTotals,
                Message = "Total Product",
                Total = total
            };
            return Ok(result);
        }
        [HttpGet]
        [Route("Product/CountProduct/{minPrice}&&{maxPrice}")]
        public IActionResult CountProductByPriceRange(double minPrice,double maxPrice ) {
            List<ProductInfo> productListWithPriceDiscount = GetProductListWithDiscount();
            List<ProductInfo> productFound = productListWithPriceDiscount.Where(p => p.PriceWithDiscount >= minPrice && p.PriceWithDiscount <= maxPrice)
                .Select(p => new ProductInfo
                {
                    Id = p.Id,
                    Name = p.Name,
                    OriginalPrice = p.OriginalPrice,
                    PriceWithDiscount = p.PriceWithDiscount,
                    Discount = p.Discount
                })
                .ToList();
            int total = productFound.Count();
            if (total < 0)
            {
                return NotFound("Not found");
            }
            return Ok(new
            {
                Message = "Total Product",
                Total = total,
                Product = productFound
            });
        }

        [HttpGet]
        [Route("Product/CountProductByDiscountPeriod")]
        public IActionResult CountProductByDiscountPeriod()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List <ProductInfo> ProductByDiscountPeriod = productListWithDiscount.Where(p=>p.Discount?.StartDate <=DateTime.Now && 
            p.Discount?.EndDate >= DateTime.Now)
            .Select(product => new ProductInfo
            {
                Id = product.Id,
                Name = product.Name,
                OriginalPrice = product.OriginalPrice,
                PriceWithDiscount = product.PriceWithDiscount,
                Category = product.Category,
                Discount = product.Discount
            })
            .ToList();

            int total = ProductByDiscountPeriod.Count();
            if (total < 0)
            {
                return NotFound("No products are currently on discount.");
            }
            var result = new
            {
                Message = "Total Product",
                Total = total,
                Product = ProductByDiscountPeriod,
            };
            return Ok(result);
           
        }
        [HttpGet]
        [Route("Product/CountProductDiscounted")]
        public IActionResult CountProductDiscountedPrice()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productDiscounted= productListWithDiscount.Where(p=>p.PriceWithDiscount != p.OriginalPrice)
                .ToList();
            int totalProductDiscounted = productDiscounted.Count();
            int totalProduct = productListWithDiscount.Count();
            return Ok(new
            {
                Message1 = "Total product discounted",
                ProductDiscounted = totalProductDiscounted,
                Message2 = "Products not yet discounted",
                ProductsNotYetDiscounted = totalProduct - totalProductDiscounted,
            });
        }
    }
}
