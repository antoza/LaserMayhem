using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable

public class UIManagerMenu : UIManager
{
#if !DEDICATED_SERVER
    public static new UIManagerMenu Instance => (UIManagerMenu)UIManager.Instance;

#endif
}
