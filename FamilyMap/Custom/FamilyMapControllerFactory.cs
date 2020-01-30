using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FamilyMapWCFService;


namespace FamilyMap.Custom
{
    public class FamilyMapControllerFactory:System.Web.Mvc.DefaultControllerFactory
    { 
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            //FamilyMap.Controllers.UserController userController = new Controllers.UserController();
            //return userController;
            // return base.CreateController(requestContext, controllerName);

            //Reflection
            
            familytree family = new familytree();
            var activeMember = family.tbl_User.Where(obj => obj.isActive == 1).FirstOrDefault();
            if (activeMember != null)
            {
                HttpContext.Current.Session["UserID"] = activeMember.userLoginId;
                HttpContext.Current.Session["UserName"] = activeMember.userName;
            }
            else
            {
                HttpContext.Current.Session["UserID"] = null;
                HttpContext.Current.Session["UserName"] = null;
            }
            Type typeOfController = Type.GetType(string.Format("FamilyMap.Controllers.{0}Controller", controllerName));
            return Activator.CreateInstance(typeOfController) as System.Web.Mvc.IController;
           
            
        }
    }
}