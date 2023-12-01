using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogInButton : MonoBehaviour
{
    [field: SerializeField]
    private TMP_InputField UsernameInputField;
    [field: SerializeField]
    private TMP_InputField PasswordInputField;

    public void LogIn()
    {
        string username = UsernameInputField.text;
        string password = PasswordInputField.text;
        MenuMessageManager.Instance.SendRequest($"LogIn+{username}+{password}");
    }
}
