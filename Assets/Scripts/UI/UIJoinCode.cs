using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class UIJoinCode : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_CodeText;

    public static async void SetCodeTextAsync(string joinCode)
    {
        while (FindObjectOfType<UIJoinCode>() == null)
        {
            await Task.Yield();
        }
        FindObjectOfType<UIJoinCode>().SetCodeText(joinCode);
    }

    public void SetCodeText(string joinCode)
    {
        m_CodeText.text = joinCode;
    }
}
