using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PulpitManager : MonoBehaviour {

    public GameObject platformPrefab;
    public Transform player;
    public float platformXSize = 9f;
    public float platformZSize = 9f;
    public float platformYSize = 0.2f;

    private GameObject currentPlatform;
    private GameObject nextPlatform;
    private bool isSpawningNext = false;

    public float platformDuration;

    void Awake()
    {
        platformDuration = Random.Range(4f, 5f);
    }

    void Start()
    {
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
        yield return new WaitForSeconds(timeRemaining - 2.5f);

        if (currentPlatform != null)
        {
            Vector3 spawnPosition = GetRandomAdjacentPosition(currentPlatform.transform.position);
            nextPlatform = SpawnPlatform(spawnPosition);

            StartCoroutine(PlatformLifecycle(nextPlatform));
        }

        yield return new WaitForSeconds(2.5f);

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

        int randomIndex = Random.Range(0, directions.Length);
        return currentPos + directions[randomIndex];
    }

    GameObject SpawnPlatform(Vector3 position)
    {
        return Instantiate(platformPrefab, position, Quaternion.identity);
    }
}
