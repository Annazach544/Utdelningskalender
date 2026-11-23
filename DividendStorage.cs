using System.Text.Json;

public class DividendStorage
{
    private const string FilePath = "dividends.json";

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

    public void Save(List<Dividend> dividends)
    {
        string tempPath = FilePath + ".tmp";
        string json = JsonSerializer.Serialize(dividends);

        // Skriv klart den nya filen innan den ersätter den gamla.
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, FilePath, overwrite: true);
    }
}