using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IReviewService
    {
        ResponseResult AddReview(int UserId,AddReviewResquest review);
        List<ReviewDTO> GetReviewsForProduct(int productId);
        double CalculateAverageRatingForProduct(int productId);
        ResponseResult DeleteReview(int reviewId,int userId);
        ResponseResult UpdateReview(int reviewId,int userId, float newRatingg, string newComment);
        ResponseResult GetTotalReviewCountForProduct(int productId);
        List<ReviewDTO> GetReviewsForProductSortedByDate(int productId);
        List<ReviewDTO> GetReviewsForProductSortedByRating(int productId);
    }
}
