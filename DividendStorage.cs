using System.Text.Json;

// Ansvarar för att läsa och spara utdelningar i en JSON-fil.
public class DividendStorage
{
    private const string FilePath = "dividends.json";

    // Läser filens innehåll och omvandlar JSON till en lista med utdelningar.
    public List<Dividend> Load()
    {
        try
        {
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Dividend>>(json)
                ?? throw new JsonException("Sparfilen innehåller ingen lista.");
        }
        catch (FileNotFoundException)
        {
            // Första gången programmet körs börjar listan tom.
            return new List<Dividend>();
        }
    }

    // Omvandlar listan till JSON och sparar den i filen.
    public void Save(List<Dividend> dividends)
    {
        string tempPath = FilePath + ".tmp";
        string json = JsonSerializer.Serialize(dividends);

        // Skriv klart den nya filen innan den ersätter den gamla.
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, FilePath, overwrite: true);
    }
}