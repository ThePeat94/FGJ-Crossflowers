using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SellSeedRowUI : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private TextMeshProUGUI m_valueText;
        [SerializeField] private Button m_button;


        public void ShowSeed(Seed seed)
        {
            this.m_image.color = seed.Color;
            this.m_name.text = seed.Name;
            this.m_valueText.text = $"{seed.SellValue:0.00}";
            this.m_button.onClick.AddListener(() =>
            {
                Shop.Instance.SellSeed(seed);
            });
        }
    }
}