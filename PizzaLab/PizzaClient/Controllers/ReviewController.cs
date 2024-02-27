using Microsoft.AspNetCore.Mvc;
using PizzaLabApi.Models;

namespace PizzaClient.Controllers
{
    public class ReviewController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateReviewDTOAsync(ReviewDTO review)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(
                "Review/AddAReviewDTO", review);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
               ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            //Tra ve san pham da add
            ViewBag.ReviewDTO = await response.Content.ReadAsAsync<ReviewDTO>();
            // return URI of the created resource.
            return View();//TO-DO: Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> GetReviewDTOAsync(int Id)
        {
            HttpResponseMessage response = await client.GetAsync("Review/GetAReview?Id=" + Id);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
               ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            if (response.IsSuccessStatusCode)
            {
                ViewBag.ReviewDTO = await response.Content.ReadAsAsync<ReviewDTO>();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> GetAllReviewDTOAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Review/GetAllReview");
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
               ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            if (response.IsSuccessStatusCode)
            {
                ViewBag.reviews = await response.Content.ReadAsAsync<List<ReviewDTO>>();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> UpdateReviewDTOAsync(ReviewDTO review)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"Review/UpdateAReview", review);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
               ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }

            // Deserialize the updated review from the response body.
            ViewBag.ReviewDTO = await response.Content.ReadAsAsync<ReviewDTO>();
            return View();//TO-DO:Dat Duong dan rang tra ve
        }

        public async Task<IActionResult> DeleteReviewDTOAsync(int Id)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                $"Review/DeleteAReview?Id=" + Id);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                //Tra ve thong bao loi
               ViewBag.Error = await response.Content.ReadAsStringAsync(); return View();
            }
            return View();//TO-DO:Dat Duong dan rang tra ve
        }
    }
}
