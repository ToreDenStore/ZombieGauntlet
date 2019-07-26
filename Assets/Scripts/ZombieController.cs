using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour, IAttacks, IDestroyable
{
    public int hitPoints;
    public float zombieAlertDistance = 10f;
    public float zombieAttackDistance = 2f;
    public float zombieStopDistance = 1.5f;
    public float moveSpeed = 10;
    public float attackRate;
    public int attackDamage;

    private GameObject player;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Detect
        if (Vector3.Distance(transform.position, player.transform.position) < zombieAlertDistance)
        {
            TurnTowardsPlayer();
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
            gameObject.SetActive(false);
        }
        print("hitpoints left: " + hitPoints);
    }
}
