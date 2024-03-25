
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate : BookDtoForManipulation
    {
        [Required]
        public int Id { get; init; }
        public String? Title { get; init; }
        public decimal Price { get; init; }
    }


}
