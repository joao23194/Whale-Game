using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI objective;
    public Color completedColor;
    public Color activeColor;
    public bool isQuestFinish = false;
    public GameObject questToActivate;
    
    // Start is called before the first frame update
    private void Start()
    {
        objective.color = activeColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isQuestFinish == true)
            {
                FinishQuest();
                gameObject.SetActive(false);
            }
            questToActivate.SetActive(true);
        }
    }

    private void FinishQuest()
    {
        objective.color = completedColor;

        // Activate the "Map 1" to "Map 4" GameObjects
        for (int i = 1; i <= 4; i++)
        {
            GameObject map = GameObject.Find("Map " + i);
            if (map != null)
            {
                map.SetActive(true);
            }
            else
            {
                Debug.LogWarning("GameObject 'Map " + i + "' not found.");
            }
        }
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Quest : MonoBehaviour
{
    public TextMeshProUGUI objective;
    public Color completedColor;
    public Color activeColor;
    public bool isQuestFinish = false;
    public int questIndex = 0;
    public List<GameObject> maps;

    // Start is called before the first frame update
    private void Start()
    {
        objective.color = activeColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isQuestFinish == true)
            {
                FinishQuest();
                gameObject.SetActive(false);
            }
        }
    }

    private void FinishQuest()
    {
        objective.color = completedColor;

        // Activate the map associated with the current quest
        if (questIndex < maps.Count)
        {
            maps[questIndex].SetActive(true);
            questIndex++;
        }
    }
}
*/