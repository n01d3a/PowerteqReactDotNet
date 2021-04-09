using System.ComponentModel.DataAnnotations;

namespace backend.Transfers
{
    public record TodoItemCreateTransfer
    {
        [Required]
        public string Title { get; init; }
        public string Description { get; init; }
    }
}
