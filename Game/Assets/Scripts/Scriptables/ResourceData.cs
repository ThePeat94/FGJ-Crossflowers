using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Resource Data", menuName = "Data/Resource Data", order = 0)]
    public class ResourceData : ScriptableObject
    {
        [SerializeField] private float m_initMaxValue;

        public float InitMaxValue => this.m_initMaxValue;
    }
}