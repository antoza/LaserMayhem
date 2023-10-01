using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class MovePieceAction : RevertableAction
{
    public Tile SourceTile { get; private set; }
    public Tile TargetTile { get; private set; }
    public Piece Piece { get; private set; }

    public MovePieceAction(DataManager dataManager, PlayerData playerData, Tile sourceTile, Tile targetTile, Piece piece) : base(dataManager, playerData)
    {
        SourceTile = sourceTile;
        TargetTile = targetTile;
        Piece = piece;
    }

    public override void Revert()
    {
        DM.GameMode.RevertMove(SourceTile, TargetTile, Piece, PlayerData);
    }

    [ServerRpc]
    public void AskServerRPC(Action sentAction)
    {
        Action receivedAction = RecreateAction(sentAction)
        TryMoveToDestinationTileServerRPC(SourceTile.name, TargetTile.name, PlayerData.m_playerID);
    }

    public static Action RecreateAction(int playerID, int sourceTileID, int targetTileID, int pieceID)
    {
        return new MovePieceAction(PlayersManager.GetPlayer(playerID), TilesManager.GetTile(sourceTileID), TilesManager.GetTile(targetTileID), PiecesManager.GetPiece(pieceID));
    }

    // TODO : GameObject.Find est temporaire, je pense ajouter un dictionnaire de tiles plus tard pour les référencer avec un id
    [ServerRpc(RequireOwnership = false)]
    public void TryMoveToDestinationTileServerRPC(string sourceTileName, string destinationTileName, int playerID, ServerRpcParams serverRpcParams = default)
    {
        // TODO : A changer d'endroit. L'Action doit exister avant d'arriver ici.
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        Tile sourceTile = GameObject.Find(sourceTileName).GetComponent<Tile>();
        Tile destinationTile = GameObject.Find(destinationTileName).GetComponent<Tile>();
        Piece? piece = sourceTile.m_Piece;

        if (!playerData.PlayerActions.m_CanPlay) return;
        if (DM.GameMode.MoveToDestinationTile(sourceTile, destinationTile, playerData))
        {
            MoveToDestinationTileClientRPC(sourceTileName, destinationTileName, playerID);
            DM.RewindManager.AddAction(new MovePieceAction(DM, playerData, sourceTile, destinationTile, piece!));
        }
    }

    [ClientRpc]
    private void MoveToDestinationTileClientRPC(string sourceTileName, string destinationTileName, int playerID)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        Tile sourceTile = GameObject.Find(sourceTileName).GetComponent<Tile>();
        Tile destinationTile = GameObject.Find(destinationTileName).GetComponent<Tile>();
        Piece? piece = sourceTile.m_Piece;

        DM.GameMode.MoveToDestinationTile(sourceTile, destinationTile, playerData);
        DM.RewindManager.AddAction(new MovePieceAction(DM, playerData, sourceTile, destinationTile, piece!));
    }

}
