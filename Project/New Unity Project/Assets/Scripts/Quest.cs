using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI  objective;
    public Color completedColor;
    public Color activeColor;
    public GameObject questToActivate;
    // Start is called before the first frame update
    private void Start(){
        objective.color = activeColor;
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            FinishQuest();
            gameObject.SetActive(false);
            questToActivate.SetActive(true);
        }
    }
    private void FinishQuest(){
        objective.color = completedColor;
    }
}
