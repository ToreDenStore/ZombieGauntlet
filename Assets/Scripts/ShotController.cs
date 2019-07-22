using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float speed;
    public float lifeTimeSeconds;
    private float lifeTimeStart;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        lifeTimeStart = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lifeTimeSeconds > lifeTimeStart)
        {
            Destroy(gameObject);
        }
    }
}
