

namespace Entities.Exceptions
{
    public sealed class BookNotFound : NotFound
    {
        public BookNotFound(int id) : base($"the book with id : {id} could not found. ")
        {
        }
    }
}
