using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyMapBusinessObjectLayer
{
    class Family
    {
        public int familyMemberId { get; set; }
        public string suffix { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public System.DateTime dob { get; set; }
        public string gender { get; set; }
        public string memberCategory { get; set; }
    }
}
