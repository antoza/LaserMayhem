using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#nullable enable

[CreateAssetMenu]
public class LaserManager : ScriptableObject
{
    private DataManager m_DataManager;
    private BoardManager m_Board;

    private (int, int) m_StartingSpot = (-1, 3);
    private (int, int) m_StartingDirection = (1, 0);

    //Laser Objects
    private GameObject m_LaserContainer;
    private GameObject m_LaserShotTemplate;
    private GameObject m_LaserPredictionTemplate;
    private GameObject[] m_LaserShots = new GameObject[] { };
    private GameObject[] m_LaserPredictions = new GameObject[] { };

    private bool[,,] m_LaserGrid;

    private void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_Board = m_DataManager.BoardManager;
        m_LaserGrid = new bool[m_Board.width, m_Board.height, 4];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetLaserPositions();
            DisplayLaserBeam();
        }
    }


    private void SetLaserPositions()
    {
        (int, int)[][] lasersPositions = new (int, int)[][] { new (int, int)[] { m_StartingSpot } };
        (int, int)[] lasersLastDirection = new (int, int)[] { m_StartingDirection };

        int laserIndex = 0;

        while(laserIndex < lasersPositions.Length)
        {
            (int, int) currentPosition = lasersPositions[laserIndex][0];
            (int, int) currentDirection = lasersLastDirection[laserIndex];

            while(currentPosition == m_StartingDirection || m_Board.IsOnBoard(currentPosition))
            {
                currentPosition.Item1 += currentDirection.Item1;
                currentPosition.Item2 += currentDirection.Item2;

                if (!m_Board.IsOnBoard(currentPosition))
                {
                    lasersPositions[laserIndex].Append(currentPosition); //Out of the board, end of this laser
                    laserIndex++;
                    break;
                }

                Piece? currentPiece = m_Board.GetPiece(currentPosition);
                if (currentPiece != null)
                {
                    (int, int)[] newDirections = currentPiece.computeNewDirections(currentDirection);
                    currentDirection = newDirections[0];
                    lasersLastDirection[laserIndex] = currentDirection; //Useless, for correctness of the laserslastdirection array
                    lasersPositions[laserIndex].Append(currentPosition);
                    for (int i = 1; i < newDirections.Length; i++) 
                    {
                        lasersLastDirection.Append(newDirections[i]);
                        lasersPositions.Append(new (int, int)[] { currentPosition });
                    }
                }

            }
        }
        //m_LaserShotLineRenderer.SetPositions(positions);    
    }

    private void DisplayLaserBeam()
    {

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

    private Vector3 TileCoordinateToVector3((int, int) tilePosition)
    {
        return new Vector3(tilePosition.Item1, tilePosition.Item2);
    }
}
