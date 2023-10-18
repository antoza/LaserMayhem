using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public class ServerSendPiecesListAction : Action
{
    // TODO : Faire une version avec l'id dans la liste des pi�ces qui peuvent appara�tre
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
            Debug.Log(serializedAction);
        }
        return serializedAction;
    }

    public override bool DeserializeSubAction(Queue<string> parsedString)
    {
        PiecesList = new List<PieceName>(parsedString.Count);
        base.DeserializeSubAction(parsedString);
        try
        {
            while (parsedString.Count > 0)
            {
                PieceName pieceName;
                Assert.IsTrue(Enum.TryParse(parsedString.Dequeue(), out pieceName));
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
