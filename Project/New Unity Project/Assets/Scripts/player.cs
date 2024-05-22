using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player : MonoBehaviour
{
    public static player Instance;
    CharacterController characterController; // Component that controls the player

    public float speed = 6.0f; // Movement speed, settable in the Inspector
    public float jumpSpeed = 8.0f; // Jump speed, settable in the Inspector
    public float gravity = 20.0f; // Gravity, settable in the Inspector
    public float rotSpeed = 720.0f;

    public int Health;
    public int Exp;
    public TMP_Text HealthText;
    public TMP_Text ExpText;
    private Vector3 moveDirection = Vector3.zero; // Vector that controls movement direction
    private bool isMovementLocked = false; // Whether the player's movement is locked

    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get the component from the GameObject
    }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!isMovementLocked)
        {
            if (characterController.isGrounded) // If the character is grounded
            {
                // Recalculate move direction directly from axes
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                moveDirection *= speed; // Multiply this vector by the speed variable
                if (Input.GetButton("Jump")) // If the jump button is pressed (spacebar)
                {
                    moveDirection.y = jumpSpeed; // Add speed in the Y axis
                }

                if (moveDirection != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed * Time.deltaTime);
                }
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below when the moveDirection is multiplied by deltaTime).
            // This is because gravity should be applied as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    public void LockMovement()
    {
        isMovementLocked = true;
        moveDirection = Vector3.zero; // Reset movement direction
        characterController.Move(Vector3.zero); // Stop movement
    }

    public void UnlockMovement()
    {
        isMovementLocked = false;
    }

public void LookAt(Transform target)
{
    Vector3 direction = (target.position - transform.position).normalized;
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
}


    void MouseControl()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void IncreaseHP(int healValue)
    {
        Health += healValue;
    }
}
