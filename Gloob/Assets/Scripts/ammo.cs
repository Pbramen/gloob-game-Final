using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ammo
{
    public int DamageMultiplier{ get; set; }
    public float Speed{ get; set; }
    public void Fire(int direction, float speedMult, int baseDamage);
    public void Revert();

}
