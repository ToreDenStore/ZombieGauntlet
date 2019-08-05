using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAttacks, IDestroyable
{
    public float Speed;
    private CharacterController _controller;
    Vector3 mousePosition;
    //Vector3 mousePosition2;
    private AudioSource footstepAudioSource;
    Vector3 lastPosition;
    bool isMoving = false;

    //Shots
    public GameObject gun;
    public GameObject shotAudio;
    public Transform shotSpawn;
    public GameObject shotExplosion;

    public int hitPoints;
    public float fireRate;
    public float weaponRange;
    public int weaponDamage;

    private float nextFire;
    //private LineRenderer _laserLine;
    private static readonly float _shotEffectLifetime = 2;
    private static readonly float _explosionLifetime = 2;


    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        _controller = GetComponent<CharacterController>();
        footstepAudioSource = GetComponent<AudioSource>();
        //_laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //print("mouse: " + mousePosition);
        //print("mouse2: " + mousePosition2);
        
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

        if (transform.position != lastPosition)
        {
            isMoving = true;
            lastPosition = transform.position;
        } else
        {
            isMoving = false;
        }

        //Play footstep sound
        if (isMoving && !footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
        }
        if (!isMoving && footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }
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
        //Vector3 gunPositionViewport = Camera.main.ScreenToViewportPoint(shotSpawn.position);

        //Vector3 gunDirection = mousePosition2 - gunPositionViewport;
        

        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            RaycastHit hit;
            //_laserLine.SetPosition(0, shotSpawn.position);
            Vector3 rayOrigin = shotSpawn.position;
            Vector3 gunDirection = new Vector3(
                mousePosition.x - shotSpawn.position.x,
                0,
                mousePosition.z - shotSpawn.position.z
            );
            //print("gun direction: " + gunDirection);
            if (Physics.Raycast(rayOrigin, gunDirection, out hit, weaponRange))
            {
                //_laserLine.SetPosition(1, hit.point);
                StartCoroutine(HitEffect(hit));

                DoDamage(weaponDamage, hit.transform.gameObject.GetComponent<IDestroyable>());
            }
            else
            {
                //_laserLine.SetPosition(1, rayOrigin + (gunDirection * weaponRange));
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        GameObject shotAudio = Instantiate(this.shotAudio);
        yield return new WaitForSeconds(_shotEffectLifetime);
        Destroy(shotAudio);
    }

    private IEnumerator HitEffect(RaycastHit hit)
    {
        GameObject explosion = Instantiate(shotExplosion, hit.point, hit.transform.rotation);
        yield return new WaitForSeconds(_explosionLifetime);
        Destroy(explosion);
    }

    public void DoDamage(int damage, IDestroyable target)
    {
        if (target != null)
        {
            target.ReceiveDamage(damage);
        }
    }

    public void ReceiveDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            gameObject.SetActive(false);
            //TODO End game:
            print("YOU DIED!!");
        }
        print("hitpoints left: " + hitPoints);
    }
}
