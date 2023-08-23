using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;

namespace TrainingBE.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ResponseResult AddReview(Review review)
        {
            if(_unitOfWork.UserRepository.GetById(review.UserId)==null){
                return new ResponseResult { IsError = true, ErrorMessage = "User not found" };
            }
            if (_unitOfWork.ProductRepository.GetById(review.ProductId) == null)
            {
                return new ResponseResult { IsError = true, ErrorMessage = "Product not found" };
            }
            if (review.Rating < 0 || review.Rating > 5)
            {
                return new ResponseResult { IsError = true, ErrorMessage = "Rating must be between 0 and 5." };
            }
            _unitOfWork.ReviewRepository.Add(review);
            _unitOfWork.Save();
            return new ResponseResult { IsError = false, ErrorMessage = "Review Add Sucessfully." };
        }

        public List<ReviewDTO> GetReviewsForProduct(int productId)
        {
            var reviews = _unitOfWork.ReviewRepository.GetReviewsForProduct(productId);
            var reviewDTOs = new List<ReviewDTO>();

            foreach (var review in reviews)
            {
                var user = _unitOfWork.UserRepository.GetById(review.UserId);
                var reviewDTO = new ReviewDTO
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    UserName = user?.name,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    DateCreated = review.DateCreated,
                    TimeAgo = CalculateTimeAgo(review.DateCreated)
                };
                reviewDTOs.Add(reviewDTO);
            }

            return reviewDTOs;
        }

        public double CalculateAverageRatingForProduct(int productId)
        {
            return _unitOfWork.ReviewRepository.CalculateAverageRatingForProduct(productId);
        }

        public ResponseResult DeleteReview(int reviewId)
        {
            var existingReview = _unitOfWork.ReviewRepository.GetById(reviewId);
            if (existingReview == null)
            {
                return new ResponseResult { IsError = true, ErrorMessage = "Review not found." };
            }
            _unitOfWork.ReviewRepository.Delete(existingReview);
            _unitOfWork.Save();
            return new ResponseResult { IsError = false, Message = "Review Deleted Successfully." };
        }
        private string CalculateTimeAgo(DateTime dateTime)
        {
            TimeSpan timeDifference = DateTime.UtcNow - dateTime;

            if (timeDifference.TotalMinutes < 1)
                return "just now";
            if (timeDifference.TotalMinutes < 60)
                return $"{(int)timeDifference.TotalMinutes}m ago";
            if (timeDifference.TotalHours < 24)
                return $"{(int)timeDifference.TotalHours}h ago";
            if (timeDifference.TotalDays < 7)
                return $"{(int)timeDifference.TotalDays}d ago";
            if (timeDifference.TotalDays < 14)
                return "1w ago";
            if (timeDifference.TotalDays < 30)
                return $"{(int)(timeDifference.TotalDays / 7)}w ago";
            if (timeDifference.TotalDays < 60)
                return "1m ago";
            if (timeDifference.TotalDays < 365)
                return $"{(int)(timeDifference.TotalDays / 30)}m ago";

            return $"{(int)(timeDifference.TotalDays / 365)}y ago";
        }

        public ResponseResult UpdateReview(int reviewId, float newRating, string newComment)
        {
            var existingReview = _unitOfWork.ReviewRepository.GetById(reviewId);
            if (existingReview == null)
            {
                return new ResponseResult { IsError = true, ErrorMessage = "Review not found." };
            }

            existingReview.Rating = newRating;
            existingReview.Comment = newComment;

            _unitOfWork.ReviewRepository.Update(existingReview);
            _unitOfWork.Save();

            return new ResponseResult { IsError = false, Message = "Review updated successfully." };
        }
        public ResponseResult GetTotalReviewCountForProduct(int productId)
        {
            if (_unitOfWork.ProductRepository.GetById(productId) == null)
            {
                return new ResponseResult { IsError = true, ErrorMessage = "Product not found." };

            }
            var reviews = _unitOfWork.ReviewRepository.GetReviewsForProduct(productId);
            return new ResponseResult { IsError = false, Message = $"Product have {reviews.Count} comment" };
        }
        public List<ReviewDTO> GetReviewsForProductSortedByDate(int productId)
        {
            var reviews = _unitOfWork.ReviewRepository.GetReviewsForProductSortedByDate(productId);
            var reviewDTOs = new List<ReviewDTO>();

            foreach (var review in reviews)
            {
                var user = _unitOfWork.UserRepository.GetById(review.UserId);
                var reviewDTO = new ReviewDTO
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    UserName = user?.name,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    DateCreated = review.DateCreated,
                    TimeAgo = CalculateTimeAgo(review.DateCreated)
                };
                reviewDTOs.Add(reviewDTO);
            }

            return reviewDTOs;
        }
        public List<ReviewDTO> GetReviewsForProductSortedByRating(int productId)
        {
            var reviews = _unitOfWork.ReviewRepository.GetReviewsForProductSortedByRating(productId);
            var reviewDTOs = new List<ReviewDTO>();

            foreach (var review in reviews)
            {
                var user = _unitOfWork.UserRepository.GetById(review.UserId);
                var reviewDTO = new ReviewDTO
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    UserName = user?.name,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    DateCreated = review.DateCreated,
                    TimeAgo = CalculateTimeAgo(review.DateCreated)
                };
                reviewDTOs.Add(reviewDTO);
            }

            return reviewDTOs;
        }
    }
}
