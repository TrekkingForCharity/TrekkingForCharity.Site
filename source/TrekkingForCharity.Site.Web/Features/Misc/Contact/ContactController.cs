using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrekkingForCharity.Site.Web.Integrations.Doorbell;

namespace TrekkingForCharity.Site.Web.Features.Misc.Contact
{
    public class ContactController : Controller
    {
        private readonly IDoorbellClient _doorbellClient;

        public ContactController(IDoorbellClient doorbellClient)
        {
            this._doorbellClient = doorbellClient;
        }

        [HttpGet]
        [Route("~/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("~/contact")]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            if (this.ModelState.IsValid)
            {
                await this._doorbellClient.Submit(new DoorbellMessage(model.Name, model.EmailAddress, model.Message));
            }
            return this.View();
        }
    }
}