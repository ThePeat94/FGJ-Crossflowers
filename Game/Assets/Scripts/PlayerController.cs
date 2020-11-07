using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Scriptables;
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
    
    private ResourceController m_staminaController;
    private ResourceController m_waterController;
    private PlayerInventory m_playerInventory;
    
    private Vector3 m_moveDirection;
    private CharacterController m_characterController;
    private InputProcessor m_inputProcessor;
    private Animator m_animator;

    private int m_isWalkingHash = Animator.StringToHash("IsWalking");

    public static PlayerController Instance => s_instance;
    
    public ResourceController StaminaController => this.m_staminaController;
    public ResourceController WaterController => this.m_waterController;

    public PlayerInventory PlayerInventory => this.m_playerInventory;

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
        if (this.m_inputProcessor.InteractTriggered)
            this.Interact();
    }

    private void LateUpdate()
    {
        this.UpdateAnimator();
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
                hitInteractable.GetComponent<IInteractable>().Interact();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
    
    private void RotateTowards(Vector3 dir)
    {
        var lookRotation = Quaternion.LookRotation(dir.normalized);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, this.m_rotationSpeed * Time.deltaTime);
    }

    private void UpdateAnimator()
    {
        this.m_animator.SetBool(this.m_isWalkingHash, this.m_moveDirection != Physics.gravity);
    }
}
