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
    public Tile? HeldSourceTile;
    [HideInInspector]
    public Tile? PreselectedSourceTile;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        LocalPlayer = PlayersManager.Instance.GetPlayer(GameInitialParameters.localPlayerID);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            ResetSourceTile();
        }
    }

    public bool TryToPlay()
    {
        if (!IsLocalPlayersTurn())
        {
            UIManager.Instance.DisplayErrorMessage("It is not your turn to play");
            return false;
        }
        if (!TurnManager.Instance.IsWaitingForPlayerAction) return false;
        return true;
    }

    public bool IsLocalPlayersTurn()
    {
        return PlayersManager.Instance.GetCurrentPlayer() == LocalPlayer;
    }

    public void SetSourceTile(Tile sourceTile)
    {
        if (!TryToPlay()) return;
        HeldSourceTile = sourceTile;
        PreselectedSourceTile = null;
        MouseFollower.ChangeFollowingTile(sourceTile);
    }

    public void ResetSourceTile()
    {
        HeldSourceTile = null;
        MouseFollower.ChangeFollowingTile(null);
    }

    public void SetPreselectedTile(Tile sourceTile)
    {
        if (!TryToPlay()) return;
        PreselectedSourceTile = sourceTile;
        // Afficher carr� rouge
    }

    public void ResetPreselectedTile()
    {
        PreselectedSourceTile = null;
        // D�safficher carr� rouge
    }

    protected virtual void VerifyAction(PlayerAction action) { }

    // Actions

    public virtual void CreateAndVerifyEndTurnAction()
    {
        EndTurnAction action = new EndTurnAction(LocalPlayer);
        VerifyAction(action);
    }

    public virtual void CreateAndVerifyMovePieceAction(Tile destinationTile)
    {
        Tile? sourceTile = HeldSourceTile ? HeldSourceTile : PreselectedSourceTile;
        if (sourceTile == null) return;
        MovePieceAction action = new MovePieceAction(LocalPlayer, sourceTile, destinationTile);
        VerifyAction(action);
    }

    public virtual void CreateAndVerifyUndoAction()
    {
        UndoAction action = new UndoAction(LocalPlayer);
        VerifyAction(action);
    }

    public virtual void CreateAndVerifyUndoEverythingAction()
    {
        UndoEverythingAction action = new UndoEverythingAction(LocalPlayer);
        VerifyAction(action);
    }
#endif
}