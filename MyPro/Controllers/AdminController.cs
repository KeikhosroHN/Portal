using MyPro.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPro.ViewModels;
using System.Globalization;

namespace MyPro.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext database = new ApplicationDbContext();

        string PersianDate(DateTime DateTime1)
        {

            PersianCalendar PersianCalendar1 = new PersianCalendar();
            return string.Format(@"{0}/{1}/{2}",
                         PersianCalendar1.GetYear(DateTime1),
                         PersianCalendar1.GetMonth(DateTime1),
                         PersianCalendar1.GetDayOfMonth(DateTime1));
        }

        string dat, sal, mah, roz, ret;
        public string milad(string s)
        {
            dat = s;
            sal = dat.Substring(0, 4);
            mah = dat.Substring(5, 2);
            roz = dat.Substring(8, 2);
            PersianCalendar PC = new PersianCalendar();
            ret = PC.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0).ToString();
            return ret.ToString();
        }



        // GET: Admin
        //------Login-Admin-Actions-----\\

        public ActionResult Login()
        {
            if (Session["Username"] != null)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Login(Admin usr)
        {
            var u = database.Admins.Where(m => m.Username == usr.Username).SingleOrDefault();
            if (u != null)
            {
                u = null;
                u = database.Admins.Where(m => m.Username == usr.Username && m.Password == usr.Password).SingleOrDefault();
                if (u != null)
                {
                    Session["Fname"] = u.FirstName.ToString();
                    Session["Username"] = u.Username.ToString();
                    Session["Access"] = u.Access.ToString();
                    Session["Login"] = "True";
                    Session["On"] = true;
                }
                else
                {
                    ViewBag.Check = "رمز عبور اشتباه است";
                    Session["Login"] = "False";
                    return RedirectToAction("Login", "Admin");
                }
            }
            if (u == null)
            {
                Session["Login"] = "False";
                ViewBag.Check = "نام کاربری وجود ندارد";
            }

            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Logout(Admin usr)
        {
            Session["Username"] = null;
            Session["Access"] = null;
            Session["Fname"] = null;
            Session["Login"] = null;
            Session["On"] = null;

            return RedirectToAction("Index", "Home");
        }

        //------List-Actions-----\\
        //---Regions-List
        public ActionResult Regions(string search, string Combo, string Export)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();

            ViewBag.DateNow = (pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now));
            ViewBag.AdminNum = database.Admins.Count();
            ViewBag.ProjNum = database.Pers.Count();

            string ac = (string)Session["Access"];

            List<PerModel> perlist = new List<PerModel>();
            if ((string)Session["Access"] != null)
            {
                var per = database.Pers.OrderByDescending(x=>x.ID).ToList();

                if ((string)Session["Access"] != "MainAdmin")
                {
                    per = database.Pers.OrderByDescending(x => x.ID).Where(x => x.RegionName == ac).ToList();
                }
                //---MainAdmin
                if (!String.IsNullOrWhiteSpace(search))
                {
                    if (Combo == "RN")
                        per = per.Where(x => x.RegionName == search).ToList();

                    if (Combo == "OD")
                        per = per.Where(x => x.OD.Contains(search)).ToList();

                    if (Combo == "NM")
                        per = per.Where(x => x.NMD.Contains(search)).ToList();

                    if (Combo == "MB")
                        per = per.Where(x => x.MBD.Contains(search)).ToList();

                    if (Combo == "TSH")
                        per = per.Where(x => x.TSHD == search).ToList();

                    if (Combo == "TP")
                        per = per.Where(x => x.TPD == search).ToList();

                    if (Combo == "RVSB")
                        per = per.Where(x => x.RVSBD == search).ToList();

                    if (Combo == "TA")
                        per = per.Where(x => x.TA == search).ToList();

                    if (Combo == "TF")
                        per = per.Where(x => x.TF == Int32.Parse(search)).ToList();

                    if (Combo == "DG")
                        per = per.Where(x => x.DG == Int32.Parse(search)).ToList();
                }

                foreach (var item in per)
                {
                    PerModel p = new PerModel();
                    p.ID = item.ID;
                    p.RegionName = item.RegionName;
                    p.OD = item.OD;
                    p.NMD = item.NMD;
                    p.MBD = item.MBD;
                    p.TSHD = item.TSHD;
                    p.TPD = item.TPD;
                    p.RVSBD = item.RVSBD;
                    p.TA = item.TA;
                    p.TF = item.TF;
                    p.DG = item.DG;
                    perlist.Add(p);
                }



                //---Export
                if (Export == "1")
                {
                    //var admins = database.Admins.ToList();

                    ExcelPackage pck = new ExcelPackage();
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                    ws.Cells["A1"].Value = "comm";
                    ws.Cells["B1"].Value = "com1";

                    ws.Cells["A2"].Value = "گزارش";
                    ws.Cells["B2"].Value = "لیست کاربران منطقه";

                    ws.Cells["A3"].Value = "تاریخ";
                    ws.Cells["B3"].Value = string.Format("{0:dd MMM yyyy} at {0:H mm tt})", pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now));

                    ws.Cells["A6"].Value = "منطقه";
                    ws.Cells["B6"].Value = "عنوان دوره";
                    ws.Cells["C6"].Value = "نام مدیر دوره";
                    ws.Cells["D6"].Value = "مکان برگذاری دوره";
                    ws.Cells["E6"].Value = "تاریخ شروع";
                    ws.Cells["F6"].Value = "تاریخ پایان";
                    ws.Cells["G6"].Value = "روز و ساعت برگذاری";
                    ws.Cells["H6"].Value = "تاریخ آزمون";
                    ws.Cells["I6"].Value = "تعداد فراگیر";
                    ws.Cells["J6"].Value = "درآمد گواهینامه";

                    int _i = 7;
                    foreach (var item in perlist)
                    {
                        ws.Row(_i).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        if (_i % 2 == 0)
                            ws.Row(_i).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("GreenYellow")));

                        else
                            ws.Row(_i).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("Pink")));

                        ws.Cells[string.Format("A{0}", _i)].Value = item.RegionName;
                        ws.Cells[string.Format("B{0}", _i)].Value = item.OD;
                        ws.Cells[string.Format("C{0}", _i)].Value = item.NMD;
                        ws.Cells[string.Format("D{0}", _i)].Value = item.MBD;
                        ws.Cells[string.Format("E{0}", _i)].Value = item.TSHD;
                        ws.Cells[string.Format("F{0}", _i)].Value = item.TPD;
                        ws.Cells[string.Format("G{0}", _i)].Value = item.RVSBD;
                        ws.Cells[string.Format("H{0}", _i)].Value = item.TA;
                        ws.Cells[string.Format("I{0}", _i)].Value = item.TF;
                        ws.Cells[string.Format("J{0}", _i)].Value = item.DG;

                        _i++;
                    }
                    ws.Cells["A:AZ"].AutoFitColumns();
                    Response.Clear();
                    Response.ContentType = "applicaton/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-disposition", "attachment: filename=" + "ExportReport.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();
                    string filename2 = AppDomain.CurrentDomain.BaseDirectory + "خروجی اکسل از جدول دوره های مناطق هلال احمر تهران" + DateTime.Now.Millisecond.ToString() + ".xlsx";
                }


                return View(perlist);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        //---Admins-List
        public ActionResult AdminsList(string search, string Combo)
        {
            ViewBag.AdminNum = database.Admins.Count();
            ViewBag.ProjNum = database.Pers.Count();

            if ((string)Session["Access"] == "MainAdmin")
            {
                var admins = database.Admins.OrderByDescending(x => x.Access).ToList();
                if (!String.IsNullOrWhiteSpace(search))
                {
                    if (Combo == "Username")
                        admins = database.Admins.Where(x => x.Username.Contains(search)).ToList();

                    if (Combo == "Password")
                        admins = database.Admins.Where(x => x.Password.Contains(search)).ToList();

                    if (Combo == "Number")
                        admins = database.Admins.Where(x => x.Phone.Contains(search)).ToList();

                    if (Combo == "Email")
                        admins = database.Admins.Where(x => x.Email.Contains(search)).ToList();

                    if (Combo == "Access")
                        admins = database.Admins.Where(x => x.Access == search).ToList();
                }
                return View(admins);
            }
            else
                return RedirectToAction("Login", "Admin");
        }

        //------Add-Actions-----\\
        //---Add-Periud
        public ActionResult AddPer()
        {
            if ((string)Session["Username"] != null && (string)Session["Access"] != null)
            {
                return View();
            }

            else
                return RedirectToAction("Login", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPer(PerModel region, string acc, string TSHD, string TPD, string RVSBD, string TA)
        {
            if ((string)Session["Access"] != null)
            {
                Per p = new Per();
                if ((string)Session["Access"] == "MainAdmin")
                    p.RegionName = region.RegionName;

                else
                    p.RegionName = acc;

                p.OD = region.OD;
                p.NMD = region.NMD;
                p.MBD = region.MBD;
                p.TSHD = TSHD;
                p.TPD = TPD;
                p.RVSBD = RVSBD;
                p.TA = TA;
                p.TF = region.TF;
                p.DG = region.DG;
                database.Pers.Add(p);
                database.SaveChanges();
                return RedirectToAction("Regions", "Admin");
            }
            else
            {
                Session["Save"] = "False";
                return Content("ثبت نشد");
            }
        }

        //---Add-Admin
        public ActionResult AddAdmin()
        {
            if ((string)Session["Access"] == "MainAdmin")
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(Admin a)
        {
            string usr = "";
            if ((string)Session["Access"] == "MainAdmin")
            {
                if (ModelState.IsValid)
                {
                    Admin u = new Admin();
                    u.FirstName = a.FirstName;
                    u.LastName = a.LastName;
                    usr = a.Username;
                    usr.Trim();
                    u.Username = usr;
                    u.Password = a.Password;
                    u.Phone = a.Phone;
                    u.Email = a.Email;
                    u.Access = a.Access;
                    database.Admins.Add(u);
                    database.SaveChanges();
                    return RedirectToAction("AdminsList", "Admin");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //------Edit-Actions-----\\
        //---Edit-Periud
        public ActionResult EditPer(int ID)
        {
            PerModel pm = new PerModel();
            var r = database.Pers.Find(ID);
            if (r.RegionName == (string)Session["Access"] || (string)Session["Access"] == "MainAdmin")
            {
                pm.RegionName = r.RegionName;
                pm.ID = r.ID;
                pm.OD = r.OD;
                pm.NMD = r.NMD;
                pm.MBD = r.MBD;
                pm.TSHD = r.TSHD;
                pm.TPD = r.TPD;
                pm.RVSBD = r.RVSBD;
                pm.TA = r.TA;
                pm.TF = r.TF;
                pm.DG = r.DG;
                return View(pm);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPer(PerModel y, int ID, string acc, string TSHD, string TPD, string RVSBD, string TA)
        {
            if ((string)Session["Access"] != null)
            {
                var f = database.Pers.Find(ID);

                if (f.RegionName == (string)Session["Access"] || (string)Session["Access"] == "MainAdmin")
                {
                    f.RegionName = y.RegionName;

                    if ((string)Session["Access"] != "MainAdmin")
                    {
                        f.RegionName = Session["Access"].ToString();
                    }

                    f.OD = y.OD;
                    f.NMD = y.NMD;
                    f.MBD = y.MBD;
                    f.TSHD = TSHD;
                    f.TPD = TPD;
                    f.RVSBD = RVSBD;
                    f.TA = TA;
                    f.TF = y.TF;
                    f.DG = y.DG;
                    database.SaveChanges();

                    return RedirectToAction("Regions", "Admin");
                }
                else
                    return RedirectToAction("Regions", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //---Edit-Admin
        public ActionResult EditAdmin(string username)
        {
            var e = database.Admins.Find(username);
            if ((string)Session["Access"] == "MainAdmin")
            {
                return View(e);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(Admin y, string User)
        {

            if ((string)Session["Access"] == "MainAdmin")
            {
                var f = database.Admins.SingleOrDefault(x => x.Username == User);
                f.Username = y.Username;
                f.FirstName = y.FirstName;
                f.LastName = y.LastName;
                f.Username = y.Username;
                f.Password = y.Password;
                f.Email = y.Email;
                f.Phone = y.Phone;
                f.Access = y.Access;

                database.SaveChanges();
                return RedirectToAction("AdminsList", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //------Delete-Actions-----\\
        //---Delete-Periud
        public ActionResult DeletePer(int ID)
        {
            if ((string)Session["Access"] != null)
            {
                var r = database.Pers.Find(ID);
                database.Pers.Remove(r);
                database.SaveChanges();
                return RedirectToAction("Regions", "Admin");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult DeleteAdmin(string username)
        {
            if ((string)Session["Access"] == "MainAdmin")
            {
                var r = database.Admins.Find(username);
                database.Admins.Remove(r);
                database.SaveChanges();
                return RedirectToAction("AdminsList", "Admin");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        //------Export-Excel-Actions-----\\
        //---Admins-Excel-Export
        public ActionResult ExportEx(string search, string Combo)
        {
            if ((string)Session["Username"] != null)
            {
                var admins = database.Admins.ToList();
                if (!String.IsNullOrWhiteSpace(search))
                {
                    if (Combo == "Username")
                        admins = database.Admins.Where(x => x.Username.Contains(search)).ToList();

                    if (Combo == "Password")
                        admins = database.Admins.Where(x => x.Password.Contains(search)).ToList();

                    if (Combo == "Number")
                        admins = database.Admins.Where(x => x.Phone.Contains(search)).ToList();

                    if (Combo == "Email")
                        admins = database.Admins.Where(x => x.Email.Contains(search)).ToList();

                    if (Combo == "Access")
                        admins = database.Admins.Where(x => x.Access.Contains(search)).ToList();
                }

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                ws.Cells["A1"].Value = "comm";
                ws.Cells["B1"].Value = "com1";

                ws.Cells["A2"].Value = "گزارش";
                ws.Cells["B2"].Value = "لیست کاربران منطقه";

                ws.Cells["A3"].Value = "تاریخ";
                ws.Cells["B3"].Value = string.Format("{0:dd MMM yyyy} at {0:H mm tt})", DateTimeOffset.Now);

                ws.Cells["A6"].Value = "Username";
                ws.Cells["B6"].Value = "Name";
                ws.Cells["C6"].Value = "Email";
                ws.Cells["D6"].Value = "Mobile";
                int _i = 7;
                foreach (var item in admins)
                {
                    ws.Row(_i).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (_i % 2 == 0)
                        ws.Row(_i).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("GreenYellow")));

                    else
                        ws.Row(_i).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("Pink")));

                    ws.Cells[string.Format("A{0}", _i)].Value = item.Username;
                    ws.Cells[string.Format("B{0}", _i)].Value = item.FirstName;
                    ws.Cells[string.Format("C{0}", _i)].Value = item.Email;
                    ws.Cells[string.Format("D{0}", _i)].Value = item.Phone;

                    _i++;
                }
                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "applicaton/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-disposition", "attachment: filename=" + "ExportReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
                return View(admins);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult RegionExport(string search, string Combo)
        {
            if ((string)Session["Username"] != null)
            {
                var admins = database.Admins.ToList();
                if (!String.IsNullOrWhiteSpace(search))
                {
                    if (Combo == "Username")
                        admins = database.Admins.Where(x => x.Username.Contains(search)).ToList();

                    if (Combo == "Password")
                        admins = database.Admins.Where(x => x.Password.Contains(search)).ToList();

                    if (Combo == "Number")
                        admins = database.Admins.Where(x => x.Phone.Contains(search)).ToList();

                    if (Combo == "Email")
                        admins = database.Admins.Where(x => x.Email.Contains(search)).ToList();

                    if (Combo == "Access")
                        admins = database.Admins.Where(x => x.Access.Contains(search)).ToList();
                }

                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

                ws.Cells["A1"].Value = "comm";
                ws.Cells["B1"].Value = "com1";

                ws.Cells["A2"].Value = "گزارش";
                ws.Cells["B2"].Value = "لیست کاربران منطقه";

                ws.Cells["A3"].Value = "تاریخ";
                ws.Cells["B3"].Value = string.Format("{0:dd MMM yyyy} at {0:H mm tt})", DateTimeOffset.Now);

                ws.Cells["A6"].Value = "Username";
                ws.Cells["B6"].Value = "Name";
                ws.Cells["C6"].Value = "Email";
                ws.Cells["D6"].Value = "Mobile";
                int _i = 7;
                foreach (var item in admins)
                {
                    ws.Row(_i).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    if (_i % 2 == 0)
                        ws.Row(_i).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("GreenYellow")));

                    else
                    {
                        ws.Row(_i).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("Pink")));
                    }

                    ws.Cells[string.Format("A{0}", _i)].Value = item.Username;
                    ws.Cells[string.Format("B{0}", _i)].Value = item.FirstName;
                    ws.Cells[string.Format("C{0}", _i)].Value = item.Email;
                    ws.Cells[string.Format("D{0}", _i)].Value = item.Phone;

                    _i++;
                }
                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();
                Response.ContentType = "applicaton/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-disposition", "attachment: filename=" + "ExportReport.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
                return View(admins);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}