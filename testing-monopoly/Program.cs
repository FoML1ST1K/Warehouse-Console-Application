using System;
using System.Collections.Generic;
using System.Linq;

public abstract class WarehouseItem {
    public string Id { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }
    public double Weight { get; set; }

    protected WarehouseItem(string id, double width, double height, double depth, double weight) {
        Id = id;
        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;
    }

    public double CalculateVolume() => Width * Height * Depth;

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
        Console.WriteLine($"Коробка - ID: {Id}, Размер: {Width}x{Height}x{Depth} см, Вес: {Weight} кг, Объем: {CalculateVolume()} см");
        if (ProductionDate.HasValue) {
            Console.WriteLine($"  Дата производства: {ProductionDate.Value:dd.MM.yyyy}");
        }
        if (ExpiryDate.HasValue) {
            Console.WriteLine($"  Срок годности: {ExpiryDate.Value:dd.MM.yyyy}");
        }
    }
}

public class Pallet : WarehouseItem {
    public List<Box> Boxes { get; private set; }
    public DateTime? ExpiryDate { get; private set; }

    public Pallet(string id, double width, double height, double depth, double baseWeight = 30)
        : base(id, width, height, depth, baseWeight) {
        Boxes = new List<Box>();
    }

    public void AddBox(Box box) {
        if (box.Width > Width || box.Depth > Depth) {
            throw new InvalidOperationException($"Коробка {box.Id} превышает размеры паллеты (Ширина: {box.Width} > {Width} или Глубина: {box.Depth} > {Depth}).");
        }

        Boxes.Add(box);
        RecalculateWeight();
        UpdateExpiryDate();
    }

    private void RecalculateWeight() => Weight = Boxes.Sum(box => box.Weight) + 30;

    private void UpdateExpiryDate() => ExpiryDate = Boxes
        .Where(box => box.ExpiryDate.HasValue)
        .Select(box => box.ExpiryDate.Value)
        .DefaultIfEmpty()
        .Min();

    public double CalculateTotalVolume() => Boxes.Sum(box => box.CalculateVolume()) + CalculateVolume();

    public override void DisplayInfo() {
        Console.WriteLine($"Паллет - ID: {Id}, Размеры (ШxВxГ): {Width}x{Height}x{Depth} см, Вес: {Weight} кг, Объем: {CalculateTotalVolume()} см³, Содержит {Boxes.Count} коробок(и)");
        if (ExpiryDate.HasValue) {
            Console.WriteLine($"  Срок годности паллеты: {ExpiryDate.Value:dd.MM.yyyy}");
        }
        foreach (var box in Boxes) {
            box.DisplayInfo();
        }
    }
}

public class Program {
    public static void Main(string[] args) {
        List<Pallet> pallets = new List<Pallet>();

        // Ввод данных о паллетах
        Console.WriteLine("Добро пожаловать в консольное приложение для управления складом!");
        Console.Write("Введите количество паллет: ");
        int numberOfPallets = int.Parse(Console.ReadLine());

        for (int i = 0; i < numberOfPallets; i++) {
            Console.WriteLine($"\nВвод данных для паллеты {i + 1}:");
            Console.Write("Введите ID паллеты: ");
            string palletId = Console.ReadLine();
            Console.Write("Введите ширину паллеты: ");
            double palletWidth = double.Parse(Console.ReadLine());
            Console.Write("Введите высоту паллеты: ");
            double palletHeight = double.Parse(Console.ReadLine());
            Console.Write("Введите глубину паллеты: ");
            double palletDepth = double.Parse(Console.ReadLine());

            var pallet = new Pallet(palletId, palletWidth, palletHeight, palletDepth);

            Console.Write("Введите количество коробок для этой паллеты: ");
            int numberOfBoxes = int.Parse(Console.ReadLine());

            for (int j = 0; j < numberOfBoxes; j++) {
                Console.WriteLine($"\nВвод данных для коробки {j + 1}:");
                Console.Write("Введите ID коробки: ");
                string boxId = Console.ReadLine();
                Console.Write("Введите ширину коробки: ");
                double boxWidth = double.Parse(Console.ReadLine());
                Console.Write("Введите высоту коробки: ");
                double boxHeight = double.Parse(Console.ReadLine());
                Console.Write("Введите глубину коробки: ");
                double boxDepth = double.Parse(Console.ReadLine());
                Console.Write("Введите вес коробки: ");
                double boxWeight = double.Parse(Console.ReadLine());

                Console.Write("Введите дату производства (гггг-мм-дд) или оставьте пустым для ввода срока годности: ");
                string productionDateInput = Console.ReadLine();
                DateTime? productionDate = string.IsNullOrEmpty(productionDateInput) ? (DateTime?)null : DateTime.Parse(productionDateInput);

                DateTime? expiryDate = null;
                if (!productionDate.HasValue) {
                    Console.Write("Введите срок годности (гггг-мм-дд): ");
                    expiryDate = DateTime.Parse(Console.ReadLine());
                }

                var box = new Box(boxId, boxWidth, boxHeight, boxDepth, boxWeight, productionDate, expiryDate);

                try {
                    pallet.AddBox(box);
                } catch (InvalidOperationException ex) {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            pallets.Add(pallet);
        }

        // Группировка и сортировка паллет
        var groupedPallets = pallets
            .OrderBy(p => p.ExpiryDate)
            .ThenBy(p => p.Weight)
            .GroupBy(p => p.ExpiryDate);

        Console.WriteLine("\nСгруппированные паллеты по сроку годности:");
        foreach (var group in groupedPallets) {
            Console.WriteLine($"\nСрок годности: {(group.Key.HasValue ? group.Key.Value.ToString("dd.MM.yyyy") : "Нет данных")}");
            foreach (var pallet in group) {
                pallet.DisplayInfo();
            }
        }

        // Три паллеты с коробками с наибольшим сроком годности
        var palletsWithMaxExpiry = pallets
            .Where(p => p.Boxes.Any(b => b.ExpiryDate.HasValue))
            .OrderByDescending(p => p.Boxes.Max(b => b.ExpiryDate))
            .ThenBy(p => p.CalculateTotalVolume())
            .Take(3);

        Console.WriteLine("\n3 паллеты с коробками с наибольшим сроком годности:");
        foreach (var pallet in palletsWithMaxExpiry) {
            pallet.DisplayInfo();
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
