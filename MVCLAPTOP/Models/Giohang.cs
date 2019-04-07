using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCLAPTOP.Models;

namespace MVCLAPTOP.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo. 
        dbQLstoreDataContext data = new dbQLstoreDataContext();
        public int iMalaptop { set; get; }
        public string sTenlaptop { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(int Malaptop)
        {
            iMalaptop = Malaptop;
            LAPTOP laptop = data.LAPTOPs.Single(n => n.Malaptop == iMalaptop);
            sTenlaptop = laptop.Tenlaptop;
            sAnhbia = laptop.Anhbia;
            dDongia = double.Parse(laptop.Giaban.ToString());
            iSoluong = 1;
        }
    }
}