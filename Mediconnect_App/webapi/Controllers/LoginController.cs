using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace webapi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;
        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }
        [HttpPost(Name = "ValidateUserDetails")]
        public IActionResult ValidateUser(Register reg)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("select top 1 * from UserRegistration where Email='" + reg.email + "' and Password='" + reg.password + "'", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reg.regid = reader.GetValue(0).ToString();
                    reg.role = (int)reader.GetValue(1);
                    reg.firstname = reader.GetValue(2).ToString();
                    reg.lastname = reader.GetValue(3).ToString();
                    reg.gender = reader.GetValue(4).ToString();
                    reg.dob = reader.GetValue(5).ToString();
                    reg.bloodgroup = reader.GetValue(6).ToString();
                    reg.email = reader.GetValue(7).ToString();
                    reg.address = reader.GetValue(8).ToString();
                    reg.city = reader.GetValue(9).ToString();
                    reg.state = reader.GetValue(10).ToString();
                    reg.zipcode = reader.GetValue(11).ToString();
                    reg.username = reader.GetValue(12).ToString();
                    reg.password = reader.GetValue(13).ToString();
                    reg.confirmpassword = reader.GetValue(14).ToString();
                    reg.createddate = reader.GetValue(15).ToString();
                }

                if (reg.regid=="")
                {
                    reg = new Register();
                }
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return Ok(reg);
        }
    }
}
