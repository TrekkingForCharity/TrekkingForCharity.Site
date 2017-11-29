using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrekkingForCharity.Site.Web.Features.Misc.Privacy
{
    public class PrivacyController : Controller
    {
        [HttpGet]
        [Route("~/terms-and-privacy")]
        public IActionResult TermsAndPrivacy()
        {
            return View();
        }


    }
}