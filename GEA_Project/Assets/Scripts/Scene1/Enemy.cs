using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, RunAway }
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;
    public float runAwayDistance = 25f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform player;
    private float lastAttackTime;
    public int maxHp = 5;
    private int currentHp;

    public Slider hpSlider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        lastAttackTime = -attackCooldown;
        currentHp = maxHp;
        if (hpSlider != null)
        {   
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;
        }
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                break;
            case EnemyState.Trace:
                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist >= traceRange)
                    state = EnemyState.Idle;
                else TracePlayer();
                break;
            case EnemyState.Attack:
                if (dist >= attackRange)
                    state = EnemyState.Trace;
                else if (currentHp <= maxHp / 5)
                    state = EnemyState.RunAway;
                else AttackPlayer();
                break;
            case EnemyState.RunAway:
                if (dist >= runAwayDistance)
                    state = EnemyState.Idle;
                else
                    RunAway();
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (hpSlider != null)
        {  
            hpSlider.value = currentHp;
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void TracePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    void AttackPlayer()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            transform.LookAt(player.position);
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            if (ep != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

    void RunAway()
    {
        Vector3 dir = transform.position - player.position;
        dir.y = 0;
        dir = dir.normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + dir);
    }
}