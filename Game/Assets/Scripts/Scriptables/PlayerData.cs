using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Data/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private float m_ploughStaminaCost;
        [SerializeField] private float m_gatherStaminaCost;
        [SerializeField] private float m_waterStaminaCost;
        [SerializeField] private float m_waterFieldCost;
        [SerializeField] private float m_movementSpeed;
        [SerializeField] private float m_rotationSpeed;
        [SerializeField] private float m_movementStaminaCost;
        [SerializeField] private ResourceData m_staminaData;
        [SerializeField] private ResourceData m_waterData;
        [SerializeField] private AudioClip m_wateringSoundEffect;
        [SerializeField] private AudioClip m_rakingSoundEffect;
        
        public float PloughStaminaCost => this.m_ploughStaminaCost;
        public float GatherStaminaCost => this.m_gatherStaminaCost;
        public float WaterStaminaCost => this.m_waterStaminaCost;
        public float WaterFieldCost => this.m_waterFieldCost;
        public float MovementSpeed => this.m_movementSpeed;
        public float RotationSpeed => this.m_rotationSpeed;
        public float MovementStaminaCost => this.m_movementStaminaCost;
        public ResourceData StaminaData => this.m_staminaData;
        public ResourceData WaterData => this.m_waterData;
        public AudioClip WateringSoundEffect => this.m_wateringSoundEffect;
        public AudioClip RakingSoundEffect => this.m_rakingSoundEffect;
    }
}