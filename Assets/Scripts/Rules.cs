using UnityEngine;

[CreateAssetMenu]
public class Rules : ScriptableObject
{
    [field: SerializeField]
    public int NumberOfPlayers { get; private set; } = 2;
    [field: SerializeField]
    public int InitialHealth { get; private set; } = 30;
    [field: SerializeField]
    public int SkipTurnCooldown { get; private set; } = 3;
}
