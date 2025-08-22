using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public ParticleSystem muzzelEffect;
    public ParticleSystem shellEffect;
    public Transform firePos;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ShotEffect());
        }
    }

    private IEnumerator ShotEffect()
    {
        muzzelEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePos.position); // (인덱스, 위치)

        Vector3 endPos = firePos.position + firePos.forward * 10f; // 끝 점 위치 설정
        lineRenderer.SetPosition(1, endPos);

        yield return new WaitForSeconds(1f);

        lineRenderer.enabled = false;
    }
}