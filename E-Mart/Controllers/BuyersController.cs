using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Mart.Models;

namespace E_Mart.Controllers
{
    public class BuyersController : Controller
    {
        private Entities6 db = new Entities6();

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Buyer buyer)
        {
            Buyer check = db.Buyers.Where(u => u.BuyerEmail.Equals(buyer.BuyerEmail)).FirstOrDefault();
            if(check!=null)
            {
                TempData["Referrer"] = "SignUp";
                return RedirectToAction("SignUp");
            }
            else if (ModelState.IsValid)
            {
                db.Buyers.Add(buyer);
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
        public ActionResult LogIn(TempBuyer tempbuyer)
        {
            if (ModelState.IsValid)
            {
                var check = db.Buyers.Where(u => u.BuyerEmail.Equals(tempbuyer.BuyerEmail) && u.BuyerPassword.Equals(tempbuyer.BuyerPassword)).FirstOrDefault();

                if (check != null)
                {
                    Session["buyer_email"] = check.BuyerEmail;
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
            if (Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email = Convert.ToString(Session["buyer_email"]);
            var user = db.Buyers.Where(u => u.BuyerEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }
        public ActionResult DashBoard()
        {
            if (Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email = Convert.ToString(Session["buyer_email"]);
            var user = db.Buyers.Where(u => u.BuyerEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buyer buyer = db.Buyers.Find(id);
            if (buyer == null)
            {
                return HttpNotFound();
            }
            
            return View(buyer);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Buyer buyer)
        {
            if (Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

          
            try 
            {
              
             
                db.Database.ExecuteSqlCommand("Update Buyers set BuyerName = '"+buyer.BuyerName+"'  , BuyerPassword = '"+buyer.BuyerPassword+" ' , BuyerPhone = '"+buyer.BuyerPhone +
                   "' , BuyerAdress = '"+buyer.BuyerAdress+"'  where BuyerEmail = '"+
                   Session["buyer_email"]+"'");
                db.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            catch(Exception e)
            {
                return Content(e.ToString());
            }
           
         
        }


        
    }
}
