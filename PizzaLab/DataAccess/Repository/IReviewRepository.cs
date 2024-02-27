using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IReviewRepository
    {
        IEnumerable<Review> GetReviews();
        bool SaveReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        Review findById(int Id);
    }
}
