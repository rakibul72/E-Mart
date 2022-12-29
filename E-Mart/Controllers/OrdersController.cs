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
    public class OrdersController : Controller
    {
        private Entities6 db = new Entities6();

        // GET: Orders
        public ActionResult Index(int? id)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }
            if (id == null)
            {

                var orders = db.Orders.Where(u => u.OrderStatus == 0);
                Order order2 = new Order();
                order2 = (db.Orders).FirstOrDefault();
                return View(orders.ToList());
            }
            else
            {
                string str = "";
                if (Session["admin_email"] != null)
                    str = Session["admin_email"].ToString();

                Admin admin = db.Admins.Where(u => u.AdminEmail.Equals(str)).FirstOrDefault();
                db.Database.ExecuteSqlCommand("Update Orders set OrderStatus = 1 , AdminID = " + admin.AdminID + " where OrderID = " + id + "");
                db.SaveChanges();


                var orders = db.Orders.Where(u => u.OrderStatus == 0);
                Order order2 = new Order();
                order2 = (db.Orders).FirstOrDefault();
                return View(orders.ToList());
            }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string str="";
            if (Session["buyer_email"] != null)
                str = Session["buyer_email"].ToString();

            Buyer buyers = db.Buyers.Where(u => u.BuyerEmail.Equals(str)).FirstOrDefault();
            id = buyers.BuyerID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order orders = db.Orders.Where(u => u.BuyerID == id).FirstOrDefault();

            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        public ActionResult Purchase_History(int? id)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string str = "";
            if (Session["buyer_email"] != null)
                str = Session["buyer_email"].ToString();

            Buyer buyers = db.Buyers.Where(u => u.BuyerEmail.Equals(str)).FirstOrDefault();
            id = buyers.BuyerID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orders = db.Orders.Where(u => u.BuyerID == id && u.OrderStatus==1);
            Order order2 = new Order();
            order2 = (db.Orders).FirstOrDefault();
            return View(orders.ToList());

            if (orders == null)
            {
                return HttpNotFound();
            }
            
        }


        public ActionResult Sales_History(int? id)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            string str = "";
            if (Session["seller_email"] != null)
                str = Session["seller_email"].ToString();

            Seller sellers = db.Sellers.Where(u => u.SellerEmail.Equals(str)).FirstOrDefault();
            id = sellers.SellerID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orders = db.Orders.Where(u => u.SellerID == id && u.OrderStatus == 1);
            Order order2 = new Order();
            order2 = (db.Orders).FirstOrDefault();
            return View(orders.ToList());

            if (orders == null)
            {
                return HttpNotFound();
            }

        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName");
            ViewBag.BuyerID = new SelectList(db.Buyers, "BuyerID", "BuyerName");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,SellerID,BuyerID,ProductID,AdminID,OrderQuantity,OrderDate")] Order order)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName", order.AdminID);
            ViewBag.BuyerID = new SelectList(db.Buyers, "BuyerID", "BuyerName", order.BuyerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName", order.SellerID);
            return View(order);
        }

        // GET: Orders/Edit/5
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
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName", order.AdminID);
            ViewBag.BuyerID = new SelectList(db.Buyers, "BuyerID", "BuyerName", order.BuyerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName", order.SellerID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,SellerID,BuyerID,ProductID,AdminID,OrderQuantity,OrderDate")] Order order)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admins, "AdminID", "AdminName", order.AdminID);
            ViewBag.BuyerID = new SelectList(db.Buyers, "BuyerID", "BuyerName", order.BuyerID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", order.ProductID);
            ViewBag.SellerID = new SelectList(db.Sellers, "SellerID", "SellerName", order.SellerID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
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
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["seller_email"] == null && Session["admin_email"] == null && Session["buyer_email"] == null)
            {
                Session.Abandon();
                return RedirectToAction("../Products/Logout");
            }

            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
