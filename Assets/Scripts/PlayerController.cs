using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAttacks, IDestroyable
{
    public GameController gameController;

    public GameObject healthBar;
    public GameObject diedText;
    public float Speed;
    public int hitPointsInitial;

    private int hitPointsLeft;
    private RectTransform healthBarRect;
    private CharacterController _controller;
    private Vector3 mousePosition;

    //Shots
    public GameObject gun;
    public GameObject shotAudio;
    public Transform shotSpawn;
    public GameObject shotExplosion;

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
        _controller = GetComponent<CharacterController>();
        healthBarRect = healthBar.GetComponent<RectTransform>();
        hitPointsLeft = hitPointsInitial;
        //_laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsPaused())
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            FaceMouse();

            Move();

            ShootGun();
        }
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

            StartCoroutine(ShotEffect());

            RaycastHit hit;
            //_laserLine.SetPosition(0, shotSpawn.position);
            Vector3 rayOrigin = shotSpawn.position;
            Vector3 gunDirection = new Vector3(
                mousePosition.x - shotSpawn.position.x,
                0,
                mousePosition.z - shotSpawn.position.z
            );
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
        hitPointsLeft -= damage;

        Debug.Log(damage);
        Debug.Log(hitPointsInitial);
        float percentageDamage = (float) damage / (float) hitPointsInitial;

        Debug.Log(percentageDamage);
        healthBarRect.localScale -= new Vector3(percentageDamage, 0, 0);

        if (hitPointsLeft <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!diedText.activeSelf)
        {
            //TODO: Dying "animation"
            diedText.SetActive(true);
            gameObject.SetActive(false);
            gameController.LoseGame();
        }
        
    }
}
