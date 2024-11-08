using System;
using System.Collections.Generic;
using System.Linq;

public abstract class WarehouseItem {
    public string Id { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public double Weight { get; set; }

    public WarehouseItem(string id, double width, double height, double depth, double weight) {
        Id = id;
        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;
    }

    public double CalculateVolume() {
        return Width * Height * Depth;
    }

    public abstract void DisplayInfo();
}

public class Box : WarehouseItem {
    public DateTime? ExpiryDate { get; set; }
    public DateTime? ProductionDate { get; set; }

    public Box(string id, double width, double height, double depth, double weight, DateTime? productionDate = null, DateTime? expiryDate = null)
        : base(id, width, height, depth, weight) {
        if (productionDate.HasValue) {
            ProductionDate = productionDate;
            ExpiryDate = productionDate.Value.AddDays(100);
        } else if (expiryDate.HasValue) {
            ExpiryDate = expiryDate;
        }
    }

    public override void DisplayInfo() {
        Console.WriteLine($"Box - ID: {Id}, Dimensions (WxHxD): {Width}x{Height}x{Depth} cm, Weight: {Weight} kg, Volume: {CalculateVolume()} cm³");
        if (ProductionDate.HasValue) {
            Console.WriteLine($"  Production Date: {ProductionDate.Value:dd.MM.yyyy}");
        }
        if (ExpiryDate.HasValue) {
            Console.WriteLine($"  Expiry Date: {ExpiryDate.Value:dd.MM.yyyy}");
        }
    }
}

public class Pallet : WarehouseItem {
    public List<Box> Boxes { get; set; }
    public DateTime? ExpiryDate { get; private set; }

    public Pallet(string id, double width, double height, double depth, double baseWeight = 30)
        : base(id, width, height, depth, baseWeight) {
        Boxes = new List<Box>();
    }

    public void AddBox(Box box) {
        // Проверка, что коробка не превышает размеры паллеты по ширине и глубине
        if (box.Width > Width || box.Depth > Depth) {
            throw new InvalidOperationException($"Box {box.Id} exceeds pallet dimensions (Width: {box.Width} > {Width} or Depth: {box.Depth} > {Depth}).");
        }

        Boxes.Add(box);
        RecalculateWeight();
        UpdateExpiryDate();
    }

    private void RecalculateWeight() {
        // Вес паллеты = суммарный вес коробок + 30 кг
        Weight = Boxes.Sum(box => box.Weight) + 30;
    }

    private void UpdateExpiryDate() {
        // Срок годности паллеты равен минимальному сроку годности из коробок
        ExpiryDate = Boxes
            .Where(box => box.ExpiryDate.HasValue)
            .Select(box => box.ExpiryDate.Value)
            .DefaultIfEmpty()
            .Min();
    }

    public double CalculateTotalVolume() {
        // Объем паллеты = объем всех коробок + собственный объем паллеты
        double totalBoxVolume = Boxes.Sum(box => box.CalculateVolume());
        return totalBoxVolume + CalculateVolume();
    }

    public override void DisplayInfo() {
        Console.WriteLine($"Pallet - ID: {Id}, Dimensions (WxHxD): {Width}x{Height}x{Depth} cm, Weight: {Weight} kg, Volume: {CalculateTotalVolume()} cm³, Contains {Boxes.Count} box(es)");
        if (ExpiryDate.HasValue) {
            Console.WriteLine($"  Pallet Expiry Date: {ExpiryDate.Value:dd.MM.yyyy}");
        }
        foreach (var box in Boxes) {
            box.DisplayInfo();
        }
    }
}

// Пример использования
public class Program {
    public static void Main(string[] args) {
        Box box1 = new Box("B1", 30, 20, 10, 5, productionDate: new DateTime(2023, 1, 1));
        Box box2 = new Box("B2", 40, 30, 20, 7, expiryDate: new DateTime(2024, 2, 1));
        Box box3 = new Box("B3", 25, 15, 12, 4, expiryDate: new DateTime(2023, 12, 15));

        Pallet pallet = new Pallet("P1", 120, 100, 15);

        try {
            pallet.AddBox(box1);
            pallet.AddBox(box2);
            pallet.AddBox(box3);
            pallet.DisplayInfo();
        } catch (InvalidOperationException ex) {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();

    }
}
