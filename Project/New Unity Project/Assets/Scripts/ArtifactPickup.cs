using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public ItemData Item;
    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        audioManager.PlaySFX(audioManager.Artifact);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) ;
        {
            Pickup();
        }
    }
}
