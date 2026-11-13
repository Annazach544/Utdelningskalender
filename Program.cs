using System.Globalization;

var dividends = new List<Dividend>();

while (true)
{
    Console.WriteLine("\n=== UTDELNINGSKALENDER ===");
    Console.WriteLine("1. Lägg till utdelning");
    Console.WriteLine("2. Visa utdelningar");
    Console.WriteLine("3. Avsluta");
    Console.Write("Välj (1–3): ");

    var choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Write("Bolagsnamn: ");
        var companyName = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(companyName))
        {
            Console.WriteLine("Ange ett bolagsnamn.");
            continue;
        }

        Console.Write("Antal aktier: ");
        if (!int.TryParse(Console.ReadLine(), out int numberOfShares) || numberOfShares <= 0)
        {
            Console.WriteLine("Ange ett heltal större än noll.");
            continue;
        }

        Console.Write("Utdelning per aktie (kr): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal dividendPerShare) || dividendPerShare <= 0)
        {
            Console.WriteLine("Ange ett belopp större än noll, till exempel 5,50.");
            continue;
        }

        Console.Write("Utbetalningsdatum (åååå-mm-dd): ");
        if (!DateTime.TryParseExact(
                Console.ReadLine(),
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime paymentDate))
        {
            Console.WriteLine("Ange ett giltigt datum i formatet åååå-mm-dd.");
            continue;
        }

        var dividend = new Dividend
        {
            CompanyName = companyName,
            NumberOfShares = numberOfShares,
            DividendPerShare = dividendPerShare,
            PaymentDate = paymentDate
        };

        dividends.Add(dividend);
        Console.WriteLine("Utdelningen har lagts till.");
    }
    else if (choice == "2")
    {
        if (dividends.Count == 0)
        {
            Console.WriteLine("Det finns inga utdelningar ännu.");
        }

        foreach (var dividend in dividends)
        {
            decimal totalDividend = dividend.NumberOfShares * dividend.DividendPerShare;
            Console.WriteLine($"{dividend.CompanyName} | {dividend.PaymentDate:yyyy-MM-dd} | {totalDividend:F2} kr");
        }
    }
    else if (choice == "3" || choice == null)
    {
        break;
    }
    else
    {
        Console.WriteLine("Välj 1, 2 eller 3.");
    }
}