using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Reflection.Emit;
using System.Reflection;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;

        public FeedbackController(ILogger<FeedbackController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

     

        [HttpPost(Name = "Save feedback")]
        public IActionResult SaveFeedback(FeedbackReq feedback)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Feedback(UserId, Ratings, Feedback, CreatedDate) " + "Values('" + feedback.UserId + "','" + feedback.Rating + "','" + feedback.Feedback + "','"+ DateTime.Now+ "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new SuccessRes { Success = true, Message = "Feedback upadated successfully."});
        }
    }
}
