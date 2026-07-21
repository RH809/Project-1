/// <summary>
/// This is the scriptable boject for the bazooka boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Bazooka Boost", menuName = "Scriptable Objects/Boosts/Bazooka")]
public class BazookaBoost : Boost
{
    [SerializeField] private float damageIncrement;
    private float damage = 0;
    public float Damage { get => damage; }

    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase the explosion damage from {damage} to {damage + damageIncrement}."); }

    public override void Select()
    {
        damage += damageIncrement;
        base.Select();
    }
}
