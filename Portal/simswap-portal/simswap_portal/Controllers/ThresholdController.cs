using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using simswap_portal.Models;

namespace simswap_portal.Controllers
{

    public class ThresholdController : Controller
    {
        DBHelper _dbhelper;

        public ThresholdController()
        {
            _dbhelper = new DBHelper();

        }

        public ActionResult Index()
        {
            var currentthreshold = _dbhelper.CurrentThreshold();
            ViewBag.threshold = currentthreshold;
            return View();
        }


    }
}
