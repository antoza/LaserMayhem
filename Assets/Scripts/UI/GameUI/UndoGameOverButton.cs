using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UndoGameOverButton : MonoBehaviour
{
#if !DEDICATED_SERVER
    public void OnClick()
    {
        UIManagerGame.Instance.HideGameOverPopUps();
        GameModeManager.Instance.IsGameOver = false;
        LocalPlayerManager.Instance.CreateAndVerifyUndoAction();
    }
#endif
}
