using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateRoom : MonoBehaviour
{
    public GenRoom thisRoom;
    void Start() {
        thisRoom = GetComponentInParent<GenRoom>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (thisRoom != null)
        {
            Debug.Log("Room found");
            if (other.CompareTag("Player") && !thisRoom.active)
            {
                thisRoom.active = true;
                
            }
        }
    }
}
