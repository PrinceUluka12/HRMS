using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Payroll.Commands.AddPayrollItem;

public record AddPayrollItemCommand(
    Guid PayrollId,
    string Description,
    decimal Amount,
    PayrollItemType Type,
    string? ReferenceId = null) : IRequest<BaseResult<PayrollItemDto>>;

public class AddPayrollItemCommandHandler(
    IPayrollRepository payrollRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ITranslator translator,
    ILogger<AddPayrollItemCommandHandler> logger)
    : IRequestHandler<AddPayrollItemCommand, BaseResult<PayrollItemDto>>
{
    public async Task<BaseResult<PayrollItemDto>> Handle(
        AddPayrollItemCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var payroll = await payrollRepository.GetByIdWithIncludesAsync(request.PayrollId);
            if (payroll == null)
            {
                return BaseResult<PayrollItemDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Payroll with ID {request.PayrollId} was not found."),
                    nameof(request.PayrollId)
                ));
            }

            var payrollItem = new PayrollItem(
                request.PayrollId,
                request.Description,
                request.Amount,
                request.Type,
                request.ReferenceId);

            payroll.AddItem(payrollItem);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = mapper.Map<PayrollItemDto>(payrollItem);
            return BaseResult<PayrollItemDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding payroll item to payroll ID {PayrollId}", request.PayrollId);
            return BaseResult<PayrollItemDto>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString("An unexpected error occurred while adding the payroll item.")
            ));
        }
    }
}
