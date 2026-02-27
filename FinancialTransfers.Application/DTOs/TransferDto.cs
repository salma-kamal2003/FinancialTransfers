using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.DTOs
{
    public record TransferDto
    (
    Guid Id,
    string FromAccountName,  
    string ToAccountName,    
    decimal Amount,
    decimal Fees,
    decimal NetAmount,
    string Currency,
    string Status,           
    string? Description,
    DateTime CreatedAt
    );
}
