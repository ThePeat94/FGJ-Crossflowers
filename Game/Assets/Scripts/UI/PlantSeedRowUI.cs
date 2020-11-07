using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlantSeedRowUI : MonoBehaviour
    {
        [SerializeField] private Image m_image;
        [SerializeField] private TextMeshProUGUI m_name;
        [SerializeField] private Button m_button;

        public void ShowSeed(Seed seed, Field targetField)
        {
            this.m_image.color = seed.Color;
            this.m_name.text = seed.Name;
            this.m_button.onClick.AddListener(() =>
            {
                targetField.PlantSeed(seed);
                PlayerHudUI.Instance.ClosePlantSeedUI();
                PlayerController.Instance.PlantSeed(seed, targetField.PloughStaminaCost);
            });
        }
    }
}