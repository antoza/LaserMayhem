using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    [field: SerializeField]
    private bool IsServerButton = false;
    [field: SerializeField]
    private string SceneName = "Error";
    [field: SerializeField]
    private TextMeshProUGUI JoinCodeInputField;

    // TODO : Mettre le minimum de code dans les scripts des boutons
    public async override void ChangeMenu()
    {
        if (IsServerButton)
        {
            RelayManager.Instance.CreateRelay();
            await Task.Delay(3000);
            MenuMessageManager.StartServer(SceneName);
        }
        else
        {
            string JoinCode = JoinCodeInputField.text;
            RelayManager.Instance.JoinRelay(JoinCode.Remove(JoinCode.Length - 1));
            await Task.Delay(3000);
            MenuMessageManager.StartClient();
        }
    }
}
