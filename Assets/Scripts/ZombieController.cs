using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour, IAttacks, IDestroyable
{
    public int hitPoints;
    public float zombieAlertDistance;
    public float zombieAttackDistance;
    public float zombieStopDistance;
    public float moveSpeed;
    public float attackRate;
    public int attackDamage;
    public float dieEffectSeconds;
    public float dieEffectForce;

    private GameObject player;
    private float nextFire;
    private Rigidbody rb;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            //Detect
            if (Vector3.Distance(transform.position, player.transform.position) < zombieAlertDistance)
            {
                TurnTowardsPlayer();
                //TODO: Alert nearby zombie companions
                //Move
                if (Vector3.Distance(transform.position, player.transform.position) > zombieStopDistance)
                {
                    MoveTowardsPlayer();
                }
                //Attack
                //print(Vector3.Distance(transform.position, player.transform.position));
                if (Vector3.Distance(transform.position, player.transform.position) < zombieAttackDistance
                       && player.activeSelf
                       && Time.time > nextFire)
                {
                    AttackPlayer();
                }
            }
        }

    }

    private void AttackPlayer()
    {
        print("zombie attacks!");
        nextFire = Time.time + attackRate;
        DoDamage(attackDamage, player.GetComponent<IDestroyable>());
    }

    private void MoveTowardsPlayer()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void TurnTowardsPlayer()
    {
        transform.forward = player.transform.position - transform.position;
        transform.forward.Set(transform.forward.x, 0, transform.forward.z);
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
            //gameObject.SetActive(false);
            Die();
        }
        print("hitpoints left: " + hitPoints);
    }

    private void Die()
    {
        
        //rb.AddForce(transform.forward * (-10));
        //rb.im
        
        isDead = true;

        StartCoroutine(DieAnimation());
        //rb.detectCollisions = false;
        //BoxCollider collider = GetComponent<BoxCollider>();
        //collider.enabled = false;
    }

    private IEnumerator DieAnimation()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(transform.forward * -5, ForceMode.Impulse);
        yield return new WaitForSeconds(dieEffectSeconds);
        rb.detectCollisions = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
