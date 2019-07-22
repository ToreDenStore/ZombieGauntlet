using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
