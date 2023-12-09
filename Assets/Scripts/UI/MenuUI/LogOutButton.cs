#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOutButton : MenuButton
{
    public override void DoOnClick()
    {
        SenderManager.Instance.LogOut();
    }
}
#endif