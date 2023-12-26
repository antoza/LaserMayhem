using System.Collections;
using TMPro;
using UnityEngine;

public class PreselectInputField : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    void OnEnable()
    {
        StartCoroutine(WaitForInputActivation());
    }

    public IEnumerator WaitForInputActivation()
    {
        yield return 0;
        inputField.ActivateInputField();
    }
}
