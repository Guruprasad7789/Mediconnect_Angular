using System.Net;
using System.Security.Cryptography.Xml;

namespace webapi
{
    public class DonationDetails
    {       
        public string? donorid { get; set; }
        public string? organs { get; set; }
        public string? medinfo { get; set; }
        public string? name { get; set; }
        public string? relationship { get; set; }
        public string? contact { get; set; }
        public string? address { get; set; }
        public string? sign { get; set; }
        public string? createddate { get; set; }

    }
}
