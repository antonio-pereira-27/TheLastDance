using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    // VARIABLES
    public Vector2 size;
    public Vector2 offset; // distance between each room
    
    private int startPosition = 0;
    private List<Cell> board;

    private int roomCount = 0;

    private Vector3 scale;
    private Vector3 spawnPosition;

    // REFERENCES
    public GameObject room;
    public GameObject[] tutorialPrefabs;
    public GameObject enemy;
    public GameObject boss;
    public GameObject player;
    
    // Cell class
    public class Cell
    {
        public bool visited = false; // visited or not
        public bool[] status = new bool[4]; // up down right left
    }
    
    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }
    
    void GenerateDungeon()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Cell currentCell = board[Mathf.FloorToInt(x + y * size.x)];
                if (currentCell.visited)
                {
                    roomCount++;
                    var newRoom =Instantiate(room, new Vector3(x * offset.x, 0, -y * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + x + " - " + y;

                    spawnPosition = new Vector3(x * offset.x + 27f, 0f, -y * offset.y + 27f);
                    scale = new Vector3(5.0f, 5.0f, 5.0f);


                    if (roomCount == 1)
                    {
                        var playerInstantiate = Instantiate(player, spawnPosition, quaternion.identity);
                        playerInstantiate.transform.localScale = new Vector3(25.0f, 25.0f, 25.0f);

                    }
                    
                    if (roomCount == 4)
                    {
                        var obj1 = Instantiate(tutorialPrefabs[0], new Vector3(x * offset.x + 24f, 5f, -y * offset.y + 27f), Quaternion.identity);
                        obj1.transform.localScale = scale * 30f;
                        
                        var obj2 = Instantiate(tutorialPrefabs[1], new Vector3(x * offset.x + 29f, 5f, -y * offset.y + 27f), Quaternion.identity);
                        obj2.transform.localScale = scale * 2f;
                    }


                    if (roomCount == 6)
                    {
                        var enemyInstantiate = Instantiate(enemy, spawnPosition, quaternion.identity);
                        enemyInstantiate.transform.localScale = scale;
                    }
                        
                    
                    if (roomCount == 8)
                    {
                        var enemyInstantiate = Instantiate(boss, spawnPosition, quaternion.identity);
                        enemyInstantiate.transform.localScale = scale;
                    }
                        
                    
                }
                
            }
        }
        
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        // make sure that go out of cycle
        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
                break;
            
            
            //check neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            // if no neighbors, current cell its equal to the last cell added to the path
            if (neighbors.Count == 0)
            {
                if (path.Count == 0) 
                    break;
                else
                    currentCell = path.Pop();
            }
            else
            {
                //add the current cell to the path
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //up or left
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        
        //check up
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        //check down
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        //check right
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        //check left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        return neighbors;
    }
}
