using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    private CharacterController _controller;
    private Vector3 _cameraOffset;

    //Shots
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraOffset = Camera.main.transform.position;
    }

    void Update()
    {
        if((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Facing mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = mousePosition - _cameraOffset;
        Vector3 direction = new Vector3 (
                mousePosition.x - transform.position.x,
                0,
                mousePosition.z - transform.position.z
        );
        transform.forward = direction;

        //Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal * Speed, 0.0f, moveVertical * Speed);
        _controller.SimpleMove(movement);
    }
}
