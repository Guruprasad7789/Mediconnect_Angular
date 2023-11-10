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
                SqlCommand cmd = new SqlCommand("INSERT INTO DonorData(Organs,MedInfo,Name,Relationship,Contact,Address,Sign,CreatedDate) "
                    + "OUTPUT INSERTED.DonorID "
                    + "Values('" + "" + "','" + don.medinfo + "','" + don.name + "','" + don.relationship + "','" + don.contact + "','" + don.address + "','" + don.sign + "','" + DateTime.Now.ToLongDateString() + "')", con);
                var insertedId = cmd.ExecuteScalar();

                if (insertedId != null)
                {
                    int newId = Convert.ToInt32(insertedId);
                    // The 'newId' variable now holds the ID of the inserted element.
                    foreach (int organ_id in don.organs.organsData)
                    {
                        SqlCommand cmdMap = new SqlCommand("INSERT INTO UserOrganMap(UserId, OrganId, DonorId, IsDonated)" + "Values('" + don.userid + "','" + organ_id + "','" + newId + "','" + 0 + "')", con);

                        cmdMap.ExecuteNonQuery();
                    }
                    foreach (int med in don.medinfo)
                {
                    SqlCommand cmdMed = new SqlCommand("INSERT INTO UserMedicalMap(UserId, MedId, DonorId)" + "Values('" + don.userid + "','" + med + "','" + newId + "')", con);

                    cmdMed.ExecuteNonQuery();
                }
                }

                con.Close();
            }
            catch (Exception ex)
            {
            }
            return Ok(new SuccessRes { Success = true, Message = "Donor upadated successfully." });
        }

    }
}
