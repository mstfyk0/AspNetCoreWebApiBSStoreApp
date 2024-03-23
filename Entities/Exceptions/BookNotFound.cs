

namespace Entities.Exceptions
{
    public sealed class BookNotFound : NotFoundException
    { 
        public BookNotFound(int id) : base($"the book with id : {id} could not found. ")
        {
        }
    }
}
