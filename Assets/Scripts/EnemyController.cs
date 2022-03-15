using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    public TMPro.TextMeshPro chaseAlert;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private Gun weapon;
    private FieldOfView fov; 
    private Animator animator;

    //Patroling
    public Vector3 walkPoint;
    public Transform patrolPoint1;
    public Transform patrolPoint2;

    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private Vector3 prePosition;
    private float speed = 0f;

    // take damage
    public float health;
    public float damage;
    public GameObject bsplt;

    // internal state; 0: patrolling; 1: chasing enemy; 2: attack; 3 get shot
    private int state = 0;
    private static readonly int STATE_PATROL = 0;
    private static readonly int STATE_CHASE = 1;
    private static readonly int STATE_ATTACK = 2;
    private static readonly int STATE_GET_SHOT = 3;
    private Vector3 currentPatrolDestination;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<Gun>();
        weapon.target = player;

        
        agent.SetDestination(patrolPoint2.position);
        agent.isStopped = false;
        state = STATE_PATROL;
        currentPatrolDestination = patrolPoint2.position;


        prePosition = transform.position;
    }

    private void Update()
    {
        UpdateSpeed();
        UpdateRunningAnimation();
        UpdateAlert();

        //Check for sight and attack range
        playerInSightRange = fov.canSeePlayer;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (state == STATE_GET_SHOT) StartCoroutine(SufferPainAndRevenge());
        if (!playerInSightRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private IEnumerator SufferPainAndRevenge()
    {
        float sufferTime = 1.0f; // not attack during the time
        yield return new WaitForSeconds(sufferTime);
        animator.SetBool("isShot", false);
        agent.isStopped = false;;
        if (!playerInSightRange) ChasePlayer();
        else AttackPlayer();
    }

    private void Patroling()
    {
        if (state != STATE_PATROL)
        {
            ResetShootingAnimation();
        }

        state = STATE_PATROL;
        // go back to the patrolling spot after chasing the enemies 
        if (!vectorEqualIgnoreY(agent.destination, patrolPoint1.position) && !vectorEqualIgnoreY(agent.destination, patrolPoint2.position))
        {
            // chose the nearest patrol starting point 
            float dist1 = Vector3.Distance(patrolPoint1.position, transform.position);
            float dist2 = Vector3.Distance(patrolPoint2.position, transform.position);
            if (dist1 < dist2)
            {
                agent.SetDestination(patrolPoint1.position);
                agent.isStopped = false;
                currentPatrolDestination = patrolPoint1.position;
            }
            else 
            {
                agent.SetDestination(patrolPoint2.position);
                agent.isStopped = false;
                currentPatrolDestination = patrolPoint2.position;
            }
            return;
        }

        // reach the end of the point  
        if (vectorEqualIgnoreY(transform.position, currentPatrolDestination))
        {
            Debug.Log("path finished");
            if (vectorEqualIgnoreY(currentPatrolDestination, patrolPoint1.position))
            {
                Debug.Log(" set to point 2");
                StartCoroutine(changeDestinationWithDelay(3.0f, patrolPoint2));
            }
            else 
            {
                 Debug.Log(" set to point 1");
                StartCoroutine(changeDestinationWithDelay(3.0f, patrolPoint1));
            }
        }


    }

    private bool vectorEqualIgnoreY(Vector3 v1, Vector3 v2)
    {
        Vector3 v1Clone = new Vector3(v1.x, 0f, v1.z);
        Vector3 v2Clone = new Vector3(v2.x, 0f, v2.z);
        return (v1Clone - v2Clone).magnitude < 0.1f;
    }

    private IEnumerator changeDestinationWithDelay(float delay, Transform destination)
    {
        agent.isStopped = true; // stop and rest first 
        currentPatrolDestination = destination.position;
        transform.rotation = Quaternion.FromToRotation(destination.forward, -transform.forward) * transform.rotation;
        yield return new WaitForSeconds(delay);
        agent.SetDestination(destination.position);
        agent.isStopped = false;
    }

    

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        if (new Vector2(randomX, randomZ).magnitude <= 1.0f)
        {
            randomX = randomZ = 0.0f;
        } else if (new Vector2(randomX, randomZ).magnitude <= 3.0f)
        {
            randomX *= 2f;
            randomZ *= 2f;
        }
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        //Debug.Log(walkPoint);
        if (Physics.Raycast(walkPoint, -transform.up, 5f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void ChasePlayer()
    {
        Debug.Log("chase player");
        state = STATE_CHASE;
        ResetShootingAnimation();
        agent.SetDestination(player.position);
        agent.isStopped = false;
    }

    private void ResetShootingAnimation()
    {
        if (animator.GetBool("ShouldShoot"))
        {
            animator.SetBool("ShouldShoot", false);
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("attack player");
        //Make sure enemy doesn't move
        agent.isStopped = true;
        //agent.SetDestination(transform.position);

        state = STATE_ATTACK;

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Attack();
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void ApplyDamage(Collision dmgPoint)
    {
        agent.isStopped = true;
        Debug.Log("got hit");
        animator.SetBool("isShot", true);
        state = STATE_GET_SHOT;
        
        health -= damage;
        bsplt.transform.parent = dmgPoint.transform;
        Quaternion quaternion = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject blood = Instantiate(bsplt, dmgPoint.contacts[0].point, quaternion);
        Destroy(blood, 3.0f);

        if (health <= 0) {
            Invoke(nameof(DestroyEnemy), 2.6f);
            animator.SetBool("IsDying", true);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void UpdateSpeed()
    {
        speed = (transform.position - prePosition).magnitude / Time.deltaTime;
        prePosition = transform.position;
    }


    private void Attack()
    {
        float delay = 0.2f ;

        animator.SetBool("ShouldShoot", true);
        StartCoroutine(weapon.DelayFire(delay));
        
    }

    private IEnumerator DelayAnimation(string type, bool value)
    {
        float delay = 0.2f;
        yield return new WaitForSeconds(delay);
        animator.SetBool(type, value);
    }


    void UpdateRunningAnimation()
    {
        if (animator.GetBool("ShouldShoot"))
        {
            return;
        }

        if (speed <= 0.5f)
        {
            animator.SetBool("ShouldRun", false);
        }
        else
        {
            animator.SetBool("ShouldRun", true);
        }
    }

    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.tag != "projectile")
    //     {
    //         return;
    //     }

    //     Debug.Log("enemy is attacked");
    //     ApplyDamage(other);
    // }


    private void UpdateAlert()
    {
        if (state == STATE_ATTACK || state == STATE_CHASE)
        {
            chaseAlert.text = "!";
        }
        else 
        {
            chaseAlert.text = "";
        }
    }
}
