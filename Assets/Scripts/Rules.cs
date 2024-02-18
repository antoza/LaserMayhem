using UnityEngine;

[CreateAssetMenu]
public class Rules : ScriptableObject
{
    [field: SerializeField]
    public int InitialHealth { get; private set; } = 15;
}
