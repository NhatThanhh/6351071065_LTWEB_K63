using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using TH_Project.ViewModel;


namespace TH_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly QLBANSACHEntities2 _db;
        public ProductController(QLBANSACHEntities2 db)
        {
            _db = db;
        }
        public ProductController() : this(new QLBANSACHEntities2())
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
            ViewData["Chude"] = products.cHUDEs;
            ViewData["NXB"] = products.nxb;

            return View(products);
        }

        [HttpGet]
        public async Task<ActionResult> ProductDetail(int id)
        {

            var product = await _db.SACHes.FindAsync(id);
            var productVM = new DetailProductVM()
            {
                cHUDEs = await _db.CHUDEs.ToListAsync(),
                nxb = await _db.NHAXUATBANs.ToListAsync(),
                SACH = product
            };

            if (product == null)
            {
                return HttpNotFound();
            }
            ViewData["Chude"] = productVM.cHUDEs;
            ViewData["NXB"] = productVM.nxb;
            return View(productVM);
        }


    }
}