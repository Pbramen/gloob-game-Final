using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class gloob: MonoBehaviour{

    [SerializeField] Rigidbody2D rd;
    private SpriteRenderer spriteR;
    private BoxCollider2D box2D;
    [SerializeField] LayerMask platformMask;
    [SerializeField] LayerMask oneWayMask;
    [SerializeField] AnimtionScript anm;
    [SerializeField] PhysicsMaterial2D ph2D;
    [Header ("Movement")]
    private bool canjump = false;
    private bool jumpApex = false;
    [SerializeField]public bool isJumping = false;
    [SerializeField]private int maxNumJump = 1;
    private int curJumps = 0;
    [SerializeField]public float jumpHeight = 5f;
    [SerializeField]private float fallSpeed = 200f;
    [SerializeField]private float maxGravity;
    private float horizontal;
    private float damageTimer = 0;
    private float maxDamageTimer = 1.75f;

    [SerializeField] public float kForce = 0;
    [SerializeField] public float kCount = 0;
    [SerializeField] public float kTotalTime = 0;
    [SerializeField] public bool isRight= false, r=true, l=false;
    [SerializeField]HPBar hpBar;

    [Header ("Events")]
    public UnityEvent gemPickUp;
    public UnityEvent<int, GameObject> hpChange;
    public UnityEvent<int> damage;
    private GameObject attacked;
    public AudioClip obtainHP;
    public AudioClip obtainGem;
    public properties stats;
    public static bool atExit = false;
    void Start(){
        ph2D.bounciness = 0;
        rd = GetComponent<Rigidbody2D>();
        box2D = GetComponent<BoxCollider2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }
    void Update(){
        damageTimer += Time.deltaTime;
        if(rd.velocity.y < maxGravity){
            capGravity();
        }
    }
    //Moves Gloob and changes animation
    public void knockedBack(){
        //play animation damage!
        isJumping=true;
        if(isRight == true){
            rd.velocity = new Vector2(-kForce, kForce);
        }
        else{
            rd.velocity = new Vector2(kForce, kForce);
        }
        kCount -= Time.fixedDeltaTime;
    }
    public void Move(){
        if(horizontal !=0){
            rd.velocity = new Vector2(horizontal * stats.speed, ((rd.velocity.y)));
            anm.ChangeAnimationState("Walking", 1);
            if(horizontal < 0){
                spriteR.flipX = true;
                if(r){
                    r=false;
                    l=true;
                    hpBar.reflection();
                }
            }
            else{
                spriteR.flipX = false;
                if(l){
                    l=false;
                    r=true;
                    hpBar.reflection();
                }
            }
            
        }
        else if (rd.velocity == Vector2.zero){           
            anm.ChangeAnimationState("Idle", 1);
        }
        
    }
    public void landing(){
        if(isGround()){

            isJumping = false;
            stop();
        }
    }


    // Adds jumping mechanic using ground check
    public void gloobJump(){
        if(canjump){
            isJumping=true;
            curJumps++;
            canjump=false;
            float jForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rd.gravityScale));
            //prevent multiple jumps from exponentially increasing jump
            rd.velocity = Vector2.zero;
            rd.AddForce(new Vector2(rd.velocity.x, jForce), ForceMode2D.Impulse);
            if(Input.GetAxisRaw("Horizontal") == 0f){
                rd.velocity = new Vector2(0, rd.velocity.y);
            }
        }
        if(jumpApex){
            jumpApex = false;
            rd.AddForce(Vector2.down * fallSpeed, ForceMode2D.Impulse);
        }
    }
    //==================== Helper functions ==================== 
    // stops gloob's vertical movement.
    public void canJump(){
        if(curJumps < maxNumJump){
            canjump=true;
        }
    }
    public void resetJump(){
        if(isGround() && (rd.velocity.y > -.05f && rd.velocity.y < .05f)){
            curJumps = 0;
        }
    }
    public void stop(){
         rd.velocity = (new Vector2 (0, rd.velocity.y));
    }
    // enforces max fall speed on gloob.
    public void capGravity(){
        rd.velocity=(new Vector2(rd.velocity.x, maxGravity));
    }
    // ground check
    public bool isGround(){
        return (Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0f, Vector2.down, 0.1f, platformMask) ||
                Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0f, Vector2.down, 0.1f, oneWayMask));
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Exit")) {
            atExit = true;
        }

    }
    void OnTriggerExit2D(Collider2D other) {
        atExit = false;
    }
    void OnCollisionEnter2D(Collision2D other){

        if (other.gameObject.CompareTag("ammo")) {
            if (hpBar.CurHP < 10)
            {
                GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.2f);
                GetComponent<AudioSource>().PlayOneShot(obtainHP);
                hpChange?.Invoke(1, other.gameObject);
            }
            else
            {
                other.gameObject.transform.position = new Vector2(100000, 10000);
                Destroy(other.gameObject, 2f);
            }
        }
        if(other.gameObject.CompareTag("enemy") && damageTimer >= maxDamageTimer){
            kCount = kTotalTime;
            if(other.transform.position.x <= transform.position.x){
                isRight = false;
            }
            else{
                isRight = true;
            }
            damageTimer = 0f; 
            attacked = null;
            attacked = other.gameObject;
            //temporary value
            hpChange?.Invoke(-1, null);
            damage?.Invoke(-1);
        }
        if (other.gameObject.CompareTag("gem")) {
            GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.2f);
            GetComponent<AudioSource>().PlayOneShot(obtainGem);
            gemPickUp?.Invoke();
            other.gameObject.transform.position = new Vector2(10000, 10000);
            Destroy(other.gameObject, 3f);
        }
    }
    public Vector3 RandomPosInCircle(float radius){
        Vector3 random = Random.insideUnitCircle * radius;
        return random + transform.position;
    }
    public void test(){
        //destroy gem!!
        gemPickUp?.Invoke();
    }
    // ==================== Properties ====================
    public Rigidbody2D RD{
        get{
            return rd;
        }
        set{
            rd = value;
        }
    }
    public bool CanJump{
        get{
            return canjump;
        }
        set{
            canjump = value;
        }
    }
    public bool JumpApex{
        get{
            return jumpApex;
        }
        set{
            jumpApex = value;
        }
    }
    public float MaxGravity{
        get{
            return maxGravity;
        }
        set{
            maxGravity = value;
        }
    }

    public float Horizontal{
        get{
            return horizontal;
        }
        set{
            horizontal = value;
        }
    }

    public int CurJumps{
        get{
            return curJumps;
        }
        set{
            curJumps = value;
        }
    }
    public int MaxNumJump{
        get{
            return maxNumJump;
        }
        set{
            maxNumJump = value;
        }
    }
    public bool IsJumping{
        get{
            return isJumping;
        }
        set{
            isJumping = value;
        }
    }
}

