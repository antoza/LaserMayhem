using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RevertLastActionButton : MonoBehaviour
{
    public void OnClick()
    {
        PlayersManager.Instance.GetCurrentPlayer().PlayerActions.CreateAndVerifyRevertLastActionAction();
    }
}
