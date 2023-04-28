using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class monitorEnemies : MonoBehaviour
{
    public List<GameObject> enemies;
    public int count;
    public UnityEvent openDoor;
    public GenRoom thisRoom;
    void Start()
    {
        thisRoom = GetComponentInParent<GenRoom>();
        //add all enemy children
        foreach (Transform i in transform) {
            enemies.Add(i.gameObject);
        }
        count = transform.childCount;
        //set all to inactive(should maybe do this pre-compile time)
        foreach (GameObject i in enemies) {
            i.SetActive(false);
        }

        StartCoroutine(generateEnemies());
    }

    IEnumerator generateEnemies(){
        yield return new WaitUntil(() => ModularLevelGenerator.scanComplete && thisRoom.active);
        foreach(GameObject i in enemies){
            i.SetActive(true);
            yield return null;
        }
    }

    public void childDestroied() {
        count--;
        if (count <= 0) {
            openDoor.Invoke();
        }
    }
    

}
