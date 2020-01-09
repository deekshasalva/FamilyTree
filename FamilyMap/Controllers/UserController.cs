using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FamilyMap.Models;
using FamilyMapWCFService;

namespace FamilyMap.Controllers
{
    public class UserController : Controller
    {

        FamilyMapWCFService.Service1 service = new Service1();
        // GET: User

        
        [HttpGet]
        public ActionResult Login()
        {
            Session["USerID"] = null;
            Session["UserName"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            if (ModelState.IsValid)
            {
                using (familytree db = new familytree())
                {
                    var obj = db.tbl_User.Where(a => a.userName.Equals(objUser.userName) && a.passwd.Equals(objUser.passwd)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.userLoginId;
                        Session["UserName"] = obj.userName;
                        return RedirectToAction("HomePage","Familymap");
                    }

                    else
                    {
                        return RedirectToAction("Register");
                    }


                }
            }
            return View(objUser);
        }

        

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, WCP_User>());
            var map = config.CreateMapper();
            var data = map.Map<User, WCP_User>(user);
            int userId = service.AddUser(data);
            var urlBuilder = new UrlHelper(Request.RequestContext);
            var url = urlBuilder.Action("HomePage", "FamilyMap");
            return Json(new { status = "success", redirectUrl = url });
        }


        [HttpPost]
        public ActionResult Logout()
        {
            Session["USerID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("Login");
        }
    }
}