
namespace Entities.DataTransferObjects
{
    public record BookDtoForUpdate
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public decimal Description { get; init; }
    }
}
