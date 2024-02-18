using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RevertLastActionButton : MonoBehaviour
{
#if !DEDICATED_SERVER
    public void OnClick()
    {
        LocalPlayerManager.Instance.CreateAndVerifyRevertLastActionAction();
    }
#endif
}
