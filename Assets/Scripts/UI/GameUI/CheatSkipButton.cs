using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSkipButton : MonoBehaviour
{
    public void OnClick()
    {
        GameModeManager.Instance.TriggerGameOver(0);
    }
}
