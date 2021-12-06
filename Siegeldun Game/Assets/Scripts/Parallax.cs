using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Vector2 parallaxMultiplier;
    [SerializeField] Transform cameraTransform;
    public float parallaxYPos = 0.9f;
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
        transform.position -= new Vector3(deltaMovement.x * parallaxMultiplier.x, 0);
        transform.position = new Vector3(transform.position.x, parallaxYPos);
        lastCameraPosition = cameraTransform.position;
    }

}
