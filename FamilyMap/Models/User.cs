using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMap.Models
{
    public class User
    {
        public int userLoginId { get; set; }
        public string userName { get; set; }
        public string emailId { get; set; }
        public string passwd { get; set; }
        public string userType { get; set; }
    }
}