using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genemy2AI : MonoBehaviour
{
    public float d = 5f;
    [SerializeField] public gloob gloob;
    [SerializeField] public genemy2 genemy2{get; set;}
    public genemy2AIStates currState;
    public attackState aState;
    //enter states here;
    public patrollState pState;

    [Header("Debug")]
    public bool isPatrolling = false;
    public bool isNull = false;
    public bool otherError=false;
    void OnEnable(){
        
        gloob = FindObjectOfType<gloob>();
        genemy2 = transform.gameObject.GetComponent<genemy2>();
        pState = new patrollState(this);
        aState = new attackState(this);
        currState = pState;

        if (currState == pState) {
            isPatrolling = true;
        }
        if (currState == null) {
            isNull = true;
        }
        else {
            
            otherError = true;
        }
        currState.beginState();
    }

    void FixedUpdate(){
         currState.updateState();
    }      
    public void changeState(genemy2AIStates state){
        //  Debug.Log(state);
         currState = state;

        currState.BeginStateBase();
    }
    
    public bool getGloob(){
        if(gloob != null)
            return Vector3.Distance(transform.position, gloob.transform.position) <= d;
        return false;
    }
}
