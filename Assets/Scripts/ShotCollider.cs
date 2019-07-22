using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCollider : MonoBehaviour
{
    public GameObject shotExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Instantiate(shotExplosion, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

    }
}
