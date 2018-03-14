using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrekkingForCharity.Site.Web.Features.Profile
{
    public class ProfileController : Controller
    {
        [HttpGet]
        [Route("~/me/treks")]
        public IActionResult MyTreks()
        {
            return View();
        }
    }
}