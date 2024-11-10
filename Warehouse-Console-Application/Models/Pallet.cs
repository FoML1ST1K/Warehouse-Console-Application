using System;
using System.Collections.Generic;
using System.Linq;

public class Pallet : WarehouseItem {
    public List<Box> Boxes { get; }
    public DateTime? ExpiryDate { get; private set; }
    private const int PALLET_OWN_WEIGTH_KG = 30;

    public Pallet(string id, double width, double height, double depth, double baseWeight = PALLET_OWN_WEIGTH_KG)
        : base(id, width, height, depth, baseWeight) {
        Boxes = new List<Box>();
    }

    public void AddBox(Box box) {
        if (box.Width > Width || box.Depth > Depth)
            throw new InvalidOperationException($"Коробка {box.Id} превышает размеры паллеты (Ширина: {box.Width} > {Width} или Глубина: {box.Depth} > {Depth}).");

        Boxes.Add(box);
        RecalculateWeight();
        UpdateExpiryDate();
    }

    private void RecalculateWeight() => Weight = Boxes.Sum(box => box.Weight) + PALLET_OWN_WEIGTH_KG;

    private void UpdateExpiryDate() => ExpiryDate = Boxes
        .Where(box => box.ExpiryDate.HasValue)
        .Select(box => box.ExpiryDate.Value)
        .DefaultIfEmpty()
        .Min();

    public double CalculateTotalVolume() => Boxes.Sum(box => box.CalculateVolume()) + CalculateVolume();

    public override void DisplayInfo() {
        Console.WriteLine($"Паллет - ID: {Id}, Размеры (ШxВxГ): {Width}x{Height}x{Depth} см, Вес: {Weight} кг, Объем: {CalculateTotalVolume()} см³, Содержит {Boxes.Count} коробок(и)");
        if (ExpiryDate.HasValue) Console.WriteLine($"  Срок годности паллеты: {ExpiryDate:dd.MM.yyyy}");
        foreach (var box in Boxes) box.DisplayInfo();
    }
}