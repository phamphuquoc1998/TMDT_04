using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class OrderDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderDetail
        public ActionResult Index()
        {
            var orderDetail = db.OrderDetail.Include(o => o.Book).Include(o => o.Order);
            return View(orderDetail.ToList());
        }

        // GET: OrderDetail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetail.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // GET: OrderDetail/Create
        public ActionResult Create()
        {
            ViewBag.BookID = new SelectList(db.Book, "BookID", "BookName");
            ViewBag.OrderID = new SelectList(db.Order, "OrderID", "UserId");
            return View();
        }

        // POST: OrderDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderDetailID,OrderID,BookID,Quantity,UnitPriceSale")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetail.Add(orderDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookID = new SelectList(db.Book, "BookID", "BookName", orderDetail.BookID);
            ViewBag.OrderID = new SelectList(db.Order, "OrderID", "UserId", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetail.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookID = new SelectList(db.Book, "BookID", "BookName", orderDetail.BookID);
            ViewBag.OrderID = new SelectList(db.Order, "OrderID", "UserId", orderDetail.OrderID);
            return View(orderDetail);
        }

        // POST: OrderDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailID,OrderID,BookID,Quantity,UnitPriceSale")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookID = new SelectList(db.Book, "BookID", "BookName", orderDetail.BookID);
            ViewBag.OrderID = new SelectList(db.Order, "OrderID", "UserId", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetail.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetail.Find(id);
            db.OrderDetail.Remove(orderDetail);
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
