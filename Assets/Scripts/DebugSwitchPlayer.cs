using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugSwitchPlayer : MonoBehaviour
{
    [field: SerializeField]
    private DataManager DM;
    [field: SerializeField]
    private TextMeshProUGUI m_PlayerText;

#if !DEBUG
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
#endif

#if DEBUG
    public void OnClick()
    {
        int currentPlayer = (GameInitialParameters.localPlayerID + 1) % DM.Rules.NumberOfPlayers;
        GameInitialParameters.localPlayerID = currentPlayer;
        m_PlayerText.text = "P" + currentPlayer;
    }
#endif
}
    