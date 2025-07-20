using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Interfaces.Services;

public interface ITaxRuleProvider
{
    Task<IEnumerable<TaxRule>> GetTaxRulesAsync(string country, string? state = null);
}

public class TaxRuleProvider(
    IConfiguration configuration,
    ILogger<TaxRuleProvider> logger)
    : ITaxRuleProvider
{
    public async Task<IEnumerable<TaxRule>> GetTaxRulesAsync(string country, string? state = null)
    {
        try
        {
            // In a real implementation, this would query a database or external service
            return new List<TaxRule>
            {
                // ===== FEDERAL TAX (2024) =====
                new() { Type = "Federal", LowerBound = 0, UpperBound = 53359, Rate = 0.15m },
                new() { Type = "Federal", LowerBound = 53359, UpperBound = 106717, Rate = 0.205m },
                new() { Type = "Federal", LowerBound = 106717, UpperBound = 165430, Rate = 0.26m },
                new() { Type = "Federal", LowerBound = 165430, UpperBound = 235675, Rate = 0.29m },
                new() { Type = "Federal", LowerBound = 235675, UpperBound = null, Rate = 0.33m },

                // ===== ONTARIO PROVINCIAL TAX (2024) =====
                new() { Type = "Provincial", Province = "ON", LowerBound = 0, UpperBound = 49231, Rate = 0.0505m },
                new() { Type = "Provincial", Province = "ON", LowerBound = 49231, UpperBound = 98463, Rate = 0.0915m },
                new() { Type = "Provincial", Province = "ON", LowerBound = 98463, UpperBound = 150000, Rate = 0.1116m },
                new()
                {
                    Type = "Provincial", Province = "ON", LowerBound = 150000, UpperBound = 220000, Rate = 0.1216m
                },
                new() { Type = "Provincial", Province = "ON", LowerBound = 220000, UpperBound = null, Rate = 0.1316m },

                // ===== QUEBEC PROVINCIAL TAX (2024) =====
                new() { Type = "Provincial", Province = "QC", LowerBound = 0, UpperBound = 49275, Rate = 0.14m },
                new() { Type = "Provincial", Province = "QC", LowerBound = 49275, UpperBound = 98540, Rate = 0.19m },
                new() { Type = "Provincial", Province = "QC", LowerBound = 98540, UpperBound = 119910, Rate = 0.24m },
                new() { Type = "Provincial", Province = "QC", LowerBound = 119910, UpperBound = null, Rate = 0.2575m },


                // ===== MANITOBA (MB) PROVINCIAL TAX (2024) =====
                new() { Type = "Provincial", Province = "MB", LowerBound = 0, UpperBound = 36842, Rate = 0.1080m },
                new() { Type = "Provincial", Province = "MB", LowerBound = 36842, UpperBound = 79625, Rate = 0.1275m },
                new() { Type = "Provincial", Province = "MB", LowerBound = 79625, UpperBound = null, Rate = 0.1740m },
                
                // ===== CPP & EI (2024) =====
                new()
                {
                    Type = "CPP", LowerBound = 0, UpperBound = null, Rate = 0.0595m
                }, // 5.95% (employee contribution)
                new()
                {
                    Type = "EI", LowerBound = 0, UpperBound = null, Rate = 0.0163m
                } // 1.63% (employee contribution)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving tax rules for {Country}, {State}", country, state);
            throw;
        }
    }
}

public class TaxRule
{
    public string Country { get; set; } = "CA";
    public string Province { get; set; } // "AB", "BC", "ON", "QC", "MB".
    public int Year { get; set; } = DateTime.Now.Year;
    public decimal LowerBound { get; set; }
    public decimal? UpperBound { get; set; } // Null = no upper limit
    public decimal Rate { get; set; }
    public string Type { get; set; } // "Federal", "Provincial", "CPP", "EI"
}

public enum TaxRuleType
{
    Federal,
    Provincial,
    CPP,
    EI
}