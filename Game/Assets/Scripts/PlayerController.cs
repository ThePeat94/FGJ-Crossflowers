using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Scriptables;
using UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController s_instance;

    [SerializeField] private PlayerData m_playerData;
    [SerializeField] private GameObject m_rake;
    [SerializeField] private GameObject m_waterCan;

    
    private ResourceController m_staminaController;
    private ResourceController m_waterController;
    private PlayerInventory m_playerInventory;
    
    private Vector3 m_moveDirection;
    private CharacterController m_characterController;
    private InputProcessor m_inputProcessor;
    private Animator m_animator;
    private GameObject m_currentInteractable;

    private IInteractable m_lastHit;
    
    private static readonly int s_isWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int s_waterAnimationTriggerHash = Animator.StringToHash("Water");
    private static readonly int s_shrugTriggerHash = Animator.StringToHash("Shrug");
    private static readonly int s_gatherTriggerHash = Animator.StringToHash("Gather");

    public static PlayerController Instance => s_instance;
    
    public ResourceController StaminaController => this.m_staminaController;
    public ResourceController WaterController => this.m_waterController;
    public PlayerInventory PlayerInventory => this.m_playerInventory;

    public void PlantSeed(Seed seed)
    {
        this.m_playerInventory.RemoveSeed(seed);
        this.m_staminaController.UseResource(this.m_playerData.PloughStaminaCost);
        this.StartCoroutine(this.PlayPloughAnimation());
        AudioSource.PlayClipAtPoint(this.m_playerData.RakingSoundEffect, Camera.main.transform.position, 0.33f);
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
        
        this.m_playerInventory = new PlayerInventory();
        this.m_inputProcessor = this.GetComponent<InputProcessor>();
        this.m_characterController = this.GetComponent<CharacterController>();
        this.m_animator = this.GetComponent<Animator>();
        this.m_staminaController = new ResourceController(this.m_playerData.StaminaData);
        this.m_waterController = new ResourceController(this.m_playerData.WaterData);
    }
    
    // Update is called once per frame
    void Update()
    {
        this.Move();
        this.Rotate();
        this.OutlineInteractionTarget();
        if (this.m_inputProcessor.InteractTriggered)
            this.Interact();

        if (this.m_currentInteractable != null && Mathf.Abs(Vector3.Distance(this.transform.position, this.m_currentInteractable.transform.position)) >= 3f)
        {
            PlayerHudUI.Instance.CloseAllMenus();
            this.m_currentInteractable = null;
        }
    }

    private void LateUpdate()
    {
        this.UpdateAnimator();
    }

    private void OutlineInteractionTarget()
    {
        try
        {
            var hitObjects = Physics.OverlapBox(this.transform.position + transform.forward, new Vector3(0.5f, 0.25f, 0.5f));
            var hitInteractable = hitObjects.FirstOrDefault(o => o.GetComponent<IInteractable>() != null);
            var hit = hitInteractable?.GetComponent<IInteractable>();

            if (hit == null && this.m_lastHit != null)
            {
                this.m_lastHit.DisableOutline();
                this.m_lastHit = null;
            }
            else if (hit != null && this.m_lastHit == null)
            {
                this.m_lastHit = hit;
                this.m_lastHit.EnableOutline();
            }
            else if (hit != this.m_lastHit)
            {
                this.m_lastHit.DisableOutline();
                this.m_lastHit = hit;
                this.m_lastHit.EnableOutline();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
    }
    
    protected void Move()
    {
        this.m_moveDirection = new Vector3(-this.m_inputProcessor.Movement.y, Physics.gravity.y, this.m_inputProcessor.Movement.x);
        this.m_characterController.Move(this.m_moveDirection * Time.deltaTime * this.m_playerData.MovementSpeed);

        if (this.m_moveDirection != Physics.gravity)
        {
            this.m_staminaController.UseResource(this.m_playerData.MovementStaminaCost);
        }
    }
        
    private void Rotate()
    {
        var targetDir = new Vector3(-this.m_inputProcessor.Movement.y, 0f, this.m_inputProcessor.Movement.x);

        if (targetDir == Vector3.zero)
            targetDir = this.transform.forward;
    
        this.RotateTowards(targetDir);
    }

    private void Interact()
    {
        var hitObjects = Physics.OverlapBox(this.transform.position + transform.forward, new Vector3(0.5f, 0.25f, 0.5f));
        try
        {
            var hitInteractable = hitObjects.First(o => o.GetComponent<IInteractable>() != null);

            if (hitInteractable != null)
            {
                var interactable = hitInteractable.GetComponent<IInteractable>();
                if (interactable is Field)
                {
                    var field = (Field) interactable;
                    this.InteractWithField(field);
                }
                else
                {
                    interactable.Interact();
                }
                this.m_currentInteractable = hitInteractable.gameObject;

            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private void InteractWithField(Field field)
    {
        if (field.CanHarvestFlower() && this.m_staminaController.UseResource(this.m_playerData.GatherStaminaCost))
        {
            field.Interact();
            this.StartCoroutine(this.PlayGatherAnimation());
        }
        else if (field.CanWaterField() && this.m_staminaController.CanAfford(this.m_playerData.WaterStaminaCost) && this.m_waterController.CanAfford(this.m_playerData.WaterFieldCost))
        {
            field.Interact();
            this.StartCoroutine(this.PlayWaterAnimation());
            this.m_staminaController.UseResource(this.m_playerData.WaterStaminaCost);
            this.m_waterController.UseResource(this.m_playerData.WaterFieldCost);
        }
        else if (field.CanPlantSeed() && this.m_staminaController.CanAfford(this.m_playerData.PloughStaminaCost) && this.m_playerInventory.Seeds.Count > 0)
        {
            field.Interact();
        }
        else
        {
            StartCoroutine(this.PlayShrugAnimation());
            if (!this.m_staminaController.CanAfford(this.m_playerData.GatherStaminaCost) || !this.m_staminaController.CanAfford(this.m_playerData.WaterStaminaCost) ||
                !this.m_staminaController.CanAfford(this.m_playerData.PloughStaminaCost))
            {
                PlayerHudUI.Instance.ShowPlayerMonologue("I am too exhausted to work anymore. I need some rest.");
            }
            else if (field.IsWatered && field.IsPlanted && !field.CanHarvestFlower())
            {
                PlayerHudUI.Instance.ShowPlayerMonologue("This field is watered and has a planted seed. I should give it a day to grow.");
            }
            else if (!field.IsWatered && !this.m_waterController.CanAfford(this.m_playerData.WaterFieldCost))
            {
                PlayerHudUI.Instance.ShowPlayerMonologue("My water can can't fulfill the needs of this field! I need to fill it up again.");
            }
            else if (this.m_playerInventory.Seeds.Count == 0)
            {
                if (field.CanGrowNewFlower())
                {
                    PlayerHudUI.Instance.ShowPlayerMonologue("I don't have any seeds I can plant! But it seems like something is growing here...");
                }
                else
                {
                    PlayerHudUI.Instance.ShowPlayerMonologue("I don't have any seeds I can plant! I should wait for other flowers to grow or buy some new seeds. The chest might have some for me.");
                }
            }
        }
    }

    private IEnumerator PlayShrugAnimation()
    {
        this.m_inputProcessor.enabled = false;
        this.m_animator.SetTrigger(s_shrugTriggerHash);
        yield return new WaitForSeconds(2f);
        this.m_inputProcessor.enabled = true;
    }

    private IEnumerator PlayGatherAnimation()
    {
        this.m_inputProcessor.enabled = false;
        this.m_animator.SetTrigger(s_gatherTriggerHash);
        yield return new WaitForSeconds(0.967f);
        this.m_inputProcessor.enabled = true;
    }

    private IEnumerator PlayWaterAnimation()
    {
        this.m_inputProcessor.enabled = false;
        this.m_waterCan.SetActive(true);
        this.m_animator.SetTrigger(s_waterAnimationTriggerHash);
        yield return new WaitForSeconds(1.5f);
        AudioSource.PlayClipAtPoint(this.m_playerData.WateringSoundEffect, Camera.main.transform.position, 0.33f);
        yield return new WaitForSeconds(1.5f);
        this.m_waterCan.SetActive(false);
        this.m_inputProcessor.enabled = true;
    }

    private IEnumerator PlayPloughAnimation()
    {
        this.m_inputProcessor.enabled = false;
        this.m_rake.SetActive(true);
        this.m_animator.SetTrigger("Plough");
        yield return new WaitForSeconds(3.025f);
        this.m_rake.SetActive(false);
        this.m_inputProcessor.enabled = true;
    }
    
    private void RotateTowards(Vector3 dir)
    {
        var lookRotation = Quaternion.LookRotation(dir.normalized);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, this.m_playerData.RotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        this.m_animator.SetBool(s_isWalkingHash, this.m_moveDirection != Physics.gravity);
    }


}
