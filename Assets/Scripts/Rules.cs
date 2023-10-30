using UnityEngine;

[CreateAssetMenu]
public class Rules : ScriptableObject
{
    [field: SerializeField]
    public int NumberOfPlayers { get; private set; } = 2;
    [field: SerializeField]
    public string GameModeName { get; private set; } = "RPG";
    [field: SerializeField]
    public int InitialHealth { get; private set; } = 15;
    [field: SerializeField]
    public int SkipTurnCooldown { get; private set; } = 3;
    [field: SerializeField]
    public int LaserCooldown { get; private set; } = 3;
    [field: SerializeField]
    public int BoardWidth { get; private set; } = 7;
    [field: SerializeField]
    public int BoardHeight { get; private set; } = 7;
    [field: SerializeField]
    public float BoardScaleWidth { get; private set; } = 3.5f;
    [field: SerializeField]
    public float BoardScaleHeight { get; private set; } = 3.5f;
    [field: SerializeField]
    public GameObject TilePrefab;
}
