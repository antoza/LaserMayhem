using System;
using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

public class PlayersManagerShredder : PlayersManager
{
    public static new PlayersManagerShredder Instance => (PlayersManagerShredder)PlayersManager.Instance;
}
