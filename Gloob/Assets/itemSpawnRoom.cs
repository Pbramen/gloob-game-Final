using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class itemSpawnRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> powerUp;
    public GameObject[] location;
    public List<GameObject> item;
    int numChild;
    void Start()
    {
        numChild= transform.childCount;
        location = new GameObject[numChild];
        for (int i = 0; i < numChild; i++) {
            location[i] = transform.GetChild(i).gameObject;
        }
        StartCoroutine(generateItem());
    }
    IEnumerator generateItem(){
        yield return new WaitUntil(() => ModularLevelGenerator.scanComplete);
        for (int i = 0; i < numChild; i++)
        {
            //pick the prefab
            GameObject a = powerUp[Random.Range(0, numChild)];
            //create a new instance
            GameObject b = Instantiate(a, location[i].transform.position, Quaternion.identity);
            
            item.Add(b.gameObject);
            yield return null;
        }
    }
    public void destroyAllItems() {
        foreach (GameObject i in item) {
            Destroy(i.gameObject);
        }
    }
}
