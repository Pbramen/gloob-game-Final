using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Stats", menuName ="Stats")]
public class properties : ScriptableObject
{
    // Start is called before the first frame update
    public int curHP;
    public int maxHP;
    public int attack;
    public int armor;
    public int baseDamage;
    public float projectileSpeed;

    public void loadData(int curHP, int maxHP, int attack, int armor, int baseDamage, float projectileSpeed) {
        this.curHP = curHP;
        this.maxHP = maxHP;
        this.attack = attack;
        this.armor = armor;
        this.baseDamage = baseDamage;
        this.projectileSpeed = projectileSpeed;
    }
    public void printStats() {
        Debug.Log("HP: " + curHP + "/" + maxHP);
        Debug.Log("Attack: " + attack);
        Debug.Log("Armor: " + armor);
    }


}
