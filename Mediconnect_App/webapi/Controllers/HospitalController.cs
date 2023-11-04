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
    public class HospitalController : ControllerBase
    {
        private readonly ILogger<HospitalController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;

        public HospitalController(ILogger<HospitalController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet(Name = "GetHospitalDetails")]
        public IEnumerable<Hospital> Get()
        {
            SqlDataReader reader = null/* TODO Change to default(_) if this is not a reference type */;
            SqlConnection myConnection = new SqlConnection(_config);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select * from Hospital";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            List<Hospital> hosptalList = new List<Hospital>();
            while (reader.Read())
            {
                Hospital hospital = new Hospital();
                hospital.Name = reader.GetValue(1).ToString();
                hospital.Address = reader.GetValue(2).ToString();
                hospital.GoogleMapLink = reader.GetValue(3).ToString();
                hospital.DistanceInKm = (int)reader.GetValue(4);
                hosptalList.Add(hospital);
            }
            myConnection.Close();
            return hosptalList;
        }


    }

}