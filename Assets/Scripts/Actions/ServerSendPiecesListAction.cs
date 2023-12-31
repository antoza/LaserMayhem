using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public class ServerSendPiecesListAction : Action
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
        PiecesList = new List<PieceName>(parsedString.Count);
        base.DeserializeSubAction(parsedString);
        try
        {
            while (parsedString.Count > 0)
            {
                PieceName pieceName = PieceName.None;
                // TODO : YA UN PROBLEME A CETTE LIGNE MAIS QUE SUR LE BUILD !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                Debug.Log(parsedString.Peek());
                Assert.IsTrue(Enum.TryParse(parsedString.Dequeue(), out pieceName));
                //Debug.Log(pieceName);
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
