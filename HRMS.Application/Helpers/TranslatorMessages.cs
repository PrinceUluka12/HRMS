using HRMS.Application.Dtos;

namespace HRMS.Application.Helpers;

public static class TranslatorMessages
{
    public static class EmployeeMessages
    {
        public static TranslatorMessageDto Employee_NotFound_with_id(Guid id) =>
            new(nameof(Employee_NotFound_with_id), [id.ToString()]);

        public static TranslatorMessageDto Employee_NotFound_with_AzureEmployeeId(Guid id) =>
            new(nameof(Employee_NotFound_with_AzureEmployeeId), [id.ToString()]);

        public static TranslatorMessageDto No_Employees_Found(string error) =>
            new(nameof(No_Employees_Found),[error]);
    }

    public static class DepartmentMessages
    {
        public static TranslatorMessageDto Department_NotFound_with_id(Guid id) =>
            new(nameof(Department_NotFound_with_id), [id.ToString()]);
    }

    public static class GeneralMessages
    {
        public static TranslatorMessageDto Unexpected_Error(string error) =>
            new(nameof(Unexpected_Error),[error]);
    }

    // Add more categories here (e.g. PayrollMessages, LeaveMessages, etc.)
}