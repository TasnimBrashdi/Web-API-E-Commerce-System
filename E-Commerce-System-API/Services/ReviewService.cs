using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;

namespace E_Commerce_System_API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IOrederService _orderservice;
        private readonly IProductRepo _productRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewService(IReviewRepo reviewRepo, IOrederService orderservice, IProductRepo productRepo,
          IHttpContextAccessor httpContextAccessor)
        {
            _reviewRepo = reviewRepo;
            _orderservice = orderservice;
            _productRepo = productRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Helper method to get the current authenticated user's ID
        private int GetCurrentUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        // Recalculate the overall rating of product
        private void calculateProductRating(int productId)
        {
            var reviews = _reviewRepo.GetAllReview()
                .Where(r => r.ProductId == productId);

            // Calculate the average rating 
            int newRating = reviews.Any() ? (int)Math.Round(reviews.Average(r => r.Rating)) : 0;

            var product = _productRepo.GetProductById(productId);
            if (product != null)
            {
                // Update the product's overall rating
                product.Overall_Rating = newRating;
                _productRepo.UpdateProduct(product);
            }
        }


        // Add a new review
        public ReviewInput AddReview(ReviewInput review, int productId)
        {
            int userId = GetCurrentUserId();

            // Check if the user has purchased the product
            bool hasPurchased = _orderservice.HasUserPurchasedProduct(userId, productId);
            if (!hasPurchased)
            {
                throw new UnauthorizedAccessException("You can only review products you have purchased.");
            }

            // Check if the user has already reviewed the product
            bool alreadyReviewed = _reviewRepo.GetAllReview()
                .Any(r => r.ProductId == productId && r.UId == userId);
            if (alreadyReviewed)
            {
                throw new InvalidOperationException("You have already reviewed this product.");
            }

            // Map DTO to Review entity
            var reviews = new Review
            {
                UId = userId,
                ProductId = review.ProductId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = DateTime.UtcNow
            };
            _reviewRepo.AddReview(reviews);

            //Recalculate product overall rating
            calculateProductRating(productId);
            // Return the created review as DTO, including the new ID
            return new ReviewInput
            {
                Id = review.Id,
                ProductId = review.ProductId,
                Rating = review.Rating,
                Comment = review.Comment
            };
        }


        // Update an existing review (only by the creator)
        public ReviewInput UpdateReview(ReviewInput updatedReview)
        {
            if (!updatedReview.Id.HasValue)
            {
                throw new ArgumentException("Review ID is required for updating a review.");
            }
            int userId = GetCurrentUserId();
            // Fetch the existing review
            var existingReview = _reviewRepo.GetReviewById(updatedReview.Id.Value);

            if (existingReview == null)
            {
                throw new KeyNotFoundException("The review does not exist.");
            }
            if (existingReview.UId != userId)
            {
                throw new UnauthorizedAccessException("You can only update your own reviews.");
            }

            // Update the review
            existingReview.Rating = updatedReview.Rating;
            existingReview.Comment = updatedReview.Comment;
            existingReview.ReviewDate = DateTime.UtcNow;

            _reviewRepo.UpdateReview(existingReview);

            // Recalculate product overall rating
            calculateProductRating(existingReview.ProductId);
            // Return the updated review as DTO
            return new ReviewInput
            {
                Id = existingReview.Id,
                ProductId = existingReview.ProductId,
                Rating = existingReview.Rating,
                Comment = existingReview.Comment
            };
        }

        // Delete a review (only by the creator)
        public void DeleteReview(int reviewId)
        {
            int userId = GetCurrentUserId();
            var existingReview = _reviewRepo.GetReviewById(reviewId);

            if (existingReview == null || existingReview.UId != userId)
            {
                throw new UnauthorizedAccessException("You can only delete your own reviews.");
            }

            _reviewRepo.DeleteReview(reviewId);

            // Recalculate product overall rating
            calculateProductRating(existingReview.ProductId);
        }
        public List<Review> GetReviewsByIdProducts(int productId)
        {
            var review = _reviewRepo.GetReviewsByProductId(productId).ToList();
            if (review == null)
            {
                throw new InvalidOperationException("No books found.");
            }
            return review;
        }

    }

}
