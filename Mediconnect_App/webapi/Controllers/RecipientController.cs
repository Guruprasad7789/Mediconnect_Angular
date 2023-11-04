using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class RecipientController : ControllerBase
    {
        private readonly ILogger<RecipientController> _logger;
        private readonly IConfiguration _configuration;
        private string _config;
        public RecipientController(ILogger<RecipientController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _config = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet(Name = "GetRecipientDetails")]
        public IEnumerable<RecipientDetails> Get()
        {
            SqlDataReader reader = null/* TODO Change to default(_) if this is not a reference type */;
            SqlConnection myConnection = new SqlConnection(_config);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "Select * from RecipientData";
            sqlCmd.Connection = myConnection;
            myConnection.Open();
            reader = sqlCmd.ExecuteReader();
            List<RecipientDetails> donList = new List<RecipientDetails>();
            while (reader.Read())
            {
                RecipientDetails don = new RecipientDetails();
                don.recipientid = reader.GetValue(0).ToString();
                don.organsinfo = reader.GetValue(1).ToString();
                don.bloodtype = reader.GetValue(2).ToString();
                don.age = reader.GetValue(3).ToString();
                don.name = reader.GetValue(4).ToString();
                don.relationship = reader.GetValue(5).ToString();
                don.contact = reader.GetValue(6).ToString();
                don.address = reader.GetValue(7).ToString();
                don.createddate = reader.GetValue(8).ToString();
                donList.Add(don);
            }
            myConnection.Close();
            return donList;
        }

        [HttpPost(Name = "PostRecDetails")]
        public IActionResult CreateRecipient(RecipientDetails rec)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO RecipientData(OrgansInfo,BloodType,Age,Name,Relationship,Contact,Address,CreatedDate) " + "Values('" + rec.organsinfo + "','" + rec.bloodtype + "','" + rec.age + "','" + rec.name + "','" + rec.relationship + "','" + rec.contact + "','" + rec.address + "','" + DateTime.Now.ToLongDateString() + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
            }
            return Ok(new SuccessRes { Success = true, Message = "Recipient upadated successfully." });
        }

    }
}
