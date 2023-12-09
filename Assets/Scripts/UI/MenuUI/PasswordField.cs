#if !DEDICATED_SERVER
using TMPro;
using UnityEngine;

public class PasswordField : MonoBehaviour
{
    [field: SerializeField]
    private TMP_InputField PasswordInputField;
    //public Image ShowPassImgIcon, HidePassImgIcon;

    public void Start()
    {
        HidePassword();
    }

    public void ShowPassword()
    {
        //ShowPassImgIcon.enabled = false;
        //HidePassImgIcon.enabled = true;
        SetPasswordContentType(PasswordInputField, TMP_InputField.ContentType.Standard);
    }
    public void HidePassword()
    {
        //HidePassImgIcon.enabled = false;
        //ShowPassImgIcon.enabled = true;
        SetPasswordContentType(PasswordInputField, TMP_InputField.ContentType.Password);
    }
    private void SetPasswordContentType(TMP_InputField tmp_if, TMP_InputField.ContentType contentTypePassword)
    {
        tmp_if.contentType = contentTypePassword;
        tmp_if.DeactivateInputField();
        tmp_if.ActivateInputField();
    }
}
#endif