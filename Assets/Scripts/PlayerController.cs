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
    public GameObject shot;
    public GameObject gun;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.5f);
    public float weaponRange = 50f;
    private LineRenderer laserLine;
    public GameObject shotExplosion;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraOffset = Camera.main.transform.position;
        laserLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _cameraOffset;

        FaceMouse();

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

            StartCoroutine(ShotEffect());

            RaycastHit hit;
            laserLine.SetPosition(0, shotSpawn.position);
            Vector3 rayOrigin = shotSpawn.position;
            Vector3 gunDirection = new Vector3(
                mousePosition.x - shotSpawn.position.x,
                0,
                mousePosition.z - shotSpawn.position.z
            );
            if (Physics.Raycast(rayOrigin, gunDirection, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);
                Instantiate(shotExplosion, hit.point, transform.rotation);
            }
            else
            {
                laserLine.SetPosition(1, rayOrigin + (transform.forward * weaponRange));
            }
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
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

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
