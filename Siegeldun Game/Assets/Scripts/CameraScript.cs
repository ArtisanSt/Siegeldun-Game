using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
 
     void Update () {
         transform.position = new Vector3 (player.position.x, player.position.y + 1, player.position.z - 10);
     }
}
