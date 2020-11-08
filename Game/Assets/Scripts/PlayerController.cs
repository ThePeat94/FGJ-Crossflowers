using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Scriptables;
using UI;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController s_instance;
    
    [SerializeField] private float m_movementSpeed;
    [SerializeField] private float m_rotationSpeed;
    [SerializeField] private float m_movementStaminaCost;
    [SerializeField] private ResourceData m_staminaData;
    [SerializeField] private ResourceData m_waterData;
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
    private static readonly int s_waterAnimationTrigger = Animator.StringToHash("Water");

    public static PlayerController Instance => s_instance;
    
    public ResourceController StaminaController => this.m_staminaController;
    public ResourceController WaterController => this.m_waterController;
    public PlayerInventory PlayerInventory => this.m_playerInventory;

    public void PlantSeed(Seed seed, float ploughingCost)
    {
        this.m_playerInventory.RemoveSeed(seed);
        this.m_staminaController.UseResource(ploughingCost);
        StartCoroutine(this.PlayPloughAnimation());
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
        this.m_staminaController = new ResourceController(this.m_staminaData);
        this.m_waterController = new ResourceController(this.m_waterData);
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
        this.m_characterController.Move(this.m_moveDirection * Time.deltaTime * this.m_movementSpeed);

        if (this.m_moveDirection != Physics.gravity)
        {
            this.m_staminaController.UseResource(this.m_movementStaminaCost);
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
                    if (!field.IsWatered && !field.CanHarvestFlower())
                    {
                        StartCoroutine(this.PlayWaterAnimation());
                    }
                    else if (field.CanHarvestFlower())
                    {
                        StartCoroutine(this.PlayGatherAnimation());
                    }
                }
                
                interactable.Interact();
                this.m_currentInteractable = hitInteractable.gameObject;

            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private IEnumerator PlayGatherAnimation()
    {
        this.m_inputProcessor.enabled = false;
        this.m_animator.SetTrigger("Gather");
        yield return new WaitForSeconds(0.967f);
        this.m_inputProcessor.enabled = true;
    }

    private IEnumerator PlayWaterAnimation()
    {
        this.m_inputProcessor.enabled = false;
        this.m_waterCan.SetActive(true);
        this.m_animator.SetTrigger(s_waterAnimationTrigger);
        yield return new WaitForSeconds(5.583f);
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
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, this.m_rotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        this.m_animator.SetBool(s_isWalkingHash, this.m_moveDirection != Physics.gravity);
    }


}
