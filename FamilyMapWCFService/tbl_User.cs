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
    
    public partial class tbl_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_User()
        {
            this.tbl_familyRelation = new HashSet<tbl_familyRelation>();
        }
    
        public int userLoginId { get; set; }
        public string userName { get; set; }
        public string emailId { get; set; }
        public string passwd { get; set; }
        public string userType { get; set; }
        public Nullable<int> isActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_familyRelation> tbl_familyRelation { get; set; }
    }
}
