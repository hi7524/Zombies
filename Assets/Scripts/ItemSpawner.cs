using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.GameCenter;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrfs;
    public float spawnInterval = 1.5f;
    private float spawnItemTime = 0f;

    //
    // NavMesh.SamplePosition 넘긴 좌표의 가장 가까운 점 찾았는지 여부 반환

    private float findRange = 5f;

    private void Update()
    {
        if (Time.time >= spawnItemTime +  spawnInterval)
        {
            spawnItemTime = Time.time;
            TEST();
        }
    }

    private void TEST()
    {
        Vector3 randomPos = Vector3.zero;

        if (GetRandomPoint(out randomPos))
        {
            SpawnItem(randomPos);
        }
    }

    private void SpawnItem(Vector3 spawnPos)
    {
        int randomType = Random.Range(0, itemPrfs.Length);

        GameObject itemObj = Instantiate(itemPrfs[randomType]);

        itemObj.transform.position = spawnPos;
    }

    private bool GetRandomPoint(out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            float randomX = Random.Range(-9, 9);
            float randomZ = Random.Range(-9, 9);

            Vector3 randomPoint = new Vector3(randomX, 1.0f, randomZ);
            NavMeshHit hit;

            // 기준 위치, NavMeshHit, 검출할 최대 거리, NavMesh 영역 마스크)
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

        }

        result = Vector3.zero;
        return false;
    }
}