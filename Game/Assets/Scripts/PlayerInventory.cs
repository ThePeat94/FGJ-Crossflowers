using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{

    private EventHandler<System.EventArgs> m_inventoryChanged;
    
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
    public event EventHandler<System.EventArgs> InventoryChanged
    {
        add => this.m_inventoryChanged += value;
        remove => this.m_inventoryChanged -= value;
    }

    public void AddSeed(Seed seedToAdd)
    {
        this.Seeds.Add(seedToAdd);
        this.m_inventoryChanged?.Invoke(this, System.EventArgs.Empty);
    }

    public void RemoveSeed(Seed toRemove)
    {
        this.Seeds.Remove(toRemove);
        this.m_inventoryChanged?.Invoke(this, System.EventArgs.Empty);
    }
}