using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    [field: SerializeField]
    /*private*/public bool IsServerButton = false;
    [field: SerializeField]
    private string SceneName = "Error";
    [field: SerializeField]
    private TextMeshProUGUI JoinCodeInputField;

    // TODO : Mettre le minimum de code dans les scripts des boutons
    public async override void ChangeMenu()
    {
        if (IsServerButton)
        {
            await RelayManager.Instance.CreateRelay();
            MenuMessageManager.Instance.StartServer(SceneName);
        }
        else
        {
            MenuMessageManager.Instance.SendRequest("SearchGame+RPG");
            /*
            string JoinCode = JoinCodeInputField.text;
            await RelayManager.Instance.JoinRelay(JoinCode.Remove(JoinCode.Length - 1));
            MenuMessageManager.Instance.StartClient();*/
        }
    }
}
