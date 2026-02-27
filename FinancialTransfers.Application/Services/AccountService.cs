using FinancialTransfers.Application.DTOs;
using FinancialTransfers.Application.Interfaces;
using FinancialTransfers.Domain.Entities;
using FinancialTransfers.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.Services
{
    public class AccountService : IAccountService
    {

        private readonly IApplicationDbContext _context;

        public AccountService(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync(AccountType? type)
        {
            var query = _context.Accounts.AsQueryable();

            if (type.HasValue)
                query = query.Where(a => a.Type == type.Value);
            

            return await query
                .Select(a => new AccountDto(
                    a.Id,
                    a.Name,
                    a.Type.ToString(),
                    a.Balance,
                    a.Currency,
                    a.BankName,
                    a.IBAN
                )).ToListAsync();
        }


        public async Task<AccountDto?> GetAccountByIdAsync(Guid id) 
        {
            var a = await _context.Accounts.FindAsync(id);
            if (a == null) return null;

            return new AccountDto(a.Id, a.Name, a.Type.ToString(), a.Balance, a.Currency, a.BankName, a.IBAN);
        }


        public async Task<bool> CreateAccountAsync(CreateAccountDto dto)
        {
            if (!Enum.IsDefined(typeof(AccountType), dto.Type))
                throw new ArgumentException("Invalid account type ID");

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Balance = dto.Balance,
                Currency = dto.Currency,
                Type = (AccountType)dto.Type, 
                BankName = dto.BankName,
                IBAN = dto.IBAN,
                IsActive = true
            };

            _context.Accounts.Add(account);
            return await _context.SaveChangesAsync() > 0;
        }




    }
}
