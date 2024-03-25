
using System;

namespace Entities.DataTransferObjects
{
    // eğer get set şeklinde tanımlanmasaydı [Serializable] koduna ihtiyaç vardı
    //çünkü alanları result kısmında serializable edemiyor.
    //[Serializable]
    public record BookDto
    {
        public int Id { get; init; }
        public String? Title { get; init; }
        public decimal Price { get; init; }
    }


}
