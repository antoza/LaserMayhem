using UnityEngine;

public class Rules : ScriptableObject
{
    [field: SerializeField]
    public int NumberOfPlayers { get; private set; } = 2;
    public int InitialHealth { get; private set; } = 30;
}
