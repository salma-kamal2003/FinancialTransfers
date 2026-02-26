using FinancialTransfers.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Application.Interfaces
{
    public interface ITransferService
    {
        Task<IEnumerable<TransferDto>> GetAllTransfersAsync(
            string? status,
            string? search,
            DateTime? fromDate,
            DateTime? toDate,
            int pageNumber = 1,
            int pageSize = 10);

        Task<bool> CreateTransferAsync(CreateTransferDto dto);

        Task<bool> CompleteTransferAsync(int id);

        Task<bool> CancelTransferAsync(int id);

        Task<TransferDto?> GetTransferByIdAsync(int id);

        Task<TransferSummaryDto> GetTransferSummaryAsync();
    }
}
