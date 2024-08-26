using System.Collections;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public float platformXSize = 9f;
    public float platformZSize = 9f;
    public float platformYSize = 0.2f;

    private JSONExtraction jsonExtraction;
    private GameObject currentPlatform;
    private GameObject nextPlatform;
    private bool isSpawningNext = false;

    public float platformDuration;
    private float maxDestroyTime;
    private float minDestroyTime;
    private float spawnTime;

    private Vector3 previousDirection;

    void Start()
    {
        StartCoroutine(InitializeData());
    }

    IEnumerator InitializeData()
    {
        jsonExtraction = FindObjectOfType<JSONExtraction>();

        while (jsonExtraction.GetJsonData() == null)
        {
            yield return null;
        }

        Data data = jsonExtraction.GetJsonData();
        minDestroyTime = data.pulpit_data.min_pulpit_destroy_time;
        maxDestroyTime = data.pulpit_data.max_pulpit_destroy_time;
        spawnTime = data.pulpit_data.pulpit_spawn_time;

        platformDuration = Random.Range(minDestroyTime, maxDestroyTime);

        currentPlatform = SpawnPlatform(Vector3.zero);
        StartCoroutine(PlatformLifecycle(currentPlatform));
    }

    void Update()
    {
        if (currentPlatform != null && !isSpawningNext)
        {
            StartCoroutine(SpawnNextPlatform());
        }
    }

    IEnumerator PlatformLifecycle(GameObject platform)
    {
        yield return new WaitForSeconds(platformDuration);

        if (platform != null)
        {
            Destroy(platform);
        }
    }

    IEnumerator SpawnNextPlatform()
    {
        isSpawningNext = true;
        float timeRemaining = platformDuration - Time.timeSinceLevelLoad;
        yield return new WaitForSeconds(timeRemaining - spawnTime);

        if (currentPlatform != null)
        {
            Vector3 spawnPosition = GetRandomAdjacentPosition(currentPlatform.transform.position);
            nextPlatform = SpawnPlatform(spawnPosition);

            StartCoroutine(PlatformLifecycle(nextPlatform));
        }

        yield return new WaitForSeconds(spawnTime);

        if (nextPlatform != null)
        {
            currentPlatform = nextPlatform;
            nextPlatform = null;
            isSpawningNext = false;
        }
    }

    Vector3 GetRandomAdjacentPosition(Vector3 currentPos)
    {
        Vector3[] directions = new Vector3[]
        {
            new Vector3(platformXSize, 0, 0),
            new Vector3(-platformXSize, 0, 0),
            new Vector3(0, 0, platformZSize),
            new Vector3(0, 0, -platformZSize)
        };

        Vector3 spawnDirection;
        int randomIndex;

        do
        {
            randomIndex = Random.Range(0, directions.Length);
            spawnDirection = directions[randomIndex];
        }
        while (spawnDirection == previousDirection);

        previousDirection = spawnDirection;

        return currentPos + spawnDirection;
    }

    GameObject SpawnPlatform(Vector3 position)
    {
        return Instantiate(platformPrefab, position, Quaternion.identity);
    }
}
