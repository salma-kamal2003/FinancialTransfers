using FinancialTransfers.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Domain.Entities
{
    public class Transfer
    {
        public int Id { get; set; }

        public int FromAccountId { get; set; }
        public Account? FromAccount { get; set; } 

        public int ToAccountId { get; set; }
        public Account? ToAccount { get; set; }

        public decimal Amount { get; set; } 
        public decimal Fees { get; set; } 

        public decimal NetAmount => Amount - Fees;

        public string Currency { get; set; } = string.Empty;
        public TransferStatus Status { get; set; } = TransferStatus.Pending;
        public string? Description { get; set; } 
        public string? ReferenceNumber { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
