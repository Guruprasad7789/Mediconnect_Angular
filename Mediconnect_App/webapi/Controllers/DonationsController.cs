using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public IActionResult GetDonations(int userId)
        {
            try
            {
                SqlDataReader reader = null;
                SqlConnection myConnection = new SqlConnection(_config);
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "Select * from DonorData LEFT JOIN UserOrganMap on UserOrganMap.DonorId = DonorData.DonorID LEFT JOIN Organ ON UserOrganMap.OrganId = Organ.Id LEFT JOIN UserMedicalMap ON UserMedicalMap.DonorId = DonorData.DonorID LEFT JOIN MedicalInfo ON UserMedicalMap.MedId = MedicalInfo.Id" + (userId == 0 ? "" : " where UserOrganMap.UserId= " + userId);
                sqlCmd.Connection = myConnection;
                myConnection.Open();
                reader = sqlCmd.ExecuteReader();
                List<DonationDetails> donList = new List<DonationDetails>();
                while (reader.Read())
                {
                    DonationDetails don = new DonationDetails();
                    don.donorid = (int)reader.GetValue(0);
                    don.organs = new List<Organ> { };
                    don.medInfos = new List<Organ> { };
                    don.name = reader.GetValue(3).ToString();
                    don.relationship = reader.GetValue(4).ToString();
                    don.contact = reader.GetValue(5).ToString();
                    don.address = reader.GetValue(6).ToString();
                    don.createddate = reader.GetValue(8).ToString();
                    don.organ = (int)reader.GetValue(11);
                    don.isDontated = (bool)reader.GetValue(13);
                    don.organName = reader.GetValue(16).ToString();
                    don.medId = Convert.IsDBNull(reader.GetValue(20)) ? 0 : ((int)(reader.GetValue(20)));
                    don.details = reader.GetValue(14).ToString();
                    don.med = reader.GetValue(23).ToString();
                    donList.Add(don);
                }
                var mergeList = new List<DonationDetails>();
                foreach (var rec in donList.OrderBy(x => x.donorid))
                {
                    var i = mergeList.FindIndex(x => x.donorid == rec.donorid);
                    if (i > -1)
                    {
                        var j = mergeList[i].organs.FindIndex(x => x.Id == rec.organ);
                        var k = mergeList[i].medInfos.FindIndex(x => x.Id == rec.medId);
                        if (j == -1)
                        {
                            mergeList[i].organs.Add(new Organ
                            {
                                Id = rec.organ,
                                Name = rec.organName,
                                IsDonated = rec.isDontated
                            });
                        }
                        if (k == -1 && rec.medId != 0)
                        {
                            mergeList[i].medInfos.Add(new Organ
                            {
                                Id = rec.medId,
                                Name = rec.med
                            });
                        }
                    }
                    else
                    {
                        rec.organs = new List<Organ> {new Organ
                        {
                            Id = rec.organ,
                            Name = rec.organName,
                            IsDonated = rec.isDontated
                        } };
                        if (rec.medId != 0)
                        {
                            rec.medInfos = new List<Organ> {new Organ
                        {
                            Id = rec.medId,
                            Name = rec.med
                        } };
                        }
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
        [Route("UpdateDonation")]
        [HttpPost(Name = "UpdateDonation")]
    public IActionResult UpdateDonation(UpdateDonation payoad)
    {
        try
        {
            SqlConnection con = new SqlConnection(_config);
            con.Open();
            SqlCommand cmd = new SqlCommand($"UPDATE UserOrganMap SET IsDonated = {(payoad.donation ? 1 : 0)}{(payoad.details.Count() == 0 ? "" : ", Details = '"+ payoad.details + "'")} WHERE OrganId = {payoad.organId} AND DonorId = {payoad.donorId}", con);
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

   

