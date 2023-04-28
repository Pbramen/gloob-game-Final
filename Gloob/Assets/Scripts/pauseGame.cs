using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class pauseGame : MonoBehaviour
{
    public static bool isPausing = false;
    public UnityEvent openMenu;
    public UnityEvent closeMenu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !isPausing){
            Debug.Log("Paused");
            isPausing=true;
            openMenu?.Invoke();
            pause();
        }
        else if (Input.GetButtonDown("Cancel") && isPausing){
            Debug.Log("Resuming");
            closeMenu?.Invoke();
            resume();
        } 
    }
    void pause(){
        Time.timeScale = 0;
    }
    public void resume(){
        isPausing = false;
        Time.timeScale = 1;
    }
}
