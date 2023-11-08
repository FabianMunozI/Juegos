using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class facecamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        // Find the main camera in the scene
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (mainCameraTransform != null)
        {
            // Calculate the direction from the plane to the camera, but only in the Y-axis
            Vector3 lookDirection = mainCameraTransform.position - transform.position;
            lookDirection.y = 0f; // Ignore the Y-axis component

            // Ensure that the plane only rotates around its Y-axis to face the camera
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y, 0f);
        }
    }
}