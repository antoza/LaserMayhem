#if !DEDICATED_SERVER
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpButton : MenuButton
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
            if (!(char.IsLetter(c) || char.IsDigit(c)))
            {
                UIManager.Instance.DisplayErrorMessage("Please only use letters and digits in your credentials");
                return;
            }
        }
        if (password.Length < 8 ) {
            UIManager.Instance.DisplayErrorMessage("Please use a password that contains at least 8 characters");
            return;
        }
        SenderManager.Instance.SignUp(username, password);
    }
}
#endif