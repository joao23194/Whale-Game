using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ArtefactoPickup : MonoBehaviour
{

    Audio audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<Audio>();
    }

    public ItemData Item;
    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        audioManager.PlaySFX(audioManager.Artefacto);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
            audioManager.PlaySFX(audioManager.Artefacto);
        }
    }
}
