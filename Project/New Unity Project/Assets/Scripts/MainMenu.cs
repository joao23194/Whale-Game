using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject PauseMenu;
    void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            PauseGame();
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
}
