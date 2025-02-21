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
        
        Vector3 playerPositionOffset = Vector3.zero;
        Vector3 cameraPositionOffset = Vector3.zero;;
        
        if(gameObject.name == "NorthDoorWall"){
            playerPositionOffset = new Vector3(0, 0, offsetPlayer);
            cameraPositionOffset = new Vector3(0, 0, offsetCameraNs);
        }
        else if(gameObject.name == "SouthDoorWall"){
            playerPositionOffset = new Vector3(0, 0, -offsetPlayer);
            cameraPositionOffset = new Vector3(0, 0, -offsetCameraNs);
        }
        else if(gameObject.name == "EastDoorWall"){
            playerPositionOffset = new Vector3(offsetPlayer, 0, 0);
            cameraPositionOffset = new Vector3(offsetCameraEw, 0, 0);
        }
        else if(gameObject.name == "WestDoorWall"){
            playerPositionOffset = new Vector3(-offsetPlayer, 0, 0);
            cameraPositionOffset = new Vector3(-offsetCameraEw, 0, 0);
        }
        
        GameManager.Instance.TeleportCharacterBy(playerPositionOffset, cameraPositionOffset);
    }
}
