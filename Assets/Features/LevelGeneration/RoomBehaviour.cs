using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{

    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] angleWalls;

    private void Start() {
        foreach (GameObject angleWall in angleWalls) {
            int random = Random.Range(0, 3);

            if (random == 0) {
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

    public void UpdateRoom(bool[] status) {
        for(int i = 0; i < status.Length; i++) {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }
}
