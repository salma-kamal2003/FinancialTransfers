using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.DTOs
{
    public class CreateAccountDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "SAR";
        public string Type { get; set; } = "Treasury"; 
        public string? BankName { get; set; }
        public string? IBAN { get; set; }
    }
}
