using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class OrganController : ControllerBase
    {
        private readonly ILogger<OrganController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;
        public OrganController(ILogger<OrganController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet(Name = "Get organ list")]
        public IActionResult get()
        {
            List<Organ> dataList = new List<Organ>();
            List<Organ> medDataList = new List<Organ>();
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("select  * from Organ", con);
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    Organ dataObject = new Organ
                    {
                        Id = (int)reader[0],
                        Name = reader[1].ToString(),
                        Createddate = reader[2].ToString(),
                    };

                    dataList.Add(dataObject);
                }
                con.Close();
                con.Open();
                SqlCommand medCmd = new SqlCommand("select  * from MedicalInfo", con);
                SqlDataReader medReader = medCmd.ExecuteReader();
                while (medReader.Read())
                {
                    Organ dataObject1 = new Organ
                    {
                        Id = (int)medReader[0],
                        Name = medReader[1].ToString(),
                        Createddate = medReader[2].ToString(),
                    };

                    medDataList.Add(dataObject1);
                }


                con.Close();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(new {status = new SuccessRes { Success = true, Message = "Donor upadated successfully." }, OrganList = dataList, MedInfo = medDataList });
        }

    }
}
