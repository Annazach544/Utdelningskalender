using System.Globalization;

var dividends = new List<Dividend>();

while (true)
{
    Console.WriteLine("\n=== UTDELNINGSKALENDER ===");
    Console.WriteLine("1. Lägg till utdelning");
    Console.WriteLine("2. Visa utdelningar");
    Console.WriteLine("3. Ta bort utdelning");
    Console.WriteLine("4. Avsluta");
    Console.Write("Välj (1–4): ");

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
            continue;
        }

        foreach (var dividend in dividends.OrderBy(dividend => dividend.PaymentDate))
        {
            decimal totalDividend = dividend.NumberOfShares * dividend.DividendPerShare;
            Console.WriteLine($"{dividend.CompanyName} | {dividend.PaymentDate:yyyy-MM-dd} | {totalDividend:F2} kr");
        }

        Console.WriteLine("\n=== SUMMA PER MÅNAD ===");

        var months = dividends
            .GroupBy(dividend => new { dividend.PaymentDate.Year, dividend.PaymentDate.Month })
            .OrderBy(month => month.Key.Year)
            .ThenBy(month => month.Key.Month);

        foreach (var month in months)
        {
            decimal monthlyTotal = month.Sum(
                dividend => dividend.NumberOfShares * dividend.DividendPerShare);

            Console.WriteLine($"{month.Key.Year:D4}-{month.Key.Month:D2} | {monthlyTotal:F2} kr");
        }
    }
    else if (choice == "3")
    {
        if (dividends.Count == 0)
        {
            Console.WriteLine("Det finns inga utdelningar att ta bort.");
            continue;
        }

        var sortedDividends = dividends
            .OrderBy(dividend => dividend.PaymentDate)
            .ToList();

        for (int i = 0; i < sortedDividends.Count; i++)
        {
            var dividend = sortedDividends[i];
            Console.WriteLine($"{i + 1}. {dividend.CompanyName} | {dividend.PaymentDate:yyyy-MM-dd}");
        }

        Console.Write("Ange numret på utdelningen du vill ta bort, eller 0 för att avbryta: ");

        if (!int.TryParse(Console.ReadLine(), out int number)
            || number < 0 || number > sortedDividends.Count)
        {
            Console.WriteLine("Ogiltigt nummer.");
            continue;
        }

        if (number == 0)
        {
            continue;
        }

        var selectedDividend = sortedDividends[number - 1];
        dividends.Remove(selectedDividend);
        Console.WriteLine("Utdelningen har tagits bort.");
    }
    else if (choice == "4" || choice == null)
    {
        break;
    }
    else
    {
        Console.WriteLine("Välj 1, 2, 3 eller 4.");
    }
}