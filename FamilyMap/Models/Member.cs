using FamilyMap.Custom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMap.Models
{
    public class Member
    {
        public int familyMemberId { get; set; }
        public string suffix { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string memberCategory { get; set; }
        public int familyNo { get; set; }
    }
}