using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrekkingForCharity.Site.Web.Features.Search
{
    public class SearchController : Controller
    {
        [HttpGet]
        [Route("~/explore")]
        public IActionResult Search()
        {
            return this.View();
        }
    }
}
