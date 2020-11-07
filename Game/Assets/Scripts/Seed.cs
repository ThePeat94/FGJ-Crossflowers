using UnityEngine;

public class Seed
{
    public Seed(Color color, int sellValue)
    {
        this.Color = color;
        this.SellValue = sellValue;
    }

    public Seed(Color color, int sellValue, string name)
    {
        this.Color = color;
        this.SellValue = sellValue;
        this.Name = name;
    }

    public Color Color { get; }
    public int SellValue { get; }
    public string Name { get; }
}