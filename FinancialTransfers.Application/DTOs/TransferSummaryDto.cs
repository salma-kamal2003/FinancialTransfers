using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.DTOs
{
    public record TransferSummaryDto
    (
        decimal TotalAmount,    
        int PendingCount,     
        int CompletedCount,    
        int CancelledCount     
    );
}
