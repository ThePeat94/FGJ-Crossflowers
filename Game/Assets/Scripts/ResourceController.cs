using System;
using System.Collections;
using System.Collections.Generic;
using EventArgs;
using Scriptables;
using UnityEngine;

public class ResourceController
{

    private float m_currentValue;
    private EventHandler<ResourceValueChangedEventArgs> m_resourceValueChanged;
    
    public ResourceController(ResourceData data)
    {
        this.MaxValue = data.InitMaxValue;
        this.m_currentValue = this.MaxValue;
    }
    
    public float CurrentValue => this.m_currentValue;
    public float MaxValue { get; set; }
    
    public event EventHandler<ResourceValueChangedEventArgs> ResourceValueChanged
    {
        add => this.m_resourceValueChanged += value;
        remove => this.m_resourceValueChanged -= value;
    }
    
    public void ResetValue()
    {
        this.m_currentValue = this.MaxValue;
        this.m_resourceValueChanged?.Invoke(this, new ResourceValueChangedEventArgs(this.m_currentValue));
    }
    
    public bool UseResource(float amount)
    {
        if(!this.CanAfford(amount))
            return false;

        this.m_currentValue -= amount;
        this.m_resourceValueChanged?.Invoke(this, new ResourceValueChangedEventArgs(this.m_currentValue));
        return true;
    }
    
    private bool CanAfford(float amount)
    {
        return this.m_currentValue > 0 && amount <= this.m_currentValue;
    }
}
