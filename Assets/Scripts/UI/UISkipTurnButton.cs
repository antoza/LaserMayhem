using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkipTurnButton : MonoBehaviour
{
    private TurnManager TurnManager;

    private void Update()
    {
        TurnManager = FindObjectOfType<TurnManager>();
    }


    public void OnClick()
    {
        TurnManager.TrySkipTurn();
    }
}
