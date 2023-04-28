using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pewpew : MonoBehaviour
{
    public Vector3 target { get; set; }
    public float speed;
    private Vector3 direction;
    void Awake() {
        target = FindObjectOfType<gloob>().transform.position;
        direction = (target - transform.position).normalized;
    
    }

    void Update(){
        transform.position += direction * speed * Time.deltaTime;
    }
    void OnCollisionEnter2D(Collision2D other){
        Destroy(this.gameObject);
    }
}
