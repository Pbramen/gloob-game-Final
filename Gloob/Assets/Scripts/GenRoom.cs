using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenRoom : MonoBehaviour
{
    public List<GameObject> doors;
    public List<Exit> exits;
    public List<Transform> floors;
    public List<Exit> openPaths;
    public bool active = false;
    void Start() {
        openPaths = new List<Exit>();
        GetComponentInChildren<monitorEnemies>().openDoor.AddListener(destoryDoors);
        StartCoroutine(monitorPaths());
    }
    IEnumerator monitorPaths() {
        yield return new WaitUntil(()=>ModularLevelGenerator.scanComplete);
        foreach (Exit i in exits) {
            if (!i.door.activeInHierarchy) {
                openPaths.Add(i);
                i.CloseDoor();
            }
            yield return new WaitForSeconds(0.1f);
            
        }
        yield return null;
    }

    public void destoryDoors() {
        StartCoroutine(breakDoor());

        IEnumerator breakDoor() {
            yield return new WaitForSeconds(.5f);
            foreach (Exit i in openPaths) {
                i.OpenDoor();
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

}
