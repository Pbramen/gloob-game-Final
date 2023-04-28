using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrollState: genemy2AIStates
{

    public patrollState(genemy2AI ai): base(ai){

    }
    // Start is called before the first frame update
    public override void updateState(){
       
        if(ai.getGloob()){
            ai.genemy2.stopPatrolling();
            ai.changeState(ai.aState);
        }
        else{
            //ai.genemy2.runPatroll();
        }
    }
    public override void beginState(){
       
        ai.genemy2.runPatroll();
    }
}
