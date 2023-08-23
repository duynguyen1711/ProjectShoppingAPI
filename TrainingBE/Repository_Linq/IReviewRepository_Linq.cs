using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public interface IReviewRepository_Linq : IRepository<Review>
    {
        List<Review> GetReviewsForProduct(int productId);
        double CalculateAverageRatingForProduct(int productId);
        List<Review> GetReviewsForProductSortedByDate(int productId);
        List<Review> GetReviewsForProductSortedByRating(int productId);
    }
}
