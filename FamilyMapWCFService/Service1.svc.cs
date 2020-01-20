using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FamilyMapWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        familytree familyEntity = new familytree();

        public int AddFamilyMember(WCP_Family family)
        { 
            
            try
            {
                tbl_Family familyObject = new tbl_Family();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<WCP_Family,tbl_Family>());
                var map = config.CreateMapper();
                var data = map.Map<WCP_Family,tbl_Family>(family);
                familyEntity.tbl_Family.Add(data);
                familyEntity.SaveChanges();
                int id = (familyEntity.tbl_Family.Where(p => p.firstName == family.firstName && p.lastName == family.lastName).Select(p => p.familyMemberId)).FirstOrDefault();
                return id;
            }
            catch
            {
                return 0;
            }
            

        }

        public bool UpdateFamilyMember(WCP_Family family, int id)
        {
            try
            {
                tbl_Family familyObject = familyEntity.tbl_Family.Where(obj => obj.familyMemberId == id).First();
                {
                    familyObject.suffix = family.suffix;
                    familyObject.firstName = family.firstName;
                    familyObject.middleName = family.middleName;
                    familyObject.lastName = family.lastName;
                    familyObject.gender = family.gender;
                    familyObject.dob = family.dob;
                    familyObject.memberCategory = family.memberCategory;
                    familyEntity.SaveChanges();
                }
                return true;
            }

            catch
            {
                return false;
            }
        }

        public int AddUser(WCP_User user)
        {
            try
            {
                tbl_User userObject = new tbl_User();
                var config = new MapperConfiguration(cfg=>cfg.CreateMap<WCP_User,tbl_User>());
                var map = config.CreateMapper();
                var data = map.Map<WCP_User, tbl_User>(user);
                familyEntity.tbl_User.Add(data);
                familyEntity.SaveChanges();
                int id = (familyEntity.tbl_User.Where(p => p.userName == user.userName && p.emailId == user.emailId).Select(p => p.userLoginId)).FirstOrDefault();
                return id;
            }
            catch
            {
                return 0;
            }
        }

        public bool AddFamilyRelation(int familyNo, int familyMemberId)
        {
            try
            {
                tbl_familyRelation familyRelation = new tbl_familyRelation();
                familyRelation.familyNo = familyNo;
                familyRelation.familyMemId = familyMemberId;
                familyEntity.tbl_familyRelation.Add(familyRelation);
                familyEntity.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }

        public bool AddMemberCategory(int familyId,string member,string category)
        {
            try
            {
                var family = familyEntity.tbl_Family.Where(obj1 =>
                                                                 familyEntity.tbl_familyRelation.Any(obj2 =>
                                                                 obj2.familyNo == familyId && obj2.familyMemId == obj1.familyMemberId && obj1.firstName == member));
                foreach (var item in family.ToList())
                {
                    item.memberCategory = category;
                    
                }
                familyEntity.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

       

        public  List<WCP_Family> GetAllFamilyMember(int familyId)
        {
            try
            {
                List<WCP_Family> familyList = new List<WCP_Family>();

                //var familyMember = from data in familyEntity.tbl_Family select data;
                //var obj = db.tbl_User.Where(a => a.userName.Equals(objUser.userName) && a.passwd.Equals(objUser.passwd)).FirstOrDefault();
                //        from p in Projects
                //        join m in ProjectMembers on m.ProjectID equals p.ID
                //        where m.UserID == userID && p.ProjectOwner != userID
                //        group p by p.ID
                //select p
                //var familyMember=from family in familyEntity.tbl_Family
                //                 join relation in familyEntity.tbl_familyRelation on family.familyMemberId equals relation.familyMemId
                //                 where relation.familyMemId==familyId
                //                 group by family.familyMemberId
                //                 select family;
                //var familyMember = from familyObject in familyEntity.tbl_Family
                //                   from RelationObject in familyEntity.tbl_familyRelation
                //                   where familyObject.familyMemberId == RelationObject.familyMemId && RelationObject.familyNo == familyId
                //                   orderby familyObject.familyMemberId descending
                //                   select familyObject
                //                   ;
                var familyMember =  familyEntity.tbl_Family.Where(obj1 =>
                                                                 familyEntity.tbl_familyRelation.Any(obj2 => 
                                                                 obj2.familyNo == familyId && obj1.familyMemberId == obj2.familyMemId));
                var FamilyHead = familyMember.Min(obj => obj.familyMemberId) ;
                var presentUser = familyMember.Max(obj => obj.familyMemberId);
                foreach (var item in familyMember)
                {
                    if (item.memberCategory==null)
                    {
                        WCP_Family familyobj = new WCP_Family();
                        familyobj.suffix = item.suffix;
                        familyobj.firstName = item.firstName;
                        familyobj.middleName = item.middleName;
                        familyobj.lastName = item.lastName;
                        familyobj.dob = item.dob;
                        familyobj.gender = item.gender;
                        familyList.Add(familyobj);
                    }

                }
                return familyList;
            }
            catch
            {
                return null;
            }
        }

        public List<WCP_Family> GetEntireFamily(int familyId)
        {
            List<WCP_Family> familyList = new List<WCP_Family>();

            var familyMember= familyEntity.tbl_Family.Where(obj1 =>
                                                            familyEntity.tbl_familyRelation.Any(obj2 => 
                                                            obj2.familyNo == familyId && obj1.familyMemberId == obj2.familyMemId));
            foreach (var item in familyMember)
            {
            WCP_Family familyobj = new WCP_Family();
            familyobj.suffix = item.suffix.ToUpper();
            familyobj.firstName = item.firstName.ToUpper();
            familyobj.middleName = item.middleName.ToUpper();
            familyobj.lastName = item.lastName.ToUpper();
            familyobj.dob = item.dob;
            familyobj.gender = item.gender.ToUpper();
            familyobj.memberCategory = item.memberCategory.ToUpper();
            familyList.Add(familyobj);
            }
            return familyList;
        }

        public WCP_Family GetMemberDetails(int memberId)
        {
           tbl_Family family = familyEntity.tbl_Family.Where(obj1 =>
                                                              obj1.familyMemberId == memberId).FirstOrDefault();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<tbl_Family, WCP_Family>());
            var map = config.CreateMapper();
            var data = map.Map<tbl_Family, WCP_Family>(family);
            return data;

        }
        //
        public List<WCP_FamilyCompleteData> Search(WCP_FamilyCompleteData _family)
        {
            List<WCP_FamilyCompleteData> familyList = new List<WCP_FamilyCompleteData>();
            List<tbl_Family> families = families = familyEntity.tbl_Family.ToList();
            if (_family.firstName != null || _family.lastName != null || _family.dob.Date !=DateTime.MinValue|| _family.familyNo != 0)
            {
                if (_family.firstName != null)
                {
                     families= familyEntity.tbl_Family.Where(obj => obj.firstName == _family.firstName).ToList();
                }
                if(_family.lastName!=null)
                {
                    families = families.Where(obj => obj.lastName == _family.lastName).ToList();
                }
                if(_family.dob!=DateTime.MinValue)
                {
                    families = families.Where(obj => obj.dob == _family.dob).ToList();
                }
                if (_family.familyNo != 0)
                {
                    int familyMemberId= familyEntity.tbl_familyRelation.Where(obj1 => obj1.familyNo ==_family.familyNo).FirstOrDefault().familyMemId;
                    families = families.Where(obj => obj.familyMemberId == familyMemberId).ToList();     
                }

                foreach(var item in families)
                {
                    WCP_FamilyCompleteData family = new WCP_FamilyCompleteData();
                    family.familyMemberId = item.familyMemberId;
                    family.suffix = item.suffix;
                    family.firstName = item.firstName;
                    family.middleName = item.middleName;
                    family.lastName = item.lastName;
                    family.dob = item.dob;
                    family.gender = item.gender;
                    family.memberCategory = item.memberCategory;
                    family.familyNo= familyEntity.tbl_familyRelation.Where(obj1 => obj1.familyMemId == item.familyMemberId).FirstOrDefault().familyNo;
                    familyList.Add(family);
                }
            }
            return familyList;
        }

        public List<WCP_FamilyCompleteData> GetEntireMemberList(WCP_FamilyCompleteData _family)
        {
            List<WCP_FamilyCompleteData> familyList = new List<WCP_FamilyCompleteData>();
            if (_family.firstName != null || _family.lastName != null || _family.dob != null || _family.familyNo != 0)
            {
                var familyMember = familyEntity.tbl_Family.Where(obj1 =>
                                                                familyEntity.tbl_familyRelation.Any(obj2 =>
                                                                obj1.firstName == _family.firstName || obj1.lastName == _family.lastName || obj1.dob == _family.dob || (obj2.familyNo == _family.familyNo && obj1.familyMemberId == obj2.familyMemId)));

                foreach (var item in familyMember)
                {
                    WCP_FamilyCompleteData familyobj = new WCP_FamilyCompleteData();
                    familyobj.suffix = item.suffix;
                    familyobj.familyMemberId = item.familyMemberId;
                    familyobj.firstName = item.firstName;
                    familyobj.middleName = item.middleName;
                    familyobj.lastName = item.lastName;
                    familyobj.dob = item.dob;
                    familyobj.gender = item.gender;
                    familyobj.memberCategory = item.memberCategory;
                    familyobj.familyNo = familyEntity.tbl_familyRelation.Where(obj1 => obj1.familyMemId == item.familyMemberId).FirstOrDefault().familyNo;
                    
                    familyList.Add(familyobj);
                }
            }
            return familyList;
        }

       
    }
}


//VehicleManagementEntities vehicleManagement = new VehicleManagementEntities();
//tbl_Vehicle vehicleAdded = new tbl_Vehicle();
//vehicleAdded.VehicleName = vehicle.VehicleName;
//            vehicleAdded.VehicleType = vehicle.VehicleType;
//            vehicleAdded.NumberofSeats = vehicle.NumberofSeats;
//            vehicleManagement.tbl_Vehicle.Add(vehicleAdded);
//            vehicleManagement.SaveChanges();