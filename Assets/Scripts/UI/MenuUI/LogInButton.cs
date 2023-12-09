#if !DEDICATED_SERVER
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogInButton : MenuButton
{
    [field: SerializeField]
    private TMP_InputField UsernameInputField;
    [field: SerializeField]
    private TMP_InputField PasswordInputField;

    public override void DoOnClick()
    {
        string username = UsernameInputField.text;
        string password = PasswordInputField.text;
        SenderManager.Instance.LogIn(username, password);
    }
}
#endif