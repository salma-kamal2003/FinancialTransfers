using FinancialTransfers.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync(string? type);
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task<bool> CreateAccountAsync(CreateAccountDto dto);
    }
}
