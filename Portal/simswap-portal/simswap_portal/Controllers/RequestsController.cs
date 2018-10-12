using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace simswap_portal.Controllers
{
    public class RequestsController : Controller
    {
        public ActionResult AllRequests()
        {
            return View();
        }

        public ActionResult RequestsByUser(int userid, string username)
        {
            if(userid == 0 || username == null)
            {
                ViewBag.UserId = userid;
                ViewBag.UserName = "";
            } else
            {
                ViewBag.UserId = userid;
                ViewBag.UserName = username;
            }
           
            return View();
        }

        public ActionResult RequestDetails(string msisdn, int id)
        {
            if (msisdn == null || id == 0)
            {
                ViewBag.Msisdn = 0;
            }
            else
            {
                ViewBag.Msisdn = msisdn;
                ViewBag.Id = id;
            }
            return View();
        }

        public ActionResult EditRequest(string msisdn, int id, string datesubmitted)
        {
            
            if (msisdn == null || id == 0 || datesubmitted == null)
            {
                ViewBag.Msisdn = 0;
                ViewBag.Id = id;
                ViewBag.DateSubmitted = "Mon Jan 01 2001";
            }
            else
            {
                ViewBag.Msisdn = msisdn;
                ViewBag.Id = id;
                ViewBag.DateSubmitted = datesubmitted.Substring(0, 15);
            }
            return View();
        }

        public ActionResult PendingRequests()
        {
            return View();
        }

        public ActionResult FulfilledRequests()
        {
            ViewBag.Title = "Fulfilled Requests";
            return View();
        }
    }
}