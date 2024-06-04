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
    public float textSpeed = 0.1f;
    public GameObject dialoguePanel; // Reference to the DialoguePanel GameObject

    private Dictionary<string, Queue<string>> npcDialogues = new Dictionary<string, Queue<string>>();
    private string currentNPC;
    private bool isDialogueActive;

    public static DialogueManager Instance;

    public event Action onDialogueEnd;

    private bool isMovingToDesignatedArea = false; // Flag to track if NPC is moving to designated area
    private bool isTyping = false; // Flag to track if typing coroutine is running
    private bool textFullyDisplayed = false; // Flag to track if text is fully displayed
    private bool isFirstLine = true; // Flag to track if it is the first line

    void Start()
    {
        Instance = this;
        textComponent.text = string.Empty;
        isDialogueActive = false;
        dialoguePanel.SetActive(false); // Ensure the dialogue panel is initially inactive
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTyping && textFullyDisplayed)
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
        currentNPC = npcName;
        isDialogueActive = true;
        npcNameText.text = npcName; // Display the NPC's name
        this.isMovingToDesignatedArea = isMovingToDesignatedArea;
        isFirstLine = true; // Reset the first line flag

        if (isFirstLine)
        {
            // Display the first line immediately
            DisplayNextLine();
            StartCoroutine(DelayNextLine());
        }
        else
        {
            DisplayNextLine();
        }
    }
    else
    {
        Debug.LogWarning("Dialogue already started for NPC: " + npcName);
    }
}

IEnumerator DelayNextLine()
{
    yield return new WaitForSeconds(0.01f);
    DisplayNextLine();
}


    void DisplayNextLine()
    {
        if (npcDialogues.ContainsKey(currentNPC))
        {
            if (npcDialogues[currentNPC].Count > 0)
            {
                string line = npcDialogues[currentNPC].Dequeue();
                if (isFirstLine)
                {
                    textComponent.text = line; // Display the first line immediately
                    textFullyDisplayed = true; // Set the text fully displayed flag
                    isFirstLine = false; // Reset the first line flag
                }
                else
                {
                    StartCoroutine(TypeLine(line));
                }
            }
            else
            {
                EndDialogue();
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        textFullyDisplayed = false; // Reset the textFullyDisplayed flag
        textComponent.text = string.Empty;

        StringBuilder stringBuilder = new StringBuilder();
        foreach (char c in line.ToCharArray())
        {
            stringBuilder.Append(c);
            textComponent.text = stringBuilder.ToString();
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        textFullyDisplayed = true; // Set the textFullyDisplayed flag after typing completes
    }


    void EndDialogue()
    {
        isDialogueActive = false;
        npcDialogues.Remove(currentNPC);
        currentNPC = null;
        textComponent.text = string.Empty;
        npcNameText.text = string.Empty;
        dialoguePanel.SetActive(false); // Deactivate the DialoguePanel GameObject

        player.Instance.UnlockMovement(); // Unlock the player's movement

        if (!isMovingToDesignatedArea && onDialogueEnd != null)
        {
            onDialogueEnd.Invoke(); // Invoke the event if NPC is not moving to a designated area
        }
        textFullyDisplayed = false;
    }
}
