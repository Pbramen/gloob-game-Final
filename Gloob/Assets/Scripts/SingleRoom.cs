using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRoom : MonoBehaviour
{
    public List<GameObject>Door;
    public List<Exit>Exits;
    public List<Transform>Floor;


    void Start(){
        testExit();
    }
    public void testExit(){
        if (Exits == null){
            Debug.Log("Why");
        }
        else{
            foreach(Exit e in Exits){
                Debug.Log(e.name);
            }
        }
    }
    public void test(){
        Debug.Log("Yes");
    }
}
