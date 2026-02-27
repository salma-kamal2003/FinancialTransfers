using FinancialTransfers.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty; 
        public string? BankName { get; set; } 
        public string? IBAN { get; set; }
        public AccountType Type { get; set; } 
        public decimal Balance { get; set; } 
        public string Currency { get; set; } = "SAR"; 
        public string? Branch { get; set; } 
        public bool IsActive { get; set; } = true; 
    }
}
