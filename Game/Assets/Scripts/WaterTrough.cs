using UnityEngine;

public class WaterTrough : MonoBehaviour, IInteractable
{
    [SerializeField] private float m_fillWaterCost;
        
    public void Interact()
    {
        Debug.Log("Try resetting water...");
        if (PlayerController.Instance.StaminaController.UseResource(this.m_fillWaterCost))
        {
            PlayerController.Instance.WaterController.ResetValue();
        }
    }
}