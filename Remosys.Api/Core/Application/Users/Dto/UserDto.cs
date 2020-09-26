using System;
using System.Collections.Generic;

namespace Remosys.Api.Core.Application.Users.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActiveCode { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }

        public string Avatar { get; set; }

        public bool IsEmailConfirm { get; set; }
        public bool IsMobileConfirm { get; set; }

        public DateTime RegisterDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}