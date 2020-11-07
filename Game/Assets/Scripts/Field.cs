using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject m_center;

    private bool m_isWatered;
    private Seed m_plantedSeed;
    private bool m_isUnlocked;

    private float m_currentProgress;

    [SerializeField]
    private List<Field> m_surroundingFields;

    public bool IsWatered => this.m_isWatered;
    public bool IsPlanted => this.m_plantedSeed != null;
    public Seed PlantedSeed => this.m_plantedSeed;

    public float PloughStaminaCost => this.m_ploughStaminaCost;

    private void Awake()
    {
        this.m_currentProgress = 0f;
        this.m_flowerTransform.localScale = Vector3.zero;
        this.InitSurroundingFields();
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

    public void ProcessGrowth()
    {
        if (this.CanGrowNewFlower())
        {
            var surroundingSeeds = this.m_surroundingFields.Where(f => f.IsPlanted).Select(f => f.PlantedSeed).ToList();

            if (surroundingSeeds.Count < this.m_surroundingFields.Count / 2)
                return;
            
            var targetColor = surroundingSeeds.First().Color;
            var t = 1f / surroundingSeeds.Count;
            foreach (var seed in surroundingSeeds)
            {
                targetColor = Color.Lerp(targetColor, seed.Color, t);
            }
            
            var newSeed = new Seed(targetColor, 10, FlowerData.GetRandomFlowerName());

            Debug.Log("New Color is " + targetColor);

            this.PlantSeed(newSeed);
        }
        else if (this.CanGrow() && this.m_currentProgress < 1f)
        {
            this.m_currentProgress += this.m_progressPerDay;
            this.m_flowerTransform.localScale = Vector3.one * this.m_currentProgress;
        }
        else
        {
            this.m_currentProgress = 0f;
            this.m_plantedSeed = null;
            this.m_flowerTransform.localScale = Vector3.zero;
        }
    }

    public void ProcessNewDay()
    {
        this.m_isWatered = false;
    }

    public void PlantSeed(Seed seedToPlant)
    {
        if (this.m_plantedSeed == null)
        {
            this.m_plantedSeed = seedToPlant;
            this.m_flowerRenderer.material.color = seedToPlant.Color;
            this.m_flowerTransform.localScale = Vector3.one / 10;
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
        return this.m_isWatered && !this.IsPlanted && PlayerController.Instance.StaminaController.CanAfford(this.m_ploughStaminaCost);
    }

    private bool CanHarvestFlower()
    {
        return this.IsPlanted && this.m_currentProgress >= 1f && PlayerController.Instance.StaminaController.UseResource(this.m_ploughStaminaCost); 
    }

    private bool CanGrow()
    {
        return this.m_isWatered && this.IsPlanted;
    }

    private bool CanGrowNewFlower()
    {
       return this.m_isWatered && !this.IsPlanted && this.m_surroundingFields.Count(f => f.CanGrow()) > 1;
    }

    private void InitSurroundingFields()
    {
        this.m_surroundingFields = new List<Field>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if(i == 0 && j == 0)
                    continue;

                var hitField = this.GetFieldForDirection(new Vector3(i, 0f, j));

                if (hitField != null)
                {
                    this.m_surroundingFields.Add(hitField);
                }
            }
        }
    }
    
    private Field GetFieldForDirection(Vector3 direction)
    {
        if (Physics.Raycast(this.m_center.transform.position, direction, out var hitInfo, 10f, 1 << LayerMask.NameToLayer("Field")))
        {
            return hitInfo.collider.GetComponent<Field>();
        }
        return null;
    }
}