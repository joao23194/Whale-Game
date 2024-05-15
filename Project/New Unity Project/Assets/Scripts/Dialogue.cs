using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    public TMP_Text textComponent;
    public float textSpeed;
    private string[] lines;
    private int index;
    private bool isDialogueActive;

    public static NewBehaviourScript Instance;

    void Start()
    {
        Instance = this;
        textComponent.text = string.Empty;
        isDialogueActive = false;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.F))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    public void StartDialogue(string filePath)
    {
        if (!isDialogueActive)
        {
            lines = ReadLinesFromFile(filePath);
            if (lines.Length > 0)
            {
                  gameObject.SetActive(true);
                index = 0;
                textComponent.text = string.Empty;
                isDialogueActive = true;
                StartCoroutine(TypeLine());
            }
            else
            {
                Debug.LogError("Dialogue file is empty or could not be read: " + filePath);
            }
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            isDialogueActive = false;
            gameObject.SetActive(false);
        }
    }

    string[] ReadLinesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllLines(filePath);
        }
        else
        {
            Debug.LogError("Dialogue file not found: " + filePath);
            return new string[0];
        }
    }
}
