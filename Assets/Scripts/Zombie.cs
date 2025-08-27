using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    private Status currentStatus;
    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var prevStatus = currentStatus;
            currentStatus = value;

            switch (currentStatus)
            {
                case Status.Idle:
                    animator.SetBool(AnimIds.HasTarget, false);
                    agent.isStopped = true;
                    break;

                case Status.Trace:
                    animator.SetBool(AnimIds.HasTarget, true);
                    agent.isStopped = false;
                    break;

                case Status.Attack:
                    animator.SetBool(AnimIds.HasTarget, false);
                    agent.isStopped = true;
                    break;

                case Status.Die:
                    animator.SetTrigger(AnimIds.DieHash);
                    break;
            }
        }
    }

    public float traceDistance;
    public float attackDistance;

    public float damage = 10f;
    public float lastAttackTime;
    public float attackInterval = 0.5f;

    private NavMeshAgent agent;
    private Animator animator;

    public Transform target;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;

            case Status.Trace:
                UpdateTrace();
                break;

            case Status.Attack:
                UpdateAttack();
                break;

            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (target != null &&
            Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
        }
    }

    private void UpdateTrace()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        if (target == null && Vector3.Distance(transform.position, target.position) > traceDistance)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);
    }

    private void UpdateAttack()
    {
        if (target == null || (target != null && Vector3.Distance(transform.position, target.position) > attackDistance))
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (lastAttackTime + attackInterval <= Time.time)
        {
            lastAttackTime = Time.time;

            var damagable = target.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.OnDamage(damage, transform.position, -transform.forward);
            }
        } 
    }

    private void UpdateDie()
    {

    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    protected override void Die()
    {
        base.Die();

        CurrentStatus = Status.Die;
    }
}