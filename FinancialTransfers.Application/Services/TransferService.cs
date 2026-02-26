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
    public class TransferService : ITransferService
    {
        private readonly IApplicationDbContext _context;

        public TransferService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTransferAsync(CreateTransferDto dto)
        {
            if (dto.FromAccountId == dto.ToAccountId)
                throw new Exception("From and To account cannot be the same.");

            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero.");

            if (dto.Fees < 0)
                throw new Exception("Fees cannot be negative.");

            var fromAccount = await _context.Accounts.FindAsync(dto.FromAccountId);

            var toAccount = await _context.Accounts.FindAsync(dto.ToAccountId);

            if (fromAccount == null || toAccount == null)
                throw new Exception("From account not found.");

            if(dto.Fees >= dto.Amount)
                throw new Exception("Fees cannot be greater than or equal to the amount.");

            if (fromAccount.Currency != toAccount.Currency)
                throw new Exception("Currency mismatch between accounts.");

            var transfer = new Transfer
            {
                FromAccountId = dto.FromAccountId,
                ToAccountId = dto.ToAccountId,
                Amount = dto.Amount,
                Fees = dto.Fees,
                Currency = dto.Currency,
                Description = dto.Description,
                ReferenceNumber = dto.ReferenceNumber,
                Status = TransferStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Transfers.Add(transfer);

            await _context.SaveChangesAsync();

            return true;

        }


        public async Task<TransferDto?> GetTransferByIdAsync(int id)
        {
            var transfer = await _context.Transfers
                         .Include(t => t.FromAccount)
                         .Include(t => t.ToAccount)
                            .FirstOrDefaultAsync(t => t.Id == id);

            if (transfer == null)
                return null;

            return new TransferDto(
                transfer.Id,
                transfer.FromAccount!.Name,
                transfer.ToAccount!.Name,
                transfer.Amount,
                transfer.Fees,
                transfer.NetAmount,
                transfer.Currency,
                transfer.Status.ToString(),
                transfer.Description,
                transfer.CreatedAt
                );
        }


        public async Task<bool> CompleteTransferAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var transfer = await _context.Transfers
                    .Include(t => t.FromAccount)
                    .Include(t => t.ToAccount)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (transfer == null || transfer.Status != TransferStatus.Pending)
                    throw new Exception("Transfer not valid for completion.");

                if (transfer.FromAccount!.Balance < transfer.Amount)
                    throw new Exception("Insufficient funds.");

                transfer.FromAccount.Balance -= transfer.Amount;
                transfer.ToAccount!.Balance += transfer.NetAmount;
                transfer.Status = TransferStatus.Completed;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> CancelTransferAsync(int id)
        {
            var transfer = await _context.Transfers.FindAsync(id);

            if (transfer == null)
                throw new Exception("Transfer not found.");

            if (transfer.Status != TransferStatus.Pending)
                throw new Exception("Only pending transfers can be cancelled.");

            transfer.Status = TransferStatus.Cancelled;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TransferDto>> GetAllTransfersAsync(string? status, string? search, DateTime? fromDate, DateTime? toDate, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Transfers
                         .Include(t => t.FromAccount)
                         .Include(t => t.ToAccount)
                         .AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TransferStatus>(status, true, out var statusEnum))
                query = query.Where(t => t.Status == statusEnum);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(t => t.Description!.Contains(search) || t.ReferenceNumber!.Contains(search));

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
            {
                var endOfDay = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(t => t.CreatedAt <= endOfDay);
            }

            return await query.OrderByDescending(t => t.CreatedAt)
                .Skip((pageNumber - 1) * pageSize) 
                .Take(pageSize)                   
                .Select(t => new TransferDto(
                    t.Id,
                    t.FromAccount!.Name,
                    t.ToAccount!.Name,
                    t.Amount,
                    t.Fees,
                    t.NetAmount,
                    t.Currency,
                    t.Status.ToString(),
                    t.Description,
                    t.CreatedAt
                )).ToListAsync();
        }

        public async Task<TransferSummaryDto> GetTransferSummaryAsync()
        {
            var query = _context.Transfers.AsQueryable();

            var totalAmount = await query
                .Where(t => t.Status == TransferStatus.Completed)
                .SumAsync(t => t.Amount);

            var pendingCount = await query.CountAsync(t => t.Status == TransferStatus.Pending);
            var completedCount = await query.CountAsync(t => t.Status == TransferStatus.Completed);
            var cancelledCount = await query.CountAsync(t => t.Status == TransferStatus.Cancelled);

            return new TransferSummaryDto(totalAmount, pendingCount, completedCount, cancelledCount);
        }
    }
}
