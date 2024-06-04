using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcBehaviour : MonoBehaviour
{
    bool isPlayerNear = false;
    public string npcName;
    public string[] dialogueLines;
    public DialogueManager dialogueManager;
    private Transform playerTransform;
    private player playerScript;
    private bool isFollowingPlayer = false;
    public Transform designatedArea;
    private bool moveToDesignatedArea = false;
    public float followDistance = 2f;
    public float moveSpeed = 3f;
    public bool questActive = false;
    public Quest associatedQuest;
    public GameObject dialoguePanel;
    private CharacterController characterController;
    private Vector3 velocity;

    // Reference to the choice UI
    public GameObject choicePanel;
    public TextMeshProUGUI choiceLabel;
    public Button yesButton;
    public Button noButton;
    private bool isChoiceYes = false;

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

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController not found on the NPC.");
        }

        // Set questActive to true if the quest is already active
        questActive = CheckQuestActive();

        // Add listeners to the buttons
        yesButton.onClick.AddListener(OnAcceptQuest);
        noButton.onClick.AddListener(OnDeclineQuest);
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
            LookAtPlayer();

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (dialogueManager != null && playerScript != null)
                {
                    dialoguePanel.SetActive(true);
                    playerScript.LockMovement();
                    playerScript.LookAt(transform);
                    dialogueManager.StartDialogue(npcName, dialogueLines, false);
                    dialogueManager.onDialogueEnd += ShowQuestChoice;
                }
                else
                {
                    Debug.LogError("DialogueManager or Player reference is null.");
                }
            }
        }

        // Apply gravity
        if (!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;
        }
        characterController.Move(velocity * Time.deltaTime);
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
            playerTransform = other.transform;
            if (questActive && !moveToDesignatedArea)
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
            playerTransform = null;
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
                Vector3 move = direction * moveSpeed * Time.deltaTime;
                characterController.Move(move);
            }
        }
    }

    public void GoToDesignatedArea(Vector3 area)
    {
        if (designatedArea != null)
        {
            designatedArea.position = area;
            moveToDesignatedArea = true;
            isFollowingPlayer = false;
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

            Vector3 move = direction * moveSpeed * Time.deltaTime;
            characterController.Move(move);

            if (Vector3.Distance(transform.position, designatedArea.position) < 0.1f)
            {
                moveToDesignatedArea = false;
                StopFollowingPlayer();
            }
        }
    }

    public void StartFollowingPlayer()
    {
        if (!moveToDesignatedArea)
        {
            isFollowingPlayer = true;
            playerScript.UnlockMovement();
        }
        dialogueManager.onDialogueEnd -= ShowQuestChoice;
    }

    public void StopFollowingPlayer()
    {
        isFollowingPlayer = false;
    }

    private void ShowQuestChoice()
    {
        if (isChoiceYes) { }
        else
        {
            choiceLabel.text = "Queres aceitar a missão?";
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(true);
            playerScript.LockMovement();
        }
    }

    private void OnAcceptQuest()
    {
        choicePanel.SetActive(false);
        questActive = true;
        associatedQuest.ActivateQuest();
        playerScript.UnlockMovement();
        StartFollowingPlayer();
        isChoiceYes = true;
    }

    private void OnDeclineQuest()
    {
        choicePanel.SetActive(false);
        playerScript.UnlockMovement();
        isChoiceYes = false;
    }
}
