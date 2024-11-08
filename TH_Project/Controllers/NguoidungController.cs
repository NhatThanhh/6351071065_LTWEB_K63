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
            private readonly QLBANSACHEntities _db;
            public NguoidungController(QLBANSACHEntities db)
            {
                _db = db;
            }
            public NguoidungController() : this(new QLBANSACHEntities())
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

            [HttpGet]
            public ActionResult Dangky()
            {
            var model = new ProductVM
            {
                // Khởi tạo các thuộc tính cần thiết, chẳng hạn như danh sách chủ đề và nhà xuất bản
                cHUDEs = _db.CHUDEs.ToList(),
                nxb = _db.NHAXUATBANs.ToList()
            };

            return View(model);
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
                        ViewData["Loi2"] = "Phải nhập tên đăng nhập";
                    }
                    else if (String.IsNullOrEmpty(matkhau))
                    {
                        ViewData["Loi3"] = "Phải nhập mật khẩu";
                    }
                    else if (String.IsNullOrEmpty(matkhaunhaplai))
                    {
                        ViewData["Loi4"] = "Phải nhập lại mật khẩu";
                    }
                    else if (String.IsNullOrEmpty(email))
                    {
                        ViewData["Loi5"] = "Email không được bỏ trống";
                    }
                    else if (String.IsNullOrEmpty(diachi))
                    {
                        ViewData["Loi6"] = "Địa chỉ không được để trống";
                    }
                    else if (String.IsNullOrEmpty(dienthoai))
                    {
                        ViewData["Loi7"] = "Số điện thoại không được bỏ trống";
                    }
                    else
                    {
                        //gán giá trị cho đối tượng mới được tạo (kh)
                        kh.HoTen = hoten;
                        kh.TaiKhoan = tendn;
                        kh.MatKhau = matkhau;
                        kh.Email = email;
                        kh.DiaChiKH = diachi;
                        kh.DienThoaiKH = dienthoai;
                        kh.NgaySinh = DateTime.Parse(ngaysinh);
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
            [HttpGet]
            public ActionResult Dangnhap()
            {
            var model = new ProductVM
            {
                // Khởi tạo các thuộc tính cần thiết, chẳng hạn như danh sách chủ đề và nhà xuất bản
                cHUDEs = _db.CHUDEs.ToList(),
                nxb = _db.NHAXUATBANs.ToList()
            };

            return View(model);
        }
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
                KHACHHANG kh = _db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    //return RedirectToAction("DatHang", "GioHang");

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