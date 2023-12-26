using System.Collections;
using TMPro;
using UnityEngine;

public class AutoClearInputField : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    void OnDisable()
    {
        inputField.Select();
        inputField.text = "";
    }
}
