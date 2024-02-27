using AutoMapper;
using BusinessObject.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.DTO;
using PizzaLabApi.Models;

namespace PizzaLabApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private IReviewRepository reviewRepository;
        private readonly IMapper mapper;
        MapperConfiguration config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        public ReviewController(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
            mapper = config.CreateMapper();
        }

        [HttpGet("[action]")]
        public IActionResult GetAllReview()
        {
            return Ok(reviewRepository.GetReviews().Select(u => mapper.Map<ReviewDTO>(u)));
        }
        [HttpGet("[action]")]
        public IActionResult GetAReview(int Id)
        {
            Review review = null;
            try
            {
                List<Review> reviews = reviewRepository.GetReviews().ToList();
                review = reviews.FirstOrDefault(u => u.Id == Id);
            }
            catch (Exception ex)
            {
                return Conflict("No Review In DB");
            }
            if (review == null)
                return NotFound("Review doesnt exist");
            return Ok(mapper.Map<ReviewDTO>(review));
        }
        [HttpPost("[action]")]
        public IActionResult AddAReview(ReviewDTO reviewDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Review review = mapper.Map<Review>(reviewDTO);
                    reviewRepository.SaveReview(review);
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
                return Ok(reviewDTO);
            }
            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());

        }
        [HttpPut("[action]")]
        public IActionResult UpdateAReview(ReviewDTO reviewDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Review review = mapper.Map<Review>(reviewDTO);
                    reviewRepository.UpdateReview(review);
                }
                catch (Exception ex)
                {
                    return Conflict(ex.Message);
                }
                return Ok(reviewDTO);
            }
            return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());

        }
        [HttpDelete("[action]")]
        public IActionResult DeleteAReview(int Id)
        {
            Review review = null;
            try
            {
                List<Review> reviews = reviewRepository.GetReviews().ToList();
                review = reviews.FirstOrDefault(u => u.Id == Id);
                if (review == null)
                    return NotFound("Review doesnt exist");
                reviewRepository.DeleteReview(review);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(reviewRepository.GetReviews());
        }
    }
}
