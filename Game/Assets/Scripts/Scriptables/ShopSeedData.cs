using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "Flower Data", menuName = "Data/Seed Data", order = 0)]
    public class ShopSeedData : ScriptableObject
    {
        [SerializeField] private string m_name;
        [SerializeField] private int m_buyValue;
        [SerializeField] private Sprite m_icon;
        [SerializeField] private Color m_color;
    }
}