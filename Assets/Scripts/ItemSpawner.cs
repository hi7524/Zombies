using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    // O | 아이템 생성 위치 시각화 (인스펙터에서 범위 조정 가능하도록)
    // O | 아이템 스폰 확률
    // X | 아이템 생성시 이미 주변에 아이템이 존재한다면 스폰 위치 재탐색

    public GameObject[] itemPrfs;
    public float trySpawnInterval = 3f;
    public Vector3 spawnArea = new Vector3(5f, 0.001f, 3f);
    [Range(0, 100)]
    public int spawnProbability;

    private float spawnItemTime = 0f;
    private float findRange = 5f;

    private void Update()
    {
        if (Time.time >= spawnItemTime +  trySpawnInterval)
        {
            spawnItemTime = Time.time;

            int random = Random.Range(0, 100 + 1);
            if (random < spawnProbability)
                TrySpawnItem();
        }
    }

    private void TrySpawnItem()
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
            float randomX = Random.Range(-spawnArea.x, spawnArea.z);
            float randomZ = Random.Range(-spawnArea.z, spawnArea.z);

            Vector3 randomPoint = new Vector3(randomX, 1.0f, randomZ);
            NavMeshHit hit;

            // 기준 위치, NavMeshHit, 검출할 최대 거리, NavMesh 영역 마스크)
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) &&
                ContainPos(randomPoint, spawnArea))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    private bool ContainPos(Vector3 point, Vector3 areaSize)
    {
        float hx = areaSize.x * 0.5f;
        float hz = areaSize.z * 0.5f;
        return (point.x >= -hx && point.x <= hx) &&
               (point.z >= -hz && point.z <= hz);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var center = transform.position;
        var sz = new Vector3(spawnArea.x, spawnArea.y, spawnArea.z);

        Gizmos.DrawWireCube(center, sz);
    }
}