using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : comprendre où c'est utilisé + le placer ailleurs que dans le dossier UI

public class MenuPosUpdate : MonoBehaviour
{
    [field: SerializeField]
    private Vector3 PositionPlayer0;

    [field: SerializeField]
    private Vector3 PositionPlayer1;
    void Start()
    {
        RectTransform RectTransform = GetComponent<RectTransform>();
        if(!transform)
        {
            Debug.Log("This is not UI Object");
        }
        if (GameInitialParameters.localPlayerID == 0)
        {
            RectTransform.localPosition = PositionPlayer0;
        }
        else if(GameInitialParameters.localPlayerID == 1)
        {
            RectTransform.localPosition = PositionPlayer1;
        }
    }
}
