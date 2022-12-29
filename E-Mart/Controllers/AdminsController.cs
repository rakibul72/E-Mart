using System;
using System.Linq;
using System.Web.Mvc;
using E_Mart.Models;
using System.Data.Entity.Migrations.Model;
using System.Net;

namespace E_Mart.Controllers
{
    public class AdminsController : Controller
    {
        private Entities6 db = new Entities6();

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Admin admin)
        {
            Admin check = db.Admins.Where(u => u.AdminEmail.Equals(admin.AdminEmail)).FirstOrDefault();
            if (check != null)
            {
                TempData["Referrer"] = "SignUp";
                return RedirectToAction("SignUp");
            }
            else if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();

                TempData["Referrer"] = "SaveRegister";

                return RedirectToAction("LogIn");
            }
            return View();
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            if (Session["admin_email"] != null || Session["buyer_email"] != null || Session["seller_email"] != null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            return View();
        }

        [HttpPost]
        public ActionResult LogIn(TempAdmin tempadmin)
        {
            if (ModelState.IsValid)
            {
                var check = db.Admins.Where(u => u.AdminEmail.Equals(tempadmin.AdminEmail) && u.AdminPassword.Equals(tempadmin.AdminPassword)).FirstOrDefault();

                if(check != null)
                {
                    Session["admin_email"] = check.AdminEmail;
                    return RedirectToAction("RealDashBoard");
                }
                else
                {
                    ViewBag.LoginFailed = "User not found or password mismatched!";
                    return View();
                }
            }
            return View();
        }

        public ActionResult RealDashBoard()
        {
            if (Session["admin_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email = Convert.ToString(Session["admin_email"]);
            var user = db.Admins.Where(u => u.AdminEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }

        public ActionResult DashBoard()
        {
            if (Session["admin_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email =Convert.ToString(Session["admin_email"]);
            var user = db.Admins.Where(u => u.AdminEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["admin_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
        {
            if (Session["admin_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            
            try
            {


                db.Database.ExecuteSqlCommand("Update Admins set AdminName = '" + admin.AdminName + "'  , AdminPassword = '" + admin.AdminPassword + " '    where AdminEmail = '" +
                   Session["admin_email"] + "'");
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }


        }
    }

}
