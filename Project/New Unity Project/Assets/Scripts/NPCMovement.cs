using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoint transforms
    public float speed = 5.0f; // Movement speed

    private int currentWaypointIndex = 0;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        if (waypoints.Length > 0)
        {
            SetNextWaypoint();
        }
        else
        {
            Debug.LogError("No waypoints assigned to NPC: " + gameObject.name);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        // Move towards the target position if not already moving
        if (!isMoving)
        {
            MoveTowardsWaypoint();
        }

        // If the NPC reaches the target position, set the next waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNextWaypoint();
        }
    }

    void SetNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        targetPosition = waypoints[currentWaypointIndex].position;
    }

    void MoveTowardsWaypoint()
    {
        targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    public void StopMovement()
    {
        isMoving = false;
    }

    public void ResumeMovement()
    {
        isMoving = true;
    }
}
