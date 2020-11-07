using System;
using EventArgs;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerHudUI : MonoBehaviour
    {
        private static PlayerHudUI s_instance;
        
        [SerializeField] private Slider m_staminaSlider;
        [SerializeField] private Slider m_waterSlider;
        [SerializeField] private PlantSeedUI m_plantSeedUi;

        public static PlayerHudUI Instance => s_instance;

        public void ShowPlantSeedUi(Field targetField)
        {
            this.m_plantSeedUi.Show(targetField);
        }

        public void ClosePlantSeedUi()
        {
            this.m_plantSeedUi.CloseUi();
        }
        
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
            
            this.m_staminaSlider.maxValue = PlayerController.Instance.StaminaController.MaxValue;
            this.m_waterSlider.maxValue = PlayerController.Instance.WaterController.MaxValue;
            this.m_staminaSlider.value = PlayerController.Instance.StaminaController.CurrentValue;
            this.m_waterSlider.value = PlayerController.Instance.WaterController.CurrentValue;
            PlayerController.Instance.StaminaController.ResourceValueChanged += StaminaControllerOnResourceValueChanged;
            PlayerController.Instance.WaterController.ResourceValueChanged += WaterControllerOnResourceValueChanged;
            
            this.m_plantSeedUi.CloseUi();
        }

        private void WaterControllerOnResourceValueChanged(object sender, ResourceValueChangedEventArgs e)
        {
            this.m_waterSlider.value = e.NewValue;
        }

        private void StaminaControllerOnResourceValueChanged(object sender, ResourceValueChangedEventArgs e)
        {
            this.m_staminaSlider.value = e.NewValue;
        }
    }
}