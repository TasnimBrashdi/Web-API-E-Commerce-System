using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public interface IReviewRepo
    {
        void AddReview(Review review);
        void DeleteReview(int id);
        IEnumerable<Review> GetReviewsByProductId(int productId)
        Review GetReviewById(int id);
        void UpdateReview(Review updatedReview);
    }
}