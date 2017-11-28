using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TrekkingForCharity.Site.Web.Integrations.Doorbell;

namespace TrekkingForCharity.Site.Web.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IDoorbellClient _doorbellClient;

        public ContactModel(IDoorbellClient doorbellClient)
        {
            this._doorbellClient = doorbellClient;
        }

        [BindProperty]
        public Contact Contact { get; set; }
        

        public void OnPost()
        {
            if (this.ModelState.IsValid)
            {
                _doorbellClient.Submit(new DoorbellMessage(Contact.Name, Contact.EmailAddress, Contact.Message));
            }
        }


    }

    public class Contact
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Message { get; set; }
    }

    
}
