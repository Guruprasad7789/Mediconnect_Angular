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
    public class ContactController : ControllerBase
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;

        public ContactController(ILogger<ContactController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

     

        [HttpPost(Name = "Save contact")]
        public IActionResult SaveContact(ContactUs feedback)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Contact(UserId, Name, Email, Message, CreatedDate) " + "Values('" + feedback.UserId + "','" + feedback.Name + "','" + feedback.Email + "','" + feedback.Message + "','"+ DateTime.Now+ "')", con);
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
