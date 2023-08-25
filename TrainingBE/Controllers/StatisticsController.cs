using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TrainingBE.DTO;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IExcelService _excelService;
        public StatisticsController(IProductService productService, IOrderService orderService, IUserService userService, IExcelService excelService)
        {
            _productService = productService;
            _orderService = orderService;
            _userService = userService;
            _excelService = excelService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("best-selling-products")]
        public IActionResult GetBestSellingProducts()
        {
            try
            {
                var bestSellingProducts = _productService.GetBestSellingProducts();
                return Ok(bestSellingProducts);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("order-statistics")]
        public IActionResult GetOrderStatisticsByMonthAndYear(int year, int month)
        {
            try
            {
                var orderStatistics = _orderService.GetOrderDetailsByMonthAndYear(year, month);
                return Ok(orderStatistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("customer-statistics")]
        public IActionResult GetCustomerStatistic()
        {
            try
            {
                var orderStatistics = _userService.GetCustomerRevenues();
                return Ok(orderStatistics);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("export-excel-order")]
        public IActionResult ExportToExcelOrder(int year,int month)
        {
            try
            {
                var orderDetails = _orderService.GetOrderDetailsByMonthAndYear1(year, month);
                var excelData = _excelService.ExportToExcel<OrderDetailDTO>(orderDetails);

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderDetails.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("export-excel-customer")]
        public IActionResult ExportToExcelCustomer()
        {
            try
            {
                var orderStatistics = _userService.GetCustomerRevenues();
                var excelData = _excelService.ExportToExcel<CustomerStatisticDTO>(orderStatistics);

                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "customer.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
