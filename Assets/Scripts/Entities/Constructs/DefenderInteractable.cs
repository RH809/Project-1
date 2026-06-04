using UnityEngine;

public class DefenderInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Defender defender;
    public Defender Defender { get => defender; }
    public IInteractable.InteractType GetInteractType()
    {
        return IInteractable.InteractType.DEFENDER;
    }

    public void Interact()
    {
        defender.Repair();
        Player.Instance.Inventory.UseRepairTool();
    }
}
