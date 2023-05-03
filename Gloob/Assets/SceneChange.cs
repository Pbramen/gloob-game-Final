using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public properties stats;
    // Start is called before the first frame update
    public void quit() {
        Application.Quit();
    }
    public void switchScene() {
        stats.curHP = 10;
        SceneManager.LoadScene("SampleScene");
    }
    public void tutorialScene() {
        stats.curHP = 5;
        SceneManager.LoadScene("Tutorial");
    }
}
