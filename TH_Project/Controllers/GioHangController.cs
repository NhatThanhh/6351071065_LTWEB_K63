using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;
using TH_Project.Models;
using TH_Project.ViewModel;

namespace TH_Project.Controllers
{
    public class GioHangController : Controller
    {
        private readonly QLBANSACHEntities _db;
        public GioHangController(QLBANSACHEntities db)
        {
            _db = db;
        }
        public GioHangController() : this(new QLBANSACHEntities())
        {
        }
        // GET: Product
        [HttpGet]
        public async Task<ActionResult> Index(int? idNXB, int? idCD)
        {
            var products = new ProductVM()
            {
                cHUDEs = await _db.CHUDEs.ToListAsync(),
                nxb = await _db.NHAXUATBANs.ToListAsync()
            };

            if (idNXB.HasValue)
            {
                products.SACHes = await _db.SACHes.Where(s => s.MaNXB == idNXB.Value).ToListAsync();
            }
            else if (idCD.HasValue)
            {
                products.SACHes = await _db.SACHes.Where(s => s.MaCD == idCD.Value).ToListAsync();
            }
            else
            {
                products.SACHes = await _db.SACHes.ToListAsync();
            }

            return View(products);
        }

        public List<GioHang> LayGioHang()
        {
            var lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }

        // GET: GioHang
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            var lstGiohang = LayGioHang();
            var sanPham = lstGiohang.Find(n => n.iMaSach == iMaSach);

            if (sanPham == null)
            {
                sanPham = new GioHang(iMaSach);
                lstGiohang.Add(sanPham);
            }
            else
            {
                sanPham.iSoLuong++;
            }
            return Redirect(strURL);
        }
        public int TongSoLuong()
        {
            var list = Session["GioHang"] as List<GioHang>;
            return list?.Sum(n => n.iSoLuong) ?? 0;
        }

        public double TongTien()
        {
            var list = Session["GioHang"] as List<GioHang>;
            return list?.Sum(n => n.dThanhTien) ?? 0;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                //return RedirectToAction("Index","Product");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
    }
}
