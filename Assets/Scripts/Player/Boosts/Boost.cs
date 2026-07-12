/// <summary>
/// This script acts as the parent scriptable object for the boosts.
/// </summary>
using UnityEngine;

public abstract class Boost : ScriptableObject
{
    [SerializeField] protected string boostName;
    public virtual string Name { get => (levelCap != -1 ? boostName + " " + NumToRomanNumeral(level + 1) : boostName); }
    public string CurrentName { get => boostName + " " + NumToRomanNumeral(level); }
    [SerializeField] protected string boostDescription;
    public virtual string Description { get => boostDescription; }
    [SerializeField] protected int levelCap;
    protected int level = 0;
    public bool IsPermanent { get => levelCap != -1; }
    public bool IsActive { get => level > 0; }

    public bool IsMaxedOut { get => (levelCap != -1 && level >= levelCap); }
    public virtual void Select()
    {
        if (IsPermanent)
        {
            level++;
            if (level == 1)
            {
                Player.Instance.Boosts.SelectNewBoost(this);
            }
        }
    }

    static string NumToRomanNumeral(int num)
    {
        switch (num)
        {
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";
            case 5:
                return "V";
            default:
                return num.ToString();
        }
    }
}
