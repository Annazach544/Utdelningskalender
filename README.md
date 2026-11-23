# Utdelningskalender

Ett projekt i kursen Programmering i C#.NET vid Mittuniversitetet.

Applikationen är en konsolapp för att registrera aktieutdelningar och
få en överblick över förväntade utbetalningar per månad.
Alla belopp anges i svenska kronor.

## Funktioner

- Lägg till en utdelning med bolagsnamn, antal aktier, utdelning per aktie och datum.
- Visa utdelningar i datumordning och summera beloppen per år och månad.
- Ta bort en utdelning.
- Ändra en utdelning och behåll befintliga värden genom att trycka Enter.
- Spara och läsa in utdelningar automatiskt med JSON.
- Kontrollera inmatning och visa felmeddelanden vid ogiltiga värden.

## Krav och körning

Projektet använder .NET 10. Installera .NET 10 SDK för att köra det.

Öppna en terminal i mappen där Utdelningskalender.csproj ligger och kör:

```powershell
dotnet run
```

Välj funktion genom att skriva ett nummer och trycka Enter.
Ange datum som åååå-mm-dd och använd decimaltecknet för datorns
regionala inställningar, till exempel 5,50 med svenska inställningar.
Vid borttagning och ändring kan du ange 0 för att avbryta.
Välj 5 för att avsluta programmet.

## Projektstruktur

- Program.cs hanterar menyn, inmatning, beräkningar och utskrifter.
- Dividend.cs beskriver en utdelningspost.
- DividendStorage.cs hanterar läsning och sparning av JSON-filen.

## Lagring

Utdelningar sparas i dividends.json i den mapp programmet körs från.
Filen skapas automatiskt när en ändring sparas.
Sparfilen och den tillfälliga filen dividends.json.tmp ignoreras av Git.

## Författare

Anna Zach