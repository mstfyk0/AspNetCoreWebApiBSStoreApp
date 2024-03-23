

namespace Entities.Exceptions
{
    public sealed class BookNotFoundException : NotFoundException
    { 
        public BookNotFoundException(int id) : base($"the book with id : {id} could not found. ")
        {
        }
    }
}
