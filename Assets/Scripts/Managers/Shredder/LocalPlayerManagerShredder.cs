using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class LocalPlayerManagerShredder : LocalPlayerManager
{
#if !DEDICATED_SERVER
    public static new LocalPlayerManagerShredder Instance => (LocalPlayerManagerShredder)LocalPlayerManager.Instance;

    protected override void VerifyAction(PlayerAction action)
    {
        if (GameModeManager.Instance.VerifyAction(action))
        {
            GameModeManager.Instance.ExecuteAction(action);
        }
    }
#endif
}
