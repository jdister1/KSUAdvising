using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSUAdvising.Controllers
{
    public class AdministrativeAssistantController : Controller
    {
        //
        // GET: /AdministrativeAssistant/
        AdvisingDBContext context = new AdvisingDBContext();

        public ActionResult Index()
        {
            //gets anyone already in the walkin queue or advisers available for walkin
            AdministrativeAssistantViewModel adAssistantVm = new AdministrativeAssistantViewModel();
            adAssistantVm.currentAdviserWalkinQueueFlashline = context.Advisers.Where(a => a.WalkinAdviser == true).Select(a=>a.FlashlineID).ToList();
            adAssistantVm.currentWalkinQueueFlashline = context.WalkinQueue.Select(w=>w.FlashlineID).ToList();

            return View(adAssistantVm);
        }

    }
}
