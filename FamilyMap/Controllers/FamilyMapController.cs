﻿using FamilyMap.Models;
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

        Service1 service = new Service1();

        public ActionResult HomePageForAdmin()
        {
            return View();
        }

        public ActionResult HomePageForUser()
        {
            return View();
        }

        public ActionResult ShowApplicationNumber(Family family)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
                var map = config.CreateMapper();
                var data = map.Map<Family, WCP_Family>(family);
                int familyMemberId = service.AddFamilyMember(data);
                TempData["familyMemberId"] = familyMemberId;
                ViewBag.Message = "CONGRATULATIONS!!!!!!  YOUR APPLICATION NUMBER IS:" + Session["UserID"];
                return View();
            }
            catch(Exception exception)
            {
                return RedirectToAction("ExceptionPage",new { message=exception.Message});
            }
        }

        public ActionResult AddFamilyMember()
        {
            return View();
        }

        // need to use session variable 
        [HttpPost]
        public ActionResult AddFamilyMember(Family family)
        {
            try
            {
                if (Session["family"] != null)
                {
                    AddMultipleMembers();
                }
                int Id = Convert.ToInt32(HttpContext.Session["UserID"]);
                int count = service.GetEntireFamily(Id).Count();
                try
                {
                    while (count <= 5)
                    {
                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
                        var map = config.CreateMapper();
                        var data = map.Map<Family, WCP_Family>(family);
                        if (count == 0)
                        {
                            Session["MinDate"] = data.dob;
                        }

                        else
                        {
                            Session["MinDate"] = 1900 - 01 - 01;
                        }
                        int familyMemberId = service.AddFamilyMember(data);

                        TempData["familyMemberId"] = familyMemberId;
                        int id = Convert.ToInt32(Session["UserId"]);
                        service.AddFamilyRelation(id, familyMemberId);
                        var urlBuilder = new UrlHelper(Request.RequestContext);
                        var url = urlBuilder.Action("AddMemberCategory", "FamilyMap");
                        return Json(new { status = "success" });
                    }
                }
                catch { }
                return Json(new { status = "error" });
            }
            catch(Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }


        //List<string> myList = new List<string>();
        //        Session["var"] = myList;
        //Then, to retrieve:

        //myList = (List<string>) Session["var"];

        [HttpPost]
        public ActionResult AddMemberToSession(Family family)
        {
            try
            {
                List<Family> familyList = new List<Family>();
                familyList.Add(family);
                Session["family"] = familyList;
                return Json(new { status = "success" });
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }

        }


        [HttpPost]
        public ActionResult PrepopulateMember(Family family)
        {
            try
            {
                while (family != null)
                {
                    Session["prePopulatedSuffix"] = family.suffix;
                    Session["prePopulatedMembermiddleName"] = family.middleName;
                    Session["prePopulateMemberfirstName"] = family.firstName;
                    Session["prePopulatedMemberlastName"] = family.lastName;
                    Session["prePopulatedDob"] = family.dob;
                    Session["prePopulatedGender"] = family.gender;

                }
                if (Convert.ToInt32(HttpContext.Session["UserID"]) == 1)
                {
                    return RedirectToAction("HomePageForAdmin");
                }
                else
                {
                    return RedirectToAction("HomePageForUser");
                }
            }
            catch(Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        public bool AddMultipleMembers()
        {
   
                int result = 0;
                List<Family> familyList = new List<Family>();
                familyList = (List<Family>)Session["family"];
                foreach (var item in familyList)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
                    var map = config.CreateMapper();
                    var data = map.Map<Family, WCP_Family>(item);
                    result = service.AddFamilyMember(data);
                }
                if (result == 0) { return false; }
                else return true;
            
        }

        [HttpGet]
        public ActionResult AddMemberCategory()
        {
            try
            {
                int familyId = Convert.ToInt32(HttpContext.Session["UserID"]);
                List<Family> familyList = new List<Family>();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<List<Family>, List<WCP_Family>>());
                var map = config.CreateMapper();
                var data = map.Map<List<Family>, List<WCP_Family>>(familyList);
                data = service.GetAllFamilyMember(familyId);
                return View(data);
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        [HttpPost]
        public ActionResult AddMemberCategory(string member, string category)
        {
            try
            {
                int familyId = Convert.ToInt32(HttpContext.Session["UserID"]);
                if (service.AddMemberCategory(familyId, member, category))
                {
                    var urlBuilder = new UrlHelper(Request.RequestContext);
                    var url = urlBuilder.Action("AddMemberCategory", "FamilyMap", "Get");
                    return Json(new { status = "success" });
                }
                return Json(new { status = "error", message = "Something went wrong!!!!" });
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        [HttpGet]
        public ActionResult RemoveFromFamily()
        {
            try
            {
                int Id = Convert.ToInt32(HttpContext.Session["UserID"]);
                var result = service.GetAllFamilyMember(Id);
                List<Family> familyList = new List<Family>();
                foreach (var item in result)
                {
                    var config = new MapperConfiguration(cfg => cfg.CreateMap<WCP_Family, Family>());
                    var map = config.CreateMapper();
                    var mappedresult = map.Map<WCP_Family, Family>(item);
                    familyList.Add(mappedresult);
                }
                return View(familyList);
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                service.DeleteFamilyMember(id);
                return View();
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }

        }

        public ActionResult DownloadFamilyMap()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetFamilyMap(int Id)
        {
            try
            {
                HttpContext.Session["UserID"] = Id;
                List<Family> familyList = new List<Family>();
                var config = new MapperConfiguration(cfg => cfg.CreateMap<List<Family>, List<WCP_Family>>());
                var map = config.CreateMapper();
                var data = map.Map<List<Family>, List<WCP_Family>>(familyList);
                data = service.GetEntireFamily(Id);
                return View(data);
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        public ActionResult PrintViewToPdf()
        {
            try
            {
                int sessionid = Convert.ToInt32(HttpContext.Session["UserID"]);
                var report = new ActionAsPdf("GetFamilyMap", new { Id = sessionid });
                return report;
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        public List<Member> getMemberData(Member member)
        {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Member, WCP_FamilyCompleteData>());
                var map = config.CreateMapper();
                var data = map.Map<Member, WCP_FamilyCompleteData>(member);
                List<WCP_FamilyCompleteData> _member = service.Search(data);
                List<Member> members = new List<Member>();
                foreach (var item in _member)
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

            return JsonConvert.SerializeObject(members);
        }


        public ActionResult Edit(int id)
        {
            try
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
                TempData["id"] = id;
                return View(family);
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }
        }

        [HttpPost]
        public ActionResult Edit(Family member)
        {
            try
            {
                int id = (int)TempData["id"];
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Family, WCP_Family>());
                var map = config.CreateMapper();
                var data = map.Map<Family, WCP_Family>(member);
                service.UpdateFamilyMember(data, id);
                var urlBuilder = new UrlHelper(Request.RequestContext);
                var url = urlBuilder.Action("SearchFamilyMember", "FamilyMap");
                return Json(new { status = "success", redirectUrl = url });
            }
            catch (Exception exception)
            {
                return RedirectToAction("ExceptionPage", new { message = exception.Message });
            }

        }

        [HttpGet]
        public ActionResult ExceptionPage(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        
    }
}