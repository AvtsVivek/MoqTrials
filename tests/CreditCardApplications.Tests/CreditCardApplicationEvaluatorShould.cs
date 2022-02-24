namespace CreditCardApplications.Tests;
using Xunit;
using Moq;

public class CreditCardApplicationEvaluatorShould
{
    [Fact]
    public void AcceptHighIncomeApplications()
    {
        var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

        var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

        var application = new CreditCardApplication { GrossAnnualIncome = 100_000 };

        var decision = sut.Evaluate(application);

        Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
    }

    [Fact]
    public void ReferYoungApplications()
    {
        var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

        var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

        var application = new CreditCardApplication { Age = 19 };

        var decision = sut.Evaluate(application);

        Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
    }
}

