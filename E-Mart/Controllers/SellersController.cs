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
    public class SellersController : Controller
    {
        private Entities6 db = new Entities6();

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Seller seller)
        {

            Seller check = db.Sellers.Where(u => u.SellerEmail.Equals(seller.SellerEmail)).FirstOrDefault();
            if (check != null)
            {
                TempData["Referrer"] = "SignUp";
                return RedirectToAction("SignUp");
            }
            else  if (ModelState.IsValid)
            {
                db.Sellers.Add(seller);
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
        public ActionResult LogIn(TempSeller tempseller)
        {
            if (ModelState.IsValid)
            {
                var check = db.Sellers.Where(u => u.SellerEmail.Equals(tempseller.SellerEmail) && u.SellerPassword.Equals(tempseller.SellerPassword)).FirstOrDefault();

                if (check != null)
                {
                    Session["seller_email"] = check.SellerEmail;
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
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email = Convert.ToString(Session["seller_email"]);
            var user = db.Sellers.Where(u => u.SellerEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }

        public ActionResult DashBoard()
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email = Convert.ToString(Session["seller_email"]);
            var user = db.Sellers.Where(u => u.SellerEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }

        public ActionResult Details()
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string email = Convert.ToString(Session["seller_email"]);
            var user = db.Sellers.Where(u => u.SellerEmail.Equals(email)).FirstOrDefault();
            return View(user);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seller seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Seller seller)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (Session["seller_email"] == null)
                return RedirectToAction("LogIn");
            try
            {


                db.Database.ExecuteSqlCommand("Update Sellers set SellerName = '" + seller.SellerName + "'  , SellerPassword = '" + seller.SellerPassword + " ' , SellerPhone = '" + seller.SellerPhone + "' , SellerAdress = '" + seller.SellerAdress + "' where SellerEmail = '" + Session["seller_email"]+"'");
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }


        }


    }
}
