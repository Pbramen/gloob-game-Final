using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void quit() {
        Application.Quit();
    }
    public void switchScene() {
        SceneManager.LoadScene("SampleScene");
    }
    public void tutorialScene() {
        SceneManager.LoadScene("Tutorial");
    }
}
