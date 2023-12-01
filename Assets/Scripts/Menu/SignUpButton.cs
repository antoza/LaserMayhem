using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpButton : MonoBehaviour
{
    [field: SerializeField]
    private TMP_InputField UsernameInputField;
    [field: SerializeField]
    private TMP_InputField PasswordInputField;

    public void SignUp()
    {
        string username = UsernameInputField.text;
        string password = PasswordInputField.text;
        MenuMessageManager.Instance.SendRequest($"SignUp+{username}+{password}");
    }
}
