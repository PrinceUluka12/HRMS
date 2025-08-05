using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Interfaces;

namespace HRMS.Application.Interfaces.Services;

public class DefaultStageFactory : IDefaultStageFactory
{
    public List<OnboardingStage> Create()
    {
        var now = DateTime.Now;

        var documentationStage = new OnboardingStage("Documentation", now.AddDays(3));
        documentationStage.AddTask(new OnboardingTask(
            "Submit ID Proof",
            "Upload government-issued ID.",
            "HR",
            now.AddDays(2)));

        documentationStage.AddTask(new OnboardingTask(
            "Sign Employment Contract",
            "Electronically sign the onboarding contract.",
            "HR",
            now.AddDays(3)));

        var itSetupStage = new OnboardingStage("IT Setup", now.AddDays(5));
        itSetupStage.AddTask(new OnboardingTask(
            "Provision Laptop",
            "Assign and configure a work laptop.",
            "IT",
            now.AddDays(4)));

        itSetupStage.AddTask(new OnboardingTask(
            "Create Email Account",
            "Setup corporate email address.",
            "IT",
            now.AddDays(4)));

        var trainingStage = new OnboardingStage("Training", now.AddDays(10));
        trainingStage.AddTask(new OnboardingTask(
            "HR Policy Orientation",
            "Attend HR policy and compliance training.",
            "HR",
            now.AddDays(6)));

        trainingStage.AddTask(new OnboardingTask(
            "Product Overview",
            "Attend product training session.",
            "Manager",
            now.AddDays(8)));

        return new List<OnboardingStage> { documentationStage, itSetupStage, trainingStage };
    }
}