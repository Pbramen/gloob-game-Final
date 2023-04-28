using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HPBloob : MonoBehaviour, ammo{
    // Start is called before the first frame update
    [SerializeField] public int damageMultiplier;
    [SerializeField] public float speed;
    [SerializeField] public float fallBack; 

    Rigidbody2D rd;
    BoxCollider2D box2D;
    private bool isRight, isRunning=false, collided= true, slowing =false;
    private float deadTime = 0.1f;
    private float mTime = 0.1f;
    bool stop = false;
    [Header("Friction")]
    [SerializeField]float minSlow, maxSlow, minD, maxD;
  
    [SerializeField] LayerMask platform;
    
    void Start(){
        deadTime = mTime;
        rd = GetComponent<Rigidbody2D>();
        box2D = GetComponent<BoxCollider2D>();
        
    }
    void FixedUpdate(){
        if(isGround() && !isRunning && rd.velocity != Vector2.zero && slowing){
            StartCoroutine(slow());
        }
    }
    void Update() {
        if(rd.bodyType == RigidbodyType2D.Dynamic)
            deadTime -= Time.deltaTime;
    }
    // Once it collides and lands on a platform -> begins to slow down
    IEnumerator slow(){
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));
        float speed = Random.Range(minSlow, maxSlow);
        isRunning = true;
        rd.velocity *= speed;
        float timer = Random.Range(minD, maxD);
        yield return new WaitForSeconds(timer);
        rd.velocity = new Vector2(0, rd.velocity.y);

        isRunning = false;
    }

    public void Fire(int direction, float speedMult, int baseDamage){
        Debug.Log("fire normal ammo");
        damageMultiplier *= baseDamage;
        Vector3 temp = transform.position;
        transform.parent = null;
        transform.position = temp;
        isRight = (direction == -1) ? false : true;
        rd.bodyType = RigidbodyType2D.Dynamic;
        rd.gravityScale = 0.15f;
        rd.AddForce(new Vector2(speed * speedMult * direction, 0.25f), ForceMode2D.Impulse);
    }

    public void Revert() {
        rd.bodyType = RigidbodyType2D.Kinematic;
        StopAllCoroutines();
        rd.velocity = Vector2.zero;
        
        Debug.Log("this is still running!");
        isRight = false;
        isRunning = false;
        collided = true;
        slowing =false;
        stop = false;

    }
    IEnumerator knockedBack() {
        if (!stop)
        {
            stop = true;
            if (collided && isRight)
            {
                collided = false;
                slowing = true;
                rd.velocity = new Vector2(-fallBack, fallBack);
            }
            else if (collided)
            {
                collided = false;
                slowing = true;
                rd.velocity = new Vector2(fallBack, fallBack);
            }
            rd.gravityScale = 3f;
        }
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D other){

        StartCoroutine(knockedBack());
        if(other.gameObject.CompareTag("enemy")){
            other.gameObject.GetComponentInParent<enemyHPHandler>()?.alterHP((damageMultiplier * -1));
        }
        this.gameObject.layer = LayerMask.NameToLayer("pickUp");
        if (other.gameObject.CompareTag("Player")) {
            Revert();
        }
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        

    }

    bool isGround(){
        return (Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0f, Vector2.down, 0.1f, platform) ||
                Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0f, Vector2.down, 0.1f, platform));
    }

    #region properties
    public int DamageMultiplier{
        get
        {
            return damageMultiplier;
        }

        set {
            damageMultiplier = value;
        }
    }
    public float Speed{
        get {
            return speed;
        }
        set {
            speed = value;
        }
    } 
    #endregion
}
