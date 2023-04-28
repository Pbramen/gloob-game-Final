using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockCollision : MonoBehaviour
{
    [SerializeField]private string a;
    [SerializeField]private string b;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(a), LayerMask.NameToLayer(b), true);
    }
}
