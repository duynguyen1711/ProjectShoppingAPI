using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IReviewService
    {
        ResponseResult AddReview(Review review);
        List<ReviewDTO> GetReviewsForProduct(int productId);
        double CalculateAverageRatingForProduct(int productId);
        ResponseResult DeleteReview(int reviewId);
        ResponseResult UpdateReview(int reviewId, float newRating, string newComment);
        ResponseResult GetTotalReviewCountForProduct(int productId);
        List<ReviewDTO> GetReviewsForProductSortedByDate(int productId);
        List<ReviewDTO> GetReviewsForProductSortedByRating(int productId);
    }
}
