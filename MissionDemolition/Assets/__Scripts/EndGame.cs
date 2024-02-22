using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EndGame : MonoBehaviour
{

    public GameObject quitBtn;
    public GameObject restartBtn;

    // Restart game when restartBtn or restartBtnBack onClick is activated.
    public void RestartGame() {
        SceneManager.LoadScene(0);
    }

     // Return to menu when quitBtn or quitBtnBack onClick is activated.
    public void QuitGame() {
        Application.Quit();
    }

}