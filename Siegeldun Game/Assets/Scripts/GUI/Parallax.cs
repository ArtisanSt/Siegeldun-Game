using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField] Vector2 parallaxMultiplier;
    [SerializeField] float parallaxYPos = 0.9f;
    [SerializeField] bool enableYchange;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = this.transform.parent;
        transform.position = new Vector3(transform.position.x, parallaxYPos);
        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        if(enableYchange)
        {
            transform.position -= new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y);
            transform.position = new Vector3(transform.position.x, transform.position.y);
        } else
        {
            transform.position -= new Vector3(deltaMovement.x * parallaxMultiplier.x, 0);
            transform.position = new Vector3(transform.position.x, parallaxYPos);
        }
        lastCameraPosition = cameraTransform.position;
    }

}