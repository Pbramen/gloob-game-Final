using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ModularLevelGenerator : MonoBehaviour
{


    [Header("Config")]
    //public int seed = 0;
    public int roomsMin = 10;
    public int roomsMax = 30;
    public List<GameObject> genRooms;
    public GameObject endRoom;

    [Header("Trackers")]
    public int failures = 0;
    public int maxFailures = 100;
    bool generating = true;
    static public bool scanComplete = false;

    List<GameObject> placedRooms;
    Dictionary<Vector2Int,bool> placementDict;
    List<Exit> openExits;
    public seedLevel seed; 
    void Start(){
        if (seed.seed == 0) {
            seed.seed = Random.Range(1, int.MaxValue);
        }
        scanComplete = false;
        StartCoroutine(RegenRoutine());
    }

    IEnumerator RegenRoutine(){
            StartCoroutine(Generate(seed.seed));
            yield return new WaitUntil(()=>!generating);
            AstarPath.active.Scan();
            scanComplete = true;
    }

    void Reset(){
        
        foreach(GameObject g in placedRooms){
            Destroy(g);
        }
    }

    IEnumerator Generate(int newSeed){
        Random.InitState(newSeed);

        //initialize
        placementDict = new Dictionary<Vector2Int, bool>();
        openExits = new List<Exit>();
        failures = 0;
        placedRooms = new List<GameObject>();

        //place all the rooms we'll be using
        List<GameObject> genRoomPalette = new List<GameObject>();
        for(int i = 0; i<genRooms.Count; i++){
            genRoomPalette.Add(Instantiate(genRooms[i],new Vector3(100,i*10,0),Quaternion.identity));
        }

        //place starter room
        GameObject starter = genRoomPalette[Random.Range(0,genRoomPalette.Count)];

        GenRoom starterRoom = Instantiate(starter, Vector3.zero,Quaternion.identity).GetComponent<GenRoom>();
        starterRoom.active = true;
        LockInRoom(starterRoom);
        starterRoom.gameObject.name = "Starter";

        int roomsToSpawn = Random.Range(roomsMin,roomsMax);

        for(int i = 0; i< roomsToSpawn; i++){
            
            //first select an exit
            int selectedExit = Random.Range(0,openExits.Count);
            
            //then select a new room type
            GenRoom nextGenRoom;
            if (i == roomsToSpawn - 1) {
                nextGenRoom = endRoom.GetComponent<GenRoom>();
            }
            else
            {
                nextGenRoom = genRoomPalette[Random.Range(0, genRoomPalette.Count)].GetComponent<GenRoom>();
            }
            //position our next room where it should go
            nextGenRoom.transform.position = openExits[selectedExit].transform.position;

            //check if it overlaps with an existing room
            if(CanPlaceRoom(nextGenRoom)){
                //if it does fit, plop it down, lock it in
                GenRoom copyRoom = Instantiate(nextGenRoom.gameObject,nextGenRoom.transform.position,nextGenRoom.transform.rotation).GetComponent<GenRoom>();
                copyRoom.gameObject.name = "Copy " + i;
                //open the correct doors
                openExits[selectedExit].OpenDoor();
                
                foreach(Exit e in copyRoom.exits){
                    if(Vector3.Distance(e.door.transform.position, openExits[selectedExit].door.transform.position) <= 0.1)
                    {
                        e.OpenDoor();
                        copyRoom.exits.Remove(e);
                        break;
                    }
                }
                openExits.RemoveAt(selectedExit);
                nextGenRoom.transform.position += new Vector3(100,100,0);
                copyRoom.GetComponent<GenRoom>().active=false;
                LockInRoom(copyRoom); //lock in the new room to prevent overlaps
                
                yield return new WaitForSeconds(.1f); //wait some time, timeslicing

            }else{
                
                //if not, increase failures
                failures += 1;
                i-=1;
                if(failures > maxFailures){
                    break;
                }
            }

            //move it out of the way so it looks pretty while it's generating
            nextGenRoom.transform.position += new Vector3(100,100,0);
            
            yield return null;
            
        
        }

        foreach(GameObject g in genRoomPalette){
            Destroy(g);
        }
        generating = false;

    }
    
    bool CanPlaceRoom(GenRoom genRoom){

        
        foreach(Transform t in genRoom.floors){   
            if(placementDict.ContainsKey(Vector2Int.RoundToInt(t.position))){
                return false;
            }
        }

        return true;
    }
    void LockInRoom(GenRoom genRoom){
        foreach(Exit e in genRoom.exits){
            openExits.Add(e);
        }
        foreach(Transform t in genRoom.floors){
            placementDict[Vector2Int.RoundToInt(t.position)] = true;
        }
        placedRooms.Add(genRoom.gameObject);
    }

    public void clearRoom(GenRoom genRoom) { 

    }
}
