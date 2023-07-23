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
	private GameObject m_LaserVisualPredictionTemplate;
	private GameObject m_LaserContainer;
	private List<GameObject> m_LaserVisualHolder = new List<GameObject>() { };

	private Vector2Int m_StartingSpot = new Vector2Int(-1, 3);
	private Vector2Int m_StartingDirection = new Vector2Int(1, 0);

	private bool[,,] m_LaserGrid;

	//private float m_OffsetX = 0.25f;
	//private float m_OffsetY = 0.5f;

    private float[,] m_Offset = { { 0.5f, 0f, 0f }, { -0.5f, 0f, 180f }, {0f, 0.5f, 90f }, { 0f, -0.5f, 270f } };

	public LaserManager(DataManager dataManager, BoardManager boardManager, GameObject laserVisualTemplate, GameObject laserPredictionVisualTemplate, GameObject laserContainer)
	{
		m_DataManager = dataManager;
		m_BoardManager = boardManager;
		m_LaserVisualTemplate = laserVisualTemplate;
		m_LaserVisualPredictionTemplate = laserPredictionVisualTemplate;
		m_LaserContainer = laserContainer;
    }

	private void Start()
	{
		m_LaserGrid = new bool[m_BoardManager.width, m_BoardManager.height, 4];
    }

    public void UpdateLaser(bool prediction)
    {
        m_DataManager.LaserManager.DestroyLaserPart();
        m_DataManager.LaserManager.PrintLaserPart(prediction);
    }

    public void PrintLaserPart(bool prediction)
    {
		//Set the laser initial part
        GameObject laserPart;
		if (prediction)
		{
            laserPart = Instantiate(m_LaserVisualPredictionTemplate);
		}
		else
		{
            laserPart = Instantiate(m_LaserVisualTemplate);
			HitPlayer();
        }

		ResetBoard();
        CrossNextTile(m_StartingSpot, m_StartingDirection);
        
        TurnLaser(m_StartingDirection, m_StartingSpot, laserPart);
		m_LaserVisualHolder.Add(laserPart);
        laserPart.transform.SetParent(m_LaserContainer.transform);
        Vector2Int worldCoord = m_BoardManager.ConvertBoardCoordinateToWorldCoordinates(m_StartingSpot);
        laserPart.transform.position = new Vector3(worldCoord[0] + m_Offset[DirectionToInt(m_StartingDirection), 0], worldCoord[1] + m_Offset[DirectionToInt(m_StartingDirection), 1], 0);
		laserPart.transform.rotation = Quaternion.Euler(0, 0, m_Offset[DirectionToInt(m_StartingDirection), 2]);

        foreach ((Vector2Int, Vector2Int) displayedBeam in DisplayedBeams())
		{
			Vector2Int beamDirection = displayedBeam.Item2;
			Vector2Int beamPosition = displayedBeam.Item1;

			//Debug.Log("\n");
			//Debug.Log("Direction : " + beamDirection);
			//Debug.Log("Position : " + beamPosition);	

            if (prediction)
            {
                laserPart = Instantiate(m_LaserVisualPredictionTemplate);
            }
            else
            {
                laserPart = Instantiate(m_LaserVisualTemplate);
            }

            TurnLaser(beamDirection, beamPosition, laserPart);
            m_LaserVisualHolder.Add(laserPart);
            laserPart.transform.SetParent(m_LaserContainer.transform);
			worldCoord = m_BoardManager.ConvertBoardCoordinateToWorldCoordinates(beamPosition);
            laserPart.transform.position = new Vector3(worldCoord[0] + m_Offset[DirectionToInt(beamDirection), 0], worldCoord[1] + m_Offset[DirectionToInt(beamDirection), 1], 0);
            //laserPart.transform.rotation = Quaternion.Euler(0, 0, m_Offset[DirectionToInt(m_StartingDirection), 2]);
		}
	}

	public void DestroyLaserPart()
	{
		foreach(GameObject laserPart in m_LaserVisualHolder)
		{
			Destroy(laserPart);
		}
	}

	public void ResetBoard()
	{
		m_LaserGrid = new bool[m_BoardManager.width, m_BoardManager.height, 4];
	}

	public void CrossNextTile(Vector2Int spot, Vector2Int direction)
	{
        Vector2Int newSpot = new Vector2Int(spot[0] + direction[0], spot[1] + direction[1]);

        if (m_BoardManager.IsOnBoard(newSpot))
		{
            Piece? pieceCrossed = m_BoardManager.GetPiece(newSpot);
            if (pieceCrossed)
            {
                foreach (Vector2Int newDirection in pieceCrossed!.ComputeNewDirections(direction))
                {
                    if (!IsBeamDisplayed(newSpot, newDirection))
                    {
						DisplayBeam(newSpot, newDirection, true);
                        CrossNextTile(newSpot, newDirection);
                    }
                }
            }
            else
            {
                if (!IsBeamDisplayed(newSpot, direction))
                {
                    DisplayBeam(newSpot, direction, true);
                    CrossNextTile(newSpot, direction);
                }
            }
        }
		return;
	}

	public void DisplayBeam(Vector2Int spot, Vector2Int direction, bool display)
	{
		if (m_BoardManager.IsOnBoard(spot))
		{
			int directionNumber = DirectionToInt(direction);
			m_LaserGrid[spot[0], spot[1], directionNumber] = display;
		}
	}

	public bool IsBeamDisplayed(Vector2Int spot, Vector2Int direction)
	{
		if (m_BoardManager.IsOnBoard(spot))
		{
			int directionNumber = DirectionToInt(direction);
			return m_LaserGrid[spot[0], spot[1], directionNumber];
		}
		return false;
	}

	public IEnumerable<(Vector2Int, Vector2Int)> DisplayedBeams()
	{
		for (int i = 0; i < m_BoardManager.width; i++)
		{
			for (int j = 0; j < m_BoardManager.height; j++)
			{
				for (int d = 0; d < 4; d++)
				{
					if (m_LaserGrid[i, j, d]) yield return (new Vector2Int(i, j), IntToDirection(d));
				}
			}
		}
	}

	void HitPlayer()
	{
		for(int i = 0; i < m_BoardManager.width; i++)
		{
			if(m_LaserGrid[i, 0, 3])
			{
				m_DataManager.PlayersManager.HitPlayer(1);
			}
			if(m_LaserGrid[i, m_BoardManager.height-1, 2])
			{
				m_DataManager.PlayersManager.HitPlayer(0);
			}	
		}
	}

	void PrintLaserGrid()
	{
		for(int i = 0; i < m_BoardManager.width; i++)
		{
			for(int j = 0; j < m_BoardManager.height; j++)
			{
				bool yes = false;
				if(m_LaserGrid[i, j, 0])
				{
					if(!yes)
					{
						Debug.Log("i : " + i + " j : " + j);
						yes = true;
					}
					Debug.Log("Gauche");
				}
				if(m_LaserGrid[i, j, 1])
				{
					if(!yes)
					{
						Debug.Log("i : " + i + " j : " + j);
						yes = true;
					}
					Debug.Log("Droite");
				}
				if(m_LaserGrid[i, j, 2])
				{
					if(!yes)
					{
						Debug.Log("i : " + i + " j : " + j);
						yes = true;
					}
					Debug.Log("Haut");
				}
				if(m_LaserGrid[i, j, 3])
				{
					if(!yes)
					{
						Debug.Log("i : " + i + " j : " + j);
						yes = true;
					}
					Debug.Log("Bas");
				}
			}
		}
	}

	private int DirectionToInt(Vector2Int direction)
	{
		switch ((direction[0], direction[1]))
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

	private Vector2Int IntToDirection(int directionNumber)
	{
		switch (directionNumber)
		{
			case 0:
				return new Vector2Int(1, 0);
			case 1:
				return new Vector2Int(-1, 0);
			case 2:
				return new Vector2Int(0, 1);
			case 3:
				return new Vector2Int(0, -1);
			default:
				return new Vector2Int(0, 0);
		}
	}

	private Vector2Int GetOppositeDirection(Vector2Int direction)
	{
		return new Vector2Int(-direction[0], -direction[1]);
	}

	private void TurnLaser(Vector2Int direction,Vector2Int position, GameObject laser)
	{
		switch ((direction[0], direction[1]))
		{
			case (-1, 0):
				laser.transform.position = new Vector3(position[0] - 0.25f, position[1], 0);
				return;
			case (0, 1):
                laser.transform.Rotate(new Vector3(0, 0, 90));
                return;
			case (0, -1):
                laser.transform.Rotate(new Vector3(0, 0, -90));
                return;
			default:
				return;
		}
	}
}
