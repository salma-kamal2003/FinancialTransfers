using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.DTOs
{
    public record CreateTransferDto
    {
        public Guid FromAccountId { get; init; }
        public Guid ToAccountId { get; init; }
        public decimal Amount { get; init; }
        public decimal Fees { get; init; }
        public string Currency { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? ReferenceNumber { get; init; }
    }
}
