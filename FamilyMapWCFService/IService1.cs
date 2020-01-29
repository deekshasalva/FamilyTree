using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace FamilyMapWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<WCP_Family> GetAllFamilyMember(int familyid);
        
        [OperationContract]
        List<WCP_Family> GetEntireFamily(int familyid);

        [OperationContract]
        WCP_Family GetMemberDetails(int memberId);

        [OperationContract]
        List<WCP_FamilyCompleteData> GetEntireMemberList(WCP_FamilyCompleteData _Family);

        [OperationContract]
        int AddFamilyMember(WCP_Family family);

        [OperationContract]

        bool UpdateFamilyMember(WCP_Family family, int id);

        [OperationContract]
        bool DeleteFamilyMember(int memberId);

        [OperationContract]
        int AddUser(WCP_User user);

        [OperationContract]
        bool AddFamilyRelation(int familyNo,int familyMemberId);

        [OperationContract]
        bool AddMemberCategory(int familyid,string FamilyMember, string category);


    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class WCP_Family
    { 
        [DataMember]
        public int familyMemberId { get; set; }
        [DataMember]
        public string suffix { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public System.DateTime dob { get; set; }
        [DataMember]
        public string gender { get; set; }
        [DataMember]
        public string memberCategory { get; set; }
    }

    [DataContract]
    public class WCP_FamilyCompleteData
    {
        [DataMember]
        public int familyMemberId { get; set; }
        [DataMember]
        public string suffix { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public System.DateTime dob { get; set; }
        [DataMember]
        public string gender { get; set; }
        [DataMember]
        public string memberCategory { get; set; }
        [DataMember]
        public int familyNo { get; set; }


    }
    [DataContract]
    public class WCP_User
    {
        [DataMember]
        public int userLoginId { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string emailId { get; set; }
        [DataMember]
        public string passwd { get; set; }
        [DataMember]
        public string userType { get; set; }
    }

    [DataContract]
    public class WCP_FamilyRealation
    {
        [DataMember]
        public int familyRelationId { get; set; }
        [DataMember]
        public int familyNo { get; set; }
        [DataMember]
        public int familyMemId { get; set; }
    }
}
