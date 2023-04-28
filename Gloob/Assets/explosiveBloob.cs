using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveBloob : MonoBehaviour, ammo
{

    public int damageMultiplier;
    public float speed;
    bool isRight;
    Rigidbody2D rd;
    Animator ac; 

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        ac = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(int direction, float speedMult, int baseDamage) {
        Debug.Log("fire explosive ammo");
        damageMultiplier *= baseDamage;
        Vector3 temp = transform.position;
        transform.parent = null;
        transform.position = temp;
        isRight = (direction == -1) ? false : true;
        rd.bodyType = RigidbodyType2D.Dynamic;
        rd.gravityScale = 3f;
        rd.AddForce(new Vector2(speed * speedMult * direction, 0f), ForceMode2D.Impulse);
    }
  
    public void Revert() {  

    }

    IEnumerator explode() {
        rd.gravityScale = 0;
        rd.velocity = Vector2.zero;
        transform.localScale = new Vector3(3f, 3f, 0f);
        ac.SetTrigger("explode");
        yield return new WaitForSeconds(0.5f);
        
        Debug.Log(ac.GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
    }
    void OnCollisionEnter2D(Collision2D other){
        StartCoroutine(explode());
        if(other.gameObject.CompareTag("enemy")){
            other.gameObject.GetComponentInParent<enemyHPHandler>()?.alterHP((damageMultiplier * -1));
        }
    }
    
    public int DamageMultiplier{
        get {
            return damageMultiplier;
        }
        set { 
            damageMultiplier = value; 
        }
    }
    public float Speed{
        get
        {
            return speed;
        }
        set {
            speed = value;
        }
    }
}

