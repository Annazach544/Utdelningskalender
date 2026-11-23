// Beskriver en utdelning med bolagsnamn, antal aktier, belopp och datum.
public class Dividend
{
    public string CompanyName { get; set; } = string.Empty;
    public int NumberOfShares { get; set; }
    public decimal DividendPerShare { get; set; }
    public DateTime PaymentDate { get; set; }
}