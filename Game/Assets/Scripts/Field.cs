using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using QuickOutline.Scripts;
using UI;
using UnityEngine;
using Random = System.Random;

public class Field : MonoBehaviour, IInteractable
{
    private static Random s_random = new Random();
    
    [SerializeField] private float m_progressPerDay;
    [SerializeField] private Transform m_flowerTransform;
    [SerializeField] private MeshRenderer m_flowerRenderer;
    [SerializeField] private bool m_isUnlocked;

    [SerializeField] private Outline m_outline;

    [SerializeField] private Color m_normalColor;
    [SerializeField] private Color m_wateredColor;

    [SerializeField] private MeshRenderer m_fieldRenderer;
    [SerializeField] private GameObject m_newFlowerUi;

    private EventHandler<System.EventArgs> m_fieldHasBeenPlanted;

    private bool m_isWatered;
    private Seed m_plantedSeed;
    private float m_currentProgress;

    private List<Field> m_surroundingFields;

    public bool IsWatered => this.m_isWatered;
    public bool IsPlanted => this.m_plantedSeed != null;
    public Seed PlantedSeed => this.m_plantedSeed;
    public bool IsUnlocked => this.m_isUnlocked;

    private void Awake()
    {
        this.m_currentProgress = 0f;
        this.m_flowerTransform.localScale = Vector3.zero;
        this.m_surroundingFields = Farm.Instance.GetSurroundingFieldsForField(this);
        foreach (var surroundingField in this.m_surroundingFields)
        {
            surroundingField.m_fieldHasBeenPlanted += FieldHasBeenPlanted;
        }

        this.gameObject.SetActive(this.m_isUnlocked);
        this.DisableOutline();
    }

    private void FieldHasBeenPlanted(object sender, System.EventArgs e)
    {
        this.DisplayNewFlowerUI();
    }

    private void DisplayNewFlowerUI()
    {
        if (this.IsPlanted)
            return;
        
        if (this.m_surroundingFields.Count(f => f.CanGrow()) > 1)
        {
            m_newFlowerUi.SetActive(true);
        }
    }

    public void Interact()
    {
        if (!this.m_isUnlocked)
            return;

        if (this.CanHarvestFlower())
        {
            Debug.Log("Harvest harvest");
            var newSeed = new Seed(this.m_plantedSeed.Color, 2.25f, FlowerData.GetRandomFlowerName());
            var rndAmount = s_random.Next(2, 5);

            PlayerHudUI.Instance.ShowPlayerMonologue($"Yippie! I got {rndAmount} seeds for {newSeed.Name}. 😀");

            for (var i = 0; i < rndAmount; i++)
                PlayerController.Instance.PlayerInventory.AddSeed(newSeed);

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
            StartCoroutine(this.DelayColoringField());
            return;
        }

        if (this.CanPlantSeed())
        {
            Debug.Log("Plant Plant");
            PlayerHudUI.Instance.ShowPlantSeedUI(this);
            return;
        }
    }

    private IEnumerator DelayColoringField()
    {
        yield return new WaitForSeconds(1.75f);
        this.m_fieldRenderer.material.color = this.m_wateredColor;
    }

    public void EnableOutline()
    {
        this.m_outline.enabled = true;
    }

    public void DisableOutline()
    {
        this.m_outline.enabled = false;
    }

    public void ProcessGrowth()
    {
        if (!this.m_isUnlocked)
            return;

        if (this.CanGrowNewFlower())
        {
            var surroundingSeeds = this.m_surroundingFields.Where(f => f.IsPlanted && f.IsUnlocked).Select(f => f.PlantedSeed).ToList();
            var targetColor = surroundingSeeds.First().Color;
            var t = 1f / surroundingSeeds.Count;
            foreach (var seed in surroundingSeeds)
            {
                targetColor = Color.Lerp(targetColor, seed.Color, t);
            }

            var newSeed = new Seed(targetColor, 3.25f * surroundingSeeds.Count, FlowerData.GetRandomFlowerName());
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
        if (!this.m_isUnlocked)
            return;
        this.m_isWatered = false;
        this.m_fieldRenderer.material.color = this.m_normalColor;
        
        this.DisplayNewFlowerUI();
    }

    public void PlantSeed(Seed seedToPlant)
    {
        if (this.m_plantedSeed == null)
        {
            this.m_plantedSeed = seedToPlant;
            this.m_flowerRenderer.material.color = seedToPlant.Color;
            this.m_flowerTransform.localScale = Vector3.one / 10;
            this.m_fieldHasBeenPlanted?.Invoke(this, System.EventArgs.Empty);
            this.m_newFlowerUi.SetActive(false);
        }
    }

    public bool CanWaterField()
    {
        return !this.m_isWatered;
    }

    public bool CanPlantSeed()
    {
        return this.m_isWatered && !this.IsPlanted;
    }

    public bool CanHarvestFlower()
    {
        return this.IsPlanted && this.m_currentProgress >= 1f;
    }

    public bool CanGrow()
    {
        return this.m_isWatered && this.IsPlanted;
    }

    public bool CanGrowNewFlower()
    {
        return this.m_isWatered && !this.IsPlanted && this.m_surroundingFields.Count(f => f.CanGrow()) > 1;
    }
    
    public void Unlock()
    {
        this.m_isUnlocked = true;
        this.gameObject.SetActive(true);
    }
}