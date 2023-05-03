using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreen : MonoBehaviour
{
    public properties gloob;
    public void mainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    public void restart() {
        SceneManager.LoadScene("SampleScene");
        gloob.reset();
    }
    public void exitGame() {
        Application.Quit();
    }
}
