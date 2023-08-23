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

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public IActionResult AddReview([FromBody] Review review)
        {
            try
            {
                ResponseResult result = _reviewService.AddReview(review);

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

        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            try
            {
                ResponseResult result = _reviewService.DeleteReview(reviewId);

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
        [HttpPut("reviews/{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] UpdateReviewDTO updateDTO)
        {
            if (updateDTO == null)
            {
                return BadRequest(new ResponseResult { IsError = true, ErrorMessage = "Invalid data." });
            }

            try
            {
                ResponseResult result = _reviewService.UpdateReview(reviewId, updateDTO.NewRating, updateDTO.NewComment);

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
