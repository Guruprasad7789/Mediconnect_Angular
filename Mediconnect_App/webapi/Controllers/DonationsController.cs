using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class DonationsController : ControllerBase
    {
        private readonly ILogger<DonationsController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;
        public DonationsController(ILogger<DonationsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IEnumerable<DonationDetails> GetDonations()
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
                don.donorid = reader.GetValue(0).ToString();
                don.organs = reader.GetValue(1).ToString();
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
    }
}
