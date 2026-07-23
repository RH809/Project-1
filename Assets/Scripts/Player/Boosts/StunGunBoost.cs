/// <summary>
/// This is the scriptable boject for the hemorrhage boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Stun Gun Boost", menuName = "Scriptable Objects/Boosts/Stun Gun")]
public class StunGunBoost : Boost
{
    [SerializeField] private float stunDurationIncrement;
    private float stunDuration = 0.0f;

    public float StunDuration { get => stunDuration; }

    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase the stun duration from {stunDuration} seconds to {stunDuration + stunDurationIncrement} seconds."); }

    public override void Select()
    {
        stunDuration += stunDurationIncrement;
        base.Select();
    }
}
