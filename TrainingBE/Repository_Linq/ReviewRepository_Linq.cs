using Microsoft.EntityFrameworkCore;
using TrainingBE.Data;
using TrainingBE.Model;

namespace TrainingBE.Repository_Linq
{
    public class ReviewRepository_Linq : Repository<Review>, IReviewRepository_Linq
    {
        public ReviewRepository_Linq(MyDBContext context) : base(context)
        {
           
        }
        public double CalculateAverageRatingForProduct(int productId)
        {
            var averageRating = _dbSet
               .Where(r => r.ProductId == productId)
               .Average(r => r.Rating);

            return averageRating;
        }

        public List<Review> GetReviewsForProductSortedByDate(int productId)
        {
            return _dbSet
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.DateCreated)
                .ToList();
        }

        public List<Review> GetReviewsForProduct(int productId)
        {
            var reviews = _dbSet
                .Where(r => r.ProductId == productId)
                .ToList();

            return reviews;
        }
        public List<Review> GetReviewsForProductSortedByRating(int productId)
        {
            return _dbSet
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.Rating)
                .ThenByDescending(r => r.DateCreated)
                .ToList();
        }
    }
   
}
