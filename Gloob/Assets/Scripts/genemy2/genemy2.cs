using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
/*
    TODO: REFACTOR THIS CODE (ALONG WITH GENEMY2AI.CS)
    make the attack and move loop

*/
public class genemy2 : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rd{get;set;}
    private Vector3 curPos, targetPos;
    [SerializeField] private float pDistance = 2f, speed = 5f;
    private float[,] phase; 
    [SerializeField] public bool collided = false, isRunning = false, isAttacking = false;
    private Coroutine p, a; 
    [SerializeField] public GameObject projectile;
    IAstarAI ai; 
    public float radius = 3f;
    bool attackPhase = false;
    public Seeker seeker;
    float nextwaypoint = 1f;
    int curWaypoint =0;
    public gloob mainGloob;
    Path path;
    bool reachedEnd = false;
    float lastPath = float.NegativeInfinity;
    float delay = 1.5f;
    AIPath aStar;
    public bool patrollCoroutineNULL = false;
    void OnEnable()
    {
        mainGloob = GameObject.FindGameObjectWithTag("Player").GetComponent<gloob>();
        seeker = GetComponent<Seeker>();
        Physics2D.IgnoreLayerCollision(9, 11, true);
        phase = new float[,] {{pDistance, 0}, {-pDistance/2f,pDistance/2}, {-pDistance/2f, -pDistance/2f}};
        rd = GetComponent<Rigidbody2D>();
    }
    


    void Update()
    {
        if(attackPhase && path != null){

            if( curWaypoint >= path.vectorPath.Count){
                reachedEnd = true;
                rd.velocity = Vector2.zero;
                return;
            }
            else{
                reachedEnd = false;

            }
            Vector2 direction = ((Vector2)path.vectorPath[curWaypoint] - rd.position).normalized;
            direction.y = Mathf.RoundToInt(direction.y);
            Vector2 force = direction * speed;
            rd.velocity = force;
            float distance = Vector2.Distance(rd.position, path.vectorPath[curWaypoint]);
        
            if(distance < nextwaypoint){
                curWaypoint++;
            }
        }   
    }

    public void runPatroll(){
        attackPhase = false;
        p = StartCoroutine(patrolling());
        if (p == null) 
        {
            patrollCoroutineNULL = true;
        }
    }
    public void stopPatrolling(){
        if(p != null)
            StopCoroutine(p);
    }
    public void stopAttacking(){
        if(a != null)
            StopCoroutine(a);
    }
    Vector3 RandomPos(Transform T){
        Vector3 test = new Vector2();
        return test;
    }
    public void startAttack(){
        attackPhase = true;

        a= StartCoroutine(attacking());
       
        IEnumerator attacking(){
            while (true)
            {
                if(seeker.IsDone() && Time.time > lastPath + delay){
                    seeker.StartPath(transform.position, mainGloob.RandomPosInCircle(radius/2), OnPathComplete);
                    lastPath = Time.time;
                }
                yield return new WaitForSeconds(2f);
                
                pewpew pew = Instantiate(projectile, rd.transform.position, Quaternion.identity).GetComponent<pewpew>();
                
            }
        }

    }
    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            curWaypoint = 1;
        }
    }
    public IEnumerator patrolling(){
        isRunning = true;
        
        while (true)
        {
            float distance = resetPosition(phase[0, 0], phase[0, 1]);
            for (int i = 1; i <= 3; i++)
            {
                while (!collided && distance > 0.1f)
                {
                    MoveToward(targetPos);
                    curPos = transform.position;
                    distance = Vector3.Distance(curPos, targetPos);
                    yield return null;
                }
                if (collided)
                {
                    int index = i % 3;
                    distance = resetPosition(phase[index, 0], phase[index, 0]);
                    MoveToward(targetPos);
                }
                yield return new WaitForSeconds(0.25f);
                if (i == 3) { break; }
                distance = resetPosition(phase[i, 0], phase[i, 1]);
            }
        }
       
    }

    public float resetPosition(float a, float b){
        curPos = transform.position;
        targetPos = new Vector3(curPos.x + a, curPos.y + b, 0);
        return Vector3.Distance(curPos, targetPos);
    }

    public void Move(Vector3 offset){
        if(offset != Vector3.zero){
            offset.Normalize();
            offset *= Time.fixedDeltaTime;
            rd.MovePosition(transform.position + ((offset)*speed));
        }
    }


    public void MoveToward(Vector3 position){
        Move(position - transform.position);
    }    
    public void MoveAway(Vector3 position){
        Move((position - transform.position) * -1);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(!other.gameObject.CompareTag("ammo"))
        collided = true;
    }
    private void OnCollisionExit2D(Collision2D other) {
            collided = false;

    }


}
