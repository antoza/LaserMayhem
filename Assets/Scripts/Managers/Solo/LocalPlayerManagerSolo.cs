using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class LocalPlayerManagerSolo : LocalPlayerManager
{
#if !DEDICATED_SERVER
    public static new LocalPlayerManagerSolo Instance => (LocalPlayerManagerSolo)LocalPlayerManager.Instance;

    protected override void VerifyAction(PlayerAction action)
    {
        if (GameModeManager.Instance.VerifyAction(action))
        {
            GameModeManager.Instance.ExecuteAction(action);
        }
    }
#endif
}
