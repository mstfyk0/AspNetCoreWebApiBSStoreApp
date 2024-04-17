using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public record CategoryDto
    {
        public int CategoryId { get; init; }
        public string? CategoryName { get; init; }
    }
}
