using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastViewer : MonoBehaviour
{
    public float weaponRange = 50f;
    //public GameObject mainCharacter;
    public GameObject gun;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lineOriginCharacter = transform.position;
        Vector3 lineOriginGun = gun.transform.position;

        Debug.DrawLine(lineOriginCharacter, transform.forward * weaponRange, Color.green);
    }
}
