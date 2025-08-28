using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.Audio;

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
    private CapsuleCollider capsuleCollider;
    private AudioSource audioSource;

    private Transform target;
    public LayerMask targetLayer;

    public ParticleSystem bloodEffect;

    public AudioClip zombieDamage;
    public AudioClip zombieDie;

    public Renderer zombieRenderer;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        capsuleCollider.enabled = true;
        CurrentStatus = Status.Idle;
    }

    public void Setup(ZombieData data)
    {
        MaxHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;
        zombieRenderer.material.color = data.skinColor; // 좋지 못한 방법 
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

        target = FindTarget(traceDistance);
    }

    private void UpdateTrace()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }
        if (target == null || Vector3.Distance(transform.position, target.position) > traceDistance)
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
        BloodEffect(hitPoint, hitNormal);
        audioSource.PlayOneShot(zombieDamage);
    }

    private void BloodEffect(Vector3 hitPos, Vector3 hitNormal)
    {
        bloodEffect.transform.position = hitPos;
        bloodEffect.transform.forward = hitNormal;
        //bloodEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        bloodEffect.Play();
    }

    protected override void Die()
    {
        base.Die();

        audioSource.PlayOneShot(zombieDie);
        capsuleCollider.enabled = false;
        CurrentStatus = Status.Die;
    }

    protected Transform FindTarget(float radius)
    {
        var colliders = Physics.OverlapSphere(transform.position, radius, targetLayer.value);

        if (colliders.Length == 0)
            return null;

        // 거리 오름차순 정렬 후 가장 첫 번째 요소 반환
        var target = colliders.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).First();
        return target.transform;
    }
}