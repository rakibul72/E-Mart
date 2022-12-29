using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Mart.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            if (Session["admin_email"] != null || Session["buyer_email"] != null || Session["seller_email"] != null)
            {
                Session["admin_email"] = null;
                Session["seller_email"] = null;
                Session["buyer_email"] = null;

                Session.Remove("admin_email");
                Session.Remove("seller_email");
                Session.Remove("buyer_email");

                Session.Abandon();
                
                return RedirectToAction("../Products/Logout");
            }
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}