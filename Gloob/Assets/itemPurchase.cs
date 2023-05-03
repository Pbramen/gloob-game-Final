using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class itemPurchase : MonoBehaviour
{
    public properties stats;
    public gemPickUp points;

    bool itemSpeed = false;
    bool itemDamage = false;
    bool itemAS = false;
    bool itemBomb = false;
    bool itemHeart = false;
    public Text popUP;


    public UnityEvent hpPurchase;
    public UnityEvent bombPurchase;

    IEnumerator purchaseMessage() {
        popUP.enabled = true;
        yield return new WaitForSeconds(0.5f);
        popUP.enabled = false;
    }
    void Update() {
        if (itemSpeed && Input.GetKeyDown(KeyCode.F) && points.pointTracker > 10 ) {
            points.substractPoints(10);
            stats.speed += 1;
            StartCoroutine(purchaseMessage());
            return;
        }
        else if (itemDamage && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 15) {
            points.substractPoints(15);
            stats.baseDamage += 1;
            StartCoroutine(purchaseMessage());
            return;
        }
        else if (itemAS && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 20) {
            points.substractPoints(20);
            stats.attackSpeed -= 0.10f;
            StartCoroutine(purchaseMessage());
            return;
        }
        else if (itemBomb && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 10) {
           points.substractPoints(10);
            //add bomb
            bombPurchase?.Invoke();
            StartCoroutine(purchaseMessage());
            return;
        }
        else if (itemHeart && Input.GetKeyDown(KeyCode.F)  && points.pointTracker > 20) {
            points.substractPoints(20);
            //add regular gem
            hpPurchase?.Invoke();
            StartCoroutine(purchaseMessage());
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
