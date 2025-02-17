using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        int offsetPlayer = 28;
        int offsetCameraNs = 44;
        int offsetCameraEw = 56;
        
        Vector3 playerPosition = MainCharacter.Instance.transform.position;
        Vector3 cameraPosition = Camera.main.transform.position;
        
        if(gameObject.name == "NorthDoorWall"){
            playerPosition += new Vector3(0, 0, offsetPlayer);
            cameraPosition += new Vector3(0, 0, offsetCameraNs);
        }
        else if(gameObject.name == "SouthDoorWall"){
            playerPosition -= new Vector3(0, 0, offsetPlayer);
            cameraPosition -= new Vector3(0, 0, offsetCameraNs);
        }
        else if(gameObject.name == "EastDoorWall"){
            playerPosition += new Vector3(offsetPlayer, 0, 0);
            cameraPosition += new Vector3(offsetCameraEw, 0, 0);
        }
        else if(gameObject.name == "WestDoorWall"){
            playerPosition -= new Vector3(offsetPlayer, 0, 0);
            cameraPosition -= new Vector3(offsetCameraEw, 0, 0);
        }
        
        MainCharacter.Instance.TeleportTo(playerPosition, cameraPosition);
    }
}
