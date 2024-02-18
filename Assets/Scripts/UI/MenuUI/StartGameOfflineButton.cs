#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameOfflineButton : MenuButton
{
    [field: SerializeField]
    private string SceneName;

    public override void DoOnClick()
    {
        if (SceneName != null)
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
#endif