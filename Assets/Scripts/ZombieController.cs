using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private GameObject player;

    public int hitPoints;
    public float zombieAlertDistance = 10f;
    public float zombieAttackDistance = 2f;
    public float zombieStopDistance = 1.5f;
    public float moveSpeed = 10;

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
            if (Vector3.Distance(transform.position, player.transform.position) < zombieAttackDistance)
            {
                AttackPlayer();
            }
        }
    }

    void AttackPlayer()
    {
        print("zombie attacks!");
    }

    void MoveTowardsPlayer()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void TurnTowardsPlayer()
    {
        transform.forward = player.transform.position - transform.position;
        transform.forward.Set(transform.forward.x, 0, transform.forward.z);
    }

    public void DoDamageTo(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            gameObject.SetActive(false);
        }
        print("hitpoints left: " + hitPoints);
    }
}
