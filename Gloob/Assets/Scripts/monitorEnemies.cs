using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class monitorEnemies : MonoBehaviour
{
    public List<GameObject> enemies;
    public int count =0;
    public UnityEvent openDoor;
    public GenRoom thisRoom;
    bool nonCombat = false;
    void Start()
    {
        thisRoom = GetComponentInParent<GenRoom>();
        //add all enemy children
        foreach (Transform i in transform) {
            enemies.Add(i.gameObject);
        }
        count = transform.childCount;
        if (count == 0) {
            nonCombat = true;
        }
        else
        {
            //set all to inactive(should maybe do this pre-compile time)
            foreach (GameObject i in enemies)
            {
                i.SetActive(false);
            }
        }

        StartCoroutine(generateEnemies());
    }

    IEnumerator generateEnemies(){
        yield return new WaitUntil(() => ModularLevelGenerator.scanComplete && thisRoom.active);
        if (nonCombat) {
            Debug.Log("door is opened!");
            yield return new WaitForSeconds(1f);
            openDoor?.Invoke();
        }
        else
        {
            foreach (GameObject i in enemies)
            {
                i.SetActive(true);
                yield return null;
            }
        }
    }

    public void childDestroied() {
        count--;
        if (count <= 0) {
            openDoor.Invoke();
        }
    }
    

}
