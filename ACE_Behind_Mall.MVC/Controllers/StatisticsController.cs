using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACE_Behind_Mall.MVC.Controllers
{
    public class StatisticsController : Controller
    {
        // GET: Statistics
        public ActionResult GoodSaleList()
        {
            return View();
        }
    }
}