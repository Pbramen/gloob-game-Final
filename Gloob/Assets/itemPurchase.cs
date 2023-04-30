using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemPurchase : MonoBehaviour
{
    public properties stats;
    public gemPickUp points;

    bool itemSpeed = false;
    bool itemDamage = false;
    bool itemAS = false;
    bool itemBomb = false;
    bool itemHeart = false;


    void Update() {
        if (itemSpeed && Input.GetKeyDown(KeyCode.F) && points.pointTracker > 10 ) {
            points.substractPoints(10);
            return;
        }
        if (itemDamage && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 15) {
            points.substractPoints(15);
            stats.attack += 1;
            return;
        }
        if (itemAS && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 20) {
            points.substractPoints(20);
            
            return;
        }
        if (itemBomb && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 10) {
           points.substractPoints(10);
           return;
        }
        if (itemHeart && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 20) {
            points.substractPoints(20);
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("itemSpeed")) {
            itemSpeed = true;
        }
    
        if (other.gameObject.CompareTag("itemAS")) {
            itemAS = true;
        }
        if (other.gameObject.CompareTag("itemDamage")) {
            itemDamage = true;
        }
        if (stats.curHP < 10)
        {
            if (other.gameObject.CompareTag("itemHeart"))
            {
                itemHeart = true;
            }
            if (other.gameObject.CompareTag("itemBomb"))
            {
                itemBomb = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("itemSpeed")) {
            itemSpeed = false;
        }
    
        if (other.gameObject.CompareTag("itemAS")) {
            itemAS = false;
        }
        if (other.gameObject.CompareTag("itemDamage")) {
            itemDamage = false;
        }
        if (stats.curHP < 10)
        {
            if (other.gameObject.CompareTag("itemHeart"))
            {
                itemHeart = false;
            }
            if (other.gameObject.CompareTag("itemBomb"))
            {
                itemBomb = false;
            }
        }
    }
}