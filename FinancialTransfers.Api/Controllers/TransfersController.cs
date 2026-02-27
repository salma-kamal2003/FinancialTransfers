using FinancialTransfers.Application.DTOs;
using FinancialTransfers.Application.Interfaces;
using FinancialTransfers.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransfers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] TransferStatus? status,
    [FromQuery] string? search,
    [FromQuery] DateTime? fromDate,
    [FromQuery] DateTime? toDate,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var transfers = await _transferService.GetAllTransfersAsync(status, search, fromDate, toDate, pageNumber, pageSize);
            return Ok(transfers);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransferDto dto)
        {
            try
            {
                var success = await _transferService.CreateTransferAsync(dto);

                if (success)
                    return Ok(new { Message = "Transfer created successfully." });

                return BadRequest(new { Message = "Failed to create transfer." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            try
            {
                var result = await _transferService.CompleteTransferAsync(id);

                return result
                     ? Ok(new { Message = "Transfer completed successfully." })
                     : BadRequest(new { Message = "Failed to complete transfer." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                var result = await _transferService.CancelTransferAsync(id);

                return result
                     ? Ok(new { Message = "Transfer cancelled successfully." })
                     : BadRequest(new { Message = "Failed to cancel transfer." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transfer = await _transferService.GetTransferByIdAsync(id);

            if (transfer == null)
                return NotFound(new { Message = "Transfer not found." });

            return Ok(transfer);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _transferService.GetTransferSummaryAsync();
            return Ok(summary);
        }

    }
}
