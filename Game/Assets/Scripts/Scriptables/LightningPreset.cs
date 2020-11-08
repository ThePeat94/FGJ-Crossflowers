using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Lightning Preset", menuName = "Data/Lightning Preset", order = 0)]
    public class LightningPreset : ScriptableObject
    {
        [SerializeField] private Gradient m_ambientColor;
        [SerializeField] private Gradient m_directionalColor;
        [SerializeField] private Gradient m_fogColor;

        public Gradient AmbientColor => this.m_ambientColor;
        public Gradient DirectionalColor => this.m_directionalColor;
        public Gradient FogColor => this.m_fogColor;
    }
}