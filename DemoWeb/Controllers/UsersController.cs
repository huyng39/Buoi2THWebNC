using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWeb.Models;
using System.Data;
using System.IO;
using System.Data.Entity;

namespace DemoWeb.Controllers
{
    public class UsersController : Controller
    {
        private DBSportStoreEntities database = new DBSportStoreEntities();

        // Chức năng đăng ký tài khoản
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //Hàm kiểm tra cú pháp đăng ký
        [HttpPost]
        public ActionResult Register(Customer cust)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra bỏ trống
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(cust.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(cust.EmailCus))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(cust.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                //Kiểm tra thông tin đã tồn tại trong DB chưa thông qua tên đăng nhập
                var khachhang = database.Customers.FirstOrDefault(k => k.NameCus == cust.NameCus);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");

                if (ModelState.IsValid)
                {
                    database.Customers.Add(cust);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }

        //Chức năng đăng nhập
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Hàm kiểm tra cú pháp và dữ liệu đăng nhập
        [HttpPost]
        public ActionResult Login(Customer cust)
        {
            if (ModelState.IsValid)
            {
                //Kiểm tra bỏ trống
                if (string.IsNullOrEmpty(cust.NameCus))
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được bỏ trống");
                }
                if (string.IsNullOrEmpty(cust.PassCus))
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                }
                //Kiểm tra có tồn tại dữ liệu trong CSDL
                if(ModelState.IsValid)
                {
                    //Tìm khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var khachang = database.Customers.FirstOrDefault(k => k.NameCus == cust.NameCus && k.PassCus == cust.PassCus);
                    if(khachang != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng,bạn đã đăng nhập thành công !";
                        //Lưu vào session
                        Session["TaiKhoan"] = khachang;
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng !";
                    }
                }
            }
            return View();
        }
    }
}