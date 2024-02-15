using UnityEngine;

[CreateAssetMenu]
public class Rules : ScriptableObject
{
    [field: SerializeField]
    public int InitialHealth { get; private set; } = 15;
    [field: SerializeField]
    public int LaserPhaseDuration { get; private set; } = 3;
    [field: SerializeField]
    public int AnnouncementPhaseDuration { get; private set; } = 3;
}
