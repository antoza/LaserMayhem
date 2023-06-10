using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#nullable enable

[CreateAssetMenu]
public class LaserManager : ScriptableObject
{
	private DataManager m_DataManager;
	private BoardManager m_BoardManager;

	private GameObject m_LaserVisualTemplate;
	private GameObject m_LaserContainer;
	private GameObject[] m_LaserVisualHolder = new GameObject[] { };

	private (int, int) m_StartingSpot = (3, -1);
	private (int, int) m_StartingDirection = (0, 1);

	private bool[,,] m_LaserGrid;

	private float m_OffsetX = 0.25f;
	private float m_OffsetY = 0.5f;

    private float[,] m_Offset = { { 0.5f, 0f, 0f }, { -0.5f, 0f, 180f }, {0f, 0.5f, 90f }, { 0f, -0.5f, 270f } };

    public LaserManager()
	{
		m_LaserVisualTemplate = GameObject.FindGameObjectWithTag("LaserVisual");
		m_DataManager = FindObjectOfType<DataManager>();
		m_BoardManager = m_DataManager.BoardManager;
		m_LaserContainer = GameObject.FindGameObjectWithTag("LaserContainer");
	}
	public LaserManager(DataManager dataManager, BoardManager boardManager, GameObject laserVisualTemplate, GameObject laserContainer)
	{
		m_DataManager = dataManager;
		m_BoardManager = boardManager;
		m_LaserVisualTemplate = laserVisualTemplate;
		m_LaserContainer = laserContainer;
    }

	private void Start()
	{
		m_LaserGrid = new bool[m_BoardManager.width, m_BoardManager.height, 4];
	}

	public void PrintLaserPart()
	{
		CrossBoard();

        //Set the laser initial part
        GameObject laserPart = Instantiate(m_LaserVisualTemplate);
        TurnLaser(m_StartingDirection, m_StartingSpot, laserPart);
        m_LaserVisualHolder.Append(laserPart);
        laserPart.transform.SetParent(m_LaserContainer.transform);
        (int, int) worldCoord = m_BoardManager.ConvertBoardCoordinateToWorldCoordinates(m_StartingSpot);
        laserPart.transform.position = new Vector3(worldCoord.Item1 + m_Offset[DirectionToInt(m_StartingDirection), 0], worldCoord.Item2 + m_Offset[DirectionToInt(m_StartingDirection), 1], 0);
		laserPart.transform.rotation = Quaternion.Euler(0, 0, m_Offset[DirectionToInt(m_StartingDirection), 2]);

        for (int x = 0; x < m_LaserGrid.GetLength(0); x++)
		{
			for (int y = 0; y < m_LaserGrid.GetLength(1); y++)
			{
				for(int dir = 0; dir < m_LaserGrid.GetLength(2); dir++)
				{
					if (m_LaserGrid[x, y, dir])
					{
						laserPart = Instantiate(m_LaserVisualTemplate);
                        TurnLaser(IntToDirection(dir), (x, y), laserPart);
                        m_LaserVisualHolder.Append(laserPart);
                        laserPart.transform.SetParent(m_LaserContainer.transform);
						worldCoord = m_BoardManager.ConvertBoardCoordinateToWorldCoordinates((x, y));
                        laserPart.transform.position = new Vector3(worldCoord.Item1 + m_Offset[dir,0], worldCoord.Item2 + m_Offset[dir, 1], 0);
                        laserPart.transform.rotation = Quaternion.Euler(0, 0, m_Offset[DirectionToInt(m_StartingDirection), 2]);

                    }
                }
			}
		}
	}





	public IEnumerable<((int, int), (int, int))> CrossBoard()
	{
		ResetBoard();
		CrossNextTile(m_StartingSpot, m_StartingDirection);
		return DisplayedBeams();
	}

	public void ResetBoard()
	{
		m_LaserGrid = new bool[m_BoardManager.width, m_BoardManager.height, 4];
	}

	public void CrossNextTile((int, int) spot, (int, int) direction)
	{
		DisplayBeam(spot, direction, true);
		(int, int) newSpot = (spot.Item1 + direction.Item1, spot.Item2 + direction.Item2);
		if (newSpot.Item1 < 0)
		{
			return;
		}
		if (newSpot.Item2 < 0)
		{
			return;
		}
		if (newSpot.Item2 > m_BoardManager.width)
		{
			return;
		}

		if (m_BoardManager.IsOnBoard(newSpot) && newSpot.Item1 < m_BoardManager.width && newSpot.Item2 < m_BoardManager.height)
		{
            Piece? pieceCrossed = m_BoardManager.GetPiece(newSpot);
            if (pieceCrossed)
            {
                foreach ((int, int) newDirection in pieceCrossed!.ComputeNewDirections(direction))
                {
                    if (!IsBeamDisplayed(spot, direction))
                    {
						DisplayBeam(newSpot, newDirection, true);
                        CrossNextTile(newSpot, newDirection);
                    }
                }
            }
            else
            {
                CrossNextTile(newSpot, direction);
            }
        }
		return;
	}

	public void DisplayBeam((int, int) spot, (int, int) direction, bool display)
	{
		if (m_BoardManager.IsOnBoard(spot))
		{
			int directionNumber = DirectionToInt(direction);
			m_LaserGrid[spot.Item1, spot.Item2, directionNumber] = display;
		}
	}

	public bool IsBeamDisplayed((int, int) spot, (int, int) direction)
	{
		if (m_BoardManager.IsOnBoard(spot))
		{
			int directionNumber = DirectionToInt(direction);
			return m_LaserGrid[spot.Item1, spot.Item2, directionNumber];
		}
		return false;
	}

	public IEnumerable<((int, int), (int, int))> DisplayedBeams()
	{
		for (int i = 0; i < m_BoardManager.width; i++)
		{
			for (int j = 0; j < m_BoardManager.height; j++)
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

	private (int, int) GetOppositeDirection((int, int) direction)
	{
		return (-direction.Item1, -direction.Item2);
	}

	private void TurnLaser((int, int) direction,(int, int) position, GameObject laser)
	{
		switch (direction)
		{
			case (1, 0):
                laser.transform.position = new Vector3(position.Item1 + 0.25f, position.Item2, 0);
                return;
			case (-1, 0):
				laser.transform.Rotate(new Vector3(0, 0, 180));
				laser.transform.position = new Vector3(position.Item1 - 0.25f, position.Item2, 0);
				return;
			case (0, 1):
                laser.transform.position = new Vector3(position.Item1, position.Item2 + 0.25f, 0);
                laser.transform.Rotate(new Vector3(0, 0, 90));
                return;
			case (0, -1):
                laser.transform.position = new Vector3(position.Item1, position.Item2 - 0.25f, 0);
                laser.transform.Rotate(new Vector3(0, 0, -90));
                return;
			default:
				return;
		}
	}

}
