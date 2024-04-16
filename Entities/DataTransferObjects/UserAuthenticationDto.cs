using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public record UserAuthenticationDto
    {
        [Required(ErrorMessage ="UserName is required.")]
        public string  UserName { get; init; }
        //public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; init; }
        //public string? Password { get; init; }


    }
}
