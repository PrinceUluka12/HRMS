using HRMS.Application.Interfaces.Repositories;

namespace HRMS.Application.Interfaces.Services;

public class PayrollService(
    IEmployeeRepository employeeRepository,
    IPayrollRepository payrollRepository,
    ITaxRuleProvider taxRuleProvider) : IPayrollService
{
    public async Task ProcessScheduledPayrollsAsync(CancellationToken cancellationToken)
    {
        var employees = await employeeRepository.GetAllAsync();
        var _taxRules = await taxRuleProvider.GetTaxRulesAsync("CA", "MB");

        foreach (var employee in employees)
        {
            var result = new PayrollResult();
            var payrollList = await payrollRepository.GetByEmployeeIdAsync(employee.Id);
            var payroll =  payrollList.FirstOrDefault();
            // 1. Federal Tax (Progressive Brackets)
            result.FederalTax =  await CalculateProgressiveTax(employee.BaseSalary, "Federal", null);
            // 2. Provincial Tax (Progressive Brackets)
          result.ProvincialTax = await CalculateProgressiveTax(employee.BaseSalary, "Provincial", "MB");

            // 3. CPP (Canada Pension Plan)
            if (!payroll.HasCPPExemption)
            {
                decimal cppRate = _taxRules.First(r => r.Type == "CPP").Rate;
                decimal cppMaxContributableEarnings = 68300; // 2024 CPP max (update yearly)
                decimal cppBaseExemption = 3500; // Basic exemption amount
                decimal pensionableIncome = Math.Max(0, payroll.GrossSalary - cppBaseExemption);
                result.CPP = Math.Min(pensionableIncome, cppMaxContributableEarnings) * cppRate;
            } 
            
            // 4. EI (Employment Insurance)
            if (!payroll.HasEIExemption)
            {
                decimal eiRate = _taxRules.First(r => r.Type == "EI").Rate;
                decimal eiMaxInsurableEarnings = 60300; // 2024 EI max (update yearly)
               result.EI = Math.Min(payroll.GrossSalary, eiMaxInsurableEarnings) * eiRate;
            }
            Console.WriteLine($"Tax deductions calculated for Employee - {employee.Id}");
        }
        
    }

    private async Task<decimal> CalculateProgressiveTax(decimal income, string taxType, string? province)
    {
        var _taxRules = await taxRuleProvider.GetTaxRulesAsync("CA", "MB");

        var applicableRules = _taxRules
            .Where(r => r.Type == taxType && (r.Province == province || r.Province == null))
            .OrderBy(r => r.LowerBound)
            .ToList();

        decimal tax = 0;

        foreach (var rule in applicableRules)
        {
            if (income > rule.LowerBound)
            {
                decimal taxableAmount = Math.Min(
                    (rule.UpperBound ?? income) - rule.LowerBound,
                    income - rule.LowerBound
                );
                tax += taxableAmount * rule.Rate;
            }
        }

        return tax;
    }
    public class PayrollResult
    {
        public decimal FederalTax { get; set; }
        public decimal ProvincialTax { get; set; }
        public decimal CPP { get; set; }
        public decimal EI { get; set; }
        public decimal TotalDeductions => FederalTax + ProvincialTax + CPP + EI;
        public decimal NetPay => GrossIncome - TotalDeductions;
        public decimal GrossIncome { get; set; }
    }
}