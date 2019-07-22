using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    private CharacterController _controller;
    private Vector3 _cameraOffset;
    Vector3 mousePosition;

    //Shots
    public GameObject gun;
    public Transform shotSpawn;
    public GameObject shotExplosion;

    public float fireRate;
    public float weaponRange = 50f;

    private float nextFire;
    private LineRenderer _laserLine;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraOffset = Camera.main.transform.position;
        _laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _cameraOffset;

        FaceMouse();

        Move();

        ShootGun();
    }
    
    void Move()
    {
        //Movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal * Speed, 0.0f, moveVertical * Speed);
        _controller.SimpleMove(movement);
    }

    void FaceMouse()
    {
        //Facing mouse
        Vector3 direction = new Vector3(
            mousePosition.x - transform.position.x,
            0,
            mousePosition.z - transform.position.z
        );
        transform.forward = direction;
    }

    void ShootGun()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            //StartCoroutine(ShotEffect());

            RaycastHit hit;
            _laserLine.SetPosition(0, shotSpawn.position);
            Vector3 rayOrigin = shotSpawn.position;
            Vector3 gunDirection = new Vector3(
                mousePosition.x - shotSpawn.position.x,
                0,
                mousePosition.z - shotSpawn.position.z
            );
            if (Physics.Raycast(rayOrigin, gunDirection, out hit, weaponRange))
            {
                _laserLine.SetPosition(1, hit.point);
                Instantiate(shotExplosion, hit.point, transform.rotation);
            }
            else
            {
                _laserLine.SetPosition(1, rayOrigin + (transform.forward * weaponRange));
            }
        }
    }

    //private IEnumerator ShotEffect()
    //{
    //}
}
