using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;

namespace E_Commerce_System_API.Services
{
    public interface IReviewService
    {
        ReviewInput AddReview(ReviewInput review, int productId);
        void DeleteReview(int reviewId);
        List<Review> GetReviewsByIdProducts(int productId);
        ReviewInput UpdateReview(ReviewInput updatedReview);
    }
}