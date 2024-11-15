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
    public class NguoidungController : Controller
    {
            private readonly QLBANSACHEntities2 _db;
            public NguoidungController(QLBANSACHEntities2 db)
            {
                _db = db;
            }
            public NguoidungController() : this(new QLBANSACHEntities2())
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
            ViewData["Chude"] = await _db.CHUDEs.ToListAsync();
            ViewData["NXB"] = await _db.NHAXUATBANs.ToListAsync();
            return View(products);
            }

            [HttpGet]
        public async Task<ActionResult> DangKy()
        {
            ViewData["Chude"] = await _db.CHUDEs.ToListAsync();
            ViewData["NXB"] = await _db.NHAXUATBANs.ToListAsync();
            return View(new KHACHHANG());
        }
        public ActionResult Dangky()
        {

            return View();
        }
        [HttpPost]
            public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
            {
                try
                {
                    if (kh == null)
                    {
                        kh = new KHACHHANG();
                    }
                    var hoten = collection["HotenKH"];
                    var tendn = collection["TenDN"];
                    var matkhau = collection["Matkhau"];
                    var matkhaunhaplai = collection["Matkhaunhaplai"];
                    var email = collection["email"];
                    var diachi = collection["Diachi"];
                    var dienthoai = collection["Dienthoai"];
                    var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
                    if (String.IsNullOrEmpty(hoten))
                    {
                        ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
                    }
                    else if (String.IsNullOrEmpty(tendn))
                    {
                        ViewData["Loi2"] = "Tên đăng nhập không được để trống";
                    }
                    else if (String.IsNullOrEmpty(matkhau))
                    {
                        ViewData["Loi3"] = "Mật khẩu không được để trống";
                    }
                    else if (String.IsNullOrEmpty(matkhaunhaplai) || matkhaunhaplai != matkhau )
                    {
                        ViewData["Loi4"] = "Mật khẩu không chính xác";
                    }
                    else if (String.IsNullOrEmpty(email))
                    {
                        ViewData["Loi5"] = "Email không được để trống";
                    }
                    else if (String.IsNullOrEmpty(diachi))
                    {
                        ViewData["Loi6"] = "Địa chỉ không được để trống";
                    }
                    else if (String.IsNullOrEmpty(dienthoai))
                    {
                        ViewData["Loi7"] = "Số điện thoại không được để trống";
                    }
                    else
                    {
                        //gán giá trị cho đối tượng mới được tạo (kh)
                        kh.HoTen = hoten;
                        kh.Taikhoan = tendn;
                        kh.Matkhau = matkhau;
                        kh.Email = email;
                        kh.DiachiKH = diachi;
                        kh.DienthoaiKH = dienthoai;
                        kh.Ngaysinh = DateTime.Parse(ngaysinh);
                        _db.KHACHHANGs.Add(kh);
                        _db.SaveChanges();
                        return RedirectToAction("Dangnhap");
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                    return View("Dangky");
                }
                return this.Dangky();
            }

        //public ActionResult Dangnhap()
        //{

        //    return View();
        //}
        [HttpGet]
        public async Task<ActionResult> Dangnhap()
        {
            ViewData["Chude"] = await _db.CHUDEs.ToListAsync();
            ViewData["NXB"] = await _db.NHAXUATBANs.ToListAsync();
            return View(new KHACHHANG());
        }
        //public ActionResult Dangnhap()
        //    {

        //    return View();
        //}
            [HttpPost]
            public ActionResult Dangnhap(FormCollection collection)
            {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];

            // Kiểm tra đầu vào
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else
            {
                // Kiểm tra tài khoản và mật khẩu
                KHACHHANG kh = _db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau== matkhau);
                if (kh != null)
                {
                    //ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Home");

                    // Trả về lại view với dữ liệu chủ đề và nhà xuất bản
                    var model = new ProductVM
                    {
                        cHUDEs = _db.CHUDEs.ToList(),
                        nxb = _db.NHAXUATBANs.ToList()
                    };

                      // Hoặc action khác
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            // Trả về view với dữ liệu model đầy đủ
            var modelForView = new ProductVM
            {
                cHUDEs = _db.CHUDEs.ToList(),
                nxb = _db.NHAXUATBANs.ToList()
            };
            return View(modelForView);
        }
        }
    }