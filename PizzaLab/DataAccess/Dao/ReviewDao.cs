using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dao
{
    public class ReviewDao
    {
        private static ReviewDao instance = null;
        public static readonly object instanceLock = new object();
        private static PizzaLabContext dbcontext = new PizzaLabContext();

        public static ReviewDao Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new ReviewDao();
                }
                dbcontext = new PizzaLabContext();
                return instance;
            }
        }
        public IEnumerable<Review> listReview()
        {
            var list = new List<Review>();
            try
            {
                list = dbcontext.Reviews.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public Review findById(int Id)
        {
            Review review = new Review();
            try
            {
                review = dbcontext.Reviews.SingleOrDefault(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return review;
        }
        public bool addReview(Review review)
        {
            try
            {
                dbcontext.Reviews.Add(review);
                dbcontext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool updateReview(Review review)
        {
            try
            {
                Review pro = findById(review.Id);
                if (pro != null)
                {
                    dbcontext.Entry(pro).CurrentValues.SetValues(review);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Review does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }

        public bool deleteReview(Review review)
        {
            try
            {
                Review pro = findById(review.Id);
                if (pro != null)
                {
                    dbcontext.Reviews.Remove(pro);
                    dbcontext.SaveChanges();
                }
                else
                {
                    throw new Exception("Review does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return true;
        }
    }
}
