using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController: ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public ReviewController(IReviewService reviewService, IUserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }
        [Authorize]
        [HttpPost]
        public IActionResult AddReview([FromBody] AddReviewResquest reviewRequest)
        {
            int userId = _userService.GetUserIdFromClaims(User);
            try
            {
                ResponseResult result = _reviewService.AddReview(userId, reviewRequest);

                if (result.IsError)
                {
                    return StatusCode(404, result);
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult { IsError = true, ErrorMessage = "An error occurred while deleting the review." });
            }
        }
        
        [HttpGet("{productId}")]
        public IActionResult GetReviewsForProduct(int productId)
        {
            
            var reviews = _reviewService.GetReviewsForProduct(productId);
            if (reviews == null)
            {
                return NotFound("Product not found");
            }
            return Ok(reviews);
        }

        [HttpGet("average-rating/{productId}")]
        public IActionResult CalculateAverageRatingForProduct(int productId)
        {
            var averageRating = _reviewService.CalculateAverageRatingForProduct(productId);
            return Ok(averageRating);
        }
        [Authorize]
        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            int userId = _userService.GetUserIdFromClaims(User);
            try
            {
                ResponseResult result = _reviewService.DeleteReview(reviewId,userId);

                if (result.IsError)
                {
                    return StatusCode(404, result);
                }
                else
                {
                    return Ok(result); 
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult { IsError = true, ErrorMessage = "An error occurred while deleting the review." });
            }
        }
        [Authorize]
        [HttpPut("reviews/{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] UpdateReviewDTO updateDTO)
        {
            if (updateDTO == null)
            {
                return BadRequest(new ResponseResult { IsError = true, ErrorMessage = "Invalid data." });
            }
            int userId = _userService.GetUserIdFromClaims(User);
            try
            {
                ResponseResult result = _reviewService.UpdateReview(reviewId, userId, updateDTO.NewRating, updateDTO.NewComment);

                if (result.IsError)
                {
                    return NotFound(result);
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseResult { IsError = true, ErrorMessage = "An error occurred while updating the review." });
            }
        }
        [HttpGet("{productId}/total-review-count")]
        public IActionResult GetTotalReviewCountForProduct(int productId)
        {
            ResponseResult response = _reviewService.GetTotalReviewCountForProduct(productId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpGet("{productId}/reviews-sorted-by-date")]
        public IActionResult GetReviewsForProductSortedByDate(int productId)
        {
            try
            {
                var reviews = _reviewService.GetReviewsForProductSortedByDate(productId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpGet("{productId}/reviews-sorted-by-rating")]
        public IActionResult GetReviewsForProductSortedByRating(int productId)
        {
            try
            {
                var reviews = _reviewService.GetReviewsForProductSortedByRating(productId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }   
    }
}
