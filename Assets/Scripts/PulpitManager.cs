using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class PulpitManager : MonoBehaviour {

    public GameObject pulpitPrefab;
    public Transform player;
    public float pulpitXSize = 9f;
    public float pulpitZSize = 9f;
    public float pulpitYSize = 0.2f;

    private GameObject currentPulpit;
    private GameObject nextPulpit;
    private bool isSpawningNext = false;

    void Start()
    {
        currentPulpit = SpawnPulpit(Vector3.zero);
        StartCoroutine(PulpitLifecycle(currentPulpit));
    }

    void Update()
    {
        if (nextPulpit == null && !isSpawningNext && currentPulpit != null)
        {
            StartCoroutine(SpawnNextPulpit());
        }
    }

    IEnumerator PulpitLifecycle(GameObject pulpit)
    {
        float pulpitDuration = Random.Range(4f, 5f);

        yield return new WaitForSeconds(pulpitDuration);

        Destroy(pulpit);
    }

    IEnumerator SpawnNextPulpit()
    {
        isSpawningNext = true;

        yield return new WaitForSeconds(1.5f);

        Vector3 spawnPosition = GetRandomAdjacentPosition(currentPulpit.transform.position);
        nextPulpit = SpawnPulpit(spawnPosition);

        StartCoroutine(PulpitLifecycle(nextPulpit));

        yield return new WaitForSeconds(2.5f);

        currentPulpit = nextPulpit;
        nextPulpit = null;
        isSpawningNext = false;
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

        int randomIndex = Random.Range(0, directions.Length);
        return currentPos + directions[randomIndex];
    }

    GameObject SpawnPulpit(Vector3 position)
    {
        return Instantiate(pulpitPrefab, position, Quaternion.identity);
    }
}
