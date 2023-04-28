using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackState : genemy2AIStates{
    public attackState(genemy2AI ai) : base(ai){}

    public override  void updateState(){
        //need to do something here i guess
        if(ai.getGloob()){
            
        }
        else{
            ai.genemy2.stopAttacking();
            ai.changeState(ai.pState);
        }
    }
    public override void beginState(){
        ai.genemy2.stopPatrolling();
        ai.genemy2.startAttack();


    }
    // Start is called before the first frame update
   
}
