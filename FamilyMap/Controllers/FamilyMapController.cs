using FamilyMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyMapWCFService;
using AutoMapper;
using Rotativa;
using Newtonsoft.Json;

namespace FamilyMap.Controllers
{
    public class FamilyMapController : Controller
    {
        //Session["UserID"].ToString();
        FamilyMapWCFService.Service1 service = new Service1();
        
        public ActionResult HomePageForAdmin()
        {
            return View();
        }
        // GET: FamilyMap
        public ActionResult HomePageForUser()
        {
            return View();
        }
        
        public ActionResult ShowApplicationNumber(Family family)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
            var map = config.CreateMapper();
            var data = map.Map<Family, WCP_Family>(family);
            int familyMemberId=service.AddFamilyMember(data);
            TempData["familyMemberId"] = familyMemberId;
            ViewBag.Message ="CONGRATULATIONS!!!!!!  YOUR APPLICATION NUMBER IS:"+Session["UserID"];
            return View();
        }

        public ActionResult AddFamilyMember()
        {
            return View();
        }




        [HttpPost]
        public ActionResult AddFamilyMember(Family family)
        {
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
            var map = config.CreateMapper();
            var data = map.Map<Family, WCP_Family>(family);
            int familyMemberId=service.AddFamilyMember(data);
            TempData["familyMemberId"] = familyMemberId;
            int id =Convert.ToInt32(Session["UserId"]);
            service.AddFamilyRelation(id, familyMemberId);
            var urlBuilder = new UrlHelper(Request.RequestContext);
            var url = urlBuilder.Action("AddMemberCategory", "FamilyMap");
            return Json(new { status = "success" });
        }

        [HttpGet]
        public ActionResult AddMemberCategory()
        {
            int familyId = Convert.ToInt32(Session["UserID"]);
            List<Family> familyList = new List<Family>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<List<Family>, List<WCP_Family>>());
            var map = config.CreateMapper();
            var data = map.Map<List<Family>, List<WCP_Family>>(familyList);
            data = service.GetAllFamilyMember(familyId);
            return View(data);
        }

        [HttpPost]
        public ActionResult AddMemberCategory(string member,string category)
        {
            int familyId=Convert.ToInt32(Session["UserID"]);
            if (service.AddMemberCategory(familyId, member, category))
            {
                var urlBuilder = new UrlHelper(Request.RequestContext);
                var url = urlBuilder.Action("AddMemberCategory", "FamilyMap", "Get");
                return Json(new { status = "success", redirectUrl = url });
            }
            return Json(new { status = "failed",message="Something went wrong!!!!" });
        }

        public ActionResult DownloadFamilyMap()
        {
            //click here to download recwipt
            return View();
        }


        // int FamilyNo = Session["UserId"];
        [HttpGet]
        public ActionResult GetFamilyMap(int Id)
        {
            Session["UserID"]=Id;
            List<Family> familyList = new List<Family>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<List<Family>, List<WCP_Family>>());
            var map = config.CreateMapper();
            var data = map.Map<List<Family>, List<WCP_Family>>(familyList);
            data = service.GetEntireFamily(Id);
            return View(data);
        }

        public ActionResult PrintViewToPdf()
        {
            int sessionid = Convert.ToInt32(Session["UserID"]);
            var report = new ActionAsPdf("GetFamilyMap",new {Id=sessionid });
            return report;
        }

       // [HttpPost]
        public  List<Member> getMemberData(Member member)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Member, WCP_FamilyCompleteData>());
            var map = config.CreateMapper();
            var data = map.Map<Member, WCP_FamilyCompleteData>(member);
            List<WCP_FamilyCompleteData> _member = service.Search(data);
            List<Member> members = new List<Member>();
            foreach(var item in _member)
            {
                Member memberObj = new Member();
                memberObj.familyMemberId = item.familyMemberId;
                memberObj.firstName = item.firstName;
                memberObj.lastName = item.lastName;
                memberObj.dob = item.dob;
                memberObj.familyNo = item.familyNo;
                members.Add(memberObj);
            }
            return members;

        }

        [HttpGet]
        public ActionResult SearchFamilyMember()
        {
            return View();
        }

        [HttpPost]
        public string SearchFamilyMember(Member member)
        {
            List<Member> members = getMemberData(member);
           
            return  JsonConvert.SerializeObject(members);
        }

       
        public ActionResult Edit(int id)
        {
            var member = service.GetMemberDetails(id);
            Family family = new Family();
            family.familyMemberId = member.familyMemberId;
            family.suffix = member.suffix;
            family.firstName = member.firstName;
            family.middleName = member.middleName;
            family.lastName = member.lastName;
            family.dob = member.dob;
            family.gender = member.gender;
            family.memberCategory = member.memberCategory;
            TempData["id"]=id;
            return View(family);
        }

        [HttpPost]
        public ActionResult Edit(Family member)
        {
            int id = (int)TempData["id"];
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
            var map = config.CreateMapper();
            var data = map.Map<Family, WCP_Family>(member);
            service.UpdateFamilyMember(data,id);
            var urlBuilder = new UrlHelper(Request.RequestContext);
            var url = urlBuilder.Action("SearchFamilyMember", "FamilyMap");
            return Json(new { status = "success", redirectUrl = url });

        }


    }
}