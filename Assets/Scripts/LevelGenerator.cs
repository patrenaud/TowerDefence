using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    private const int MAX_ROAD_MUTATION = 10; //The maximum amount of step (direction) added to the bridge from the entrance to the exit.
    private const int MIN_GROUND_REMOVED = 2; //The minimum amount of ground tiles removed from a "perfect rectangle" map.
    private const int MAX_GROUND_REMOVED = 20; //The maximum amount of ground tiles removed from a "perfect rectangle" map.

    [SerializeField]
    private Vector2 m_MinMapSize;
    [SerializeField]
    private Vector2 m_MaxMapSize;

    [Space(10)]
    [SerializeField]
    private List<GameObject> m_GroundPrefabs;
    private int m_GroundIndex;

    [SerializeField]
    private List<GameObject> m_EntrancePrefabs;

    [SerializeField]
    private List<GameObject> m_ExitPrefabs;

    [SerializeField]
    private List<ObstacleData> m_Obstacles;

    private Vector2 m_CurrentSize; //The current size of the map.
    private GameObject m_Entrance;
    private Vector2Int m_EntrancePos;
    private GameObject m_Exit;
    private Vector2Int m_ExitPos;
    private Dictionary<Vector2, bool> m_UsedPositions = new Dictionary<Vector2, bool>(); //List of all already used positions.
    private List<Vector3> m_AvailablePositions = new List<Vector3>();

    //Instantiate the map and obstacles.
    public void GenerateLevel()
    {
        if(LevelManager.Instance == null)
        {
            return;
        }

        GenerateMap();
        GenerateObstacles();
    }

    private void GenerateMap()
    {
        //Set the new dimensions for the level generation.
        m_CurrentSize = new Vector2(Random.Range(m_MinMapSize.x, m_MaxMapSize.x), Random.Range(m_MinMapSize.y, m_MaxMapSize.y));
       
        //Set the current ground visual
        m_GroundIndex = Random.Range(0, m_GroundPrefabs.Count - 1);

        SpawnEntrance();
        SpawnExit();
        SpawnRoad();
        SpawnGround();
    }

    private void SpawnEntrance()
    {
        if(m_EntrancePrefabs == null)
        {
            return;
        }

        m_EntrancePos = Vector2Int.zero;
        m_EntrancePos.x = (int)Random.Range(0, m_CurrentSize.x / 3);
        m_EntrancePos.y = (int)Random.Range(0, m_CurrentSize.y / 3);

        Vector3 spawnPos = new Vector3(m_EntrancePos.x, m_EntrancePos.y, 0);

        Instantiate(m_GroundPrefabs[m_GroundIndex], spawnPos, Quaternion.identity);
        m_Entrance = Instantiate(m_EntrancePrefabs[Random.Range(0, m_EntrancePrefabs.Count - 1)], spawnPos, Quaternion.identity);
    }

    private void SpawnExit()
    {
        if (m_ExitPrefabs == null)
        {
            return;
        }

        m_ExitPos = Vector2Int.zero;
        m_ExitPos.x = (int)Random.Range(m_CurrentSize.x - (m_CurrentSize.x / 3), m_CurrentSize.x);
        m_ExitPos.y = (int)Random.Range(m_CurrentSize.y - (m_CurrentSize.y / 3), m_CurrentSize.y);

        Vector3 spawnPos = new Vector3(m_ExitPos.x, m_ExitPos.y, 0);

        Instantiate(m_GroundPrefabs[m_GroundIndex], spawnPos, Quaternion.identity);
        m_Exit = Instantiate(m_ExitPrefabs[Random.Range(0, m_ExitPrefabs.Count - 1)], spawnPos, Quaternion.identity);
    }

    //Create a "safe road" so there is always a path from the entrance to the exit.
    private void SpawnRoad()
    {
        if(m_Entrance == null || m_Exit == null)
        {
            return;
        }

        List<EDirections> road = new List<EDirections>(); //Stack of each step (direction) needed to build a road from entrance to exit.

        int differenceX = m_ExitPos.x - m_EntrancePos.x;
        int differenceY = m_ExitPos.y - m_EntrancePos.y;

        m_UsedPositions.Add(m_EntrancePos, true);
        m_UsedPositions.Add(m_ExitPos, true);

        //Builds the shortest possible road in X axis.
        if (differenceX < 0)
        {
            for(int i = -1; i >= differenceX; i--)
            {
                road.Add(EDirections.West);
            }
        }
        else
        {
            for (int i = 1; i <= differenceX; i++)
            {
                road.Add(EDirections.East);
            }
        }

        //Builds the shortest possible road in Y axis.
        if (differenceY < 0)
        {
            for (int i = -1; i > differenceY; i--)
            {
                road.Add(EDirections.South);
            }
        }
        else
        {
            for (int i = 1; i < differenceY; i++)
            {
                road.Add(EDirections.North);
            }
        }


        //Mutations add some random directions to the shortest possible road.
        int mutations = Random.Range(0, MAX_ROAD_MUTATION);
        
        for(int i = 0; i < mutations; i++)
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                road.Add(EDirections.South);
                road.Add(EDirections.North);
            }
            else
            {
                road.Add(EDirections.West);
                road.Add(EDirections.East);
            }
        }
        
        road = ShuffleList(road); //Shuffle the road directions so it wont go in a straight line.

        Vector2Int currentPos = m_EntrancePos;
        for(int i = 0; i < road.Count; i++)
        {
            switch(road[i])
            {
                case EDirections.North:
                    currentPos += Vector2Int.up;
                    break;
                case EDirections.South:
                    currentPos -= Vector2Int.up;
                    break;
                case EDirections.East:
                    currentPos += Vector2Int.right;
                    break;
                case EDirections.West:
                    currentPos -= Vector2Int.right;
                    break;
            }

            //Is the position inside the map?
            if (currentPos.x >= 0
            && currentPos.x < m_CurrentSize.x
            && currentPos.y >= 0
            && currentPos.y < m_CurrentSize.y)
            {
                if (!m_UsedPositions.ContainsKey(currentPos))
                {
                    m_UsedPositions.Add(currentPos, true);
                    Instantiate(m_GroundPrefabs[m_GroundIndex], new Vector3(currentPos.x, currentPos.y, 0), Quaternion.identity);
                }
            }
        }
    }

    private void SpawnGround()
    {
        Vector2 currentPos = Vector2.zero;

        /*
         * -----------------------------------OPTIMISATION QUESTION
         * I dont know if it is faster to do 
         * currentPos.x += 1 and currentPos.y += 1 (for each loop)
         * OR
         * currentPos = new Vector2(x, y) (for each loop) 
         * ---------------------------------------------------------
         */

        m_AvailablePositions = new List<Vector3>();
        for (int y = 0; y < m_CurrentSize.y; y++)
        {
            for (int x = 0; x < m_CurrentSize.x; x++)
            {
                currentPos = new Vector2(x, y);
                if (!m_UsedPositions.ContainsKey(currentPos))
                {
                    m_AvailablePositions.Add(new Vector3(x, y, 0));
                }
            }
        }

        m_AvailablePositions = ShuffleList(m_AvailablePositions); //Shuffle available positions to remove grounds at random places.

        //Remove a certain amount of ground tiles from the map
        int removedGroundsCount = Random.Range(MIN_GROUND_REMOVED, MAX_GROUND_REMOVED);
        for(int i = 0; i < removedGroundsCount; i++)
        {
            if (m_AvailablePositions.Count <= 0)
            {
                break;
            }

            m_AvailablePositions.RemoveAt(0);            
        }

        //Spawn Ground tiles to form the map
        for (int i = 0; i < m_AvailablePositions.Count; i++)
        {
            Instantiate(m_GroundPrefabs[m_GroundIndex], m_AvailablePositions[i], Quaternion.identity);
        }
    }

    private void GenerateObstacles()
    {
        if(m_Obstacles == null)
        {
            return;
        }

        m_AvailablePositions = ShuffleList(m_AvailablePositions); //Shuffle available positions to spawn obstacles at random places.

        //Instantiate Obstacles
        for (int i = 0; i < m_Obstacles.Count; i++)
        {
            for(int x = 0; x < m_Obstacles[i].Quantity(LevelManager.Instance.CurrentLevel); x++)
            {
                //Stop if there is no more available positions
                if(m_AvailablePositions.Count <= 0)
                {
                    return;
                }

                //instantiate the obstacle at an available position
                Instantiate(m_Obstacles[i].Prefab, m_AvailablePositions[0], Quaternion.identity);

                //remove its position
                m_AvailablePositions.RemoveAt(0);
            }
        }
    }

    //Return a Shuffled version of any given list
    private List<T> ShuffleList<T>(List<T> a_List)
    {
        List<T> copy = new List<T>();
        int rand;
        while(a_List.Count > 0)
        {
            rand = Random.Range(0, a_List.Count);
            copy.Add(a_List[rand]);
            a_List.RemoveAt(rand);
        }

        return copy;
    }
}
