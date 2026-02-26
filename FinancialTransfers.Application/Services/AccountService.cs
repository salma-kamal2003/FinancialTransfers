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


        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync(string? type)
        {
            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(type) && Enum.TryParse<AccountType>(type, true, out var typeEnum))
            {
                query = query.Where(a => a.Type == typeEnum);
            }

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


        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var a = await _context.Accounts.FindAsync(id);
            if (a == null) return null;

            return new AccountDto(a.Id, a.Name, a.Type.ToString(), a.Balance, a.Currency, a.BankName, a.IBAN);
        }


        public async Task<bool> CreateAccountAsync(CreateAccountDto dto)
        {
            if(!Enum.TryParse<AccountType>(dto.Type, true, out var typeEnum))
                throw new ArgumentException("Invalid account type");

            var account = new Account
            {
                Name = dto.Name,
                Balance = dto.Balance,
                Currency = dto.Currency,
                Type = typeEnum,
                BankName = dto.BankName,
                IBAN = dto.IBAN,
                IsActive = true 
            };

            _context.Accounts.Add(account);
            return await _context.SaveChangesAsync() > 0;
        }

        

        
    }
}
