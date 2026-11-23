using System.Globalization;
using System.Text.Json;

var storage = new DividendStorage();
List<Dividend> dividends;

// Läs in sparade utdelningar innan menyn visas.
try
{
    dividends = storage.Load();
}
catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
{
    Console.WriteLine($"Kunde inte läsa sparfilen. {ex.Message}");
    return;
}

while (true)
{
    Console.WriteLine("\n=== UTDELNINGSKALENDER ===");
    Console.WriteLine("1. Lägg till utdelning");
    Console.WriteLine("2. Visa utdelningar");
    Console.WriteLine("3. Ta bort utdelning");
    Console.WriteLine("4. Ändra utdelning");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj (1–5): ");

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

        var updatedDividends = new List<Dividend>(dividends);
        updatedDividends.Add(dividend);

        if (!TrySaveDividends(updatedDividends))
        {
            continue;
        }

        dividends = updatedDividends;
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

        // Gruppera efter både år och månad och summera varje grupp.
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
        var updatedDividends = new List<Dividend>(dividends);
        updatedDividends.Remove(selectedDividend);

        if (!TrySaveDividends(updatedDividends))
        {
            continue;
        }

        dividends = updatedDividends;
        Console.WriteLine("Utdelningen har tagits bort.");
    }
    else if (choice == "4")
    {
        if (dividends.Count == 0)
        {
            Console.WriteLine("Det finns inga utdelningar att ändra.");
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

        Console.Write("Ange numret på utdelningen du vill ändra, eller 0 för att avbryta: ");

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

        Console.WriteLine("Tryck Enter för att behålla ett befintligt värde.");

        Console.Write($"Bolagsnamn ({selectedDividend.CompanyName}): ");
        var companyInput = Console.ReadLine()?.Trim();

        if (companyInput == null)
        {
            continue;
        }

        string companyName = selectedDividend.CompanyName;

        if (companyInput != string.Empty)
        {
            companyName = companyInput;
        }

        Console.Write($"Antal aktier ({selectedDividend.NumberOfShares}): ");
        var sharesInput = Console.ReadLine()?.Trim();
        int numberOfShares = selectedDividend.NumberOfShares;

        if (sharesInput == null)
        {
            continue;
        }

        if (sharesInput != string.Empty
            && (!int.TryParse(sharesInput, out numberOfShares) || numberOfShares <= 0))
        {
            Console.WriteLine("Ange ett heltal större än noll.");
            continue;
        }

        Console.Write($"Utdelning per aktie ({selectedDividend.DividendPerShare:F2} kr): ");
        var amountInput = Console.ReadLine()?.Trim();
        decimal dividendPerShare = selectedDividend.DividendPerShare;

        if (amountInput == null)
        {
            continue;
        }

        if (amountInput != string.Empty
            && (!decimal.TryParse(amountInput, out dividendPerShare) || dividendPerShare <= 0))
        {
            Console.WriteLine("Ange ett belopp större än noll.");
            continue;
        }

        Console.Write($"Utbetalningsdatum ({selectedDividend.PaymentDate:yyyy-MM-dd}): ");
        var dateInput = Console.ReadLine()?.Trim();
        DateTime paymentDate = selectedDividend.PaymentDate;

        if (dateInput == null)
        {
            continue;
        }

        if (dateInput != string.Empty
            && !DateTime.TryParseExact(
                dateInput,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out paymentDate))
        {
            Console.WriteLine("Ange ett giltigt datum i formatet åååå-mm-dd.");
            continue;
        }

        // Skapa en ny post så att originalet behålls om sparningen misslyckas.
        var editedDividend = new Dividend
        {
            CompanyName = companyName,
            NumberOfShares = numberOfShares,
            DividendPerShare = dividendPerShare,
            PaymentDate = paymentDate
        };

        var updatedDividends = new List<Dividend>(dividends);

        // Hitta postens plats i originallistan efter valet i den sorterade listan.
        int selectedIndex = dividends.IndexOf(selectedDividend);
        updatedDividends[selectedIndex] = editedDividend;

        if (!TrySaveDividends(updatedDividends))
        {
            continue;
        }

        dividends = updatedDividends;
        Console.WriteLine("Utdelningen har ändrats.");
    }
    else if (choice == "5" || choice == null)
    {
        break;
    }
    else
    {
        Console.WriteLine("Välj 1, 2, 3, 4 eller 5.");
    }
}

// Hantera sparfel och meddela om ändringen kunde sparas.
bool TrySaveDividends(List<Dividend> updatedDividends)
{
    try
    {
        storage.Save(updatedDividends);
        return true;
    }
    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
    {
        Console.WriteLine($"Kunde inte spara ändringen. {ex.Message}");
        return false;
    }
}
