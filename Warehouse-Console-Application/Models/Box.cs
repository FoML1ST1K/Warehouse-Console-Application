using System;

public class Box : WarehouseItem {
    public DateTime? ExpiryDate { get; private set; }
    public DateTime? ProductionDate { get; private set; }

    /// Конструктор для Box с параметрами (ID, ширина, высота, глубина, вес, дата производства)
    public Box(string id, double width, double height, double depth, double weight, DateTime productionDate)
        : base(id, width, height, depth, weight) {
        ProductionDate = productionDate;
        ExpiryDate = productionDate.AddDays(100); // Срок годности = 100 дней от даты производства
    }

    /// Конструктор для Box с параметрами (ID, ширина, высота, глубина, вес, срок годности)
    public Box(string id, double width, double height, double depth, double weight, DateTime? expiryDate)
        : base(id, width, height, depth, weight) {
        ExpiryDate = expiryDate;
    }

    public override void DisplayInfo() {
        Console.WriteLine($"Коробка - ID: {Id}, Размеры (ШxВxГ): {Width}x{Height}x{Depth} см, Вес: {Weight} кг, Объем: {CalculateVolume()} см");
        if (ProductionDate.HasValue) Console.WriteLine($"  Дата производства: {ProductionDate:dd.MM.yyyy}");
        if (ExpiryDate.HasValue) Console.WriteLine($"  Срок годности: {ExpiryDate:dd.MM.yyyy}");
    }
}

