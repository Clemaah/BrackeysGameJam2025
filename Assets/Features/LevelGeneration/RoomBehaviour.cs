using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.VersionControl.Asset;
using static UnityEngine.InputSystem.Controls.AxisControl;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] doorsBlockers;
    public GameObject[] mapElements;
    public GameObject[] mapEnemies;
    public GameObject lamp;
    public GameObject[] weapons;

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

    bool roomOpened = false;
    int roomType = -1;

    int CountEmptySpaces(bool[] spaces)
    {
        int count = 0;
        for (int i = 0; i < spaces.Length; i++) {
            if (!spaces[i]) count++;
        }
        return count;
    }

    void Update()
    {
        if (roomOpened) return;

        if ((roomType == 0 && CountEnemies() == 0) ||
            (roomType == 1 && (lamp.gameObject.IsDestroyed())))
        {
            OpenRoom();
            roomOpened = true;
        }
    }

    public void BuildRoom(bool[] status, int maxDifficulty)
    {
        if (transform.name.Contains("MobRoom")) roomType = 0;
        if (transform.name.Contains("EntranceRoom")) roomType = 1;
        if (transform.name.Contains("ExitRoom")) roomType = 2;
        if (transform.name.Contains("HealRoom")) roomType = 3;
        if (transform.name.Contains("WishRoom")) roomType = 4;
        if (transform.name.Contains("BossRoom")) roomType = 5;
        if (transform.name.Contains("WeaponRoom")) roomType = 6;

        int[] doorsIndex = { 1, 7, 5, 3 };
        bool[] map = { false, false, false, false, false, false, false, false, false };

        for (int i = 0; i < status.Length; i++) {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);

            if (roomType == 0 || roomType == 1) doorsBlockers[i].SetActive(status[i]);
            else doorsBlockers[i].SetActive(false);

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

    public void OpenRoom()
    {
        if (doorsBlockers.Length == 0) return;
        StartCoroutine(Tween.To(1.0f, doorsBlockers[0].gameObject.transform.position.y, doorsBlockers[0].gameObject.transform.position.y - 3.75f, 
            height =>
            {
                foreach (GameObject t in doorsBlockers)
                {
                    Vector3 vector3 = t.gameObject.transform.position;
                    vector3.y = height;
                    t.gameObject.transform.position = vector3;
                }
            }
            , easeType: Tween.EaseType.EaseOutCubic));
    }

    int CountEnemies() {
        int count = 0;
        foreach (GameObject mapEnemy in mapEnemies) {
            for (int i = 0; i < mapEnemy.transform.childCount; i++) {
                if (mapEnemy.transform.GetChild(i).gameObject.activeInHierarchy && !mapEnemy.transform.GetChild(i).gameObject.IsDestroyed()) {
                    count++;
                }
            }
        }
        return count;
    }
}