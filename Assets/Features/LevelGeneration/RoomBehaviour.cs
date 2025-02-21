using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] mapElements;
    public GameObject[] mapEnemies;

    List<bool[]> elementsZones = new List<bool[]>{
        new bool[] {true, false, false, false, false, false, false, false, false},
        new bool[] {false, false, true, false, false, false, false, false, false},
        new bool[] {false, false, false, false, false, false, false, false, true},
        new bool[] {false, false, false, false, false, false, true, false, false},

        new bool[] {true, true, true, false, false, false, false, false, false},
        new bool[] {false, false, true, false, false, true, false, false, true},
        new bool[] {false, false, false, false, false, false, true, true, true},
        new bool[] {true, false, false, true, false, false, true, false, false},

        new bool[] {true, true, true, false, false, true, false, false, true},
        new bool[] {false, false, true, false, false, true, true, true, true},
        new bool[] {true, false, false, true, false, false, true, true, true},
        new bool[] {true, true, true, true, false, false, true, false, false},

        /*new bool[] {true, false, true, true, false, true, true, true, true},
        new bool[] {true, true, true, true, false, false, true, true, true},
        new bool[] {true, true, true, true, false, true, true, false, true},
        new bool[] {true, true, true, false, false, true, true, true, true},

        new bool[] {true, false, true, true, true, true, true, true, true},
        new bool[] {true, true, true, true, true, false, true, true, true},
        new bool[] {true, true, true, true, true, true, true, false, true},
        new bool[] {true, truez, true, false, true, true, true, true, true},

        new bool[] {true, true, true, false, true, true, true, true, true}*/};
    int CountEmptySpaces(bool[] spaces)
    {
        int count = 0;
        for (int i = 0; i < spaces.Length; i++) {
            if (!spaces[i]) count++;
        }
        return count;
    }

    public void BuildRoom(bool[] status, int maxDifficulty)
    {
        int[] doorsIndex = { 1, 7, 5, 3 };
        bool[] map = { false, false, false, false, false, false, false, false, false };

        for (int i = 0; i < status.Length; i++) {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);

            if (status[i]) map[doorsIndex[i]] = true;
        }

        if (mapElements.Length > 0) {
            foreach (GameObject mapElement in mapElements) {
                for (int i = 0; i < mapElement.transform.childCount; i++) {
                    mapElement.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            int rows = elementsZones.Count;
            int cols = 9;

            Dictionary<int, bool[]> zones = new Dictionary<int, bool[]>();

            for (int i = 0; i < rows; i++)
            {
                bool[] row = new bool[cols];
                for (int j = 0; j < cols; j++)
                {
                    row[j] = elementsZones[i][j];
                }
                zones.Add(i, row);
            }

            while (zones.Count > 0)
            {
                List<int> keys = new List<int>(zones.Keys);
                int randomKey = keys[Random.Range(0, keys.Count)];
                bool[] removedElement = zones[randomKey];

                // Check if the element can be added or not :
                bool canBeAdded = true;
                for (int i = 0; i < map.Length; i++)
                {
                    if (map[i] && removedElement[i])
                    {
                        canBeAdded = false;
                        break;
                    }
                }

                int willBeAdded = 0;
                if (canBeAdded) willBeAdded = Random.Range(0, 2);

                // Add the element to the virtual map :
                if (willBeAdded == 1)
                {
                    for (int i = 0; i < map.Length; i++)
                    {
                        if (removedElement[i])
                        {
                            map[i] = removedElement[i];
                        }
                    }

                    // Add the element to the map :
                    int randomChild = Random.Range(0, mapElements[randomKey].transform.childCount);
                    mapElements[randomKey].transform.GetChild(randomChild).gameObject.SetActive(true);
                }

                zones.Remove(randomKey);
            }

            foreach (GameObject mapEnemy in mapEnemies)
            {
                for (int i = 0; i < mapEnemy.transform.childCount; i++)
                {
                    mapEnemy.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (maxDifficulty > 0) {
                int difficulty = Random.Range(1, maxDifficulty + 1);
                int nbEmptySpaces = CountEmptySpaces(map);
                int nbEnemies = Mathf.Max(Mathf.Min(maxDifficulty, nbEmptySpaces), (int)Mathf.Floor(nbEmptySpaces / 3) * difficulty);
                
                List<int> enemiesPositions = new List<int>();
                while (nbEnemies > 0)
                {
                    int randomPosition = Random.Range(0, 9);

                    if (!map[randomPosition] && !enemiesPositions.Contains(randomPosition))
                    {
                        int randomEnemy = Random.Range(0, mapEnemies[randomPosition].transform.childCount);
                        mapEnemies[randomPosition].transform.GetChild(randomEnemy).gameObject.SetActive(true);
                        enemiesPositions.Add(randomPosition);
                        nbEnemies--;
                    }
                }
            }
        }
    }
}
    /*
    void SetupCross() {
        foreach (GameObject angleWall in angleWalls) {
            int random;
            if (angleWall.name == "NorthWest" || angleWall.name == "NorthEast") {
                random = Random.Range(0, 4);

                if (random == 0)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(true);
                    angleWall.transform.GetChild(1).gameObject.SetActive(false);
                    angleWall.transform.GetChild(2).gameObject.SetActive(false);
                    angleWall.transform.GetChild(3).gameObject.SetActive(false);
                }

                else if (random == 1)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(false);
                    angleWall.transform.GetChild(1).gameObject.SetActive(true);
                    angleWall.transform.GetChild(2).gameObject.SetActive(false);
                    angleWall.transform.GetChild(3).gameObject.SetActive(false);
                }

                else if (random == 2)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(false);
                    angleWall.transform.GetChild(1).gameObject.SetActive(true);
                    angleWall.transform.GetChild(2).gameObject.SetActive(true);
                    angleWall.transform.GetChild(3).gameObject.SetActive(false);
                }

                else if (random == 3)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(false);
                    angleWall.transform.GetChild(1).gameObject.SetActive(true);
                    angleWall.transform.GetChild(2).gameObject.SetActive(false);
                    angleWall.transform.GetChild(3).gameObject.SetActive(true);
                }
            }

            else
            {
                random = Random.Range(0, 3);

                if (random == 0)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(true);
                    angleWall.transform.GetChild(1).gameObject.SetActive(false);
                    angleWall.transform.GetChild(2).gameObject.SetActive(false);
                }

                else if (random == 1)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(false);
                    angleWall.transform.GetChild(1).gameObject.SetActive(true);
                    angleWall.transform.GetChild(2).gameObject.SetActive(false);
                }

                else if (random == 2)
                {
                    angleWall.transform.GetChild(0).gameObject.SetActive(false);
                    angleWall.transform.GetChild(1).gameObject.SetActive(true);
                    angleWall.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
}
    */