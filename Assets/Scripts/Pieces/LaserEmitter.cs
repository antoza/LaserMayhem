using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : Piece
{

    [SerializeField]
    private Direction StartingDirection;
    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        yield return LaserManager.Instance.DirectionEnumToVector(StartingDirection);
    }

    //TODO : Events may be renamed
    private void Start()
    {
        TurnManager.OnEndTurn += OnEndTurn;
        TurnManager.OnEndLaserPhase += OnEndLaserPhase;
    }

    private void OnEndTurn()
    {
        StartLaser(false);
    }

    private void OnEndLaserPhase()
    {
        StartLaser(true);
    }

    private void StartLaser(bool prediction)
    {
        if(ParentTile is BoardTile)
        {
            ((BoardTile)ParentTile).PropagateLaser(prediction, StartingDirection);
        }
    }
}
