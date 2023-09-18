using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSwitchPlayer : MonoBehaviour
{
    [field: SerializeField]
    private DataManager DM;

#if !DEBUG
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
#endif

#if DEBUG
    public void OnClick()
    {
        GameInitialParameters.localPlayerID = (GameInitialParameters.localPlayerID + 1) % DM.Rules.NumberOfPlayers;
        Debug.Log("DEBUG : You are playing as player " + DM.PlayersManager.GetLocalPlayer().m_name);
    }
#endif
}
    