namespace CreditCardApplications;

public class CreditCardApplication
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public int Age { get; set; }
    public decimal GrossAnnualIncome { get; set; }
    public string FrequentFlyerNumber { get; set; } = default!;
}
