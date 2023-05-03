using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreen : MonoBehaviour
{
    public void mainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void restart() {
        SceneManager.LoadScene("SampleScene");
    }
    public void exitGame() {
        Application.Quit();
    }
}
