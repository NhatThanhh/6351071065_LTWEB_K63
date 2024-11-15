//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TH_Project.Data;

//namespace TH_Project.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly QLBANSACHEntities _db;
//        public AdminController(QLBANSACHEntities db)
//        {
//            _db = db;
//        }
//        public AdminController() : this(new QLBANSACHEntities())
//        { }
//        // GET: Admin
//        public ActionResult Index()
//        {
//            return View();
//        }
//        [HttpGet]
//        public ActionResult Login()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult Login(FormCollection collection)
//        {
//            var tendn = collection["username"];
//            var matkhau = collection["password"];
//            if (String.IsNullOrEmpty(tendn))
//            {
//                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
//            }
//            else if (String.IsNullOrEmpty(matkhau))
//            {
//                ViewData["Loi2"] = "Mật khẩu không được để trống";
//            }
//            else
//            {
//                Admin ad = _db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
//                if (ad != null)
//                {
//                    Session["TaikhoanAdmin"] = ad;
//                    return RedirectToAction("Index", "Admin");
//                }
//                else
//                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";

//            }
//            return View();
//        }
//    }
//}