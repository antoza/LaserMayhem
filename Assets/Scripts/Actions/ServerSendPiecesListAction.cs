using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public class ServerSendPiecesListAction : GameAction
{
    // TODO : Faire une version avec l'id dans la liste des pièces qui peuvent apparaître
    public List<PieceName> PiecesList;

    public ServerSendPiecesListAction() : base()
    {
    }

    public ServerSendPiecesListAction(List<PieceName> pieceList) : base()
    {
        PiecesList = pieceList;
    }

    public override string SerializeAction()
    {
        string serializedAction = base.SerializeAction();
        foreach (PieceName pieceName in PiecesList)
        {
            serializedAction += "+" + pieceName;
        }
        return serializedAction;
    }

    public override bool DeserializeSubAction(Queue<string> parsedString)
    {
        base.DeserializeSubAction(parsedString);
        PiecesList = new List<PieceName>(parsedString.Count);
        try
        {
            while (parsedString.Count > 0)
            {
                bool result = Enum.TryParse(parsedString.Dequeue(), out PieceName pieceName);
                Assert.IsTrue(result);
                PiecesList.Add(pieceName);
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
