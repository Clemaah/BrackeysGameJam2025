using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public class Cell {
        public (int x, int y) coordinates = (-1, -1);
        public int type = -1;
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Room {
        public GameObject room;
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

    public Vector2Int size;
    public int startPos = 0;
    public Room[] rooms;
    public Vector2 pathLengthBounds;
    public Vector2 offset;
    List<Cell> board;
    int [,] graph;
    
    void Start() {
        GenerateMaze();
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
        for(int neighbor = 0; neighbor < graph.GetLength(0); neighbor++) {
            if(graph[cellNumber, neighbor] == 1) {
                Cell neighborCell = board[neighbor];

                // Check if the neighbor cell is a special room (i.e. an entrance room, an exit room, a heal room or a wish room) :
                if(neighborCell.type > 0) {
                    return true;
                }
            }
        }

        return false;
    }

    void GenerateLevel() {
        // Generate exit room :
        List<(int x, int y)> availableExitRooms = new List<(int x, int y)>();

        for(int i = 0; i < size.x; i++) {
            for(int j = 0; j < size.y; j++) {
                Cell currentCell = board[i + j * size.x];
                if(currentCell.visited) {
                    int count = CountDoors(currentCell.status);

                    if(count == 1) {
                        availableExitRooms.Add((i, j));
                    }
                }
            }
        }

        (int x, int y) exitCoordinates = availableExitRooms[Random.Range(0, availableExitRooms.Count)];
        
        Cell exitCell = board[exitCoordinates.x + exitCoordinates.y * size.x];
        exitCell.type = 2;
        exitCell.coordinates = exitCoordinates;

        var exitRoom = Instantiate(rooms[2].room, new Vector3(exitCoordinates.x * offset.x, 0, -exitCoordinates.y * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
        exitRoom.name += " " + exitCoordinates.x + "-" + exitCoordinates.y;
        exitRoom.UpdateRoom(exitCell.status);

        rooms[2].maxNbRooms--;

        // Generate entrance room :
        List<(int x, int y)> availableEntranceRooms = new List<(int x, int y)>();
        
        for(int i = 0; i < size.x; i++) {
            for(int j = 0; j < size.y; j++) {
                Cell currentCell = board[i + j * size.x];

                if(currentCell.visited) {
                    int shortestPath = GetShortestPathLength(exitCoordinates.x + exitCoordinates.y * size.x, i + j * size.x);
                    if(shortestPath >= pathLengthBounds.x && shortestPath <= pathLengthBounds.y){
                        availableEntranceRooms.Add((i, j));
                    }
                }
            }
        }

        (int x, int y) entranceCoordinates = availableEntranceRooms[Random.Range(0, availableEntranceRooms.Count)];
        
        Cell entranceCell = board[entranceCoordinates.x + entranceCoordinates.y * size.x];
        entranceCell.type = 1;
        entranceCell.coordinates = entranceCoordinates;

        var entranceRoom = Instantiate(rooms[1].room, new Vector3(entranceCoordinates.x * offset.x, 0, -entranceCoordinates.y * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
        entranceRoom.name += " " + entranceCoordinates.x + "-" + entranceCoordinates.y;
        entranceRoom.UpdateRoom(entranceCell.status);

        rooms[1].maxNbRooms--;


        // Generate other rooms :
        for(int i = 0; i < size.x; i++) {
            for(int j = 0; j < size.y; j++) {
                int cellNumber = i + j * size.x;
                Cell currentCell = board[cellNumber];

                if(currentCell.visited && cellNumber != (entranceCoordinates.x + entranceCoordinates.y * size.x) && cellNumber != (exitCoordinates.x + exitCoordinates.y * size.x)) {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for(int k = 0; k < rooms.Length; k++) {
                        int count = CountDoors(currentCell.status);
                        bool isNextToSpecialRoom = IsNextToSpecialRoom(cellNumber);
                        int probability = rooms[k].ProbabilityOfSpawning(i, j, count, isNextToSpecialRoom);

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

                    if(rooms[randomRoom].maxNbRooms != -1) {
                        rooms[randomRoom].maxNbRooms--;
                    }

                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.name += " " + i + "-" + j;
                    newRoom.UpdateRoom(currentCell.status);

                    currentCell.type = randomRoom;
                    currentCell.coordinates = (i, j);
                }
            }
        }
    }

    void GenerateMaze() {
        board = new List<Cell>();

        for(int i = 0; i < size.x; i++) {
            for(int j = 0; j < size.y; j++) {
                board.Add(new Cell());
            }
        }

        // Initialize graph :
        graph = new int[board.Count, board.Count];
        for(int i = 0; i < board.Count; i++) {
            for(int j = 0; j < board.Count; j++) {
                graph[i, j] = 0;
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int> ();
        int k = 0;

        while(k < 1000) {
            k++;
            board[currentCell].visited = true;

            if(currentCell == board.Count - 1) break;

            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0) {
                if(path.Count == 0) break;
                else currentCell = path.Pop();
            }

            else {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if(newCell > currentCell){
                    // Going east :
                    if(newCell - 1 == currentCell) {
                        graph[currentCell, newCell] = 1;
                        graph[newCell, currentCell] = 1;
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }

                    // Going south :
                    else{
                        graph[currentCell, newCell] = 1;
                        graph[newCell, currentCell] = 1;
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }

                else {
                    // Going west :
                    if(newCell + 1 == currentCell) {
                        graph[currentCell, newCell] = 1;
                        graph[newCell, currentCell] = 1;
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }

                    // Going north :
                    else{
                        graph[currentCell, newCell] = 1;
                        graph[newCell, currentCell] = 1;
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        GenerateLevel();
    }

    List<int> CheckNeighbors(int cell) {
        List<int> neighbors = new List<int>();

        // Check north neighbor :
        if(cell - size.x >= 0 && !board[cell - size.x].visited) {
            neighbors.Add(cell - size.x);
        }

        // Check south neighbor :
        if(cell + size.x < board.Count && !board[cell + size.x].visited) {
            neighbors.Add(cell + size.x);
        }

        // Check west neighbor :
        if((cell + 1) % size.x != 0 && !board[cell + 1].visited) {
            neighbors.Add(cell + 1);
        }

        // Check east neighbor :
        if(cell % size.x != 0 && !board[cell - 1].visited) {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }
}
