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
    public class HomeController : Controller
    {
        private readonly QLBANSACHEntities _db;
        public HomeController(QLBANSACHEntities db)
        {
            _db = db;
        }
        public HomeController() : this(new QLBANSACHEntities())
        {
        }
        [HttpGet]
        public async  Task<ActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                SACHes = await _db.SACHes.ToListAsync(),
                cHUDEs =await _db.CHUDEs.ToListAsync(),
                nxb = await _db.NHAXUATBANs.ToListAsync()
            };
          
            return View(homeVM);
        }


    }
}