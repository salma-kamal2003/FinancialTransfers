using FinancialTransfers.Application.DTOs;
using FinancialTransfers.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync(AccountType? type);
        Task<AccountDto?> GetAccountByIdAsync(Guid id);
        Task<bool> CreateAccountAsync(CreateAccountDto dto);
    }
}
