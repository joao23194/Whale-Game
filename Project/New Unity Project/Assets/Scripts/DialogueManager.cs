using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text textComponent;
    public TMP_Text npcNameText; // Text component to display NPC's name
    public float textSpeed;
    public GameObject dialoguePanel; // Reference to the DialoguePanel GameObject

    private Dictionary<string, Queue<string>> npcDialogues = new Dictionary<string, Queue<string>>();
    private string currentNPC;
    private bool isDialogueActive;

    public static DialogueManager Instance;

    public event Action onDialogueEnd;

    private bool isMovingToDesignatedArea = false; // Flag to track if NPC is moving to designated area

    private bool isTyping = false; // Flag to track if typing coroutine is running
    private bool textFullyDisplayed = false; // Flag to track if text is fully displayed

    private bool isFirstLineDisplayed = false; // Flag to track if the first line has been displayed

    void Start()
    {
        Instance = this;
        textComponent.text = string.Empty;
        isDialogueActive = false;
        dialoguePanel.SetActive(false); // Ensure the dialogue panel is initially inactive
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.F))
        {
            if (!isTyping && textFullyDisplayed) // Check if not typing and text is fully displayed
            {
                DisplayNextLine();
            }
        }
    }

    public void StartDialogue(string npcName, string[] dialogueLines, bool isMovingToDesignatedArea)
    {
        if (!isDialogueActive)
        {
            if (!npcDialogues.ContainsKey(npcName))
            {
                Queue<string> dialogueQueue = new Queue<string>(dialogueLines);
                npcDialogues.Add(npcName, dialogueQueue);
            }
            currentNPC = npcName; // Set current NPC
            isDialogueActive = true;
            dialoguePanel.SetActive(true); // Activate the DialoguePanel GameObject
            npcNameText.text = npcName; // Display the NPC's name
            this.isMovingToDesignatedArea = isMovingToDesignatedArea; // Set NPC movement state

            // Display the first line of dialogue immediately
            DisplayNextLine();
        }
        else
        {
            Debug.LogWarning("Dialogue already started for NPC: " + npcName);
        }
    }

    void DisplayNextLine()
    {
        if (npcDialogues.ContainsKey(currentNPC))
        {
            if (npcDialogues[currentNPC].Count > 0)
            {
                string line = npcDialogues[currentNPC].Dequeue();
                
                if (!isFirstLineDisplayed)
                {
                    // Display the first line character by character
                    StartCoroutine(TypeFirstLine(line));
                }
                else
                {
                    // Display subsequent lines normally
                    StartCoroutine(TypeLine(line));
                }
            }
            else
            {
                EndDialogue();
            }
        }
    }

    IEnumerator TypeFirstLine(string line)
    {
        isTyping = true; // Set typing flag to true

        StringBuilder stringBuilder = new StringBuilder(); // Use StringBuilder to efficiently build the line

        foreach (char c in line.ToCharArray())
        {
            stringBuilder.Append(c); // Append each character to the StringBuilder
            textComponent.text = stringBuilder.ToString(); // Update the text with the current content of the StringBuilder
            yield return new WaitForSeconds(0.1f); // Delay for character by character display
        }

        isTyping = false; // Reset typing flag
        textFullyDisplayed = true; // Set text fully displayed flag after typing completes
        isFirstLineDisplayed = true; // Set the flag to indicate that the first line has been displayed

        // Display the second line after a very short delay
        yield return new WaitForSeconds(0.01f);
        DisplayNextLine();
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true; // Set typing flag to true
        textComponent.text = string.Empty; // Clear the text component for the new line

        StringBuilder stringBuilder = new StringBuilder(); // Use StringBuilder to efficiently build the line

        foreach (char c in line.ToCharArray())
        {
            stringBuilder.Append(c); // Append each character to the StringBuilder
            textComponent.text = stringBuilder.ToString(); // Update the text with the current content of the StringBuilder
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false; // Reset typing flag
        textFullyDisplayed = true; // Set text fully displayed flag after typing completes
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        npcDialogues.Remove(currentNPC); // Remove the dialogue from the dictionary
        currentNPC = null;
        textComponent.text = string.Empty;
        npcNameText.text = string.Empty;
        dialoguePanel.SetActive(false); // Deactivate the DialoguePanel GameObject

        player.Instance.UnlockMovement(); // Unlock the player's movement

        if (!isMovingToDesignatedArea) // Invoke event only if NPC is not moving to designated area
        {
            if (onDialogueEnd != null)
            {
                onDialogueEnd.Invoke(); // Invoke the event
            }
        }
        textFullyDisplayed = false; // Reset text fully displayed flag
    }
}
