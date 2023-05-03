using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gemPickUp : MonoBehaviour
{
    public Text points;
    public int pointTracker = 0;
    // Start is called before the first frame update
    void Start() {
        points.text = "0";
        pointTracker = 0;
    }
    public void addPoints(){
        pointTracker += 1;
        points.text = pointTracker.ToString();
    }
    public void substractPoints(int i) {
        pointTracker -= i;
        points.text = pointTracker.ToString();
    }
}
