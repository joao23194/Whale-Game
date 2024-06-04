using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerBehaviour : MonoBehaviour
{
    public string nivelACarregar;

    public void NextLevel()
    {
        if (FadeManager.instance != null)
        {
            FadeManager.instance.FadeOutAndIn(nivelACarregar);
        }
        else
        {
            SceneManager.LoadScene(nivelACarregar);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NextLevel();
        }
    }
}
