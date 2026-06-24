using UnityEngine;

public class ShopInteractable : MonoBehaviour, IInteractable
{

    public IInteractable.InteractType GetInteractType()
    {
        return IInteractable.InteractType.SHOP;
    }

    public void Interact()
    {
        Debug.Log("Opening shop...");
        UIManager.Instance.SwitchState(UIManager.UIState.SHOP);
    }
}
