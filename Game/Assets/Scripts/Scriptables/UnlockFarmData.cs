using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Unlock Farm Data", menuName = "Data/Unlock Farm Data", order = 0)]
    public class UnlockFarmData : ScriptableObject
    {
        [SerializeField] private float m_cost;

        public float Cost => this.m_cost;
    }
}