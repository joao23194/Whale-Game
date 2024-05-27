using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{
    bool isPlayerNear = false;
    public string npcName; // Unique name for this NPC
    public string[] dialogueLines; // Dialogue lines for this NPC
    public DialogueManager dialogueManager;
    private Transform playerTransform; // Reference to the player's transform
    private player playerScript; // Reference to the player script
    private bool isFollowingPlayer = false;
    public Transform designatedArea; // Reference to the designated area
    private bool moveToDesignatedArea = false;
    public float followDistance = 2f; // Minimum distance to maintain from the player
    public float moveSpeed = 3f; // Movement speed
    public bool questActive = false; // Indicates whether the associated quest is active
    public Quest associatedQuest;
    public GameObject dialoguePanel; // Reference to the DialoguePanel GameObject

    // Other existing code

    private void Start()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("DialogueManager not found in the scene.");
        }
        playerScript = FindObjectOfType<player>();
        if (playerScript == null)
        {
            Debug.LogError("Player script not found in the scene.");
        }

        // Set questActive to true if the quest is already active
        questActive = CheckQuestActive();
    }

    void Update()
    {
        if (isFollowingPlayer && playerTransform != null && !moveToDesignatedArea)
        {
            FollowPlayer();
        }

        if (moveToDesignatedArea && designatedArea != null)
        {
            MoveToDesignatedArea();
        }

        if (isPlayerNear)
        {
            LookAtPlayer(); // Make the NPC look at the player when near

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialogueManager != null && playerScript != null)
                {
                    dialoguePanel.SetActive(true); // Activate the DialoguePanel GameObject
                    playerScript.LockMovement();
                    playerScript.LookAt(transform); // Make the player look at the NPC
                    dialogueManager.StartDialogue(npcName, dialogueLines, false);
                    dialogueManager.onDialogueEnd += StartFollowingPlayer;
                }
                else
                {
                    Debug.LogError("DialogueManager or Player reference is null.");
                }
            }
        }
    }

    public bool CheckQuestActive()
    {
        if (associatedQuest != null)
        {
            return associatedQuest.IsQuestActive() && associatedQuest.isQuestFinish;
        }
        else
        {
            Debug.LogError("Associated quest not set properly for NPC: " + gameObject.name);
            return false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerTransform = other.transform; // Store reference to the player's transform
            if (questActive && !moveToDesignatedArea) // Check if quest is active and NPC is not moving to designated area
            {
                StartFollowingPlayer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerTransform = null; // Clear reference to the player's transform
        }
    }

    private void LookAtPlayer()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            if (distance > followDistance)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position - direction * followDistance, Time.deltaTime * moveSpeed);
            }
        }
    }

    public void GoToDesignatedArea(Vector3 area)
    {
        if (designatedArea != null)
        {
            designatedArea.position = area;
            moveToDesignatedArea = true;
            isFollowingPlayer = false; // Ensure NPC stops following the player
        }
        else
        {
            Debug.LogError("Designated area is not assigned.");
        }
    }

    private void MoveToDesignatedArea()
    {
        if (designatedArea != null)
        {
            Vector3 direction = (designatedArea.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            transform.position = Vector3.MoveTowards(transform.position, designatedArea.position, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, designatedArea.position) < 0.1f) // Threshold distance to stop moving
            {
                moveToDesignatedArea = false; // Stop moving
                StopFollowingPlayer(); // Stop following the player
            }
        }
    }

    private void StartFollowingPlayer()
    {
        if (!moveToDesignatedArea && !IsAtDesignatedArea()) // Ensure NPC doesn't start following if it is supposed to move to the designated area or already at the designated area
        {
            isFollowingPlayer = true;
            playerScript.UnlockMovement();
        }
        dialogueManager.onDialogueEnd -= StartFollowingPlayer;
    }

    private bool IsAtDesignatedArea()
    {
        if (designatedArea != null)
        {
            float distance = Vector3.Distance(transform.position, designatedArea.position);
            return distance < 0.1f; // Adjust threshold as necessary
        }
        return false;
    }

    public void StopFollowingPlayer()
    {
        isFollowingPlayer = false;
    }
}
