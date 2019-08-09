using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void OnMouseEnter()
    {
        print("mouse enter");
        GetComponent<Renderer>().material.color = Color.white;
    }

    void OnMouseDown()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
