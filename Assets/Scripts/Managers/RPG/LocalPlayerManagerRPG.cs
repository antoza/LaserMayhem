using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class LocalPlayerManagerRPG : LocalPlayerManager
{
#if !DEDICATED_SERVER
    public static new LocalPlayerManagerRPG Instance => (LocalPlayerManagerRPG)LocalPlayerManager.Instance;

    protected override void VerifyAction(PlayerAction action)
    {
        if (GameModeManager.Instance.VerifyAction(action))
        {
            ((ClientSendActionsManager)SendActionsManager.Instance).SendAction(action);
        }
    }
#endif
}
