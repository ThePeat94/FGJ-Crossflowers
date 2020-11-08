using System;
using System.Collections;
using System.Collections.Generic;
using EventArgs;
using TMPro;
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
        [SerializeField] private ShopUI m_shopUI;
        [SerializeField] private Animator m_animator;
        [SerializeField] private TextMeshProUGUI m_dayTime;
        [SerializeField] private DayTimeManager m_dayTimeManager;
        [SerializeField] private MonologueUI m_monologueUI;

        public static PlayerHudUI Instance => s_instance;

        public void ShowPlantSeedUI(Field targetField)
        {
            this.m_plantSeedUi.Show(targetField);
        }

        public void ClosePlantSeedUI()
        {
            this.m_plantSeedUi.CloseUI();
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
            PlayerController.Instance.StaminaController.MaxValueChanged += (sender, args) => this.m_staminaSlider.maxValue = args.NewValue;
            PlayerController.Instance.WaterController.MaxValueChanged += (sender, args) => this.m_waterSlider.maxValue = args.NewValue;
            
            this.CloseAllMenus();
        }

        private void LateUpdate()
        {
            this.m_dayTime.text = this.m_dayTimeManager.GetDateTime();
        }

        public void CloseAllMenus()
        {
            this.m_plantSeedUi.CloseUI();
            this.m_shopUI.CloseUI();
            this.m_monologueUI.CloseUI();
        }

        public void ShowShopUI()
        {
            this.m_shopUI.ShowUI();
        }
        
        private void WaterControllerOnResourceValueChanged(object sender, ResourceValueChangedEventArgs e)
        {
            this.m_waterSlider.value = e.NewValue;
        }

        private void StaminaControllerOnResourceValueChanged(object sender, ResourceValueChangedEventArgs e)
        {
            this.m_staminaSlider.value = e.NewValue;
        }

        public void PlayFadeAnimation()
        {
            this.CloseAllMenus();
            this.m_animator.Play("UIFade");
        }

        public void ShowPlayerMonologue(string text)
        {
            this.m_monologueUI.ShowMonologue(text);
        }
    }
}