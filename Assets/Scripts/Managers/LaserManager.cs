using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

[CreateAssetMenu]
public class LaserManager : ScriptableObject
{
    private DataManager m_DataManager;
    private BoardManager m_Board;

    private (int, int) m_StartingSpot = (-1, 3);
    private (int, int) m_StartingDirection = (1, 0);

    private bool[,,] m_LaserGrid;

    private void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_Board = m_DataManager.BoardManager;
        m_LaserGrid = new bool[m_Board.width, m_Board.height, 4];
    }

    public IEnumerable<((int, int), (int, int))> CrossBoard()
    {
        ResetBoard();
        CrossNextTile(m_StartingSpot, m_StartingDirection);
        return DisplayedBeams();
    }

    public void ResetBoard()
    {
        m_LaserGrid = new bool[m_Board.width, m_Board.height, 4];
    }

    public void CrossNextTile((int, int) spot, (int, int) direction)
    {
        DisplayBeam(spot, direction, true);
        (int, int) newSpot = (spot.Item1 + direction.Item1, spot.Item2 + direction.Item2);
        if (newSpot.Item1 < 0 || newSpot.Item1 > m_Board.width)
        {
            return;
        }
        if (newSpot.Item2 < 0)
        {
            return;
        }
        if (newSpot.Item2 > m_Board.width)
        {
            return;
        }

        Piece? pieceCrossed = m_Board.GetPiece(spot);
        if (pieceCrossed)
        {
            foreach ((int, int) newDirection in pieceCrossed!.computeNewDirections(direction))
            {
                if (!IsBeamDisplayed(spot, direction))
                {
                    CrossNextTile(newSpot, newDirection);
                }
            }
        }
        else
        {
            CrossNextTile(newSpot, direction);
        }

        return;
    }

    public void DisplayBeam((int, int) spot, (int, int) direction, bool display)
    {
        if (m_Board.IsOnBoard(spot))
        {
            int directionNumber = DirectionToInt(direction);
            m_LaserGrid[spot.Item1, spot.Item2, directionNumber] = display;
        }
    }

    public bool IsBeamDisplayed((int, int) spot, (int, int) direction)
    {
        if (m_Board.IsOnBoard(spot))
        {
            int directionNumber = DirectionToInt(direction);
            return m_LaserGrid[spot.Item1, spot.Item2, directionNumber];
        }
        return false;
    }

    public IEnumerable<((int, int), (int, int))> DisplayedBeams()
    {
        for (int i = 0; i < m_Board.width; i++)
        {
            for (int j = 0; j < m_Board.height; j++)
            {
                for (int d = 0; d < 4; d++)
                {
                    if (m_LaserGrid[i, j, d]) yield return ((i, j), IntToDirection(d));
                }
            }
        }
    }

    private int DirectionToInt((int, int) direction)
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

    private (int, int) IntToDirection(int directionNumber)
    {
        switch (directionNumber)
        {
            case 0:
                return (1, 0);
            case 1:
                return (-1, 0);
            case 2:
                return (0, 1);
            case 3:
                return (0, -1);
            default:
                return (0, 0);
        }
    }
}
