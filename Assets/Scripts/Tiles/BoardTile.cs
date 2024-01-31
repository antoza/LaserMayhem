using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class BoardTile : Tile
{
    public Vector2Int Spot { get; set; }

    private Dictionary<Vector2Int, Laser?> _displayedLasers = new Dictionary<Vector2Int, Laser?>() {
        { Vector2Int.right, null },
        { Vector2Int.left, null },
        { Vector2Int.up, null },
        { Vector2Int.down, null }
    };

    protected virtual void InitTilePositions()
    {
        //if (!belongsToBoard) return;
        int sign = GameInitialParameters.localPlayerID == 1 ? -1 : 1;
        transform.position = sign * (Vector2.right * Spot.x + Vector2.up * Spot.y);
        //transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
    }

    /* TODO : Faire comme ça un jour
    public void DisplayLaser(LaserName laserName, Direction direction)
    {
        Laser? laser = LaserPrefabs.GetLaser(laserName)?.Instantiate();
        Laser? oldLaser = _displayedLasers[direction];
        if (oldLaser != null) Destroy(oldLaser);
        _displayedLasers[direction] = laser;
    }*/

    public void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        if (Piece) Piece!.ReceiveLaser(laser, inDirection);
        else TransferLaser(laser, inDirection);
    }

    public void TransferLaser(Laser? laser, Vector2Int outDirection)
    {
        // if (laserPrefab et _displayedLasers[outDirection] sont les mêmes laser)
        DisplayLaser(laser, outDirection);
        BoardManager.Instance.GetBoardTile(Spot + outDirection)?.ReceiveLaser(laser, outDirection);
    }

    private void DisplayLaser(Laser? laser, Vector2Int outDirection)
    {
        Laser? oldLaser = _displayedLasers[outDirection];
        if (oldLaser != null) Destroy(oldLaser.gameObject);
        Laser? newLaser = null;
        if (laser != null)
        {
            newLaser = Instantiate(laser, transform);
            // TODO : généraliser ce calcul de rotation de sprite aux tiles et aux pièces
            int displayRotation = (outDirection.x - Math.Abs(outDirection.x)) * 90 + outDirection.y * 90;
            Vector2 displayTranslation = (Vector2)outDirection / 2;
            if (GameInitialParameters.localPlayerID == 1)
            {
                displayRotation += 180;
                displayTranslation *= -1;
            }
            newLaser.transform.Translate(displayTranslation);
            newLaser.transform.Rotate(new Vector3(0, 0, displayRotation));

        }
        _displayedLasers[outDirection] = newLaser;
    }






    /*

    [SerializedDictionary("Direction", "Laser Sprite")]
    public SerializedDictionary<Direction, Laser> LaserParts;
    // C'est le dico ou tu mets tes LaserParts (qui doivent être sur des Gameobject fils de la tile).

    protected override void Start()
    {
        base.Start();
        TurnManager.DestroyLaser += DestroyLaser;
    }

    //Faut override le Start dans Tile.cs

    private void DestroyLaser()
    {
        foreach (var paire in LaserParts)
        {
            paire.Value.gameObject.SetActive(false);
        }
    }

    public void PropagateLaser(bool prediction, Direction direction)
    {
        if (!Piece)
        {
            LaserParts[direction].gameObject.SetActive(true);
            LaserParts[direction].StartLaser(prediction);
            if (BoardManager.Instance.IsOnBoard(new Vector2Int(x, y) + LaserManager.Instance.DirectionEnumToVector(direction)))
            {
                Tile? nextTile = BoardManager.Instance.GetTile(new Vector2Int(x, y) + LaserManager.Instance.DirectionEnumToVector(direction));
                if (nextTile)
                {
                    NormalBoardTile boardTile = (NormalBoardTile)nextTile!;
                    if (boardTile)
                    {
                        boardTile.PropagateLaser(prediction, direction);
                    }
                }
            }

        }
        else
        {
            IEnumerable<Vector2Int> newDirectionsVector = Piece.ComputeNewDirections(LaserManager.Instance.DirectionEnumToVector(direction));

            foreach (Vector2Int newDirectionVector in newDirectionsVector)
            {
                Direction newDirection = LaserManager.Instance.VectorToDirectionEnum(newDirectionVector);
                if (!LaserParts[newDirection].gameObject.activeSelf)
                {
                    LaserParts[newDirection].gameObject.SetActive(true);
                    LaserParts[newDirection].StartLaser(prediction);
                    if (BoardManager.Instance.IsOnBoard(new Vector2Int(x, y) + LaserManager.Instance.DirectionEnumToVector(newDirection)))
                    {
                        Tile? nextTile = BoardManager.Instance.GetTile(new Vector2Int(x, y) + LaserManager.Instance.DirectionEnumToVector(newDirection));

                        if (nextTile)
                        {
                            NormalBoardTile boardTile = (NormalBoardTile)nextTile!;
                            if (boardTile)
                            {
                                boardTile.PropagateLaser(prediction, newDirection);
                            }
                        }
                    }
                }
            }
        }
    }*/
}