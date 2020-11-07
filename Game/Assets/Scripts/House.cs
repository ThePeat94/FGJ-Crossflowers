using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Rest and process current day");
        PlayerController.Instance.StaminaController.ResetValue();
        GameManager.Instance.ProcessCurrentDay();
    }
}