Console.Write("Bolagsnamn: ");
var companyName = Console.ReadLine()?.Trim() ?? string.Empty;

if (string.IsNullOrEmpty(companyName))
{
    Console.WriteLine("Ange ett bolagsnamn.");
    return;
}

Console.Write("Antal aktier: ");
string? sharesInput = Console.ReadLine();

if (!int.TryParse(sharesInput, out int numberOfShares) || numberOfShares <= 0)
{
    Console.WriteLine("Ange ett heltal större än noll.");
    return;
}

Console.Write("Utdelning per aktie (kr): ");
string? dividendInput = Console.ReadLine();

if (!decimal.TryParse(dividendInput, out decimal dividendPerShare) || dividendPerShare <= 0)
{
    Console.WriteLine("Ange ett belopp större än noll, till exempel 5,50.");
    return;
}

Console.Write("Utbetalningsdatum (åååå-mm-dd): ");

if (!DateTime.TryParseExact(
        Console.ReadLine(),
        "yyyy-MM-dd",
        System.Globalization.CultureInfo.InvariantCulture,
        System.Globalization.DateTimeStyles.None,
        out DateTime paymentDate))
{
    Console.WriteLine("Ange ett giltigt datum i formatet åååå-mm-dd.");
    return;
}

var dividend = new Dividend
{
    CompanyName = companyName,
    NumberOfShares = numberOfShares,
    DividendPerShare = dividendPerShare,
    PaymentDate = paymentDate
};

decimal totalDividend = dividend.NumberOfShares * dividend.DividendPerShare;
Console.WriteLine($"{dividend.CompanyName} | {dividend.PaymentDate:yyyy-MM-dd} | {totalDividend:F2} kr");
