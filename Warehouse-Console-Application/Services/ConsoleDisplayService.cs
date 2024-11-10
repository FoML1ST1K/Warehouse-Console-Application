using System;
using System.Collections.Generic;
using System.Linq;

public class ConsoleDisplayService : IDisplayService {
    public void DisplayPalletsGroupedByExpiryDate(List<Pallet> pallets) {
        var groupedPallets = pallets
            .OrderBy(p => p.ExpiryDate)
            .ThenBy(p => p.Weight)
            .GroupBy(p => p.ExpiryDate);

        Console.WriteLine("\nСгруппированные паллеты по сроку годности:");
        foreach (var group in groupedPallets) {
            Console.WriteLine($"\nСрок годности: {(group.Key.HasValue ? group.Key.Value.ToString("dd.MM.yyyy") : "Нет данных")}");
            foreach (var pallet in group) pallet.DisplayInfo();
        }
    }

    public void DisplayTop3PalletsWithMaxExpiryDate(List<Pallet> pallets) {
        var palletsWithMaxExpiry = pallets
            .Where(p => p.Boxes.Any(b => b.ExpiryDate.HasValue))
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpiryDate))
            .ThenBy(p => p.CalculateTotalVolume())
            .Take(3);

        Console.WriteLine("\n3 паллеты с коробками с наибольшим сроком годности:");
        foreach (var pallet in palletsWithMaxExpiry) pallet.DisplayInfo();
    }
}