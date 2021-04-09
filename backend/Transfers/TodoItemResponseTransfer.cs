using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Transfers
{
    public record TodoItemResponseTransfer : TodoItemUpdateTransfer
    {
        [Required]
        public DateTime CreatedDate { get; init; }
    }
}
