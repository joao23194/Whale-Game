using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public TextMeshProUGUI  objective;
    public Color completedColor;
    public Color activeColor;
    public bool isQuestFinish = false;
    public GameObject questToActivate;
    // Start is called before the first frame update
    private void Start(){
        objective.color = activeColor;
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            if(isQuestFinish)
                FinishQuest();
            gameObject.SetActive(false);
            questToActivate.SetActive(true);
            audioManager.PlaySFX(audioManager.Quest);
        }
    }
    private void FinishQuest(){
        objective.color = completedColor;
    }
}
