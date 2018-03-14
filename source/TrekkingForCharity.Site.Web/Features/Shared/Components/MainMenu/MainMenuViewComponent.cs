using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrekkingForCharity.Site.Web.Features.Shared.Components.MainMenu
{
    public class MainMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new MainMenuModel();
            model.IsAuthenticated = this.HttpContext.User.Identity.IsAuthenticated;
            return View(model);
        }
    }
}
