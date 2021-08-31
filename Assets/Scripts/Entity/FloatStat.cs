using UnityEngine;

[System.Serializable]
public class FloatStat
{
    [SerializeField]
    private float baseValue;

    public float BaseValue
    {
        get { return baseValue; }
        set { baseValue = value; }
    }
}
