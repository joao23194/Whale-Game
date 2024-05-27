using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI objective;
    public Color completedColor;
    public Color activeColor;
    private bool isQuestActive = false; // Flag to track quest activation status
    public bool isQuestFinish = false; // Flag to track quest completion status
    public bool isNPC = false;
    public GameObject questToActivate;
    public int questIndex = 0;
    public List<GameObject> maps;

    public GameObject DesignatedNPC; // Reference to the NPC associated with this quest
    public Transform designatedArea; // The area where the NPC goes after the quest is finished

    // Method to check if the quest is active
    public bool IsQuestActive()
    {
        return isQuestActive && !isQuestFinish; // Quest is active if it's active and not finished
    }

    private void Start()
    {
        objective.color = activeColor;
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
            // Set NPC behavior to move towards the designated area
            DesignatedNPC.GetComponent<NpcBehaviour>().GoToDesignatedArea(designatedArea.position);
        }
    }

}
