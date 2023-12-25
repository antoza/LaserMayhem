#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CancelMatchmakingButton : MenuButton
{
    public override void DoOnClick()
    {
        SenderManager.Instance.CancelMatchmaking();
    }
}
#endif