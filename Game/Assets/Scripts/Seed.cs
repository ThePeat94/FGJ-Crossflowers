using Scriptables;
using UnityEngine;

public class Seed
{
    public Seed(ShopSeedData seedData)
    {
        this.Color = seedData.Color;
        this.SellValue = 2.25f;
        this.Name = seedData.Name;
    }
    
    public Seed(Color color, float sellValue)
    {
        this.Color = color;
        this.SellValue = sellValue;
    }

    public Seed(Color color, float sellValue, string name)
    {
        this.Color = color;
        this.SellValue = sellValue;
        this.Name = name;
    }

    public Color Color { get; }
    public float SellValue { get; }
    public string Name { get; }
}