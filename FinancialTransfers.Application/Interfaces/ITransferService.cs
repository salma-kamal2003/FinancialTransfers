using FinancialTransfers.Application.DTOs;
using FinancialTransfers.Domain.Enums;
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
            TransferStatus? status,
            string? search,
            DateTime? fromDate,
            DateTime? toDate,
            int pageNumber = 1,
            int pageSize = 10);

        Task<bool> CreateTransferAsync(CreateTransferDto dto);

        Task<bool> CompleteTransferAsync(Guid id);

        Task<bool> CancelTransferAsync(Guid id);

        Task<TransferDto?> GetTransferByIdAsync(Guid id);

        Task<TransferSummaryDto> GetTransferSummaryAsync();
    }
}
