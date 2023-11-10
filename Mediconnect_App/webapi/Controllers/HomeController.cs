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
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet(Name = "GetDonationDetails")]
        public IEnumerable<DonationDetails> Get()
        {
            SqlDataReader reader = null/* TODO Change to default(_) if this is not a reference type */;
            SqlConnection myConnection = new SqlConnection(_config);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select * from DonorData";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            List<DonationDetails> donList = new List<DonationDetails>();
            while (reader.Read())
            {
                DonationDetails don = new DonationDetails();
                don.donorid = (int)reader.GetValue(0);
                don.organs = new List<Organ> { };
                don.medinfo = reader.GetValue(2).ToString();
                don.name = reader.GetValue(3).ToString();
                don.relationship = reader.GetValue(4).ToString();
                don.contact = reader.GetValue(5).ToString();
                don.address = reader.GetValue(6).ToString();
                don.sign = reader.GetValue(7).ToString();
                don.createddate = reader.GetValue(8).ToString();
                donList.Add(don);
            }
            myConnection.Close();
            return donList;
        }

        [HttpPost(Name = "PostRegDetails")]
        public IActionResult CreateUser(Register reg)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO UserRegistration(Role, Firstname,Lastname,Gender,DOB,BloodGroup,Email,Address,City,State,Zipcode,Username,Password,ConfirmPassword,CreatedDate) " + "Values('" + 0 + "','" + reg.firstname + "','" + reg.lastname + "','" + reg.gender + "','" + DateTime.Now.ToLongDateString() + "','" + reg.bloodgroup + "','" + reg.email + "','" + reg.address + "','" + reg.city + "','" + reg.state + "','" + reg.zipcode + "','" + reg.email + "','" + reg.password + "','" + reg.confirmpassword + "','" + DateTime.Now + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return Ok(reg);
        }
    }
}
