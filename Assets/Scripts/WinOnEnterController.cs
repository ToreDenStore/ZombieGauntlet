﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOnEnterController : MonoBehaviour
{
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Player entered win box");
            gameController.WinGame();
        }
    }


}
