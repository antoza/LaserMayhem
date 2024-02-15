using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public abstract class LocalPlayerManager : Manager<LocalPlayerManager>
{
#if !DEDICATED_SERVER
    public PlayerData LocalPlayer { get; private set; }

    [field: SerializeField]
    private MouseFollower MouseFollower;

    [HideInInspector]
    public Tile? SourceTile;

    private void Awake()
    {
        base.Awake();
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
        MouseFollower.ChangeFollowingTile(sourceTile);
    }

    public void ResetSourceTile()
    {
        SourceTile = null;
        MouseFollower.ChangeFollowingTile(null);
    }

    protected virtual void VerifyAction(PlayerAction action) { }

    // Actions

    public virtual void CreateAndVerifyEndTurnAction()
    {
        EndTurnAction action = new EndTurnAction(LocalPlayer);
        VerifyAction(action);
    }

    public void CreateAndVerifyMovePieceAction(Tile destinationTile)
    {
        if (SourceTile == null) return;
        MovePieceAction action = new MovePieceAction(LocalPlayer, SourceTile, destinationTile);
        VerifyAction(action);
    }

    public void CreateAndVerifyRevertLastActionAction()
    {
        RevertLastActionAction action = new RevertLastActionAction(LocalPlayer);
        VerifyAction(action);
    }

    public void CreateAndVerifyRevertAllActionsAction()
    {
        RevertAllActionsAction action = new RevertAllActionsAction(LocalPlayer);
        VerifyAction(action);
    }
#endif
}
