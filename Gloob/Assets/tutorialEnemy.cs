using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Exit exit;
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("ammo")) {
            transform.position = new Vector2(10000, 1000);
            exit.OpenDoor();
        }
    }
}
