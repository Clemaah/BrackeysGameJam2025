using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static LevelGenerator;

public class LevelGenerator : MonoBehaviour
{
    public class Cell {
        public (int x, int y) coordinates = (-1, -1);
        public int type = -1;
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Room
    {
        public GameObject room;
        public int maxDifficulty;
        public int maxEntrances;
        public int maxNbRooms;
        public float probability;
        public int ProbabilityOfSpawning(int x, int y, int nbEntrances, bool isNextToSpecialRoom) {
            int random = Random.Range(0, 100);
            
            if(!isNextToSpecialRoom && random <= probability * 100 && (maxNbRooms == -1 || maxNbRooms > 0) && nbEntrances <= maxEntrances) {
                return 1;
            }

            return 0;
        }
    }

    [System.Serializable]
    public class LevelConfig
    {
        public Vector2Int mapSize;
        public Vector2Int pathLength;
        public Room[] rooms;
    }

        // In inspector :
    // Camera settings :
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;

    // Level settings :
    public LevelConfig[] levelsConfigs;
    public GameObject startRoom;
    public GameObject bossRoom;
    public Light sun;
    public Vector3[] sunRotations;

    // Not in inspector :
    LevelConfig currentLevelConfig;
    List<Cell> board;
    int [,] graph;
    Vector2 roomOffset;
    
    void Start() {
        int currentLevel = GameManager.Instance.CurrentLevel;
        sun.transform.rotation = Quaternion.Euler(sunRotations[currentLevel]);

        if (currentLevel == 0)
            DisplayStartRoom();
        
        else if (currentLevel > levelsConfigs.Length)
            DisplayBossRoom();
        
        else
        {
            currentLevelConfig = levelsConfigs[(int)GameManager.Instance.CurrentLevel - 1];
            roomOffset = new Vector2(56, 44);
            GenerateMaze();
        }
    }

    //Breadth-first search :
    int GetShortestPathLength(int startCell, int targetCell) {
        if(startCell == targetCell) return 0;

        Queue<(int, int)> queue = new Queue<(int, int)>();
        List<int> visitedCells = new List<int>();

        queue.Enqueue((startCell, 0));
        visitedCells.Add(startCell);

        while(queue.Count > 0) {
            (int currentCell, int distance) = queue.Dequeue();

            for (int neighborCell = 0; neighborCell < graph.GetLength(0); neighborCell++) {
                if(graph[currentCell, neighborCell] == 1 && !visitedCells.Contains(neighborCell)) {
                    if (neighborCell == targetCell) return distance + 1; 

                    visitedCells.Add(neighborCell);
                    queue.Enqueue((neighborCell, distance + 1));
                }
            }
        }

        return -1;
    }

    int CountDoors(bool[] entrances) {
        int count = 0;
        for(int i = 0; i < entrances.Length; i++) {
            if(entrances[i]) count++;
        }
        return count;
    }

    bool IsNextToSpecialRoom(int cellNumber) {
        for(int neighbor = 0; neighbor < graph.GetLength(0); neighbor++)
        {
            if (graph[cellNumber, neighbor] != 1) continue;
            
            Cell neighborCell = board[neighbor];

            // Check if the neighbor cell is a special room (i.e. an entrance room, an exit room, a heal room or a wish room) :
            if(neighborCell.type > 0) return true;
        }

        return false;
    }
    void DisplayStartRoom()
    {
        RoomBehaviour startRoomBehaviour = startRoom.GetComponent<RoomBehaviour>();
        startRoomBehaviour.doorsGates[0].SetActive(true);
        var room = Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<RoomBehaviour>();

        Vector3 playerPosition = new Vector3(0, 0, 0 * roomOffset.y - 5);
        cameraPosition += new Vector3(roomOffset.x, 0, roomOffset.y - 10);
        GameManager.Instance.TeleportCharacterTo(playerPosition, cameraPosition);
        Camera.main.transform.rotation = cameraRotation;
    }
    void DisplayBossRoom() {
        var room = Instantiate(bossRoom, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<RoomBehaviour>();

        Vector3 playerPosition = new Vector3(0, 0, 0 * roomOffset.y - 5);
        cameraPosition += new Vector3(roomOffset.x, 0, roomOffset.y - 10);
        GameManager.Instance.TeleportCharacterTo(playerPosition, cameraPosition);
        Camera.main.transform.rotation = cameraRotation;
    }


    bool GenerateLevel() {
        // Generate exit room :
        List<(int x, int y)> availableExitRooms = new List<(int x, int y)>();

        for(int i = 0; i < currentLevelConfig.mapSize.x; i++) {
            for(int j = 0; j < currentLevelConfig.mapSize.y; j++) {
                Cell currentCell = board[i + j * currentLevelConfig.mapSize.x];
                if (!currentCell.visited) continue;
                
                int count = CountDoors(currentCell.status);

                if(count == 1) {
                    availableExitRooms.Add((i, j));
                }
            }
        }

        (int x, int y) exitCoordinates = availableExitRooms[Random.Range(0, availableExitRooms.Count)];
        
        Cell exitCell = board[exitCoordinates.x + exitCoordinates.y * currentLevelConfig.mapSize.x];
        exitCell.type = 2;
        exitCell.coordinates = exitCoordinates;

        var exitRoom = Instantiate(currentLevelConfig.rooms[2].room, new Vector3(exitCoordinates.x * roomOffset.x, 0, -exitCoordinates.y * roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
        exitRoom.name += " " + exitCoordinates.x + "-" + exitCoordinates.y;
        exitRoom.BuildRoom(exitCell.status, 0);

        currentLevelConfig.rooms[2].maxNbRooms--;

        // Generate entrance room :
        List<(int x, int y)> availableEntranceRooms = new List<(int x, int y)>();
        
        for(int i = 0; i < currentLevelConfig.mapSize.x; i++) {
            for(int j = 0; j < currentLevelConfig.mapSize.y; j++) {
                Cell currentCell = board[i + j * currentLevelConfig.mapSize.x];

                if (!currentCell.visited) continue;
                
                int shortestPath = GetShortestPathLength(exitCoordinates.x + exitCoordinates.y * currentLevelConfig.mapSize.x, i + j * currentLevelConfig.mapSize.x);
                if(shortestPath >= currentLevelConfig.pathLength.x && shortestPath <= currentLevelConfig.pathLength.y){
                    availableEntranceRooms.Add((i, j));
                }
            }
        }

        if (availableEntranceRooms.Count == 0) return false;

        (int x, int y) entranceCoordinates = availableEntranceRooms[Random.Range(0, availableEntranceRooms.Count)];
        
        Cell entranceCell = board[entranceCoordinates.x + entranceCoordinates.y * currentLevelConfig.mapSize.x];
        entranceCell.type = 1;
        entranceCell.coordinates = entranceCoordinates;

        Vector3 playerPosition = new Vector3(entranceCoordinates.x * roomOffset.x, 0.5f, -entranceCoordinates.y * roomOffset.y - 5);
        cameraPosition += new Vector3(entranceCoordinates.x * roomOffset.x, 0, -entranceCoordinates.y * roomOffset.y - 10);
        GameManager.Instance.TeleportCharacterTo(playerPosition, cameraPosition);
        Camera.main.transform.rotation = cameraRotation;

        var entranceRoom = Instantiate(currentLevelConfig.rooms[1].room, new Vector3(entranceCoordinates.x * roomOffset.x, 0, -entranceCoordinates.y * roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
        entranceRoom.name += " " + entranceCoordinates.x + "-" + entranceCoordinates.y;
        entranceRoom.BuildRoom(entranceCell.status, 0);

        currentLevelConfig.rooms[1].maxNbRooms--;

        // Generate other rooms :
        for(int i = 0; i < currentLevelConfig.mapSize.x; i++) {
            for(int j = 0; j < currentLevelConfig.mapSize.y; j++) {
                int cellNumber = i + j * currentLevelConfig.mapSize.x;
                Cell currentCell = board[cellNumber];

                if(currentCell.visited && cellNumber != (entranceCoordinates.x + entranceCoordinates.y * currentLevelConfig.mapSize.x) && cellNumber != (exitCoordinates.x + exitCoordinates.y * currentLevelConfig.mapSize.x)) {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for(int k = 0; k < currentLevelConfig.rooms.Length; k++) {
                        int count = CountDoors(currentCell.status);
                        bool isNextToSpecialRoom = IsNextToSpecialRoom(cellNumber);
                        int probability = currentLevelConfig.rooms[k].ProbabilityOfSpawning(i, j, count, isNextToSpecialRoom);

                        if(probability == 1) {
                            availableRooms.Add(k);
                        }
                    }

                    if(randomRoom == -1) {
                        if(availableRooms.Count > 0) {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else {
                            randomRoom = 0;
                        }
                    }

                    if(currentLevelConfig.rooms[randomRoom].maxNbRooms != -1) {
                        currentLevelConfig.rooms[randomRoom].maxNbRooms--;
                    }

                    var newRoom = Instantiate(currentLevelConfig.rooms[randomRoom].room, new Vector3(i * roomOffset.x, 0, -j * roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.name += " " + i + "-" + j;
                    newRoom.BuildRoom(currentCell.status, currentLevelConfig.rooms[randomRoom].maxDifficulty);

                    currentCell.type = randomRoom;
                    currentCell.coordinates = (i, j);
                }
            }
        }

        return true;
    }

    public void GenerateMaze() {
        bool mazeValid = false;

        while (!mazeValid)
        {
            board = new List<Cell>();

            for (int i = 0; i < currentLevelConfig.mapSize.x; i++)
            {
                for (int j = 0; j < currentLevelConfig.mapSize.y; j++)
                {
                    board.Add(new Cell());
                }
            }

            // Initialize graph :
            graph = new int[board.Count, board.Count];
            for (int i = 0; i < board.Count; i++)
            {
                for (int j = 0; j < board.Count; j++)
                {
                    graph[i, j] = 0;
                }
            }

            int currentCell = 0;
            Stack<int> path = new Stack<int>();
            int k = 0;

            while (k < 1000)
            {
                k++;
                board[currentCell].visited = true;

                if (currentCell == board.Count - 1) break;

                List<int> neighbors = CheckNeighbors(currentCell);

                if (neighbors.Count == 0)
                {
                    if (path.Count == 0) break;
                    else currentCell = path.Pop();
                }

                else
                {
                    path.Push(currentCell);
                    int newCell = neighbors[Random.Range(0, neighbors.Count)];

                    if (newCell > currentCell)
                    {
                        // Going east :
                        if (newCell - 1 == currentCell)
                        {
                            graph[currentCell, newCell] = 1;
                            graph[newCell, currentCell] = 1;
                            board[currentCell].status[2] = true;
                            currentCell = newCell;
                            board[currentCell].status[3] = true;
                        }

                        // Going south :
                        else
                        {
                            graph[currentCell, newCell] = 1;
                            graph[newCell, currentCell] = 1;
                            board[currentCell].status[1] = true;
                            currentCell = newCell;
                            board[currentCell].status[0] = true;
                        }
                    }

                    else
                    {
                        // Going west :
                        if (newCell + 1 == currentCell)
                        {
                            graph[currentCell, newCell] = 1;
                            graph[newCell, currentCell] = 1;
                            board[currentCell].status[3] = true;
                            currentCell = newCell;
                            board[currentCell].status[2] = true;
                        }

                        // Going north :
                        else
                        {
                            graph[currentCell, newCell] = 1;
                            graph[newCell, currentCell] = 1;
                            board[currentCell].status[0] = true;
                            currentCell = newCell;
                            board[currentCell].status[1] = true;
                        }
                    }
                }
            }

            mazeValid = GenerateLevel();
        } 
    }

    List<int> CheckNeighbors(int cell) {
        List<int> neighbors = new List<int>();

        // Check north neighbor :
        if(cell - currentLevelConfig.mapSize.x >= 0 && !board[cell - currentLevelConfig.mapSize.x].visited) {
            neighbors.Add(cell - currentLevelConfig.mapSize.x);
        }

        // Check south neighbor :
        if(cell + currentLevelConfig.mapSize.x < board.Count && !board[cell + currentLevelConfig.mapSize.x].visited) {
            neighbors.Add(cell + currentLevelConfig.mapSize.x);
        }

        // Check west neighbor :
        if((cell + 1) % currentLevelConfig.mapSize.x != 0 && !board[cell + 1].visited) {
            neighbors.Add(cell + 1);
        }

        // Check east neighbor :
        if(cell % currentLevelConfig.mapSize.x != 0 && !board[cell - 1].visited) {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }
}
