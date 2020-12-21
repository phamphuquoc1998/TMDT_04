using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TMDT.Mail;
using TMDT.Models;
using TMDT.Payment;

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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                float total = 0;
                Cart cart = Session["Cart"] as Cart;
                foreach (var item in cart.Items)
                {

                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderID = order.OrderID;
                    orderDetail.BookID = item._shopping_product.BookID;
                    orderDetail.UnitPriceSale = item._shopping_product.BookPrice;
                    orderDetail.Quantity = item._shopping_quantity;
                    total += item._shopping_quantity * item._shopping_product.BookPrice;
                    db.OrderDetail.Add(orderDetail);

                }

                if (payment == 0)
                {
                    //try
                    //{
                    
                    //                       


                    string nameItem = "";
                    foreach (var item in cart.Items)
                    {
                        
                        nameItem += "<tr>" +
                            "<td>" + item._shopping_product.BookName.ToString() + "</td>" +
                            "<td>" + item._shopping_quantity.ToString() + "</td>" +
                            "<td>" + item._shopping_product.BookPrice.ToString() + "</td>" +
                            "</tr>";

                    }
                    string content = "<html>" +
                       "<head><link rel=" + "stylesheet" + " href=" + "https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" + " integrity=" + "sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" + " crossorigin=" + "anonymous" + "></head>" +
                       "<body>" +
                           "<label class=" + "label - info" + ">Tên khách hàng: " + order.NameRec.ToString() + "</label><br/>" +
                           "<label class=" + "label - info" + ">Địa chỉ: " + order.AddressOrder.ToString() + "</label><br/>" +
                           "<label class=" + "label - info" + ">Số điện thoại: " + order.PhoneOrder.ToString() + "</label><br/>" +
                           "<label class=" + "label - info" + ">Email: " + userName.ToString() + "</label><br/>" +
                                "<table class=" + "table table-hover table - bordered table - condensed" + ">" +
                                     "<thead>" +
                                         "<td>Tên</td>" +
                                         "<td>Số lượng</td>" +
                                         "<td>Đơn giá</td>" +
                                     "</thead>" +
                                     "<tbody>" + nameItem.ToString() +
                                     "<tr><td>" + total.ToString("N0") + "$</td></tr>" +
                                     "</tbody>" +
                               "</table>" +
                             "</body>" +
                       " </html>";





                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new MailHelper().SendMail(userName, "Đơn hàng mới từ Shop TMDT04", content);
                    new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Shop TMDT04", content);
                    cart.ClearCart();

                    db.SaveChanges();

                    return RedirectToAction("SuccessView", "Paypal");
                    //}
                    //catch
                    //{
                    //    return Content("Error CHeckout. Please information of Customer....");
                    //}


                }
                if (payment == 1)
                {
                    //momo payment
                    string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
                    string partnerCode = "MOMO5RGX20191128";
                    string accessKey = "M8brj9K6E22vXoDB";
                    string serectkey = "nqQiVSgDMy809JoPF6OzP5OdBUB550Y4";
                    string orderInfo = "Đơn hàng từ SHOP TMD04";
                    string returnUrl = "/Orders";
                    string notifyurl = "https://momo.vn/notify";

                    string amount = total.ToString();
                    string orderid = order.OrderID.ToString();
                    string requestId = "10";
                    string extraData = "";

                    //Before sign HMAC SHA256 signature
                    string rawHash = "partnerCode=" +
                        partnerCode + "&accessKey=" +
                        accessKey + "&requestId=" +
                        requestId + "&amount=" +
                        amount + "&orderId=" +
                        orderid + "&orderInfo=" +
                        orderInfo + "&returnUrl=" +
                        returnUrl + "&notifyUrl=" +
                        notifyurl + "&extraData=" +
                        extraData;

                    log.Debug("rawHash = " + rawHash);


                    MomoPayment crypto = new MomoPayment();
                    //sign signature SHA256
                    string signature = crypto.signSHA256(rawHash, serectkey);
                    log.Debug("Signature = " + signature);

                    //build body json request
                    JObject message = new JObject
                                            {
                                                { "partnerCode", partnerCode },
                                                { "accessKey", accessKey },
                                                { "requestId", requestId },
                                                { "amount", amount },
                                                { "orderId", orderid },
                                                { "orderInfo", orderInfo },
                                                { "returnUrl", returnUrl },
                                                { "notifyUrl", notifyurl },
                                                { "extraData", extraData },
                                                { "requestType", "captureMoMoWallet" },
                                                { "signature", signature }

                                            };
                    log.Debug("Json request to MoMo: " + message.ToString());
                    string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

                    JObject jmessage = JObject.Parse(responseFromMomo);
                    log.Debug("Return from MoMo: " + jmessage.ToString());
                   
                    //if (result == DialogResult.OK)
                    //{
                    //    //yes...
                    //    System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString());
                    //}
                    //else if (result == DialogResult.Cancel)
                    //{
                    //    //no...
                    //}


                    return null;
                }

            }

            return View();
        }

        [AccessDeniedAuthorize(Roles = "ADMIN")]
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
        public ActionResult Edit([Bind(Include = "OrderID,UserId,NameRec,AddressOrder,PhoneOrder,OrderDate,Status,Payment")] Order order)
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
