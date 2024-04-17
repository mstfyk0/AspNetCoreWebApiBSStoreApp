using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public abstract record CategoryDtoForManipulation
    {
        [Required(ErrorMessage = "Category Name is required field.")]
        [MinLength(2, ErrorMessage = " Title must consist of at least 2 character.")]
        [MaxLength(50, ErrorMessage = " Title must consist of at maximum 50 character.")]
        public String CategoryName { get; init; }
    }
}
