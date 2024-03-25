﻿
using System;

namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate
    {
        public int Id { get; init; }
        public String? Title { get; init; }
        public decimal Price { get; init; }
    }


}
