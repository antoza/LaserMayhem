using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu]
public class SkinData : ScriptableObject
{
    [SerializedDictionary("Skin Names", "Sprite")]
    public SerializedDictionary<string, Sprite> BackgroundSkin;
}
