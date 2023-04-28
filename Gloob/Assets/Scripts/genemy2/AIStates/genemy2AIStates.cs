using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class genemy2AIStates 
{
    float timer = 0f;
    public genemy2AI ai; 
    public genemy2AIStates(genemy2AI ai){
        this.ai = ai;
    }
    public void UpdateStateBase(){
        timer += Time.fixedDeltaTime;
        updateState();
    }
    public void BeginStateBase(){
        timer = 0f;
        beginState();
    }
    public abstract void updateState();
    public abstract void beginState();
}
