using Microsoft.AspNetCore.Mvc;

namespace TrekkingForCharity.Site.Web.Features.Trek.Details
{
    public class DetailsController : Controller
    {
        [HttpGet]
        [Route("~/trek")]
        public IActionResult Details()
        {
            return this.View();
        }
    }
}