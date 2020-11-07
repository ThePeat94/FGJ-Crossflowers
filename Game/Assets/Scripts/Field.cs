using System;
using Helper;
using UI;
using UnityEngine;
using Random = System.Random;

public class Field : MonoBehaviour, IInteractable
{
    private static Random s_random = new Random();
    
    [SerializeField] private float m_ploughStaminaCost;
    [SerializeField] private float m_waterCost;
    [SerializeField] private float m_progressPerDay;
    [SerializeField] private Transform m_flowerTransform;
    [SerializeField] private MeshRenderer m_flowerRenderer;

    private bool m_isWatered;
    private Seed m_plantedSeed;

    private float m_currentProgress;

    private void Awake()
    {
        this.m_currentProgress = 0f;
        this.m_flowerTransform.localScale = Vector3.zero;
    }

    public void Interact()
    {
        if (this.CanHarvestFlower())
        {
            Debug.Log("Harvest harvest");
            var newSeed = new Seed(this.m_plantedSeed.Color, 10, FlowerData.GetRandomFlowerName());
            var rndAmount = s_random.Next(1, 5);

            Debug.Log($"Adding {rndAmount} new {newSeed.Name}s to inventory");

            for (var i = 0; i < rndAmount; i++)
                PlayerController.Instance.PlayerInventory.Seeds.Add(newSeed);
            
            this.m_currentProgress = 0f;
            this.m_flowerTransform.localScale = Vector3.zero;
            this.m_isWatered = false;
            this.m_plantedSeed = null;
            return;
        }
            
        if (this.CanWaterField())
        {
            Debug.Log("Water water");
            this.m_isWatered = true;
            return;
        }

        if (this.CanPlantSeed())
        {
            Debug.Log("Plant Plant");
            PlayerHudUI.Instance.ShowPlantSeedUi(this);
            return;
        }
    }

    public void ProcessCurrentDay()
    {

        if (this.CanGrow() && this.m_currentProgress < 1f)
        {
            this.m_currentProgress += this.m_progressPerDay;
        }
        else
        {
            this.m_currentProgress = 0f;
            this.m_plantedSeed = null;
        }

        this.m_flowerTransform.localScale = Vector3.one * this.m_currentProgress;
        this.m_isWatered = false;
    }

    public void PlantSeed(Seed seedToPlant)
    {
        if (seedToPlant != null)
        {
            this.m_plantedSeed = seedToPlant;
            this.m_flowerRenderer.material.color = seedToPlant.Color;
        }
    }
    
    private bool CanWaterField()
    {
        return !this.m_isWatered &&
               PlayerController.Instance.StaminaController.UseResource(this.m_ploughStaminaCost) &&
               PlayerController.Instance.WaterController.UseResource(this.m_waterCost);
    }

    private bool CanPlantSeed()
    {
        return this.m_isWatered && this.m_plantedSeed == null && PlayerController.Instance.StaminaController.UseResource(this.m_ploughStaminaCost);
    }

    private bool CanHarvestFlower()
    {
        return this.m_plantedSeed != null && this.m_currentProgress >= 1f && PlayerController.Instance.StaminaController.UseResource(this.m_ploughStaminaCost); 
    }

    private bool CanGrow()
    {
        return this.m_isWatered && this.m_plantedSeed != null;
    }
}