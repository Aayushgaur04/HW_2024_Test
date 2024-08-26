using System.Collections;
using UnityEngine;

public class PulpitManager : MonoBehaviour
{
    public GameObject pulpitPrefab;
    public float pulpitXSize = 9f;
    public float pulpitZSize = 9f;
    public float pulpitYSize = 0.2f;

    private JSONExtraction jsonExtraction;
    private GameObject currentPulpit;
    private GameObject nextPulpit;
    private bool isSpawningNext = false;

    public float pulpitDuration;
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

        pulpitDuration = Random.Range(minDestroyTime, maxDestroyTime);

        currentPulpit = SpawnPulpit(Vector3.zero);
        StartCoroutine(PulpitLifecycle(currentPulpit));
    }

    void Update()
    {

        if (currentPulpit != null && !isSpawningNext)
        {
            StartCoroutine(SpawnNextPulpit());
        }
    }

    IEnumerator PulpitLifecycle(GameObject pulpit)
    {
        yield return new WaitForSeconds(pulpitDuration);

        if (pulpit != null)
        {
            Destroy(pulpit);
        }
    }

    IEnumerator SpawnNextPulpit()
    {
        isSpawningNext = true;
        float timeRemaining = pulpitDuration - Time.timeSinceLevelLoad;
        yield return new WaitForSeconds(timeRemaining - spawnTime);

        if (currentPulpit != null)
        {
            Vector3 spawnPosition = GetRandomAdjacentPosition(currentPulpit.transform.position);
            nextPulpit = SpawnPulpit(spawnPosition);

            StartCoroutine(PulpitLifecycle(nextPulpit));
        }

        yield return new WaitForSeconds(spawnTime);

        if (nextPulpit != null)
        {
            currentPulpit = nextPulpit;
            nextPulpit = null;
            isSpawningNext = false;
        }
    }

    Vector3 GetRandomAdjacentPosition(Vector3 currentPos)
    {
        Vector3[] directions = new Vector3[]
        {
            new Vector3(pulpitXSize, 0, 0),
            new Vector3(-pulpitXSize, 0, 0),
            new Vector3(0, 0, pulpitZSize),
            new Vector3(0, 0, -pulpitZSize)
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

    GameObject SpawnPulpit(Vector3 position)
    {
        GameObject pulpit = Instantiate(pulpitPrefab, position, Quaternion.identity);

        PlatformTimer platformTimer = pulpit.GetComponent<PlatformTimer>();
        if (platformTimer != null)
        {
            platformTimer.platformDuration = pulpitDuration;
        }

        return pulpit;
    }
}
