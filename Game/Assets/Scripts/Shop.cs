using System;
using QuickOutline.Scripts;
using Scriptables;
using UI;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    private static Shop s_instance;
    
    [SerializeField] private ShopSeedData[] m_availableSeeds;
    [SerializeField] private ResourceData m_moneyResourceData;

    [SerializeField] private ResourceUpgradeData m_staminaUpgradeData;
    [SerializeField] private ResourceUpgradeData m_waterUpgradeData;
    [SerializeField] private UnlockFarmData m_unlockFarmData;
    [SerializeField] private Outline m_outline;
    public ResourceUpgradeData StaminaUpgradeData => this.m_staminaUpgradeData;
    public ResourceUpgradeData WaterUpgradeData => this.m_waterUpgradeData;
    public UnlockFarmData UnlockFarmData => this.m_unlockFarmData;
    public ShopSeedData[] AvailableSeeds => this.m_availableSeeds;

    private ResourceController m_moneyController;

    public static Shop Instance => s_instance ?? FindObjectOfType<Shop>();

    public ResourceController MoneyController => this.m_moneyController;
    
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        this.m_moneyController = new ResourceController(this.m_moneyResourceData);
        
        this.DisableOutline();
    }

    public void Interact()
    {
        PlayerHudUI.Instance.ShowShopUI();
    }

    public void BuyPlayerStaminaUpgrade()
    {
        if (this.m_moneyController.UseResource(this.m_staminaUpgradeData.Cost))
        {
            PlayerController.Instance.StaminaController.IncreaseMaximum(this.m_staminaUpgradeData.IncreaseValue);
        }
    }

    public void BuyWaterCanUpgrade()
    {
        if (this.m_moneyController.UseResource(this.m_staminaUpgradeData.Cost))
        {
            PlayerController.Instance.WaterController.IncreaseMaximum(this.m_waterUpgradeData.IncreaseValue);
        }
    }

    public void BuyFarmUpgrade()
    {
        if (this.m_moneyController.UseResource(this.m_unlockFarmData.Cost))
        {
            Farm.Instance.UnlockNextFarm();
        }
    }

    public void BuySeed(ShopSeedData seedData)
    {
        if (this.m_moneyController.UseResource(seedData.BuyValue))
        {
            PlayerController.Instance.PlayerInventory.AddSeed(new Seed(seedData));
        }
    }

    public void SellSeed(Seed toSell)
    {
        PlayerController.Instance.PlayerInventory.RemoveSeed(toSell);
        this.m_moneyController.Add(toSell.SellValue);
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