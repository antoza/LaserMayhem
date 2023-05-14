using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public class LaserTrajectory : MonoBehaviour
{
    [SerializeField] private (int, int) startingSpot = (-1, 3);
    [SerializeField] private (int, int) startingDirection = (1, 0);

    [SerializeField] private Board board;

    [SerializeField] private bool[,,] laserGrid;

    private void Start()
    {
        laserGrid = new bool[board.getWidth(), board.getHeight(), 4];
    }

    public void displayBeam((int, int) spot, (int, int) direction, bool display)
    {
        if (board.isOnBoard(spot))
        {
            int directionNumber = convertDirection(direction);
            laserGrid[spot.Item1, spot.Item2, directionNumber] = display;
        }
    }

    public bool isBeamDisplayed((int, int) spot, (int, int) direction)
    {
        if (board.isOnBoard(spot))
        {
            int directionNumber = convertDirection(direction);
            return laserGrid[spot.Item1, spot.Item2, directionNumber];
        }
        return false;
    }

    private static int convertDirection((int, int) direction)
    {
        switch (direction)
        {
            case (1, 0):
                return 0;
            case (-1, 0):
                return 1;
            case (0, 1):
                return 2;
            case (0, -1):
                return 3;
            default:
                return -1;
        } 
    }

    public void crossBoard()
    {
        
    }
    public void crossNextTile((int, int) spot, (int, int) direction)
    {
        displayBeam(spot, direction, true);
        (int, int) newSpot = (spot.Item1 + direction.Item1, spot.Item2 + direction.Item2);
        if (newSpot.Item1 < 0 || newSpot.Item1 > board.getWidth())
        {
            return;
        }
        if (newSpot.Item2 < 0)
        {
            return;
        }
        if (newSpot.Item2 > board.getHeight())
        {
            return;
        }

        Piece? pieceCrossed = board.getPiece(spot);
        if (pieceCrossed)
        {
            foreach ((int, int) newDirection in pieceCrossed.computeNewDirections(direction)) {
                if (!isBeamDisplayed(spot, direction)) {
                    crossNextTile(newSpot, newDirection);
                }
            }
        }
        else {
            crossNextTile(newSpot, direction);
        }

        return;
    }
}
