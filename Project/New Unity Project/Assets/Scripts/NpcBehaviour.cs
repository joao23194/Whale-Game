using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBehaviour : MonoBehaviour
{

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    bool isPlayerNear = false;
    public NewBehaviourScript dialogueScript;
    public string dialogueFilePath; // Path to the dialogue file for this NPC

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F))
        {
            dialogueScript.StartDialogue(dialogueFilePath);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            audioManager.PlaySFX(audioManager.OtherPlayerTouch);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
