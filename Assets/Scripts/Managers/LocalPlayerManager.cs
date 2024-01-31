using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class LocalPlayerManager : MonoBehaviour
{
#if !DEDICATED_SERVER
    public static LocalPlayerManager Instance { get; private set; }

    public PlayerData LocalPlayer { get; private set; }
    [HideInInspector]
    public Tile? SourceTile;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LocalPlayer = PlayersManager.Instance.GetPlayer(GameInitialParameters.localPlayerID);
    }

    public bool TryToPlay()
    {
        if (!IsLocalPlayersTurn())
        {
            UIManager.Instance.DisplayErrorMessage("It is not your turn to play");
            return false;
        }
        return true;
    }

    public bool IsLocalPlayersTurn()
    {
        return PlayersManager.Instance.GetCurrentPlayer() == LocalPlayer;
    }

    public void SetSourceTile(Tile sourceTile)
    {
        if (!TryToPlay()) return;
        SourceTile = sourceTile;
        DataManager.Instance.MouseFollower.ChangeFollowingTile(sourceTile);
    }

    public void ResetSourceTile()
    {
        SourceTile = null;
        DataManager.Instance.MouseFollower.ChangeFollowingTile(null);
    }

    // Actions

    public void CreateAndVerifyEndTurnAction()
    {
        EndTurnAction action = new EndTurnAction(LocalPlayer);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

    public void CreateAndVerifyMovePieceAction(Tile destinationTile)
    {
        if (SourceTile == null) return;
        MovePieceAction action = new MovePieceAction(LocalPlayer, SourceTile, destinationTile);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

    public void CreateAndVerifyRevertLastActionAction()
    {
        RevertLastActionAction action = new RevertLastActionAction(LocalPlayer);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }

    public void CreateAndVerifyRevertAllActionsAction()
    {
        RevertAllActionsAction action = new RevertAllActionsAction(LocalPlayer);
        ((ClientSendActionsManager)SendActionsManager.Instance).VerifyAndSendAction(action);
    }
#endif
}
