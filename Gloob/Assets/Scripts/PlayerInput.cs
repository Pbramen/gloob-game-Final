using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public gloob gloob;
    public Rigidbody2D rd;

    //Handles Player Input
    void Update(){

        float velocityY = rd.velocity.y;
        bool ground = gloob.isGround();
        
        gloob.Horizontal = Input.GetAxisRaw("Horizontal");
        if(!pauseGame.isPausing){
            if((Input.GetButtonDown("Jump")/* && ground) || Input.GetButtonDown("Jump")*/)){
                gloob.canJump();
            }
            else if(!ground && (Input.GetButtonUp("Jump") || velocityY < 0.0f)){
                
                gloob.JumpApex = true;
            }
        }
        if(Input.GetButtonUp("Horizontal")){
            gloob.stop();
        }
    }

    void FixedUpdate(){
        if(gloob.kCount <= 0){
            gloob.Move();
            if(gloob.IsJumping){
                gloob.landing();
            }
            gloob.gloobJump();
        }
        else{
            gloob.knockedBack();
        }
        gloob.resetJump();
        
    }
}