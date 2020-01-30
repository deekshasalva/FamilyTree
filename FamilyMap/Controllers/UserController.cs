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
            if (HttpContext.Session["UserID"] == null)
            {
                return View();
            }
            else
            {
                if (HttpContext.Session["UserID"].ToString() == "1")
                {
                    return RedirectToAction("HomePageForAdmin", "FamilyMap");
                }
                else
                {
                    return RedirectToAction("HomePageForUser", "FamilyMap");
                }
            }
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
                    obj.isActive = 1;
                    db.SaveChanges();
                    
                    if (obj != null)
                    {
                        HttpContext.Session["UserID"] = obj.userLoginId;
                        HttpContext.Session["UserName"] = obj.userName;
                        if ((int)Session["UserID"] == 1)
                        {
                            return RedirectToAction("HomePageForAdmin", "FamilyMap");
                        }
                        else
                        {
                            return RedirectToAction("HomePageForUser", "FamilyMap");
                        }
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
            var url = urlBuilder.Action("Login", "User");
            return Json(new { status = "success", redirectUrl = url });
        }


        
        public ActionResult Logout()
        {
            int loginId = Convert.ToInt32(HttpContext.Session["UserID"]);
            string userName = Convert.ToString(HttpContext.Session["UserName"]);
            using (familytree db = new familytree())
            {
                var result = db.tbl_User.Where(obj => obj.userName.Equals(userName) && obj.userLoginId.Equals(loginId)).FirstOrDefault();
                result.isActive = 0;
                db.SaveChanges();
            }

            HttpContext.Session["USerID"] = null;
            HttpContext.Session["UserName"] = null;
            return RedirectToAction("Login","User");
        }
    }
}