using UnityEngine;

[System.Serializable]
public class StateWeight
{
    public StateType type;
    [Range(0f, 100f)] public float weight = 33f;
}