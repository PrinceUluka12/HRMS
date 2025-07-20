using AutoMapper;
using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Payroll.Commands.AddPayrollItem;

public record AddPayrollItemCommand(
    Guid PayrollId,
    string Description,
    decimal Amount,
    PayrollItemType Type,
    string? ReferenceId = null) : IRequest<PayrollItemDto>;

public class AddPayrollItemCommandHandler(
    IPayrollRepository payrollRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<AddPayrollItemCommand, PayrollItemDto>
{
    public async Task<PayrollItemDto> Handle(
        AddPayrollItemCommand request,
        CancellationToken cancellationToken)
    {
        var payroll = await payrollRepository.GetByIdAsyncIncludeRelationship(request.PayrollId);
        if (payroll == null)
        {
            throw new NotFoundException(nameof(Payroll), request.PayrollId);
        }

        var payrollItem = new PayrollItem(
            request.PayrollId,
            request.Description,
            request.Amount,
            request.Type,
            request.ReferenceId);

        payroll.AddItem(payrollItem);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<PayrollItemDto>(payrollItem);
    }
}