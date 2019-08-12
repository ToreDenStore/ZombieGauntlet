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

    private bool isDead;
    private bool isAlerted;

    //Audio
    public AudioSource attackAudio;
    public AudioSource alertAudio;
    public AudioSource deathAudio;

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
                Alert();
            }

            if (isAlerted) {
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

    private void Alert()
    {
        if (!isAlerted)
        {
            isAlerted = true;
            alertAudio.Play();
        }
    }

    private void AttackPlayer()
    {
        attackAudio.Play();
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
            Die();
        }
        print("hitpoints left: " + hitPoints);
    }

    private void Die()
    {
        isDead = true;
        deathAudio.Play();
        StartCoroutine(DieAnimation());
    }

    private IEnumerator DieAnimation()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(transform.forward * -1 * dieEffectForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dieEffectSeconds);
        rb.detectCollisions = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
