using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gemPickUp : MonoBehaviour
{
    public Text points;
    public int pointTracker = 0;
    // Start is called before the first frame update
    public void addPoints(){
        pointTracker += 1;
        points.text = pointTracker.ToString();
    }
    public void substractPoints(int i) {
        pointTracker -= i;
        points.text = pointTracker.ToString();
    }
}
