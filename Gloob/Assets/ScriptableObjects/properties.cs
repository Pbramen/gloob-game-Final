using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Stats", menuName ="Stats")]
public class properties : ScriptableObject
{
    // Start is called before the first frame update
    public int curHP;
    public int maxHP;
  
    public int armor;
    public int baseDamage;
    public float projectileSpeed;
    public float speed;
    public float attackSpeed;

    public void loadData(int curHP, int maxHP, int armor, int baseDamage, float projectileSpeed, float speed) {
        this.curHP = curHP;
        this.maxHP = maxHP;
      
        this.armor = armor;
        this.baseDamage = baseDamage;
        this.projectileSpeed = projectileSpeed;
        this.speed = speed;
    }
    public void printStats() {
        Debug.Log("HP: " + curHP + "/" + maxHP);
    }

    public void reset() {
        curHP = 10;
        maxHP = 10;
        baseDamage = 1;
        projectileSpeed = 1;
        speed = 6f;
        attackSpeed = 1.5f;
    }
}
