//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FamilyMapWCFService
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Family
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Family()
        {
            this.tbl_familyRelation = new HashSet<tbl_familyRelation>();
        }
    
        public int familyMemberId { get; set; }
        public string suffix { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public System.DateTime dob { get; set; }
        public string gender { get; set; }
        public string memberCategory { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_familyRelation> tbl_familyRelation { get; set; }
    }
}
