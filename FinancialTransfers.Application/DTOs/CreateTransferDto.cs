using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.DTOs
{
    public record CreateTransferDto
    (
    int FromAccountId,
    int ToAccountId,
    decimal Amount,
    decimal Fees,
    string Currency,
    string? Description,
    string? ReferenceNumber
    );
}
