using System;
using System.Collections.Generic;
using System.Linq;

// Core Models
public abstract class WarehouseItem {
    public string Id { get; }
    public double Width { get; }
    public double Height { get; }
    public double Depth { get; }
    public double Weight { get; protected set; }

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