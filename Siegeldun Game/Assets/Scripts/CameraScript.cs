using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float cameraYPos = 0;
 
     void Update () {
         transform.position = new Vector3 (player.position.x, player.position.y + cameraYPos, player.position.z - 10);
     }
}
