using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TMDT.Models;
using PagedList;
using PagedList.Mvc;
namespace TMDT.Controllers
{
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        public ActionResult Index(int? i)
        {
          
            var book = db.Book.Include(b => b.Author).Include(b => b.Category).Include(b => b.Provider).Include(b => b.Publisher);
            return View(book.ToList().ToPagedList(i ??  1,6));
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorName");
            ViewBag.CateID = new SelectList(db.Category, "CateID", "CateName");
            ViewBag.ProviderID = new SelectList(db.Provider, "ProviderID", "ProviderName");
            ViewBag.PublisherID = new SelectList(db.Publisher, "PublisherID", "PublisherName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.ImageUpLoad != null)
                {
                    string fileNameImg = Path.GetFileNameWithoutExtension(book.ImageUpLoad.FileName);
                    string extension = Path.GetExtension(book.ImageUpLoad.FileName);
                    fileNameImg = fileNameImg + extension;
                    book.Image = "~/Content/Images/" + fileNameImg;
                    book.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/"), fileNameImg));
                }
                db.Book.Add(book);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CateID = new SelectList(db.Category, "CateID", "CateName", book.CateID);
            ViewBag.ProviderID = new SelectList(db.Provider, "ProviderID", "ProviderName", book.ProviderID);
            ViewBag.PublisherID = new SelectList(db.Publisher, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CateID = new SelectList(db.Category, "CateID", "CateName", book.CateID);
            ViewBag.ProviderID = new SelectList(db.Provider, "ProviderID", "ProviderName", book.ProviderID);
            ViewBag.PublisherID = new SelectList(db.Publisher, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookID,BookName,BookPrice,BookDescription,PublisherDate,Image,AuthorID,PublisherID,ProviderID,CateID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Author, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CateID = new SelectList(db.Category, "CateID", "CateName", book.CateID);
            ViewBag.ProviderID = new SelectList(db.Provider, "ProviderID", "ProviderName", book.ProviderID);
            ViewBag.PublisherID = new SelectList(db.Publisher, "PublisherID", "PublisherName", book.PublisherID);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Book.Find(id);
            db.Book.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SearchTenSach(string tenSach)
        {
            var tenSachs = from m in db.Book select m;

            if (!String.IsNullOrEmpty(tenSach))
            {
                tenSachs = tenSachs.Where(s => s.BookName.Contains(tenSach));
            }
            else
            {
                return RedirectToAction("Index", "Books");
            }

            return View("Index", tenSachs);

        }
        public ActionResult LoadSachTheoDanhMuc(string name)
        {
            var ten_loai = name;
            var tenDanhMuc = db.Book.Where(m => m.Category.CateName == ten_loai);
            return View("Index", tenDanhMuc.ToList());

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //public ActionResult PhanTrang(int? page)
        //{

        //    // 1. Tham số int? dùng để thể hiện null và kiểu int
        //    // page có thể có giá trị là null và kiểu int.

        //    // 2. Nếu page = null thì đặt lại là 1.
        //    if (page == null) page = 1;

        //    // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
        //    // theo LinkID mới có thể phân trang.
        //    var links = (from l in db.Book
        //                 select l).OrderBy(x => x.BookID);

        //    // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
        //    int pageSize = 3;

        //    // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
        //    // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
        //    int pageNumber = (page ?? 1);

        //    // 5. Trả về các Link được phân trang theo kích thước và số trang.
        //    return View(links.ToPagedList(pageNumber, pageSize));
        //}
    }
}
