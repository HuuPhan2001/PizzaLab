using BusinessObject.Models;
using DataAccess.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        public bool DeleteReview(Review review) => ReviewDao.Instance.deleteReview(review);

        public Review findById(int Id) => ReviewDao.Instance.findById(Id);

        public IEnumerable<Review> GetReviews() => ReviewDao.Instance.listReview();

        public bool SaveReview(Review review) => ReviewDao.Instance.addReview(review);

        public bool UpdateReview(Review review) => ReviewDao.Instance.updateReview(review);
    }
}
