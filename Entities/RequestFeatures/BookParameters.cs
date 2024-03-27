using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public  class BookParameters : RequestParameters 
    {

        public uint minPrice { get; set; } = 0;
        public uint maxPrice { get; set; } = 1000;
        public bool ValidPriceRange => maxPrice > minPrice;

        public String? SearchTerm { get; set; }
    }
}
