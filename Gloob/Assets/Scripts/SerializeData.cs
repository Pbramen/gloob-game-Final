using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializeData 
{

    public int curHP;
    public int maxHP;
    public int attack;
    public int armor;
    public int baseDamage;
    public float projectileSpeed;
    public SerializeData(HPBar status) {
        properties stats = status.stats;
        this.curHP = stats.curHP;
        this.maxHP = stats.maxHP;
        
        this.armor = stats.armor;
        this.baseDamage = stats.baseDamage;
        this.projectileSpeed = stats.projectileSpeed;
    }


}
