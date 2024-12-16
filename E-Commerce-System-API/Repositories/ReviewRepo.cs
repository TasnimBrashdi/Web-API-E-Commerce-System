using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        // Add a new review
        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }
        //Get list of review 
        public List<Review> GetAllReview()
        {
            return _context.Reviews.ToList();

        }
        // Get review by ID
        public Review GetReviewById(int id)
        {
            return _context.Reviews.FirstOrDefault(p => p.Id == id);
        }
        //delete review 
        public void DeleteReview(int id)
        {
            var review = _context.Reviews.FirstOrDefault(p => p.Id == id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
        }
        // Update an existing review
        public void UpdateReview(Review updatedReview)
        {
            var existingReview = _context.Reviews.FirstOrDefault(r => r.Id == updatedReview.Id);
            if (existingReview != null)
            {
                existingReview.Rating = updatedReview.Rating;
                existingReview.Comment = updatedReview.Comment;
                existingReview.ReviewDate = DateTime.UtcNow; // Update to current timestamp
                _context.SaveChanges();
            }
        }
    }
}
