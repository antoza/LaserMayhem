using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRevertLastAction : MonoBehaviour
{
    public void OnClick()
    {
        PlayersManager.Instance.GetCurrentPlayer().PlayerActions.CreateAndVerifyRevertLastActionAction();
    }
}
