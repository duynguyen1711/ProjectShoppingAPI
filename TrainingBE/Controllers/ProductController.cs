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
                        PercentageDiscount = product.Discount.Where(d => CheckDay(d, today))
                        .Select(d => d.Percentage)
                        .ToList(),
                        Discount = product.Discount.Where(d => CheckDay(d, today)).ToList(),
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
        // sap xep theo % giam
        [HttpGet]
        [Route("Product/Sort/Percentage")]
        public IActionResult GetAllProductSortByPercentage()
        {
              List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productSortByPercentageIncrease = productListWithDiscount
                                    .OrderBy(p => p.Discount.Count > 0 ? p.Discount.Where(d => CheckDay(d, today))
                                        .Select(d => d.Percentage)
                                        .DefaultIfEmpty(0)
                                        .Max() : 0)
                                    .ThenBy(p => p.Name)
                                    .ToList();

            List<ProductInfo> productSortByPercentageDecrease = productListWithDiscount
                                    .OrderByDescending(p => p.Discount.Count > 0 ? p.Discount.Where(d => CheckDay(d, today))
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
                    Discount = product.Discount.Where(d => CheckDay(d, today)).ToList<Discount>()
                })
            };
            return Ok(result);

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

        //đếm sản phẩm đã giảm giá và chưa giảm
        [HttpGet]
        [Route("Product/CountProductDiscounted")]
        public IActionResult CountProductDiscountedPrice()
        {
            List<ProductInfo> productListWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> productDiscounted = productListWithDiscount.Where(p => p.PriceWithDiscount < p.OriginalPrice && p.PriceWithDiscount > 0 && p.OriginalPrice > 0).ToList();
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
        // checkout
        [HttpPost]
        [Route("Product/process-payment")]
        public IActionResult ProcessPayment([FromBody] List<int> idProduct)
        {
            List<ProductInfo> productPriceWithDiscount = GetProductListWithDiscount();
            List<ProductInfo> selectedProducts = productPriceWithDiscount.Where(p => idProduct.Contains(p.Id)).ToList();

            double subtotal = CalculateSubtotal(selectedProducts);
            double shippingFee = CalculateShippingFee(subtotal);
            double totalAmount = subtotal + shippingFee;
            var result = new
            {
                Product = selectedProducts.Select(product => new ProductInfo
                {
                    Id = product.Id,
                    Name = product.Name,
                    OriginalPrice = product.OriginalPrice,
                    PriceWithDiscount = product.PriceWithDiscount,
                }),
                Subtotal = subtotal,
                ShippingFee = shippingFee,
                TotalAmount = totalAmount
            };
            return Ok(result);
        }
        private double CalculateSubtotal(List<ProductInfo> selectedProducts)
        {
            double subtotal = 0;

            subtotal = selectedProducts.Sum(p => p.PriceWithDiscount);
            if (subtotal >= 200)
            {
                return subtotal = subtotal * 0.9;
            }
            return subtotal;
        }
        private double CalculateShippingFee(double subtotal)
        {
            double shippingFee = 3;
            if (subtotal < 50)
            {
                return shippingFee;
            }

            if (subtotal < 70)
            {
                shippingFee = shippingFee - 1;
            }
            if (subtotal <= 100)
                shippingFee = shippingFee - 1.5;
            shippingFee = shippingFee - 2;
            return shippingFee;
        }
        // add product
        [HttpPost]
        [Route("Product/AddProduct")]
        public IActionResult AddProduct([FromBody] ProductWithoutID productAdd)
        {
            var productListSorted = productList.OrderBy(p => p.Id).ToList();
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
            int index = productList.FindIndex(p => p.Id == id);
            if (index != -1)
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
        public IActionResult DeleteProduct(int id)
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
