using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TMDT.Models;

namespace TMDT.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {

            return View(db.Order.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var orderDetail = (from m in db.OrderDetail where m.OrderID == id select m).ToList();
            var userrOrderId = orderDetail.FirstOrDefault().Order.UserId.ToString();
            var userOrderDetail = db.Users.FirstOrDefault(x => x.Id == userrOrderId);

            ViewBag.name = orderDetail.FirstOrDefault().Order.NameRec;
            ViewBag.email = userOrderDetail.Email;
            ViewBag.address = orderDetail.FirstOrDefault().Order.AddressOrder;
            ViewBag.phone = orderDetail.FirstOrDefault().Order.PhoneOrder;

            return View(orderDetail);
        }

        public ActionResult DetailOfUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var orderDetail = (from m in db.OrderDetail where m.OrderID == id select m).ToList();
            var userrOrderId = orderDetail.FirstOrDefault().Order.UserId.ToString();
            var userOrderDetail = db.Users.FirstOrDefault(x => x.Id == userrOrderId);

            ViewBag.name = orderDetail.FirstOrDefault().Order.NameRec;
            ViewBag.email = userOrderDetail.Email;
            ViewBag.address = orderDetail.FirstOrDefault().Order.AddressOrder;
            ViewBag.phone = orderDetail.FirstOrDefault().Order.PhoneOrder;

            return View(orderDetail);
        }
        public ActionResult ListOrderOfUser()
        {
            string currentUserId = User.Identity.GetUserId();
            var listOrder = (from m in db.Order where m.UserId == currentUserId select m).ToList();
            return View(listOrder);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string userName, string phoneNumber, string addressShip, int payment)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                var currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);

                Order order = new Order();
                order.PhoneOrder = phoneNumber;
                order.UserId = currentUser.Id;
                order.AddressOrder = addressShip;
                order.NameRec = userName;

                if (payment == 0)
                {
                    order.Payment = "Thanh toán khi nhận hàng";
                }
                if (payment == 1)
                {
                    order.Payment = "Thanh toán trực tuyến";
                }

                order.Status = StatusEnum.WAIT;
                DateTime currentDate = DateTime.Now;
                order.OrderDate = currentDate;
                db.Order.Add(order);

                Cart cart = Session["Cart"] as Cart;
                foreach (var item in cart.Items)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderID = order.OrderID;
                    orderDetail.BookID = item._shopping_product.BookID;
                    orderDetail.UnitPriceSale = item._shopping_product.BookPrice;
                    orderDetail.Quantity = item._shopping_quantity;
                    db.OrderDetail.Add(orderDetail);

                }
                db.SaveChanges();

                if (payment == 0)
                {
                    try
                    {
                        float total = 0;
                        //                       
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/MailHelper/SendMailOrder.html"));
                        content = content.Replace("{{OrderDate}}", order.OrderDate.ToString());
                        content = content.Replace("{{CustomerName}}", order.NameRec);
                        content = content.Replace("{{Phone}}", order.PhoneOrder);
                        content = content.Replace("{{Email}}", userName);
                        content = content.Replace("{{Address}}", order.AddressOrder);
                        content = content.Replace("{{Total}}", total.ToString("N0"));
                        var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                        new MailHelper.MailHelper().SendMail(userName, "Đơn hàng mới từ Shop TMDT04", content);
                        new MailHelper.MailHelper().SendMail(toEmail, "Đơn hàng mới từ Shop TMDT04", content);
                        cart.ClearCart();


                        return RedirectToAction("SuccessView", "Paypal");
                    }
                    catch
                    {
                        return Content("Error CHeckout. Please information of Customer....");
                    }
                }
                if (payment == 1)
                {
                    return null;
                }

            }

            return View();
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserId,AddressOrder,PhoneOrder,OrderDate,Status,Payment")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Order.Find(id);
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
            Order order = db.Order.Find(id);
            db.Order.Remove(order);
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
