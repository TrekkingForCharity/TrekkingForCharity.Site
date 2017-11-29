using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrekkingForCharity.Site.Web.Features.Misc.Contact
{
    public class ContactModel
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
