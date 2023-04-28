using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class pointTracker : MonoBehaviour
{
    public int points = 0;
    public Text displayPoint;
    void Start()
    {
        //load points from save file
    }
    public void addPoint() {
        points++;
    }
    public void subtractPoints(int i) {
        points -= i;
    }
    public void updatePoints(int newPoint) {
        displayPoint.text = newPoint.ToString();
    }
}
