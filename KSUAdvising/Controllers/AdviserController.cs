using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSUAdvising.Controllers
{
    public class AdviserController : Controller
    {
        //
        // GET: /Advisor/
        //AdvisingDBContext context = new AdvisingDBContext();
        public ActionResult Index()
        {
            //Adviser firstAdviser = new Adviser();
            //firstAdviser.AdvisorID = 123;
            //firstAdviser.FirstName = "Joe";
            //firstAdviser.LastName = "Dister";
            //context.Advisers.Add(firstAdviser);

            //context.SaveChanges();

            //List<Adviser> advisers = context.Advisers.ToList();
            return View();
        }

    }
}
