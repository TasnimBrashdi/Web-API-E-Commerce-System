using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReviewController:ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // Add a new review
        [HttpPost("{productId}")]
        public IActionResult AddReview(int productId, [FromBody] ReviewInput review)
        {
            if (review == null)
            {
                return BadRequest("Review cannot be null.");
            }

            try
            {
                _reviewService.AddReview(review, productId);
                return Ok("Review added successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update an existing review
        [HttpPut("{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewInput review)
        {
            if (review == null || review.Id != reviewId)
            {
                return BadRequest("Invalid review data.");
            }

            try
            {
                _reviewService.UpdateReview(review);
                return Ok("Review updated successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        // Delete a review
        [HttpDelete("{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            try
            {
                _reviewService.DeleteReview(reviewId);
                return Ok("Review deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        // Get all reviews for a product
        [HttpGet("product/{productId}")]
        public IActionResult GetReviewsByProductId(int productId)
        {
            try
            {
                var reviews = _reviewService.GetReviewsByIdProducts(productId);
                return Ok(reviews);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
