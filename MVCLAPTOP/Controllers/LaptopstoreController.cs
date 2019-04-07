using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCLAPTOP.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCLAPTOP.Controllers
{
    public class LaptopstoreController : Controller
    {
        dbQLstoreDataContext data = new dbQLstoreDataContext();
        // GET: Laptopstore
        private List<LAPTOP> Laylaptop(int count)
        {
            return data.LAPTOPs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageNum = (page ?? 1);
            int pageSize = 6;
            var laptopmoi = Laylaptop(7);

            return View(laptopmoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Hang()
        {
            var hang = from H in data.HANGs select H;
            return PartialView(hang);
        }
        public ActionResult SPtheochude(int id)
        {
            var laptop = from l in data.LAPTOPs where l.MaCD == id select l;
            return View(laptop);
        }
        public ActionResult SPtheohang(int id)
        {
            var laptop = from l in data.LAPTOPs where l.MaHang == id select l;
            return View(laptop);
        }
        public ActionResult Details(int id)
        {
            var laptop = from l in data.LAPTOPs
                         where l.Malaptop == id
                         select l;
            return View(laptop.Single());
        }
    }
}