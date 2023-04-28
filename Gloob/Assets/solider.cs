using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class solider : MonoBehaviour
{
    gloob mainGloob;
    public Animator attack;
    Coroutine dash;
    private bool isAttacking = false;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        mainGloob = FindObjectOfType<gloob>();
        dash = StartCoroutine(forwardDash());
    }

    IEnumerator forwardDash() {
        while (true)
        {
            yield return new WaitUntil(() => isAttacking);
            Vector2 direction = (mainGloob.transform.position - transform.position).normalized;
            direction.y = 0;
            if (direction.x > 0) {
                this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else{
                this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            attack.SetTrigger("isAttacking");
            yield return new WaitForSeconds(0.05f);
            this.gameObject.layer = LayerMask.NameToLayer("projectile");
            GetComponent<Rigidbody2D>().velocity = direction * speed;
            yield return new WaitForSeconds(0.95f);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            this.gameObject.layer = LayerMask.NameToLayer("enemy");
            yield return new WaitForSeconds(1f);
            isAttacking = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, mainGloob.transform.position) <=3) {
            isAttacking = true;
        }
    }
    
}
