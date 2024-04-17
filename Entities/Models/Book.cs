
using System;

namespace Entities.Models
{
    public class Book
    {
        public int Id { get; set; }
        public String? Title { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; } 
        public Category Category { get; set; }
    }
}
