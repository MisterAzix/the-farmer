using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    [SerializeField] Transform player;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    [SerializeField] Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField] float walkPointRange;

    //Attacking
    [SerializeField] float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    [SerializeField] float sightRange, attackRange;
    [SerializeField] bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) ChasePlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) 
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1f) 
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, transform.up, 2f, whatIsGround)) 
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

    }
}
