using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player")) {
            int offset_player = 28;
            int offset_camera_ns = 44;
            int offset_camera_ew = 56;

            GameObject player = other.gameObject;
            CharacterController characterController = player.GetComponent<CharacterController>();
            characterController.enabled = false;

            if(gameObject.name == "NorthDoorWall"){
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + offset_player);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + offset_camera_ns);
            }
            else if(gameObject.name == "SouthDoorWall"){
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - offset_player);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - offset_camera_ns);
            }
            else if(gameObject.name == "EastDoorWall"){
                player.transform.position = new Vector3(player.transform.position.x + offset_player, player.transform.position.y, player.transform.position.z);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + offset_camera_ew, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
            else if(gameObject.name == "WestDoorWall"){
                player.transform.position = new Vector3(player.transform.position.x - offset_player, player.transform.position.y, player.transform.position.z);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - offset_camera_ew, Camera.main.transform.position.y, Camera.main.transform.position.z);
            }
            
            characterController.enabled = true;
        }
    }
}
