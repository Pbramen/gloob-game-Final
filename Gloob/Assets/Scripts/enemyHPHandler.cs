using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHPHandler : MonoBehaviour, hpHandler
{
    [SerializeField] public int curHP;
    [SerializeField] public int maxHP;
    public GameObject gem;
    bool active = true;

    // Update is called once per frame
    void Update()
    {
        if (curHP <= 0 && active){
            active = false;
            monitorEnemies m = GetComponentInParent<monitorEnemies>();
            m.childDestroied();
            Instantiate(gem, transform.position, Quaternion.identity);
            transform.position = new Vector2(100000, 100000);
            Destroy(this.gameObject, 3f);
        }
    }
    public void alterHP(int a){
        curHP+=a;
        if(curHP > maxHP){
            curHP = maxHP;
        }
    }
    public int CurHP{
        get{return curHP;}
        set{curHP = value;}
    }
    public int MaxHP{
        get{return maxHP;}
        set{maxHP = value;}
    }
}
