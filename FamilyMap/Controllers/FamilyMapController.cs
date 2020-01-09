using FamilyMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FamilyMapWCFService;
using AutoMapper;
using Rotativa;

namespace FamilyMap.Controllers
{
    public class FamilyMapController : Controller
    {
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
            service.AddFamilyRelation(1, familyMemberId);
            var urlBuilder = new UrlHelper(Request.RequestContext);
            var url = urlBuilder.Action("AddMemberCategory", "FamilyMap");
            return Json(new { status = "success", redirectUrl = url });
        }

        [HttpGet]
        public ActionResult AddMemberCategory(int familyId=1)
        {
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
            int familyId = 1;
            if (service.AddMemberCategory(familyId, member, category))
            {
                var urlBuilder = new UrlHelper(Request.RequestContext);
                var url = urlBuilder.Action("AddMemberCategory", "FamilyMap", "Get");
                return Json(new { status = "success", redirectUrl = url });
            }
            return Json(new { status = "failed",message="Something went wrong!!!!" });
        }

        [HttpGet]

        public ActionResult GetFamilyMap(int familyId=1)
        {
            List<Family> familyList = new List<Family>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<List<Family>, List<WCP_Family>>());
            var map = config.CreateMapper();
            var data = map.Map<List<Family>, List<WCP_Family>>(familyList);
            data = service.GetEntireFamily(familyId);
            return View(data);

        }

        public ActionResult PrintViewToPdf()
        {
            var report = new ActionAsPdf("GetFamilyMap");
            return report;
        }

       // [HttpPost]
        public  List<Member> getMemberData(Member member)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Member, WCP_FamilyCompleteData>());
            var map = config.CreateMapper();
            var data = map.Map<Member, WCP_FamilyCompleteData>(member);
            List<WCP_FamilyCompleteData> _member = service.GetEntireMemberList(data);
            List<Member> members = new List<Member>();
            foreach(var item in _member)
            {
                Member memberObj = new Member();
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
        public ActionResult SearchFamilyMember(Member member)
        {
            List<Member> members = getMemberData(member);
            return View(members);
        }


        public ActionResult SearchAMember()
        {
            return View();
        }


    }
}