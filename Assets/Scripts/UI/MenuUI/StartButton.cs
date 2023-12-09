#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    [field: SerializeField]
    private string SceneName;

    // TODO : Mettre le minimum de code dans les scripts des boutons
    public override void DoOnClick()
    {
        if (SceneName != null)
        {
            SenderManager.Instance.SearchGame(SceneName);
        }
    }
}
#endif