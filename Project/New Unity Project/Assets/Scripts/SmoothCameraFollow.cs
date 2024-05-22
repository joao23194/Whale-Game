using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform player;       // Reference to the player’s transform
    public Vector3 offset;         // Offset between the player and the camera
    public float smoothSpeed = 0.125f; // Smooth speed factor

    private void LateUpdate()
    {
        // Desired position of the camera
        Vector3 desiredPosition = player.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Set the camera's position to the smoothed position
        transform.position = smoothedPosition;

        // Optionally, you can set the camera's rotation to a fixed value if you want it to remain static
        // transform.rotation = Quaternion.Euler(30f, 0f, 0f); // Example of a fixed rotation
    }
}
