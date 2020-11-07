using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Resource Upgrade", menuName = "Data/Resource Upgrade", order = 0)]
    public class ResourceUpgradeData : ScriptableObject
    {
        [SerializeField] private float m_increaseValue;
        [SerializeField] private float m_cost;

        public float IncreaseValue => this.m_increaseValue;
        public float Cost => this.m_cost;
    }
}