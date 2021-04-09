using System.ComponentModel.DataAnnotations;

namespace backend.Transfers
{
    public record TodoItemUpdateTransfer : TodoItemCreateTransfer
    {
        [Required]
        public int Id { get; init; }
    }
}
