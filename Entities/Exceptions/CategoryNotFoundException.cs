﻿

namespace Entities.Exceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(int id) : base($"the category with id : {id} could not found. ")
        {
        }
    }
}
