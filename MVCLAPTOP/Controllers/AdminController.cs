using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCLAPTOP.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Data.Entity;

namespace MVCLAPTOP.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLstoreDataContext db = new dbQLstoreDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Laptop(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.LAPTOPs.ToList().OrderBy(n => n.Malaptop).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult hangsanxuat()
        {
            return View(db.HANGs.ToList());
        }
        public ActionResult chudesp()
        {
            return View(db.CHUDEs.ToList());
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        

                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSanpham()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSanpham(LAPTOP laptop, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }

                    laptop.Anhbia = fileName;
                    //Luu vao CSDL
                    db.LAPTOPs.InsertOnSubmit(laptop);
                    db.SubmitChanges();
                }
                return RedirectToAction("Laptop");
            }
        }
        [HttpGet]
        public ActionResult Themmoihang()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
    

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoihang(HANG hang, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
      
            //Kiem tra duong dan file
          
            //Them vao CSDL
           
                if (ModelState.IsValid)
                {
                   
                   
                    //Luu vao CSDL
                    db.HANGs.InsertOnSubmit(hang);
                    db.SubmitChanges();
                }
                return RedirectToAction("hangsanxuat");
           
        }
        [HttpGet]
        public ActionResult Suahang(int id)
        {
            HANG hang = db.HANGs.SingleOrDefault(n => n.MaHang == id);
            ViewBag.MaHang = hang.MaHang;
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(hang);
           
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suahang(HANG hang, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
          
            //Kiem tra duong dan file
          
            //Them vao CSDL
            
                if (ModelState.IsValid)
                {
                
                  

                    HANG h = db.HANGs.SingleOrDefault(n => n.MaHang == hang.MaHang);
                h.MaHang = hang.MaHang;
                    h.TenHang = hang.TenHang;
                   
                    //Luu vao CSDL   
                    UpdateModel(hang);
                    db.SubmitChanges();

                }
                return RedirectToAction("hangsanxuat");
            
        }
        [HttpGet]
        public ActionResult Xoahang(int id)
        {
            return View();
        }
        [HttpPost, ActionName("Xoahang")]
        public ActionResult Banmuonxoa(int id)
        {

            HANG hang = db.HANGs.SingleOrDefault(n => n.MaHang == id);
            ViewBag.MaHang = hang.MaHang;
            if (hang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.HANGs.DeleteOnSubmit(hang);
            db.SubmitChanges();
            return RedirectToAction("hangsanxuat");
        }
        [HttpGet]
        public ActionResult Themmoicdsp()
        {
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");


            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoicdsp(CHUDE chude, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload

            //Kiem tra duong dan file

            //Them vao CSDL

            if (ModelState.IsValid)
            {


                //Luu vao CSDL
                db.CHUDEs.InsertOnSubmit(chude);
                db.SubmitChanges();
            }
            return RedirectToAction("chudesp");

        }
        [HttpGet]
        public ActionResult SuaCD(int id)
        {
            CHUDE chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if(chude== null)
            {
                Response.StatusCode = 404;
                return null;
               
            }
            return View(chude);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaCD(CHUDE chude, HttpPostedFileBase fileUpload)
        {
      
         
            //Them vao CSDL
          
                if (ModelState.IsValid)
                {
                    

                    CHUDE cd = db.CHUDEs.SingleOrDefault(n => n.MaCD == chude.MaCD);
                    cd.MaCD = chude.MaCD;
                cd.TenChuDe = chude.TenChuDe;
                    
                    //Luu vao CSDL   
                    UpdateModel(chude);
                    db.SubmitChanges();

                }
                return RedirectToAction("chudesp");
            
        }
        [HttpGet]
        public ActionResult XoaCD(int id)
        {
            return View();
        }
        [HttpPost, ActionName("XoaCD")]
        public ActionResult Chacchanxoa(int id)
        {

            CHUDE chude = db.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            ViewBag.MaCD = chude.MaCD;
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CHUDEs.DeleteOnSubmit(chude);
            db.SubmitChanges();
            return RedirectToAction("chudesp");
        }
        public ActionResult ChitietSanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            LAPTOP laptop = db.LAPTOPs.SingleOrDefault(n => n.Malaptop == id);
            ViewBag.Malaptop = laptop.Malaptop;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(laptop);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            LAPTOP laptop = db.LAPTOPs.SingleOrDefault(n => n.Malaptop == id);
            ViewBag.Malaptop = laptop.Malaptop;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(laptop);
        }

          [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
        
            LAPTOP laptop = db.LAPTOPs.SingleOrDefault(n => n.Malaptop == id);
            ViewBag.Masach = laptop.Malaptop;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LAPTOPs.DeleteOnSubmit(laptop);
            db.SubmitChanges();
            return RedirectToAction("Laptop");
        }

        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            LAPTOP laptop = db.LAPTOPs.SingleOrDefault(n => n.Malaptop == id);
            ViewBag.Malaptop = laptop.Malaptop;
            if (laptop == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", laptop.MaCD);
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", laptop.MaHang);
            return View(laptop);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(LAPTOP laptop, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaHang = new SelectList(db.HANGs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    //Kiem tra hình anh ton tai chua?
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    
                    LAPTOP lp = db.LAPTOPs.SingleOrDefault(n => n.Malaptop == laptop.Malaptop);
                    lp.Tenlaptop = laptop.Tenlaptop;
                    lp.Mota = laptop.Mota;
                    lp.Anhbia = fileName;
                    lp.Soluongton = laptop.Soluongton;
                    lp.Giaban = laptop.Giaban;
                    //Luu vao CSDL   
                    UpdateModel(laptop);
                    db.SubmitChanges();

                }
                return RedirectToAction("Laptop");
            }
        }
    }
}