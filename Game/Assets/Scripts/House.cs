using System;
using QuickOutline.Scripts;
using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    [SerializeField] private Outline m_outline;

    private void Awake()
    {
        this.DisableOutline();
    }

    public void Interact()
    {
        Debug.Log("Rest and process current day");
        PlayerController.Instance.StaminaController.ResetValue();
        GameManager.Instance.ProcessCurrentDay();
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