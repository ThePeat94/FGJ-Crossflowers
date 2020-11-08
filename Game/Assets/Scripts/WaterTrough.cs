using System;
using QuickOutline.Scripts;
using UnityEngine;

public class WaterTrough : MonoBehaviour, IInteractable
{
    [SerializeField] private float m_fillWaterCost;
    [SerializeField] private Outline m_outline;

    private void Awake()
    {
        this.DisableOutline();
    }

    public void Interact()
    {
        Debug.Log("Try resetting water...");
        if (PlayerController.Instance.StaminaController.UseResource(this.m_fillWaterCost))
        {
            PlayerController.Instance.WaterController.ResetValue();
        }
    }
    
    public void EnableOutline()
    {
        this.m_outline.enabled = true;
    }

    public void DisableOutline()
    {
        this.m_outline.enabled = false;
    }
}