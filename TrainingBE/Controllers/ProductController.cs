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
        public static DateTime today = new (2023, 6, 14);
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

                if (CheckDay(discount ,today))
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
    }
}
