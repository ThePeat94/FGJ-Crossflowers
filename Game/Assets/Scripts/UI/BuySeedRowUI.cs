using Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuySeedRowUI : MonoBehaviour
    {
        [SerializeField] private Button m_buyButton;
        [SerializeField] private TextMeshProUGUI m_seedName;
        [SerializeField] private TextMeshProUGUI m_costText;
        [SerializeField] private Image m_image;
        
        public void ShowBuySeedRow(ShopSeedData data)
        {
            this.m_image.color = data.Color;
            this.m_seedName.text = data.Name;
            this.m_costText.text = $"{data.BuyValue:0.00}";
            this.m_buyButton.onClick.AddListener(() =>
            {
                Shop.Instance.BuySeed(data);
            });
        }
    }
}