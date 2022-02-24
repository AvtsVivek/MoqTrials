namespace CreditCardApplications.Tests;
using Xunit;
using Moq;

public class CreditCardApplicationEvaluatorShould
{
    [Fact]
    public void AcceptHighIncomeApplications()
    {
        var mockValidator =
            new Mock<IFrequentFlyerNumberValidator>();

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

    [Fact]
    public void DeclineLowIncomeApplications()
    {
        var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

        // mockValidator.Setup(x => x.IsValid("x")).Returns(true);

        // mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

        // The predicate
        // number => number.StartsWith("Y")
        // Says that if the frequentflyernumber starts with y then retrun true.
        //mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith("y")))).Returns(true);

        // If the passed in value is in the range
        //mockValidator.Setup(x => x.IsValid(It.IsInRange("a", "z", Range.Inclusive))).Returns(true);

        // If the passed in value is in the set of acceptable values
        //mockValidator.Setup(x => x.IsValid(It.IsIn("a", "z", "y"))).Returns(true);

        mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]"))).Returns(true);


        var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

        var application = new CreditCardApplication
        {
            GrossAnnualIncome = 19_999,
            Age = 42,
            FrequentFlyerNumber = "y"
        };

        var decision = sut.Evaluate(application);

        Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
    }
    [Fact]
    public void ReferInvalidFrequentFlyerApplications()
    {
        var mockValidator =
            new Mock<IFrequentFlyerNumberValidator>(MockBehavior.Strict);

        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
        mockValidator.Setup(x => x.LicenseKey).Returns("NotEXPIRED");

        var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

        var application = new CreditCardApplication();

        var decision = sut.Evaluate(application);

        Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
    }

    [Fact]
    public void DeclineLowIncomeApplicationsOutDemo()
    {
        var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

        var isValid = true;
        mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

        var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

        var application = new CreditCardApplication
        {
            GrossAnnualIncome = 19_999,
            Age = 42
        };

        var decision = sut.EvaluateUsingOut(application);

        Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
    }
    [Fact]
    public void ReferWhenLicenseKeyExpired()
    {
        var mockValidator = new Mock<IFrequentFlyerNumberValidator>();

        mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

        // mockValidator.Setup(x => x.LicenseKey).Returns("EXPIRED");

        // The following is another way. Getting the stuff from a function.
        mockValidator.Setup(x => x.LicenseKey).Returns(this.GetLicenseKeyExpiryString);

        var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

        var application = new CreditCardApplication { Age = 42 };

        var decision = sut.Evaluate(application);

        Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
    }

    // E.g. read from vendor-supplied constants file
    private string GetLicenseKeyExpiryString() => "EXPIRED";
}

