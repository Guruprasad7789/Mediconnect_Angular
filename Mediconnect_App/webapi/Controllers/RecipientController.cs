using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Diagnostics.Metrics;

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
        public IActionResult Get(int userId)
        {
            try
            {
                SqlDataReader reader = null;
                SqlConnection myConnection = new SqlConnection(_config);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select * from RecipientData LEFT JOIN UserOrganReceiverMap on UserOrganReceiverMap.RecipientId = RecipientData.RecipientId LEFT JOIN Organ ON UserOrganReceiverMap.OrganId = Organ.Id" + (userId == 0 ? "" : " where UserId = " + userId);
                sqlCmd.Connection = myConnection;
                myConnection.Open();
                reader = sqlCmd.ExecuteReader();
                List<RecipientDetails> donList = new List<RecipientDetails>();
                while (reader.Read())
                {
                    RecipientDetails don = new RecipientDetails();
                    don.recipientid = (int)reader.GetValue(0);
                    don.organsList = new List<Organ>{ };
                    don.bloodtype = reader.GetValue(2).ToString();
                    don.age = reader.GetValue(3).ToString();
                    don.name = reader.GetValue(4).ToString();
                    don.relationship = reader.GetValue(5).ToString();
                    don.contact = reader.GetValue(6).ToString();
                    don.address = reader.GetValue(7).ToString();
                    don.createddate = reader.GetValue(8).ToString();
                    don.organ = (int)reader.GetValue(11);
                    don.isReceived = (bool)reader.GetValue(13);
                    don.details = reader.GetValue(14).ToString();
                    don.organName = reader.GetValue(15).ToString();
                    donList.Add(don);
                }
                var mergeList = new List<RecipientDetails>();
                foreach (var rec in donList.OrderBy(x=> x.recipientid))
                {
                    var i = mergeList.FindIndex(x => x.recipientid == rec.recipientid);
                    if (i>-1)
                    {
                        var j = mergeList[i].organsList.FindIndex(x => x.Id == rec.organ);
                        if (j == -1)
                        {
                            mergeList[i].organsList.Add(new Organ
                            {
                                Id = rec.organ ?? 0,
                                Name = rec.organName,
                                IsReceived = rec.isReceived
                            });
                        }
                    }
                    else
                    {
                        rec.organsList = new List<Organ> {new Organ
                        {
                            Id = rec.organ ?? 0 ,
                            Name = rec.organName,
                            IsReceived = rec.isReceived
                        } };
                        mergeList.Add(rec);
                    }
                }
                return Ok(mergeList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("PostRecDetails")]
        [HttpPost(Name = "PostRecDetails")]
        public IActionResult CreateRecipient(RecipientDetails rec)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO RecipientData(OrgansInfo,BloodType,Age,Name,Relationship,Contact,Address,CreatedDate) "
                                 + "OUTPUT INSERTED.RecipientID " + "Values('" + "" + "','" + rec.bloodtype + "','" + rec.age + "','" + rec.name + "','" + rec.relationship + "','" + rec.contact + "','" + rec.address + "','" + DateTime.Now.ToLongDateString() + "')", con);
                var insertedId = cmd.ExecuteScalar();

                if (insertedId != null)
                {
                    int newId = Convert.ToInt32(insertedId);
                    // The 'newId' variable now holds the ID of the inserted element.
                    foreach (int organ_id in rec.organsinfo)
                    {
                        SqlCommand cmdMap = new SqlCommand("INSERT INTO UserOrganReceiverMap(UserId, OrganId, RecipientId, IsReceived)" + "Values('" + rec.userid + "','" + organ_id + "','" + newId + "','" + 0 + "')", con);

                        cmdMap.ExecuteNonQuery();
                    }
                }

                    con.Close();
            }
            catch (Exception ex)
            {
            }
            return Ok(new SuccessRes { Success = true, Message = "Recipient upadated successfully." });
        }

        [Route("UpdateRecipient")]
        [HttpPost(Name = "UpdateRecipient")]
        public IActionResult UpdateDonation(UpdateRecipient payoad)
        {
            try
            {
                SqlConnection con = new SqlConnection(_config);
                con.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE UserOrganReceiverMap SET IsReceived = {(payoad.IsReceived ? 1 : 0)}, Details = '{payoad.details}' WHERE OrganId = {payoad.organId} AND RecipientId = {payoad.recipientId}", con);
                var insertedId = cmd.ExecuteNonQuery();

                con.Close();
                return Ok(insertedId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
