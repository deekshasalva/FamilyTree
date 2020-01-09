using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FamilyMap.Models
{
    public class FamilyRelation
    {
        public int familyRelationId { get; set; }
        public int familyNo { get; set; }
        public int familyMemId { get; set; }
    }
}