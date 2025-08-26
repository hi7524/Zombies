using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // 상태패턴 위한 enum
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    private State currentState = State.Ready;

    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            switch (currentState)
            {
                case State.Ready:
                    break;

                case State.Empty:
                    break;

                case State.Reloading:
                    break;
            }
        }
    }

    public GunData gunData;

    public ParticleSystem muzzelEffect;
    public ParticleSystem shellEffect;
    public Transform firePos;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public int ammoRemain; // 남은 총알
    public int magAmmo;    // 탄창 크기

    private float lastFireTime;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0f;

        CurrentState = State.Ready;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

        switch (currentState)
        {
            case State.Ready:
                UpdateReady();
                break;
            case State.Empty:
                UpdateEmpty();
                break;
            case State.Reloading:
                UpdateReloading();
                break;
        }
    }

    private void UpdateReady()
    {

    }

    private void UpdateEmpty()
    {

    }

    private void UpdateReloading()
    {

    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        audioSource.PlayOneShot(gunData.shootClip);

        muzzelEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePos.position); // (인덱스, 위치)
        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.1f);

        lineRenderer.enabled = false;
    }

    public void Fire()
    {
        if (currentState == State.Ready && Time.time > (lastFireTime + gunData.timeBetFire))
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;

        RaycastHit hit; // 구조체 → 레이캐스팅 된 결과가 담기는 구조체

        // Physics.Raycast(시작지점, 나아가는 방향, 정보를 담을 구조체, 거리)
        // → 충돌했으면 true 안했으면 false return
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, gunData.fireDistance))
        {
            //hit.point 충돌 지점 (위치)
            hitPosition = hit.point;

            var target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = firePos.position + firePos.forward * gunData.fireDistance;
        }

        StartCoroutine(CoShotEffect(hitPosition));

        --magAmmo;

        Debug.Log($"{magAmmo}/{ammoRemain}");
        if (magAmmo == 0)
        {
            CurrentState = State.Empty;
        }
    }

    public bool Reload()
    {
        if (currentState == State.Reloading || ammoRemain == 0 || magAmmo == gunData.magCapacity)
            return false;

        
        StartCoroutine(CoReload());
        return true;
    }

    private IEnumerator CoReload()
    {
        currentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;
        if (magAmmo > gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
        }
        else
        {
            ammoRemain = 0;
        }

        currentState = State.Ready;
        Debug.Log($"{magAmmo}/{ammoRemain} << 재장전");

        //int amount = gunData.magCapacity - magAmmo;
        //int fillAmount = Mathf.Min(amount, ammoRemain);

        //magAmmo += fillAmount;
        //ammoRemain -= fillAmount;
    }
}