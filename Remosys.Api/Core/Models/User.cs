using System;
using System.Collections.Generic;
using Remosys.Common.Types;

namespace Remosys.Api.Core.Models
{
    public class User : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActiveCode { get; set; }
        public string Avatar { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }

        public bool IsEmailConfirm { get; set; }
        public bool IsMobileConfirm { get; set; }
        public bool IsDelete { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public DateTime ExpiredCode { get; set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }
}