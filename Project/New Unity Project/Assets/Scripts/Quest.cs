using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI objective;
    public Color completedColor;
    public Color activeColor;
    private bool isQuestActive = false;
    public bool isQuestFinish = false;
    public bool isNPC = false;
    public GameObject questToActivate;
    public int questIndex = 0;
    public List<GameObject> maps;

    public GameObject DesignatedNPC;
    public Transform designatedArea;
    private player playerScript;

    public bool IsQuestActive()
    {
        return isQuestActive && !isQuestFinish;
    }

    private void Start()
    {
        objective.color = activeColor;
        objective.gameObject.SetActive(false);
        questToActivate.SetActive(false);
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
            if (objective != null && isNPC)
            {
                objective.gameObject.SetActive(true);
                isQuestActive = true;
                questToActivate.SetActive(true);
                isNPC = false;
            }
            CheckQuestCompletion();
        }
    }

    public void ActivateQuest()
    {
        isQuestActive = true;
        objective.gameObject.SetActive(true);
        questToActivate.SetActive(true);
    }

    public void CompleteQuest()
    {
        isQuestFinish = true;
        FinishQuest();
    }

    public void DeactivateQuest()
    {
        isQuestActive = false;
    }

    private void FinishQuest()
    {
        objective.color = completedColor;
        gameObject.SetActive(false);

        if (questIndex < maps.Count)
        {
            maps[questIndex].SetActive(true);
            questIndex++;
        }

        if (designatedArea != null && isQuestFinish)
        {
            DesignatedNPC.GetComponent<NpcBehaviour>().GoToDesignatedArea(designatedArea.position);
        }
    }
}
