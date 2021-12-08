using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] bool yFreeze;

    [Range(1,10)]
    [SerializeField] float smoothFactor = 2.5f;

    public void Start()
    {
        cameraOffset.z = -10;
    }

    public void FixedUpdate()
    {
        Follow();
    }
    
    private void Follow()
    {
        Vector3 playerPosition;

        if(yFreeze)
        {
            cameraOffset.y = -1;
            playerPosition = new Vector3 (player.position.x + cameraOffset.x, cameraOffset.y, cameraOffset.z);
        }
        else
        {
            cameraOffset.y = 2;
            playerPosition = player.position + cameraOffset;
        }

        transform.position = Vector3.Lerp(transform.position, playerPosition, smoothFactor*Time.fixedDeltaTime); // Linear Interpolation, Moves in an axis in a linear motion
    }
}
