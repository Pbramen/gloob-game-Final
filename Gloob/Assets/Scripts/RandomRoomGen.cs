using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoomGen : MonoBehaviour
{
    [Header("Config")]
    public int seed;
    public int minRoom, maxRoom;
    public List<GameObject> roomPrefabs;

    [Header("Trackers")]
    public int failures = 0;
    public int maxFailures = 100;
    bool generating = false;

    //list of rooms to be placed
    List<GameObject> placedRooms;
    //Dictionary to see if room needs to be placed
    Dictionary<Vector2Int,bool> placementDict;
    //list of exits
    List<Exit> openExits;
    

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartGeneration());
    
        placementDict = new Dictionary<Vector2Int, bool>();
        openExits = new List<Exit>();
        failures = 0;
        placedRooms = new List<GameObject>();

        List<GameObject> roomPalette= new List<GameObject>();
        for(int i = 0; i < roomPrefabs.Count; i++){
            roomPalette.Add(Instantiate(roomPrefabs[i], new Vector3(100, i*30, 0), Quaternion.identity));
        }
        GameObject start = roomPalette[Random.Range(0, roomPalette.Count)];
        GameObject startRoom = Instantiate(start, Vector3.zero, Quaternion.identity);

        //SingleRoom startingRoom = Instantiate(start, Vector3.zero, Quaternion.identity).GetComponent<SingleRoom>();
    }

    IEnumerator StartGeneration(){
        generating = true;
        StartCoroutine(Generate(seed));
        yield return new WaitUntil(()=>!generating);

    }
    IEnumerator Generate(int newseed){

        Random.InitState(newseed);

        placementDict = new Dictionary<Vector2Int, bool>();
        openExits = new List<Exit>();
        failures = 0;
        placedRooms = new List<GameObject>();

        List<GameObject> roomPalette= new List<GameObject>();
        for(int i = 0; i < roomPrefabs.Count; i++){
            roomPalette.Add(Instantiate(roomPrefabs[i], new Vector3(100, i*30, 0), Quaternion.identity));
        }
        GameObject start = roomPalette[Random.Range(0, roomPalette.Count)];
        SingleRoom startingRoom = Instantiate(start, Vector3.zero, Quaternion.identity).GetComponent<SingleRoom>();
        startingRoom.testExit();
   
        yield return null;
    }
    // Update is called once per frame

    // check the floors if it was added to the dictionary already
    bool CanPlaceRoom(SingleRoom room){
        foreach(Transform t in room.Floor){
            if(placementDict.ContainsKey(Vector2Int.RoundToInt(t.position))){
                return false;
            }
        }
        return true;
    }

    void LockInRoom(SingleRoom room){
        foreach(Exit e in room.Exits){
            openExits.Add(e);
        }
        foreach(Transform t in room.Floor){
            placementDict[Vector2Int.RoundToInt(t.position)]=true;
        }
        placedRooms.Add(room.gameObject);
    }

}
