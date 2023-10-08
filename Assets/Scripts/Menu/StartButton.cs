using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    [field: SerializeField]
    private bool IsServerButton = false;
    [field: SerializeField]
    private string SceneName = "Error";
    public override void ChangeMenu()
    {
        if (IsServerButton) 
        {
            MenuMessageManager.StartServer(SceneName);
        }
        else
        {
            MenuMessageManager.StartClient();
        }
    }
}
