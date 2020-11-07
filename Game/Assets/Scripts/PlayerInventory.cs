using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{

    public PlayerInventory()
    {
        this.Seeds = new List<Seed>
        {
            new Seed(Color.cyan, 10, "Cyan"),
            new Seed(Color.red, 10, "Red"),
            new Seed(Color.yellow, 10, "Yellow")
        };
    }
    
    public List<Seed> Seeds { get; }

    public void AddSeed(Seed seedToAdd)
    {
        this.Seeds.Add(seedToAdd);
    }
}