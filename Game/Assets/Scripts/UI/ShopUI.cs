using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_moneyText;
        [SerializeField] private TextMeshProUGUI m_staminaUpgradeCostText;
        [SerializeField] private TextMeshProUGUI m_waterUpgradeCostText;
        [SerializeField] private TextMeshProUGUI m_farmUpgradeCostText;
        [SerializeField] private Transform m_content;
        [SerializeField] private GameObject m_buySeedRowPrefab;
        [SerializeField] private GameObject m_sellSeedRowPrefab;

        [SerializeField] private List<GameObject> m_availableShopItems;

        private List<GameObject> m_availablePlayerItems;

        private bool m_sellUiIsActive = false;
        
        private void Awake()
        {
            Shop.Instance.MoneyController.ResourceValueChanged += (sender, args) => this.m_moneyText.text = $"{args.NewValue:0.00}";
            this.m_moneyText.text = $"{Shop.Instance.MoneyController.CurrentValue:0.00}";
            PlayerController.Instance.PlayerInventory.InventoryChanged += (sender, args) =>
            {
                this.SetupAvailablePlayerItems();
            };
            
            this.m_availablePlayerItems = new List<GameObject>();
            this.SetupAvailablePlayerItems();
            this.SetupAvailableShopItems();
        }
        public void ShowUI()
        {
            this.gameObject.SetActive(true);
            this.m_farmUpgradeCostText.text = $"{Shop.Instance.UnlockFarmData.Cost:0.00}";
            this.m_staminaUpgradeCostText.text = $"{Shop.Instance.StaminaUpgradeData.Cost:0.00}";
            this.m_waterUpgradeCostText.text = $"{Shop.Instance.WaterUpgradeData.Cost:0.00}";
        }

        public void CloseUI()
        {
            this.gameObject.SetActive(false);
        }
        
        public void BuyFarmUpgradeClick()
        {
            Shop.Instance.BuyFarmUpgrade();
        }

        public void BuyPlayerStaminaUpgradeClick()
        {
            Shop.Instance.BuyPlayerStaminaUpgrade();
        }

        public void BuyPlayerWaterUpgradeClick()
        {
            Shop.Instance.BuyWaterCanUpgrade();
        }

        public void ShowAvailableShopItems()
        {
            this.m_availablePlayerItems.ForEach(go => go.SetActive(false));
            this.m_availableShopItems.ForEach(go => go.SetActive(true));
            this.m_sellUiIsActive = false;
        }
        
        public void ShowAvailablePlayerItems()
        {
            this.m_availablePlayerItems.ForEach(go => go.SetActive(true));
            this.m_availableShopItems.ForEach(go => go.SetActive(false));
            this.m_sellUiIsActive = true;
        }
        private void SetupAvailablePlayerItems()
        {
            if (this.m_availablePlayerItems.Count > 0)
            {
                this.m_availablePlayerItems.ForEach(go => Destroy(go));
                this.m_availablePlayerItems.Clear();
            }
            
            foreach (var seed in PlayerController.Instance.PlayerInventory.Seeds)
            {
                var sellSeedRowUI = Instantiate(this.m_sellSeedRowPrefab, this.m_content).GetComponent<SellSeedRowUI>();
                sellSeedRowUI.ShowSeed(seed);
                this.m_availablePlayerItems.Add(sellSeedRowUI.gameObject);
                sellSeedRowUI.gameObject.SetActive(this.m_sellUiIsActive);
            }
        }

        private void SetupAvailableShopItems()
        {
            foreach (var seedData in Shop.Instance.AvailableSeeds)
            {
                var buySeedRowUi = Instantiate(this.m_buySeedRowPrefab, this.m_content).GetComponent<BuySeedRowUI>();
                buySeedRowUi.ShowBuySeedRow(seedData);
                this.m_availableShopItems.Add(buySeedRowUi.gameObject);
            }
        }
    }
}