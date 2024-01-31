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
        foreach (char c in username + password)
        {
            if (!(char.IsLetter(c) || char.IsDigit(c) || password.Length < 8))
            {
                UIManager.Instance.DisplayErrorMessage("Incorrect username or password");
                return;
            }
        }
        SenderManager.Instance.LogIn(username, password);
    }
}
#endif