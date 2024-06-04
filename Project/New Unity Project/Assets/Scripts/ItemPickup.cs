using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public ItemData Item;
    AudioManager audioManager;

    // Reference to the choice UI
    public GameObject choicePanel;
    public TextMeshProUGUI choiceLabel;
    public Button yesButton;
    public Button noButton;
    private bool isChoiceYes = false;
    private Transform playerTransform;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        // Add listeners to the buttons
        yesButton.onClick.AddListener(OnAcceptItem);
        noButton.onClick.AddListener(OnDeclineItem);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            ShowItemChoice();
        }
    }

    private void ShowItemChoice()
    {
        choiceLabel.text = "Queres apanhar este " + Item.itemName + "?";
        choicePanel.SetActive(true);
        playerTransform.GetComponent<player>().LockMovement();
    }

    private void OnAcceptItem()
    {
        choicePanel.SetActive(false);
        Pickup();
        playerTransform.GetComponent<player>().UnlockMovement();
        isChoiceYes = true;
    }

    private void OnDeclineItem()
    {
        choicePanel.SetActive(false);
        playerTransform.GetComponent<player>().UnlockMovement();
        isChoiceYes = false;
    }

    private void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        audioManager.PlaySFX(audioManager.BrickTouch);
        Debug.Log("Item picked up!");
    }
}
