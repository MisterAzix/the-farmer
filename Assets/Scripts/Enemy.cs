using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    [SerializeField] private NavMeshAgent agent;
    public GameObject playerRef;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    public LayerMask obstructionMask;

    //Patroling
    [SerializeField] private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    //States
    public float soundRange, attackRange;
    private bool playerInSoundRange, playerInAttackRange;

    [SerializeField] public bool canSeePlayer;

    private PlayerController playerController;

    private void Awake()
    {
        playerRef = GameObject.Find("Player");
        player = playerRef.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerController = playerRef.GetComponent<PlayerController>();

        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        bool rangeCheck = Physics.CheckSphere(transform.position, radius, whatIsPlayer);

        if (rangeCheck)
        {
            Vector3 directionToTarget = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSoundRange = Physics.CheckSphere(transform.position, soundRange, whatIsPlayer) && playerController.isRunning;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange && !canSeePlayer) Patroling();
        if (!playerInAttackRange && playerInSoundRange || !playerInAttackRange && canSeePlayer) ChasePlayer();
        if (playerInSoundRange && playerInAttackRange) AttackPlayer();

        if (agent.velocity.magnitude < 1f) animator.SetFloat("Speed", 0);
        else animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Patroling()
    {
        animator.SetFloat("Speed", 0.5f);

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 3f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 playerDir = player.transform.position - transform.position;
        Quaternion playerRotation = Quaternion.LookRotation(playerDir);
        transform.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0);

        if (!alreadyAttacked)
        {
            //Attack code
            animator.SetTrigger("Attack");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
