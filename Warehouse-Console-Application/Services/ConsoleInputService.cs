using System.Collections.Generic;
using System;

public class ConsoleInputService : IInputService {
    public List<Pallet> CollectPalletData() {
        var pallets = new List<Pallet>();

        Console.WriteLine("Добро пожаловать в консольное приложение для управления складом!");
        Console.Write("Введите количество паллет: ");
        int numberOfPallets = int.Parse(Console.ReadLine());

        for (int i = 0; i < numberOfPallets; i++) {
            var pallet = CreatePallet(i + 1);

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

                var box = productionDate.HasValue
                    ? new Box(boxId, boxWidth, boxHeight, boxDepth, boxWeight, productionDate.Value)
                    : new Box(boxId, boxWidth, boxHeight, boxDepth, boxWeight, expiryDate);

                try {
                    pallet.AddBox(box);
                } catch (InvalidOperationException ex) {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            pallets.Add(pallet);
        }

        return pallets;
    }

    private Pallet CreatePallet(int palletNumber) {
        Console.WriteLine($"\nВвод данных для паллеты {palletNumber}:");
        Console.Write("Введите ID паллеты: ");
        string palletId = Console.ReadLine();
        Console.Write("Введите ширину паллеты: ");
        double palletWidth = double.Parse(Console.ReadLine());
        Console.Write("Введите высоту паллеты: ");
        double palletHeight = double.Parse(Console.ReadLine());
        Console.Write("Введите глубину паллеты: ");
        double palletDepth = double.Parse(Console.ReadLine());

        var pallet = new Pallet(palletId, palletWidth, palletHeight, palletDepth);
        return pallet;
    }
}