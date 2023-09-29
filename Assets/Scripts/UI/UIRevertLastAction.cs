using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRevertLastAction : MonoBehaviour
{
    private DataManager DM;

    private void Awake()
    {
        DM = FindObjectOfType<DataManager>();
    }

    public void OnClick()
    {
        DM.RewindManager.PrepareRevertLastAction();
    }
}
