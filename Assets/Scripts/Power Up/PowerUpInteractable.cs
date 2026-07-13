using UnityEngine;

public class PowerUpInteractable : MonoBehaviour, IInteractable
{

    public IInteractable.InteractType GetInteractType()
    {
        return IInteractable.InteractType.POWER_UP;
    }

    public void Interact()
    {
        //Debug.Log("Grabbing Power Up...");
        Player.Instance.PowerUp.Activate();
        PowerUpManager.Instance.Respawn();
    }
}
