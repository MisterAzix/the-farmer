using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    private PlayerController playerController;

    //Mask
    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    public LayerMask obstructionMask;

    //Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;
    private int currentCheckpoint = 0;
    [SerializeField] private Transform[] checkpoints;

    //Range
    //[SerializeField] private float walkPointRange;
    public float soundRange, attackRange;

    //Attacking
    [SerializeField] private float timeBetweenAttacks;

    //States
    [SerializeField] public bool canSeePlayer;
    private bool alreadyAttacked;
    private bool playerInSoundRange, playerInAttackRange;


    private void Awake()
    {
        playerRef = GameObject.Find("Player");
        player = playerRef.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerController = playerRef.GetComponent<PlayerController>();

        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSoundRange = Physics.CheckSphere(transform.position, soundRange, whatIsPlayer) && playerController.isRunning;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange && !canSeePlayer) Patroling();
        if (!playerInAttackRange && playerInSoundRange || !playerInAttackRange && canSeePlayer) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();

        if (agent.velocity.magnitude < 1f) animator.SetFloat("Speed", 0);
        else animator.SetFloat("Speed", agent.velocity.magnitude);
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

    private void Patroling()
    {
        animator.SetFloat("Speed", 0.5f);

        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 2f)
        {
            int randomIndex = Random.Range(0, (checkpoints.Length - 1));
            if (randomIndex > (checkpoints.Length * 0.5)) currentCheckpoint = randomIndex;
            else currentCheckpoint = (currentCheckpoint + 1) % checkpoints.Length;
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        walkPoint = checkpoints[currentCheckpoint].position;
        walkPointSet = true;

        //if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
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
            Invoke(nameof(checkAttackDamage), timeBetweenAttacks / 2);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void checkAttackDamage()
    {
        if ((player.transform.position - transform.position).magnitude < 3.3f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
