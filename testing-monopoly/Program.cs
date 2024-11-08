using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseApp {
    // Базовый класс, описывающий объект на складе
    public abstract class WarehouseObject {
        public Guid ID { get; } = Guid.NewGuid();
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public virtual double Weight { get; set; }

        public abstract double CalculateVolume();
    }

    // Класс коробки
    public class Box : WarehouseObject {
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ProductionDate { get; set; }

        public override double CalculateVolume() {
            return Width * Height * Depth;
        }

        public DateTime GetExpirationDate() {
            return ExpirationDate ?? ProductionDate?.AddDays(100) ?? throw new InvalidOperationException("Expiration or production date must be provided.");
        }
    }

    // Класс паллеты
    public class Pallet : WarehouseObject {
        public List<Box> Boxes { get; set; } = new List<Box>();

        public override double CalculateVolume() {
            double totalBoxVolume = Boxes.Sum(box => box.CalculateVolume());
            double palletVolume = Width * Height * Depth;
            return totalBoxVolume + palletVolume;
        }

        public DateTime? GetExpirationDate() {
            if (!Boxes.Any())
                return null;
            return Boxes.Min(box => box.GetExpirationDate());
        }

        public override double Weight => Boxes.Sum(box => box.Weight) + 30;
    }

    // Основное приложение
    public class Program {
        public static List<Pallet> GeneratePallets() {
            var pallets = new List<Pallet>();
            for (int i = 0; i < 5; i++) {
                var pallet = new Pallet {
                    Width = 120,
                    Height = 100,
                    Depth = 100
                };

                // Добавляем коробки на паллету
                for (int j = 0; j < 3; j++) {
                    pallet.Boxes.Add(new Box {
                        Width = 30,
                        Height = 30,
                        Depth = 30,
                        Weight = 10,
                        ProductionDate = DateTime.Now.AddDays(-j * 20) // Пример установки даты производства
                    });
                }

                pallets.Add(pallet);
            }

            return pallets;
        }

        public static void Main(string[] args) {
            var pallets = GeneratePallets();

            // Группировка паллет по сроку годности, сортировка
            var groupedPallets = pallets
                .OrderBy(p => p.GetExpirationDate())
                .ThenBy(p => p.Weight)
                .GroupBy(p => p.GetExpirationDate());

            Console.WriteLine("Группировка паллет по сроку годности:");
            foreach (var group in groupedPallets) {
                Console.WriteLine($"Срок годности: {group.Key?.ToShortDateString() ?? "Не указан"}");
                foreach (var pallet in group) {
                    Console.WriteLine($"  - Вес паллеты: {pallet.Weight} кг");
                }
            }

            // Три паллеты с коробками с наибольшим сроком годности, отсортированные по объему
            var topPallets = pallets
                .OrderByDescending(p => p.Boxes.Max(b => b.GetExpirationDate()))
                .ThenBy(p => p.CalculateVolume())
                .Take(3);

            Console.WriteLine("\nТри паллеты с наибольшим сроком годности:");
            foreach (var pallet in topPallets) {
                Console.WriteLine($"- Объем паллеты: {pallet.CalculateVolume()}, Срок годности коробок: {pallet.Boxes.Max(b => (DateTime?)b.GetExpirationDate())?.ToShortDateString()}");

            }
        }
    }
}
