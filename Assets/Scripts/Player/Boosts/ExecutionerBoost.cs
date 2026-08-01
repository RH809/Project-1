/// <summary>
/// This is the scriptable object for the executioner boost.
/// </summary>
using UnityEngine;

[CreateAssetMenu(fileName = "Executioner Boost", menuName = "Scriptable Objects/Boosts/Executioner")]
public class ExecutionerBoost : Boost
{
    [SerializeField] private float executionIncrement;
    private float executeThreshold = 0f;
    public float ExecuteThreshold { get => executeThreshold; }

    public override string Description { get => (level == 0 ? boostDescription :
            $"Increase the execute threshold from {(executeThreshold * 100)}% to {((executeThreshold + executionIncrement) * 100)}%."); }

    public override void Select()
    {
        executeThreshold += executionIncrement;
        base.Select();
    }
}
