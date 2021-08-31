using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue;

    public int BaseValue
    {
        get { return baseValue; }
        set { baseValue = value; }
    }
}
