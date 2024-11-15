using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH_Project.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using TH_Project.ViewModel;
using PagedList;
using PagedList.Mvc;

namespace TH_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLBANSACHEntities2 _db;
        public HomeController(QLBANSACHEntities2 db)
        {
            _db = db;
        }
        public HomeController() : this(new QLBANSACHEntities2())
        {
        }
        [HttpGet]
        public async  Task<ActionResult> Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            HomeVM homeVM = new HomeVM()
            {
                SACHes = (await _db.SACHes.ToListAsync()).ToPagedList(pageNum,pageSize),
                cHUDEs =await _db.CHUDEs.ToListAsync(),
                nxb = await _db.NHAXUATBANs.ToListAsync()
            };
            ViewData["Chude"] = homeVM.cHUDEs;
            ViewData["NXB"] = homeVM.nxb;
            return View(homeVM);
        }


    }
}