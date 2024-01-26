#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLOpener : MenuButton
{
    [SerializeField]
    private string _link;

    public override void DoOnClick()
    {
        base.DoOnClick();
        Application.OpenURL(_link);
    }
}
#endif
