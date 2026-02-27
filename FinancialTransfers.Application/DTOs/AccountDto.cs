using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.DTOs
{
    public record AccountDto
    (
    Guid Id,
    string Name,
    string Type,
    decimal Balance,
    string Currency,
    string? BankName,
    string? IBAN
    );
}
