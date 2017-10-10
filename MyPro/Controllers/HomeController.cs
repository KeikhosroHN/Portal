using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPro.ViewModels;
using MyPro.Models;

namespace MyPro.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext database = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.AdminsList = database.Admins.Count();
            ViewBag.RegionsList = database.Pers.Count();

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            ViewBag.date = (pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now));

            return View();
        }


        public ActionResult Contact()
        {
            if ((string)Session["Access"] == "MainAdmin")
            {
                return View();
            }
            else
               return RedirectToAction("Index", "Home");
        }

    }
}