using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimtionScript : MonoBehaviour

{
    public Animator animator;
    public string currentState ="Idle";
    public float speed =1;

    public void ChangeAnimationState(string newState, float speed){
        animator.speed=speed;
        if(currentState == newState){
            return;
        }
        currentState=newState;
        animator.Play(newState);
    }

}
