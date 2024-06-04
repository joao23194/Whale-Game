using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Quest2 : MonoBehaviour
{
    public TextMeshProUGUI objective;
     public TextMeshProUGUI TimeLimitText;
    public Color completedColor;
    public Color activeColor;
    private bool isQuestActive = false; // Flag to track quest activation status
    public bool isQuestFinish = false; // Flag to track quest completion status
    public bool isNPC = false;
    public GameObject questToActivate;
    public int questIndex = 0;
    public List<GameObject> maps;
    public float timeLimit = 120f; // Time limit for the quest in seconds
    private float timer = 0f;
    private bool isTimerRunning = false;
    private bool isGameOver = false;

    public List<GameObject> NPCsToEscort;
    private int currentNPCIndex = 0;

    public Transform designatedArea; // The area where the NPC goes after the quest is finished
    private player playerScript; // Reference to the player script

    private void Start()
    {
        objective.color = activeColor;
        playerScript = FindObjectOfType<player>(); // Assuming your player script is named Player
    }

    private void Update()
    {
        TimeLimitText.text = timer.ToString();
        if (isTimerRunning && !isQuestFinish)
        {
            timer += Time.deltaTime;
            if (timer >= timeLimit)
            {
                GameOver();
            }
        }
    }

    private void CheckQuestCompletion()
    {
        if (isQuestFinish && !isNPC)
        {
            CompleteQuest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the objective GameObject is valid
            if (objective != null && isNPC)
            {
                // Activate the objective GameObject
                objective.gameObject.SetActive(true);
                isQuestActive = true;
                isTimerRunning = true;
                // Always activate the quest UI when the player enters the trigger zone
                questToActivate.SetActive(true);
                isNPC = false;
            }
            CheckQuestCompletion(); // Check if the quest is finished
        }
    }

    // Method to manually mark the quest as completed
    public void CompleteQuest()
    {
        isQuestFinish = true; // Mark the quest as finished
        FinishQuest();
    }

    // Method to deactivate the quest
    public void DeactivateQuest()
    {
        isQuestActive = false;
    }

    // Method to finish the quest
    private void FinishQuest()
    {
        objective.color = completedColor;
        gameObject.SetActive(false);
        // Activate the map associated with the current quest
        if (questIndex < maps.Count)
        {
            maps[questIndex].SetActive(true);
            questIndex++;
        }

        // Check if the quest is completed and designated area is set
        if (designatedArea != null && isQuestFinish)
        {
            NPCsToEscort[currentNPCIndex].GetComponent<NpcBehaviour>().GoToDesignatedArea(designatedArea.position);
            currentNPCIndex++;
            if (currentNPCIndex >= NPCsToEscort.Count)
            {
                isTimerRunning = false;
            }
        }
    }

    private void GameOver()
    {
        isTimerRunning = false;
        isGameOver = true;
        // Handle game over logic here, e.g., show game over UI, reset quest, etc.
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isNPC)
        {
            // Deactivate the objective GameObject
            objective.gameObject.SetActive(false);
            isQuestActive = false;
            isTimerRunning = false;
            // Deactivate the quest UI when the player exits the trigger zone
            questToActivate.SetActive(false);
            if (!isGameOver)
            {
                // Reset timer if the quest is not completed
                timer = 0f;
            }
        }
    }

    public void RestartQuest()
    {
        isQuestActive = false;
        isQuestFinish = false;
        isNPC = false;
        isTimerRunning = false;
        isGameOver = false;
        timer = 0f;
        objective.color = activeColor;
        gameObject.SetActive(true);
    }
}
