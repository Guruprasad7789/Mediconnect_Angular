using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class DonorController : ControllerBase
    {
        private readonly ILogger<DonorController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;
        public DonorController(ILogger<DonorController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost(Name = "PostDonorDetails")]
        public IActionResult CreateDonor(DonorDetails don)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO DonorData(Organs,MedInfo,Name,Relationship,Contact,Address,Sign,CreatedDate) " + "Values('" + don.organs + "','" + don.medinfo + "','" + don.name + "','" + don.relationship + "','" + don.contact + "','" + don.address + "','" + don.sign + "','" + DateTime.Now.ToLongDateString() + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return Ok(new SuccessRes { Success = true, Message = "Donor upadated successfully." });
        }

    }
}
